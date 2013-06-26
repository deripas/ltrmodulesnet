using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ltrModulesNet;

/* Все статические функции и внутренняя структура оставлены для доступа напрямую.
 * Есть также доступ через методы/свойства, как указано в данном примере, не 
 * отклоняясь от старой концепции... */

namespace ltr24_console
{
    class Program
    {
        const int READ_BLOCKS = 100;
        const int READ_BLOCK_SIZE = 10240;

        static int Main(string[] args)
        {          
            /* LTR24_Init() вызывается уже в конструкторе */
            _ltr24api hltr24 = new _ltr24api();
            /* отрываем модуль. есть вариант как с только со слотам, так и с серийным крейта и слотом 
             *  + полный */
            _LTRNative.LTRERROR err = hltr24.Open(7);
            //_LTRNative.LTRERROR err = hltr24.Open("1R815094", 6);
            if (err != _LTRNative.LTRERROR.OK)
            {
                Console.WriteLine("Не удалось открыть модуль. Ошибка {0}: {1}",
                    err, _ltr24api.GetErrorString(err));
            }
            else
            {
                int ch_cnt = 0;
                /* выводим информацию из hltr24.ModuleInfo */ 
                Console.WriteLine("Модуль открыт успешно. Название = {0}, серийный = {1}, версия PLD = {2}",
                    hltr24.ModuleInfo.Name, hltr24.ModuleInfo.Serial, hltr24.ModuleInfo.VerPLD);


                /* настраиваем модуль с помощью свойств */

                /* формат - 24 или 20 битный. В первом случае 2 слова на отсчет */
                hltr24.DataFmt = _ltr24api.DataFormat.Format24;
                /* устанавливаем частоту с помощью одной из констант (Для 24-битного режима
                   макс. частота только при 2-х каналах, все 4 - только пр 58)  */
                hltr24.AdcFreqCode = _ltr24api.FreqCode.Freq_117K;
                hltr24.ZeroMode = false;

                /* каналы можем настраивать челиком через присвоение нового объекта _ltr24api.Channel */
                hltr24.ChannelMode[0] = new _ltr24api.Channel(true, _ltr24api.AdcRange.Range_2, true);
                /* или устанавилвать в ручную поля */
                hltr24.ChannelMode[1].Range = _ltr24api.AdcRange.Range_10;
                hltr24.ChannelMode[1].AC = false;
                hltr24.ChannelMode[1].Enable = true;

                hltr24.ChannelMode[2].Enable = false;
                hltr24.ChannelMode[3].Enable = false;

                err = hltr24.SetADC();
                if (err != _LTRNative.LTRERROR.OK)
                {
                    Console.WriteLine("Не удалось сконфигурировать модуль. Ошибка {0}: {1}",
                            err, _ltr24api.GetErrorString(err));
                }
                else
                {
                    /* подсчитываем кол-во разрешенных каналов */ 
                    for (int i = 0; i < _ltr24api.LTR24_CHANNEL_NUM; i++)
                    {
                        if (hltr24.ChannelMode[i].Enable)
                            ch_cnt++;
                    }

                    /* после SetADC() обновляется поле AdcFreq. Становится равной действительной
                     * установленной частоте */
                    Console.WriteLine("Модуль настроен успешно. Установленная частота {0}, каналов {1}",
                            hltr24.AdcFreq.ToString("F7"), ch_cnt);
                }




                if (err == _LTRNative.LTRERROR.OK)
                {
                    err = hltr24.Start();
                    if (err != _LTRNative.LTRERROR.OK)
                    {
                        Console.WriteLine("Не могу запустить сбор данных. Ошибка {0}: {1}",
                            err, _ltr24api.GetErrorString(err));
                    }
                }

                if (err == _LTRNative.LTRERROR.OK)
                {
                    _LTRNative.LTRERROR stop_err;
                    /* при 24 битном формате по 2 слова на отсчет */
                    uint[] wrds = new uint[READ_BLOCK_SIZE * 2];
                    /* метки приходят на кждое слово, а не на отсчет */
                    uint[] marks = new uint[READ_BLOCK_SIZE * 2];
                    double[] data = new double[READ_BLOCK_SIZE];
                    /* признаки перегрузки на каждый отсчет */
                    bool[] ovrlds = new bool[READ_BLOCK_SIZE];

                    for (int i = 0; (i < READ_BLOCKS) && 
                        (err == _LTRNative.LTRERROR.OK); i++)
                    {
                        int rcv_cnt;
                        /* прием необработанных слов */
                        rcv_cnt = hltr24.Recv(wrds, marks, (uint)wrds.Length, 
                                1000 + (uint)(hltr24.AdcFreq * READ_BLOCK_SIZE/(ch_cnt*1000)));

                        /* значение меньше 0 => код ошибки */
                        if (rcv_cnt < 0)
                        {
                            err = (_LTRNative.LTRERROR)rcv_cnt;
                            Console.WriteLine("Ошибка приема данных. Ошибка {0}: {1}",
                                err, _ltr24api.GetErrorString(err));
                        }
                        else if (rcv_cnt != wrds.Length)
                        {
                            err = _LTRNative.LTRERROR.ERROR_RECV;
                            Console.WriteLine("Приняли недостаточно данных: запрашивали {0}, приняли {1}",
                                              wrds.Length, rcv_cnt);
                        }
                        else
                        {
                            err = hltr24.ProcessData(wrds, data, ref rcv_cnt, true, true, ovrlds);
                            if (err != _LTRNative.LTRERROR.OK)
                            {
                                Console.WriteLine("Приняли недостаточно данных: запрашивали {0}, приняли {1}",
                                              wrds.Length, rcv_cnt);
                            }
                            else
                            {
                                /* при успешной обработке выводим для примера первые два значения (соответствующие первым
                                 * значениям каждого канала) и показания сек. метки и старт. метки первого отсчета */
                                Console.WriteLine("Блок {0}. adc[0] = {1}, adc[1] = {2}, start = {3}, sec = {4}",
                                        i, data[0].ToString("F7"), data[1].ToString("F7"), 
                                        (marks[0] >> 16)&0xFFFF, marks[0]&0xFFFF);
                            }
                        }
                    }

                    /* остановка сбора данных */
                    stop_err = hltr24.Stop();
                    if (stop_err != _LTRNative.LTRERROR.OK)
                    {
                        Console.WriteLine("Не могу остановить сбор данных. Ошибка {0}: {1}",
                            err, _ltr24api.GetErrorString(stop_err));
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
