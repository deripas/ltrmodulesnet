using System;
using ltrModulesNet;


/* Данный пример демонстрирует работу с модулем LTR12 из программы на языке C#.
 * Пример представляет собой консольную программу, которая устанавливает связь с модулем,
 * выводит информацию о модуле, устанавливает настройки и собирает заданное кол-во 
 * блоков заданного размера, выводя на экран только по первому значению каждого для канала.
 * 
 * Необходимо установить номер слота, в котором вставлен модуль (константа SLOT).
 * Настройки сбора задаются в коде при конфигурации модуля.
 */


namespace ltr22_console
{
    class ltr22_console
    {
         /* Номер слота в крейте, где вставлен модуль */
        const int SLOT = 1;
        /* Количество отсчетов на канал, принмаемых за раз */
        const int RECV_BLOCK_CH_SIZE = 4096 * 8;
        /* Количество блоков, которые нужно принять и выйти */
        const int RECV_BLOCK_CNT = 50;
        /* Таймаут на ожидание данных при приеме (без учета времени преобразования) */
        const int RECV_TOUT = 1000;


        static int Main(string[] args)
        {          
            /* LTR22_Init() вызывается уже в конструкторе */
            ltr22api hltr22 = new ltr22api();
            /* отрываем модуль. Используем упрощенный вариант функции с указанием только слота.
             * (есть вариант как с только со слотом, так и с серийным крейта и слотом 
             *  + полный) */
            _LTRNative.LTRERROR err = hltr22.Open(SLOT);
            if (err != _LTRNative.LTRERROR.OK)
            {
                Console.WriteLine("Не удалось открыть модуль. Ошибка {0}: {1}",
                    err, ltr22api.GetErrorString(err));
            }
            else
            {
                 /* выводим информацию из hltr22.ModuleInfo */
                Console.WriteLine("Информация о модуле: ");
                Console.WriteLine("  Название модуля: {0}", hltr22.ModuleInfo.Description.DeviceName);
                Console.WriteLine("  Серийный номер : {0}", hltr22.ModuleInfo.Description.SerialNumber);
                Console.WriteLine("  Версия прошивки: {0}", hltr22.ModuleInfo.CPU.FirmwareVersionStr);

                /* --------------- задание параметров работы модуля ------------ */                    
                hltr22.AdcFreq = 78000; /* частота сбора АЦП */
                hltr22.ACDCState = true; //AC + DC
                hltr22.MeasureADCZero = false; 
                hltr22.SyncMaster = false;
                /* разрешение каналов */
                hltr22.fillChannelEnabled(0, true);                    
                hltr22.fillChannelEnabled(1, true);
                hltr22.fillChannelEnabled(2, true);
                hltr22.fillChannelEnabled(3, false);
                /* установка диапазонов каналов */
                hltr22.fillChannelRange(0, ltr22api.AdcRange.Range_10);
                hltr22.fillChannelRange(1, ltr22api.AdcRange.Range_10);
                hltr22.fillChannelRange(2, ltr22api.AdcRange.Range_10);
                hltr22.fillChannelRange(3, ltr22api.AdcRange.Range_10);

                /* Передача настроек в модуль */
                err = hltr22.SetConfig();
                if (err != _LTRNative.LTRERROR.OK)
                {
                    Console.WriteLine("Не удалось установить настройки модуля. Ошибка {0}: {1}",
                         err, ltr22api.GetErrorString(err));
                }

                if (err == _LTRNative.LTRERROR.OK)
                {
                    /* подсчитываем кол-во разрешенных каналов для дальнейшего разделения
                     * данных по каналам */
                    int enabled_ch_cnt = 0;
                    for (int i = 0; i < ltr22api.LTR22_CHANNEL_CNT; i++)
                    {
                        if (hltr22.channelEnabled(i))
                            enabled_ch_cnt++;
                    }

                    int recv_data_cnt = RECV_BLOCK_CH_SIZE * enabled_ch_cnt;

                    uint[] rbuf = new uint[recv_data_cnt];
                    double[] data = new double[recv_data_cnt];

                    /* запуск сбора данных */
                    err = hltr22.StartADC(false);
                    if (err != _LTRNative.LTRERROR.OK)
                    {
                        Console.WriteLine("Не удалось запустить сбор данных. Ошибка {0}: {1}",
                             err, ltr22api.GetErrorString(err));
                    }
                    else
                    {
                        _LTRNative.LTRERROR stop_err;
                        for (int i = 0; (i < RECV_BLOCK_CNT) && 
                                (err == _LTRNative.LTRERROR.OK); i++)
                        {
                            int rcv_cnt;
                            /* в таймауте учитываем время выполнения самого преобразования*/
                            uint tout = RECV_TOUT + (uint)(RECV_BLOCK_CH_SIZE / hltr22.AdcFreq + 1) * 1000;
                            /* прием необработанных слов. есть варинант с tmark и без него для удобства */
                            rcv_cnt = hltr22.Recv(rbuf, (uint)rbuf.Length, tout);

                            /* значение меньше 0 => код ошибки */
                            if (rcv_cnt < 0)
                            {
                                err = (_LTRNative.LTRERROR)rcv_cnt;
                                Console.WriteLine("Ошибка приема данных. Ошибка {0}: {1}",
                                                  err, ltr22api.GetErrorString(err));
                            }
                            else if (rcv_cnt != rbuf.Length)
                            {
                                err = _LTRNative.LTRERROR.ERROR_RECV_INSUFFICIENT_DATA;
                                Console.WriteLine("Приняли недостаточно данных: запрашивали {0}, приняли {1}",
                                                  rbuf.Length, rcv_cnt);
                            }
                            else
                            {
                                /* используем фунцию ProcessDataEx, которая возвращает результат
                                 * в естественном для всех LTR-модулей порядке 
                                 * (сперва первые отсчеты каждого канала, затем вторые и т.д.).
                                 * Флагами указываем необходимость применения калибровки
                                 * и перевода в Вольты */
                                err = hltr22.ProcessDataEx(rbuf, data, ref rcv_cnt, 
                                    ltr22api.ProcFlags.Calibr | ltr22api.ProcFlags.Volt);
                                if (err != _LTRNative.LTRERROR.OK)
                                {
                                    Console.WriteLine("Ошибка обработки данных. Ошибка {0}: {1}",
                                                       err, ltr22api.GetErrorString(err));
                                }
                                else
                                {
                                    /* при успешной обработке для примера выводим по одному значению
                                     * для каждого канала */
                                    Console.Write("Блок {0}.", i+1);
                                    for (int ch=0; ch < enabled_ch_cnt; ch++)
                                    {                    
                                        /* если все ок - выводим значение (для примера только первое) */
                                        Console.Write(" {1}", ch + 1, data[ch].ToString("F7"));
                                        if (ch == (enabled_ch_cnt - 1))
                                            Console.WriteLine("");
                                        else
                                            Console.Write(", ");
                                    }
                                }
                            }
                        }

                        /* останавливаем сбор данных */
                        stop_err = hltr22.StopADC();
                        if (stop_err != _LTRNative.LTRERROR.OK)
                        {
                            Console.WriteLine("Не удалось остановить сбор данных. Ошибка {0}: {1}",
                                 stop_err, ltr22api.GetErrorString(stop_err));
                            if (err == _LTRNative.LTRERROR.OK)
                                err = stop_err;
                        }

                        /* LTR22 также требует явной очитски буфера после останова сбора */
                        stop_err = hltr22.ClearBuffer(true);
                        if (stop_err != _LTRNative.LTRERROR.OK)
                        {
                            Console.WriteLine("Не удалось очистить буфер данных. Ошибка {0}: {1}",
                                 stop_err, ltr22api.GetErrorString(stop_err));
                            if (err == _LTRNative.LTRERROR.OK)
                                err = stop_err;
                        }
                    }
                }
            }

            /* закрываем соединение */
            if (hltr22.IsOpened() == _LTRNative.LTRERROR.OK)
                hltr22.Close();

            return (int)err;
        }
    }
}
