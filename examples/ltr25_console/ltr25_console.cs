using System;
using ltrModulesNet;

/* Данный пример демонстрирует работу с модулем LTR25 из программы на языке C#.
 * Пример представляет собой консольную программу, которая устанавливает связь с модулем,
 * выводит информацию о модуле, устанавливает настройки и собирает заданное кол-во 
 * блоков заданного размера, выводя на экран для примера значение пик-пик (max - min)
 * для каждого канала (т.к. LTR25 измеряет только переменную составляющую, то среднее 
 * всегда будет около нуля).
 * 
 * Необходимо установить номер слота, в котором вставлен модуль (константа SLOT).
 * Настройки сбора задаются в коде при конфигурации модуля.
 */

namespace ltr25_console
{
    class ltr25_console
    {
        /* количество отсчетов на канал, принмаемых за раз (блок) */
        const int RECV_BLOCK_CH_SIZE = 16*1024;
        /* количество блоков, после которого завершаем сбор */
        const int RECV_BLOCK_CNT = 20;
        /* Номер слота в крейте, где вставлен модуль */
        const int SLOT = 2;

        static int Main(string[] args)
        {          
            /* LTR25_Init() вызывается уже в конструкторе */
            ltr25api hltr25 = new ltr25api();
            /* отрываем модуль. есть вариант как с только со слотам, так и с серийным крейта и слотом 
             *  + полный */
            _LTRNative.LTRERROR err = hltr25.Open(SLOT);
            if (err != _LTRNative.LTRERROR.OK)
            {
                Console.WriteLine("Не удалось открыть модуль. Ошибка {0}: {1}",
                    err, ltr25api.GetErrorString(err));
            }
            else
            {
                /* выводим информацию из hltr25.ModuleInfo */
                Console.WriteLine("Модуль открыт успешно. Информация о модуле: ");
                Console.WriteLine("  Название модуля  = {0}", hltr25.ModuleInfo.Name);
                Console.WriteLine("  Серийный номер   = {0}", hltr25.ModuleInfo.Serial);
                Console.WriteLine("  Версия FPGA      = {0}", hltr25.ModuleInfo.VerFPGA);
                Console.WriteLine("  Версия PLD       = {0}", hltr25.ModuleInfo.VerPLD);
                Console.WriteLine("  Ревизия платы    = {0}", hltr25.ModuleInfo.BoardRev);
                Console.WriteLine("  Темп. диапазон   = {0}", hltr25.ModuleInfo.Industrial ? "Индустриальный" : "Коммерческий");


                ltr25api.CONFIG cfg = hltr25.Cfg;
                
                /* настраиваем модуль с помощью свойств */
                /* формат - 32 или 20 битный. В первом случае 2 слова на отсчет */
                cfg.DataFmt = ltr25api.DataFormat.Format32;
                /* устанавливаем частоту с помощью одной из констант */
                cfg.FreqCode = ltr25api.FreqCode.Freq_9K7;

                
                cfg.Ch[0].Enabled = true;
                cfg.Ch[1].Enabled = true;
                cfg.Ch[2].Enabled = true;
  
                hltr25.Cfg = cfg;

                err = hltr25.SetADC();
                if (err != _LTRNative.LTRERROR.OK)
                {
                    Console.WriteLine("Не удалось сконфигурировать модуль. Ошибка {0}: {1}",
                            err, ltr25api.GetErrorString(err));
                }
                else
                {
                    /* после SetADC() обновляется поле AdcFreq и EnabledChCnt. Становится равной действительной
                     * установленной частоте */
                    Console.WriteLine("Модуль настроен успешно. Установленная частота {0}, каналов {1}",
                            hltr25.State.AdcFreq.ToString("F7"), hltr25.State.EnabledChCnt);
                }

                if (err == _LTRNative.LTRERROR.OK)
                {
                    err = hltr25.Start();
                    if (err != _LTRNative.LTRERROR.OK)
                    {
                        Console.WriteLine("Не удалось запустить сбор данных. Ошибка {0}: {1}",
                            err, ltr25api.GetErrorString(err));
                    }
                }

                if (err == _LTRNative.LTRERROR.OK)
                {
                    _LTRNative.LTRERROR stop_err;
                    int recv_data_cnt = RECV_BLOCK_CH_SIZE * hltr25.State.EnabledChCnt;
                    /* В 20-битном формате каждому отсчету соответствует одно слово от модуля,
                       а в остальных - два */
                    int recv_wrd_cnt = recv_data_cnt * (hltr25.Cfg.DataFmt == ltr25api.DataFormat.Format20 ? 1 : 2);

                    uint[] rbuf = new uint[recv_wrd_cnt];
                    /* метки приходят на кждое слово, а не на отсчет */
                    uint[] marks = new uint[recv_wrd_cnt];
                    double[] data = new double[recv_data_cnt];
                    /* признаки обрыва/кз - по одному на каждый канал */
                    ltr25api.ChStatus[] ch_status = new ltr25api.ChStatus[hltr25.State.EnabledChCnt];

                    /* принмаем RECV_BLOCK_CNT блоков данных, после чего выходим */
                    for (int block_idx = 0; (block_idx < RECV_BLOCK_CNT) &&
                        (err == _LTRNative.LTRERROR.OK); block_idx++)
                    {
                        int rcv_cnt;
                        /* прием необработанных слов. в таймауте учитываем время выполнения самого преобразования */
                        rcv_cnt = hltr25.Recv(rbuf, marks, (uint)rbuf.Length, 
                                               4000 + (uint)(1000*RECV_BLOCK_CH_SIZE/hltr25.State.AdcFreq + 1));

                        /* значение меньше 0 => код ошибки */
                        if (rcv_cnt < 0)
                        {
                            err = (_LTRNative.LTRERROR)rcv_cnt;
                            Console.WriteLine("Ошибка приема данных. Ошибка {0}: {1}",
                                err, ltr25api.GetErrorString(err));
                        }
                        else if (rcv_cnt != rbuf.Length)
                        {
                            err = _LTRNative.LTRERROR.ERROR_RECV_INSUFFICIENT_DATA;
                            Console.WriteLine("Приняли недостаточно данных: запрашивали {0}, приняли {1}",
                                              rbuf.Length, rcv_cnt);
                        }
                        else
                        {
                            err = hltr25.ProcessData(rbuf, data, ref rcv_cnt, 
                                                    ltr25api.ProcFlags.Volt |
                                                    ltr25api.ProcFlags.PhaseCor |
                                                    ltr25api.ProcFlags.SignCor,
                                                    ch_status);

                            if (err != _LTRNative.LTRERROR.OK)
                            {
                                Console.WriteLine("Ошибка обработки данных. Ошибка {0}: {1}",
                                                   err, ltr25api.GetErrorString(err));
                            }
                            else
                            {
                                /* при успешной обработке для примера выводим по одному значению
                                   для каждого канала и показания сек. метки и старт. метки первого отсчета */
                                Console.Write("Блок {0}. ", block_idx + 1);
                                for (int ch=0; ch < hltr25.State.EnabledChCnt; ch++)
                                {
                                    /* проверяем статус канала - не обнаружен ли обрыв или кз */
                                    if (ch_status[ch] == ltr25api.ChStatus.OPEN)
                                    {
                                        Console.Write("обрыв       ");
                                    }
                                    else if (ch_status[ch] == ltr25api.ChStatus.SHORT)
                                    {
                                        Console.Write("кз          ");
                                    }
                                    else if (ch_status[ch] == ltr25api.ChStatus.OK)
                                    {
                                        double min_val, max_val, pp;
                                        /* рассчитываем разницу минимума и максимума для
                                         * примера для отображения (постоянная составляющая
                                         * не имеет смысла для LTR25) */
                                        min_val = max_val = data[ch];
                                        for (uint i = 1; i < rcv_cnt / hltr25.State.EnabledChCnt; i++)
                                        {
                                            double val = data[i * hltr25.State.EnabledChCnt + ch];
                                            if (min_val > val)
                                                min_val = val;
                                            if (max_val < val)
                                                max_val = val;
                                        }
                                        pp = (max_val - min_val);                                        

                                        /* если все ок - выводим значение*/
                                        Console.Write("{0}   ", pp.ToString("F7"));
                                    }
                                    else
                                    {                                        
                                        Console.Write("незвест. сост.!");
                                    }
                                }
                                Console.WriteLine(" start = {0}, sec = {1}", (marks[0] >> 16) & 0xFFFF, marks[0] & 0xFFFF);
                            }
                        }
                    }

                    /* остановка сбора данных */
                    stop_err = hltr25.Stop();
                    if (stop_err != _LTRNative.LTRERROR.OK)
                    {
                        Console.WriteLine("Не удалось остановить сбор данных. Ошибка {0}: {1}",
                            stop_err, ltr25api.GetErrorString(stop_err));
                        if (err == _LTRNative.LTRERROR.OK)
                            err = stop_err;
                    }
                }

                hltr25.Close();
            }

            return (int)err;
        }
    }
}
