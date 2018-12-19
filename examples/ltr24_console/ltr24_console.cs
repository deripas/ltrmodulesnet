using System;
using ltrModulesNet;

/* Данный пример демонстрирует работу с модулем LTR24 из программы на языке C#.
 * Пример представляет собой консольную программу, которая устанавливает связь с модулем,
 * выводит информацию о модуле, устанавливает настройки и собирает заданное кол-во 
 * блоков заданного размера, выводя на экран только по первому значению каждого для канала.
 * 
 * Необходимо установить номер слота, в котором вставлен модуль (константа SLOT).
 * Настройки сбора задаются в коде при конфигурации модуля.
 */

namespace ltr24_console
{
    class ltr24_console
    {
        
        /* количество отсчетов на канал, принмаемых за раз (блок) */
        const int RECV_BLOCK_CH_SIZE = 1024;
        /* количество блоков, после которого завершаем сбор */
        const int RECV_BLOCK_CNT = 100;
        /* Номер слота в крейте, где вставлен модуль */
        const int SLOT = 4;

        static int Main(string[] args)
        {          
            /* LTR24_Init() вызывается уже в конструкторе */
            ltr24api hltr24 = new ltr24api();
            /* отрываем модуль. есть вариант как с только со слотам, так и с серийным крейта и слотом 
             *  + полный */
            _LTRNative.LTRERROR err = hltr24.Open(SLOT);

            //_LTRNative.LTRERROR err = hltr24.Open("1R815094", 6);
            if (err != _LTRNative.LTRERROR.OK)
            {
                Console.WriteLine("Не удалось открыть модуль. Ошибка {0}: {1}",
                    err, ltr24api.GetErrorString(err));
            }
            else
            {
                int ch_cnt = 0;

                err = hltr24.GetConfig();
                if (err != _LTRNative.LTRERROR.OK)
                {
                    Console.WriteLine("Не удалось прочитать конфигурацию модуля. Ошибка {0}: {1}",
                        err, ltr24api.GetErrorString(err));
                }
                else
                {
                    /* выводим информацию из hltr24.ModuleInfo */
                    Console.WriteLine("Модуль открыт успешно. Информация о модуле: ");
                    Console.WriteLine("  Название модуля  = {0}", hltr24.ModuleInfo.Name);
                    Console.WriteLine("  Серийный номер   = {0}", hltr24.ModuleInfo.Serial);
                    Console.WriteLine("  Версия PLD       = {0}", hltr24.ModuleInfo.VerPLD);
                    Console.WriteLine("  Поддержка ICP    = {0}", hltr24.ModuleInfo.SupportICP);


                    /* настраиваем модуль с помощью свойств */
                    /* формат - 24 или 20 битный. В первом случае 2 слова на отсчет */
                    hltr24.DataFmt = ltr24api.DataFormat.Format24;
                    /* устанавливаем частоту с помощью одной из констант (Для 24-битного режима
                       макс. частота только при 2-х каналах, все 4 - только пр 58)  */
                    hltr24.AdcFreqCode = ltr24api.FreqCode.Freq_117K;
                    hltr24.TestMode = false;

                    /* каналы можем настраивать целиком через присвоение нового объекта _ltr24api.CHANNEL_MODE */
                    hltr24.ChannelMode[0] = new ltr24api.CHANNEL_MODE(true, ltr24api.AdcRange.Range_2, true, false);
                    /* или устанавилвать в ручную поля */
                    hltr24.ChannelMode[1].Range = ltr24api.AdcRange.Range_10;
                    hltr24.ChannelMode[1].AC = false;
                    hltr24.ChannelMode[1].Enable = true;

                    hltr24.ChannelMode[2].Enable = false;
                    hltr24.ChannelMode[3].Enable = false;

                    err = hltr24.SetADC();
                    if (err != _LTRNative.LTRERROR.OK)
                    {
                        Console.WriteLine("Не удалось сконфигурировать модуль. Ошибка {0}: {1}",
                                err, ltr24api.GetErrorString(err));
                    }
                    else
                    {
                        /* подсчитываем кол-во разрешенных каналов */
                        for (int i = 0; i < ltr24api.LTR24_CHANNEL_NUM; i++)
                        {
                            if (hltr24.ChannelMode[i].Enable)
                                ch_cnt++;
                        }

                        /* после SetADC() обновляется поле AdcFreq. Становится равной действительной
                         * установленной частоте */
                        Console.WriteLine("Модуль настроен успешно. Установленная частота {0}, каналов {1}",
                                hltr24.AdcFreq.ToString("F7"), ch_cnt);
                    }
                }

                if (err == _LTRNative.LTRERROR.OK)
                {
                    err = hltr24.Start();
                    if (err != _LTRNative.LTRERROR.OK)
                    {
                        Console.WriteLine("Не могу запустить сбор данных. Ошибка {0}: {1}",
                            err, ltr24api.GetErrorString(err));
                    }
                }

                if (err == _LTRNative.LTRERROR.OK)
                {
                    _LTRNative.LTRERROR stop_err;
                    int recv_data_cnt = RECV_BLOCK_CH_SIZE * ch_cnt;
                    /* В 24-битном формате каждому отсчету соответствует два слова от модуля,
                       а в 20-битном - одно */
                    int recv_wrd_cnt = recv_data_cnt * (hltr24.DataFmt == ltr24api.DataFormat.Format24 ? 2 : 1);

                    uint[] rbuf = new uint[recv_wrd_cnt];
                    /* метки приходят на кждое слово, а не на отсчет */
                    uint[] marks = new uint[recv_wrd_cnt];
                    double[] data = new double[recv_data_cnt];
                    /* признаки перегрузки на каждый отсчет */
                    bool[] ovrlds = new bool[recv_data_cnt];

                    /* принмаем RECV_BLOCK_CNT блоков данных, после чего выходим */
                    for (int i = 0; (i < RECV_BLOCK_CNT) && 
                        (err == _LTRNative.LTRERROR.OK); i++)
                    {
                        int rcv_cnt;
                        /* прием необработанных слов. в таймауте учитываем время выполнения самого преобразования */
                        rcv_cnt = hltr24.Recv(rbuf, marks, (uint)rbuf.Length, 
                                4000 + (uint)(1000*RECV_BLOCK_CH_SIZE/hltr24.AdcFreq + 1));

                        /* значение меньше 0 => код ошибки */
                        if (rcv_cnt < 0)
                        {
                            err = (_LTRNative.LTRERROR)rcv_cnt;
                            Console.WriteLine("Ошибка приема данных. Ошибка {0}: {1}",
                                err, ltr24api.GetErrorString(err));
                        }
                        else if (rcv_cnt != rbuf.Length)
                        {
                            err = _LTRNative.LTRERROR.ERROR_RECV_INSUFFICIENT_DATA;
                            Console.WriteLine("Приняли недостаточно данных: запрашивали {0}, приняли {1}",
                                              rbuf.Length, rcv_cnt);
                        }
                        else
                        {
                            err = hltr24.ProcessData(rbuf, data, ref rcv_cnt, 
                                                    ltr24api.ProcFlags.Volt |
                                                    ltr24api.ProcFlags.Calibr |
                                                    ltr24api.ProcFlags.AfcCorEx |
                                                    ltr24api.ProcFlags.ICP_PhaseCor, 
                                                    ovrlds);

                            if (err != _LTRNative.LTRERROR.OK)
                            {
                                Console.WriteLine("Ошибка обработки данных. Ошибка {0}: {1}",
                                                   err, ltr24api.GetErrorString(err));
                            }
                            else
                            {
                                /* при успешной обработке выводим для примера первые два значения (соответствующие первым
                                 * значениям каждого канала) и показания сек. метки и старт. метки первого отсчета */
                                Console.Write("Блок {0}.", i+1);
                                for (int ch=0, cur_pos=0; ch < ltr24api.LTR24_CHANNEL_NUM; ch++)
                                {
                                    if (hltr24.ChannelMode[ch].Enable)
                                    {
                                        Console.Write(" Канал {0} = {1}", ch+1, data[cur_pos].ToString("F7"));
                                        cur_pos++;
                                    }                                    
                                }
                                Console.WriteLine(" start = {0}, sec = {1}", (marks[0] >> 16) & 0xFFFF, marks[0] & 0xFFFF);
                            }
                        }
                    }

                    /* остановка сбора данных */
                    stop_err = hltr24.Stop();
                    if (stop_err != _LTRNative.LTRERROR.OK)
                    {
                        Console.WriteLine("Не удалось остановить сбор данных. Ошибка {0}: {1}",
                            stop_err, ltr24api.GetErrorString(stop_err));
                        if (err == _LTRNative.LTRERROR.OK)
                            err = stop_err;
                    }
                }

                hltr24.Close();
            }

            return (int)err;
        }
    }
}
