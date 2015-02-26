using System;
using ltrModulesNet;

namespace ltr212_console
{
    class ltr212_console
    {
        /* Номер слота в крейте, где вставлен модуль */
        const int SLOT = 6;
        /* Количество отсчетов на канал, принмаемых за раз */
        const int RECV_BLOCK_CH_SIZE = 32;
        /* Количество блоков, которые нужно принять и выйти */
        const int RECV_BLOCK_CNT = 50;

        static void Main(string[] args)
        {
            Console.WriteLine("Версия библиотеки: {0}", ltr212api.DllVersion.Str);

            /* LTR11_Init() вызывается уже в конструкторе */
            ltr212api hltr212 = new ltr212api();
            /* отрываем модуль. Используем упрощенный вариант функции с указанием только слота.
             * (есть вариант как с только со слотом, так и с серийным крейта и слотом 
             *  + полный) */
            _LTRNative.LTRERROR err = hltr212.Open(SLOT, "c:/Program Files (x86)/L-Card/ltr/bin/firmware/ltr212.bio");
            if (err != _LTRNative.LTRERROR.OK)
            {
                Console.WriteLine("Не удалось открыть модуль. Ошибка {0}: {1}",
                                    err, ltr212api.GetErrorString(err));
            }
            else
            {
                /* выводим информацию из hltr11.ModuleInfo */
                Console.WriteLine("Информация о модуле: ");
                Console.WriteLine("  Модификация модуля     : {0}", hltr212.ModuleInfo.TypeStr);
                Console.WriteLine("  Серийный номер         : {0}", hltr212.ModuleInfo.Serial);
                Console.WriteLine("  Версия прошивки        : {0}", hltr212.ModuleInfo.BiosVersion);
                Console.WriteLine("  Дата создания прошивки : {0}", hltr212.ModuleInfo.BiosDate);

                /* Проверка контрольной суммы EEPROM */
                err = hltr212.TestEEPROM();
                if (err != _LTRNative.LTRERROR.OK)
                {
                    Console.WriteLine("Ошибка проверки контрольной суммы EEPROM. Ошибка {0}: {1}",
                                        err, ltr212api.GetErrorString(err));
                }
                else
                {
                    /* --------------- задание параметров работы модуля ------------ */

                    /* один из 3-х режимов измерения */
                    hltr212.AcqMode = ltr212api.AcqModes.FourChannelsWithHighResolution;
                    /* пользовательская калибровка не используется */
                    hltr212.UseClb = false;
                    /* используется заводская калибровка */
                    hltr212.UseFabricClb = true;
                    /* сбор по 3-м каналам */
                    hltr212.LChQnt = 3;
                    /* используем опорное напряжение 5 В */
                    hltr212.REF = ltr212api.RefVals.REF_5V;
                    /* без знакопеременного измерения */
                    hltr212.AC = false;

                   
                    /* создание логических каналов */
                    for (uint ch = 0; ch < hltr212.LChQnt; ch++)
                        hltr212.SetLChannel(ch, ch + 1, ltr212api.Scales.B_80);


                    err = hltr212.SetADC();
                    if (err != _LTRNative.LTRERROR.OK)
                    {
                        Console.WriteLine("Не удалось установить настройки АЦП. Ошибка {0}: {1}",
                                        err, ltr212api.GetErrorString(err));     
                    }
                }

                if (err == _LTRNative.LTRERROR.OK)
                {
                    double fsBase, fs;
                    err = hltr212.CalcFS(out fsBase, out fs);
                    if (err != _LTRNative.LTRERROR.OK)
                    {
                        Console.WriteLine("Не удалось рассчитать установленные частоты. Ошибка {0}: {1}",
                                            err, ltr212api.GetErrorString(err));
                    }
                    else
                    {
                        Console.WriteLine("Установлены частоты: Частота АЦП = {0}, Частота данных = {1}", fsBase, fs);
                    }
                }

                if (err == _LTRNative.LTRERROR.OK)
                {
                    int recv_data_cnt = RECV_BLOCK_CH_SIZE * hltr212.LChQnt;

                    uint[] rbuf = new uint[recv_data_cnt];
                    double[] data = new double[recv_data_cnt];

                    uint tout = hltr212.CalcTimeOut(RECV_BLOCK_CH_SIZE);

                    /* запуск сбора данных */
                    err = hltr212.Start();
                    if (err != _LTRNative.LTRERROR.OK)
                    {
                        Console.WriteLine("Не удалось запустить сбор данных. Ошибка {0}: {1}",
                             err, ltr11api.GetErrorString(err));
                    }
                    else
                    {
                        _LTRNative.LTRERROR stop_err;
                        for (int i = 0; (i < RECV_BLOCK_CNT) && 
                                (err == _LTRNative.LTRERROR.OK); i++)
                        {
                            int rcv_cnt;
                            /* прием необработанных слов. есть варинант с tmark и без него для удобства */
                            rcv_cnt = hltr212.Recv(rbuf, (uint)rbuf.Length, tout);

                            /* значение меньше 0 => код ошибки */
                            if (rcv_cnt < 0)
                            {
                                err = (_LTRNative.LTRERROR)rcv_cnt;
                                Console.WriteLine("Ошибка приема данных. Ошибка {0}: {1}",
                                                  err, ltr212api.GetErrorString(err));
                            }
                            else if (rcv_cnt != rbuf.Length)
                            {
                                err = _LTRNative.LTRERROR.ERROR_RECV_INSUFFICIENT_DATA;
                                Console.WriteLine("Приняли недостаточно данных: запрашивали {0}, приняли {1}",
                                                  rbuf.Length, rcv_cnt);
                            }
                            else
                            {
                                err = hltr212.ProcessData(rbuf, data, ref rcv_cnt, true);
                                if (err != _LTRNative.LTRERROR.OK)
                                {
                                    Console.WriteLine("Ошибка обработки данных. Ошибка {0}: {1}",
                                                       err, ltr212api.GetErrorString(err));
                                }
                                else
                                {
                                    /* при успешной обработке для примера выводим по одному значению
                                     * для каждого канала */
                                    Console.Write("Блок {0}.", i+1);
                                    for (int ch=0; ch < hltr212.LChQnt; ch++)
                                    {                    
                                        /* если все ок - выводим значение (для примера только первое) */
                                        Console.Write(" {1}", ch + 1, data[ch].ToString("F7"));
                                        if (ch==(hltr212.LChQnt-1))
                                            Console.WriteLine("");
                                        else
                                            Console.Write(", ");
                                    }
                                }
                            }
                        }

                        /* останавливаем сбор данных */
                        stop_err = hltr212.Stop();
                        if (stop_err != _LTRNative.LTRERROR.OK)
                        {
                            Console.WriteLine("Не удалось остановить сбор данных. Ошибка {0}: {1}",
                                 err, ltr212api.GetErrorString(stop_err));
                            if (err == _LTRNative.LTRERROR.OK)
                                err = stop_err;
                        }                        
                    }
                }               

                hltr212.Close();
            }

        }
    }
}
