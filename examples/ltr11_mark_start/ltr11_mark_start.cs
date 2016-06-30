using System;
using System.Collections.Generic;
using ltrModulesNet;


/* Данный пример демонстрирует прием данных от модуля по внешнему сигналу
 * с использованием синхрометок крейта.
 * 
 * Пример решает задачу, когда нужно по каждому фронту (спаду) внешнего сигнала
 * принимать блок данных заданного размера. Пример демострирует это на примере
 * модуля LTR11, хотя аналогичным способом это может быть выполнененого и для
 * большинства других модулей LTR.
 * 
 * Сигнал, по которому выполняется прием блока данных должен быть заведен
 * на вход DIGIN1 на крейте LTR-EU/LTR-CEU/LTR-CU.
 * 
 * Выбор нужных блоков данных осуществляется программным образом. 
 * Сбор данных запускается сразу и идет постоянно. При этом идет
 * прием как слов от модуля, так и меток синхронизации от крейта.
 * Ищется момент изменения значения метки СТАРТ и все слова
 * до метки СТАРТ отбрасываются, выравниваются данные на начало кадра
 * и допринимается нужное кол-во данных, чтобы получить блок нужного размера.
 * 
 * Необходимо установить номер слота, в котором вставлен модуль (константа SLOT) и
 * размер блока (в отсчетах на канал) в RECV_BLOCK_CH_SIZE.
 * 
 * Настройки сбора задаются в коде при конфигурации модуля.
 */
namespace ltr11_mark_start
{
    class ltr11_mark_start
    {
         /* Номер слота в крейте, где вставлен модуль */
        const int SLOT = 6;
        /* Количество отсчетов на канал, принмаемых за раз */
        const int RECV_BLOCK_CH_SIZE = 4096 * 8;
        /* Количество блоков, которые нужно принять и выйти */
        const int RECV_BLOCK_CNT = 50;
        /* Таймаут на ожидание данных при приеме (без учета времени преобразования) */
        const int RECV_TOUT = 4000;
        
        

        static int Main(string[] args)
        {
            _LTRNative.LTRERROR err;
            /* Для настройки генерации метки старт по внешнему сигналу необходимо  
             * установить соединение с крейтом и изменить его настройки */
            ltrcrate crate = new ltrcrate();
            /* устанавливаем соединение. в случае нескольких
             * крейтов необходимо указать серийный номер крейта, с которым работаем,
             * иначе установка соединения с первым найденным */
            err = crate.Open();
            if (err != _LTRNative.LTRERROR.OK)
            {
                Console.WriteLine("Не удалось установить соединение с крейтом. Ошибка {0}: {1}",
                    err, ltrcrate.GetErrorString(err));
            }
            else
            {
                /* В примере настраиваем генерацию метки старт по фронту сигнала
                 * на входе разъема синхронизации крейта DIGIN1 */
                err = crate.MakeStartMark(_LTRNative.en_LTR_MarkMode.LTR_MARK_EXT_DIGIN1_RISE);
                if (err != _LTRNative.LTRERROR.OK)
                {
                    Console.WriteLine("Не удалось настроить генерацию метки СТАРТ. Ошибка {0}: {1}",
                        err, ltrcrate.GetErrorString(err));
                }
                else
                {
                    Console.WriteLine("Генерация метки старт была настроена успешно");
                }

                /* соезинение с крейтом больше не нужно */
                crate.Close();
            }
 
            /* LTR11_Init() вызывается уже в конструкторе */
            ltr11api hltr11 = new ltr11api();
            /* отрываем модуль. Используем упрощенный вариант функции с указанием только слота.
             * (есть вариант как с только со слотом, так и с серийным крейта и слотом 
             *  + полный) */
            err = hltr11.Open(SLOT);
            if (err != _LTRNative.LTRERROR.OK)
            {
                Console.WriteLine("Не удалось открыть модуль. Ошибка {0}: {1}",
                    err, ltr11api.GetErrorString(err));
            }
            else
            {
                /* получение информации о модуле из flash-памяти */
                err = hltr11.GetConfig();
                if (err != _LTRNative.LTRERROR.OK)
                {
                    Console.WriteLine("Не удалось прочитать информацию о модуле. Ошибка {0}: {1}",
                         err, ltr11api.GetErrorString(err));
                }
                else
                {
                    /* выводим информацию из hltr11.ModuleInfo */
                    Console.WriteLine("Информация о модуле: ");
                    Console.WriteLine("  Название модуля: {0}", hltr11.ModuleInfo.Name);
                    Console.WriteLine("  Серийный номер : {0}", hltr11.ModuleInfo.Serial);
                    Console.WriteLine("  Версия прошивки: {0}", hltr11.ModuleInfo.VerStr);

                    /* --------------- задание параметров работы модуля ------------ */


                    /* режим старта сбора данных - внутренний */
                    hltr11.StartADCMode = ltr11api.StartAdcModes.INT;
                    /* режим синхронизации АПЦ - внутренний */
                    hltr11.InpMode = ltr11api.InpModes.INT;
                    /* количество логических каналов - 4 */
                    hltr11.LChQnt = 3;
                    /* таблица управления логическими каналами. Для упращения сделан
                     * метод установки лог. канала,  который принимает номер логического канала и его параметры*/
                    /* диапазон - 10В, режим - дифференциальный, физический канал - 1 */
                    hltr11.SetLChannel(0, 0, ltr11api.ChModes.DIFF, ltr11api.ChRanges.Range_10000MV);
                    /* диапазон - 2.5В, режим - измерение собственного нуля, физический канал - 2 */
                    hltr11.SetLChannel(1, 1, ltr11api.ChModes.ZERO, ltr11api.ChRanges.Range_2500MV);
                    /* диапазон - 0.6В, режим - с общей землей, физический канал - 3 */
                    hltr11.SetLChannel(2, 2, ltr11api.ChModes.COMM, ltr11api.ChRanges.Range_625MV);
                    /* диапазон - 0.156В, режим - с общей землей, физический канал - 25 */
                    //hltr11.SetLChannel(3, 24, ltr11api.ChModes.COMM, ltr11api.ChRanges.Range_156MV);
                    /* режим сбора данных */
                    hltr11.ADCMode = ltr11api.AdcModes.ACQ;
                    /* частота дискретизации - 400 кГц. Данный метод сам устанавливает поля
                     * делителей в классе */
                    hltr11.FindAdcFreqParams(400000);

                    err = hltr11.SetADC();
                    if (err != _LTRNative.LTRERROR.OK)
                    {
                        Console.WriteLine("Не удалось установить настройки модуля. Ошибка {0}: {1}",
                             err, ltr11api.GetErrorString(err));
                    }
                }

                if (err == _LTRNative.LTRERROR.OK)
                {
                    /* запуск сбора данных */
                    err = hltr11.Start();
                    if (err != _LTRNative.LTRERROR.OK)
                    {
                        Console.WriteLine("Не удалось запустить сбор данных. Ошибка {0}: {1}",
                             err, ltr11api.GetErrorString(err));
                    }
                    else
                    {
                        int block_cnt = 0;
                        bool out_request = false;

                        const int TMP_BUF_SIZE = 4096;
                        uint[] tmp_buf = new uint[TMP_BUF_SIZE];
                        uint[] mark_buf = new uint[TMP_BUF_SIZE];

                        /* Сбор данных выполняем всегда, сохраняя данные во временный буфер,
                         * во входных данных ищем изменение значения метки старт и только
                         * отчеты после этого изменения обрабатываем */
                        while (!out_request && (err == _LTRNative.LTRERROR.OK))
                        {
                            bool start_mark_valid = false;
                            bool start_fnd = false;
                            ushort start_mark = 0; /* текущее значение метки СТАРТ */
                            uint rem_size = 0;                                                       

                            while (!start_fnd && !out_request && (err == _LTRNative.LTRERROR.OK))
                            {
                                /* в примере не обязательно принимать все TMP_BUF_SIZE, поэтому
                                 * можем использовать относительно небольшой таймаут.
                                 * Единственное, размер буфера не должен быть слишком маленький,
                                 * чтобы снизить нагрузку на ПК */
                                uint tout = 100;
                                /* прием необработанных слов. есть варинант с tmark и без него для удобства */
                                int rcv_cnt = hltr11.Recv(tmp_buf, mark_buf, (uint)tmp_buf.Length, tout);
                                if (rcv_cnt < 0)
                                {
                                    err = (_LTRNative.LTRERROR)rcv_cnt;
                                    Console.WriteLine("Ошибка приема данных. Ошибка {0}: {1}",
                                                      err, ltr11api.GetErrorString(err));
                                } 
                                else if (rcv_cnt > 0)
                                {
                                    /* по первому отсчету сохраняем значение метки СТАРТ. Надо иметь
                                     * ввиду, что старте сбора оно не обязательно = 0, т.к. кол-во
                                     * меток старт считается от момента подключения крейта
                                     * к ПК и не сбрасывается до его отключения */
                                    if (!start_mark_valid)
                                    {
                                        start_mark = (ushort)((mark_buf[0] >> 16) & 0xFFFF);
                                        start_mark_valid = true;
                                    }

                                    for (int i = 0; (i < rcv_cnt) && !start_fnd; i++)
                                    {
                                        /* ищем изменение метки СТАРТ */
                                        ushort cur_mark = (ushort)((mark_buf[i] >> 16) & 0xFFFF);
                                        if (cur_mark != start_mark)
                                        {
                                            start_mark = cur_mark;
                                            start_fnd = true;
                                            rem_size = (uint)(rcv_cnt - i);
                                                                  
                                            uint idx;
                                            /* если изменение метки произошло не вначале блока,
                                             * то откидываем все отсчеты, которые были до изменения
                                             * метки путем сдвига буфера */
                                            if (i != 0)
                                            {
                                                Array.Copy(tmp_buf, i, tmp_buf, 0, rem_size);
                                            }
                                            
                                            /* Следует учитывать, что если у нас используется не один канал,
                                             * то изменение метки может произойти в середине кадра, а не на 
                                             * границе. Для корректной обработки данных нам нужно выровнять
                                             * данные на границу кадра (чтобы первый отсчет соответствовал
                                             * первому каналу из логической таблицы). Функция SearchFirstFrame()
                                             * находит индекс начала ближайшего кадра в массиве */
                                            if (hltr11.SearchFirstFrame(tmp_buf, rem_size, out idx) == _LTRNative.LTRERROR.OK)
                                            {
                                                /* если индекс не равен нулю - то находимся в середине
                                                 * кадра и нужно отбросить остаток кадра -
                                                 * первые idx отсчетов */
                                                if (idx != 0)
                                                {
                                                    rem_size -= idx;
                                                    Array.Copy(tmp_buf, idx, tmp_buf, 0, rem_size);
                                                }
                                            }
                                            else
                                            {
                                                /* если не найдено начало кадра, то значит изменение 
                                                 * метки произошло в конце буфера. При корректной
                                                 * работе достаточно принять данных на один кадр
                                                 * и в нем обязательно должно быть начало */
                                                rem_size = (uint)hltr11.LChQnt;

                                                rcv_cnt = hltr11.Recv(tmp_buf, mark_buf, rem_size, RECV_TOUT);
                                                if (rcv_cnt < 0)
                                                {
                                                    err = (_LTRNative.LTRERROR)rcv_cnt;
                                                    Console.WriteLine("Ошибка приема данных. Ошибка {0}: {1}",
                                                                      err, ltr11api.GetErrorString(err));
                                                }
                                                else if (rcv_cnt != rem_size)
                                                {
                                                    err = _LTRNative.LTRERROR.ERROR_RECV_INSUFFICIENT_DATA;
                                                    Console.WriteLine("Приняли недостаточно данных: запрашивали {0}, приняли {1}",
                                                                      rem_size, rcv_cnt);
                                                }
                                                else
                                                {
                                                    err = hltr11.SearchFirstFrame(tmp_buf, rem_size, out idx);
                                                    if (err != _LTRNative.LTRERROR.OK)
                                                    {
                                                        Console.WriteLine("Не удалось найти начало кадра. Ошибка {0}: {1}",
                                                                      err, ltr11api.GetErrorString(err));
                                                    } else if (idx != 0) {
                                                        rem_size -= idx;
                                                        Array.Copy(tmp_buf, idx, tmp_buf, 0, rem_size);
                                                    }
                                                }
                                            }                                            
                                        }
                                    }
                                }
                                 

                                if (Console.KeyAvailable)
                                {
                                    /* прерываем ожидание по произвольной клавише */
                                    out_request = true;
                                    Console.WriteLine("Запрос завершения работы...");
                                }
                            }

                            /* если нашли изменения - допринимаем и обрабатываем блок */
                            if (start_fnd)
                            {                                
                                int block_data_cnt = (RECV_BLOCK_CH_SIZE * hltr11.LChQnt);

                                uint[] rbuf = new uint[block_data_cnt];
                                double[] data = new double[block_data_cnt];

                                /* rem_size полезных отсчетов находится во временном
                                 * буфере, определяем, сколько данных нужно еще допринять,
                                 * чтобы получить целый блок */
                                int block_recv_cnt = (int)(block_data_cnt - rem_size);

                                if (block_recv_cnt > 0)
                                {
                                    uint tout = RECV_TOUT + (uint)(block_recv_cnt / hltr11.ChRate + 1);

                                    int rcv_cnt = hltr11.Recv(rbuf, (uint)block_recv_cnt, tout);
                                    /* значение меньше 0 => код ошибки */
                                    if (rcv_cnt < 0)
                                    {
                                        err = (_LTRNative.LTRERROR)rcv_cnt;
                                        Console.WriteLine("Ошибка приема данных. Ошибка {0}: {1}",
                                                          err, ltr11api.GetErrorString(err));
                                    }
                                    else if (rcv_cnt != block_recv_cnt)
                                    {
                                        err = _LTRNative.LTRERROR.ERROR_RECV_INSUFFICIENT_DATA;
                                        Console.WriteLine("Приняли недостаточно данных: запрашивали {0}, приняли {1}",
                                                          block_recv_cnt, rcv_cnt);
                                    }
                                }
                                if (err == _LTRNative.LTRERROR.OK)
                                {
                                    /* склеиваем две части в один массив -
                                     * сдвигаем принятые данные на rem_size влево
                                     * и в освободившееся место вначале копируем
                                     * действительные данные из временного буфера */
                                    if (rem_size != 0)
                                    {
                                        if (block_recv_cnt > 0)
                                            Array.Copy(rbuf, 0, rbuf, rem_size, block_recv_cnt);
                                        Array.Copy(tmp_buf, rbuf, rem_size);
                                    }


                                    err = hltr11.ProcessData(rbuf, data, ref block_data_cnt, true, true);
                                    if (err != _LTRNative.LTRERROR.OK)
                                    {
                                        Console.WriteLine("Ошибка обработки данных. Ошибка {0}: {1}",
                                                           err, ltr11api.GetErrorString(err));
                                    }
                                    else
                                    {
                                        /* при успешной обработке для примера выводим по одному значению
                                         * для каждого канала */
                                        Console.Write("Блок {0} (СТАРТ = {1}): ", block_cnt + 1, start_mark);
                                        for (int ch = 0; ch < hltr11.LChQnt; ch++)
                                        {
                                            /* если все ок - выводим значение (для примера только первое) */
                                            Console.Write(" {1}", ch + 1, data[ch].ToString("F7"));
                                            if (ch == (hltr11.LChQnt - 1))
                                                Console.WriteLine("");
                                            else
                                                Console.Write(", ");
                                        }
                                        block_cnt++;
                                    }
                                }
                            }
                        }




                        /* останавливаем сбор данных */
                        _LTRNative.LTRERROR stop_err = hltr11.Stop();
                        if (stop_err != _LTRNative.LTRERROR.OK)
                        {
                            Console.WriteLine("Не удалось остановить сбор данных. Ошибка {0}: {1}",
                                 stop_err, ltr11api.GetErrorString(stop_err));
                            if (err == _LTRNative.LTRERROR.OK)
                                err = stop_err;
                        }                        
                    }
                }
            }

            /* закрываем соединение */
            if (hltr11.IsOpened() == _LTRNative.LTRERROR.OK)
                hltr11.Close();

            return (int)err;
        }
    }
}
