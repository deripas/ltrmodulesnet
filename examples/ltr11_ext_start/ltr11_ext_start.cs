﻿using System;
using System.Collections.Generic;
using ltrModulesNet;


/* Данный пример демонстрирует работу с модулем LTR11 из программы на языке C# при запуске
 * сбора от внешнего старта.
 *  
 * Пример представляет собой консольную программу, которая устанавливает связь с модулем,
 * выводит информацию о модуле, устанавливает настройки и запускает сбор от внешенего сигнала START.
 * Пример сперва ждет первого слова, которое может прийти в произвольный момент, а затем
 * дочитывает первый блок, после чего уже обрабатывает все блоки аналогично случаю работы от внутреннего запуска,
 * выводя на экран только по первому значению каждого для канала.
 * Ожидание первого слова и сбор идет до нажатия любой кнопки клавиатуры.
 * 
 * Необходимо установить номер слота, в котором вставлен модуль (константа SLOT).
 * Настройки сбора задаются в коде при конфигурации модуля.
 */


namespace ltr11_ext_start
{
    class ltr11_ext_start
    {
         /* Номер слота в крейте, где вставлен модуль */
        const int SLOT = 4;
        /* Количество отсчетов на канал, принмаемых за раз */
        const int RECV_BLOCK_CH_SIZE = 4096 * 8;
        /* Количество блоков, которые нужно принять и выйти */
        const int RECV_BLOCK_CNT = 50;
        /* Таймаут на ожидание данных при приеме (без учета времени преобразования) */
        const int RECV_TOUT = 4000;


        static int Main(string[] args)
        {          
            /* LTR11_Init() вызывается уже в конструкторе */
            ltr11api hltr11 = new ltr11api();
            /* отрываем модуль. Используем упрощенный вариант функции с указанием только слота.
             * (есть вариант как с только со слотом, так и с серийным крейта и слотом 
             *  + полный) */
            _LTRNative.LTRERROR err = hltr11.Open(SLOT);
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
                    hltr11.StartADCMode = ltr11api.StartAdcModes.EXTRISE;
                    /* режим синхронизации АПЦ - внутренний */
                    hltr11.InpMode = ltr11api.InpModes.INT;
                    /* количество логических каналов - 4 */
                    hltr11.LChQnt = 4;
                    /* таблица управления логическими каналами. Для упращения сделан
                     * метод установки лог. канала,  который принимает номер логического канала и его параметры*/
                    /* диапазон - 10В, режим - дифференциальный, физический канал - 1 */
                    hltr11.SetLChannel(0, 0, ltr11api.ChModes.DIFF, ltr11api.ChRanges.Range_10000MV);
                    /* диапазон - 2.5В, режим - измерение собственного нуля, физический канал - 2 */
                    hltr11.SetLChannel(1, 1, ltr11api.ChModes.ZERO, ltr11api.ChRanges.Range_2500MV);
                    /* диапазон - 0.6В, режим - с общей землей, физический канал - 3 */
                    hltr11.SetLChannel(2, 2, ltr11api.ChModes.COMM, ltr11api.ChRanges.Range_625MV);
                    /* диапазон - 0.156В, режим - с общей землей, физический канал - 25 */
                    hltr11.SetLChannel(3, 24, ltr11api.ChModes.COMM, ltr11api.ChRanges.Range_156MV);
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
                    int recv_data_cnt = RECV_BLOCK_CH_SIZE * hltr11.LChQnt;
                   

                    /* запуск сбора данных */
                    err = hltr11.Start();
                    if (err != _LTRNative.LTRERROR.OK)
                    {
                        Console.WriteLine("Не удалось запустить сбор данных. Ошибка {0}: {1}",
                             err, ltr11api.GetErrorString(err));
                    }
                    else
                    {
                        uint[] first_wrd = new uint[1];
                        uint[] rbuf = new uint[recv_data_cnt];
                        double[] data = new double[recv_data_cnt];
                        _LTRNative.LTRERROR stop_err;
                        /* признак того, что приняли первое слово.
                         * он же в дальнейшем используется как признак, 
                         * что сейчас обрабатывается первый блок */ 
                        bool first_valid = false;
                        /* признак запроса на завершение работы (устанавливаем по нажатию любой клавиши) */
                        bool out_request = false;
                        uint block_cnt = 0;

                        Console.WriteLine("Запущен сбор данных от внешнего сигнала запуска.");
                        Console.WriteLine("Ожидается приход первых данных от модуля. Для выхода нажмите любую клавишу");
                        /* Реально сбор данных начнется только по фронту сигнала на входе START.
                         * Единственным признаком, что сигнал пришел и модуль начал сбирать данные,
                         * является собственно наличие данных на прием (начало поступления данных). 
                         * Время прихода первого блока при внешенм старте мы не можем знать, более того,
                         * независимо от использованного таймаута, мы не можем быть уверены, что примем
                         * первый блок целиком (так как начало приема блока может произойти почти на 
                         * истечении таймаута, указанного в Recv()), т.е. его в любом случае может
                         * понадобится принимать за два раза.
                         * В примере мы ждем прихода одного первого слова, чтобы убедиться, что сбор 
                         * начался. До реального начала прихода данных Recv() будет возвращать 0.
                         * После приема первого слова надо будет принять остаток блока и соединить с первым словом */
                        while (!out_request && !first_valid && (err == _LTRNative.LTRERROR.OK))
                        {
                            int rcvd_cnt = hltr11.Recv(first_wrd, 1, 100);
                            if (rcvd_cnt < 0)
                            {
                                err = (_LTRNative.LTRERROR)rcvd_cnt;
                                Console.WriteLine("Ошибка приема данных. Ошибка {0}: {1}",
                                                  err, ltr11api.GetErrorString(err));
                            }
                            else if (rcvd_cnt != 0)
                            {                               
                                /* приняли первое слово */
                                first_valid = true;
                                Console.WriteLine("Принято первого слово!");
                            }
                            else if (Console.KeyAvailable)
                            {
                                /* прерываем ожидание по произвольной клавише */
                                out_request = true;
                            }
                        }

                        while (!out_request && (err == _LTRNative.LTRERROR.OK))
                        {
                            int rcv_cnt;
                            /* в таймауте учитываем время выполнения самого преобразования*/
                            uint tout = RECV_TOUT + (uint)(RECV_BLOCK_CH_SIZE / hltr11.ChRate + 1);
                            /* если принимаем остаток первого блока, то 1-ое слово мы уже приняли => размер на 1 меньше */
                            int rcv_request = first_valid ? rbuf.Length-1 : rbuf.Length;
                            /* прием необработанных слов. есть варинант с tmark и без него для удобства */
                            rcv_cnt = hltr11.Recv(rbuf, (uint)rcv_request, tout);

                            /* значение меньше 0 => код ошибки */
                            if (rcv_cnt < 0)
                            {
                                err = (_LTRNative.LTRERROR)rcv_cnt;
                                Console.WriteLine("Ошибка приема данных. Ошибка {0}: {1}",
                                                  err, ltr11api.GetErrorString(err));
                            }
                            else if (rcv_cnt != rcv_request)
                            {
                                err = _LTRNative.LTRERROR.ERROR_RECV_INSUFFICIENT_DATA;
                                Console.WriteLine("Приняли недостаточно данных: запрашивали {0}, приняли {1}",
                                                  rbuf.Length, rcv_cnt);
                            }
                            else
                            {
                                /* для первого блока объединяем принятый массив с первым словом -
                                 * сдвигаем его на 1 элемент вправо и на место первого элемента
                                 * записываем ранее принятое слово */
                                if (first_valid)
                                {
                                    Array.Copy(rbuf, 0, rbuf, 1, rcv_request);
                                    rbuf[0] = first_wrd[0];
                                    /* т.к. first_valid мы также используем для отличия
                                     * дочитывания первого блока, от остальных, то 
                                     * сбрасываем его тут, чтобы выполнять
                                     * прием остальных блоков обычным образом */
                                    first_valid = false;
                                }

                                err = hltr11.ProcessData(rbuf, data, ref rcv_cnt, true, true);
                                if (err != _LTRNative.LTRERROR.OK)
                                {
                                    Console.WriteLine("Ошибка обработки данных. Ошибка {0}: {1}",
                                                       err, ltr11api.GetErrorString(err));
                                }
                                else
                                {
                                    /* при успешной обработке для примера выводим по одному значению
                                     * для каждого канала */
                                    Console.Write("Блок {0}.", block_cnt+1);
                                    for (int ch=0; ch < hltr11.LChQnt; ch++)
                                    {                    
                                        /* если все ок - выводим значение (для примера только первое) */
                                        Console.Write(" {1}", ch + 1, data[ch].ToString("F7"));
                                        if (ch==(hltr11.LChQnt-1))
                                            Console.WriteLine("");
                                        else
                                            Console.Write(", ");
                                    }
                                    block_cnt++;
                                }
                            }

                            if (Console.KeyAvailable)
                            {
                                /* прерываем ожидание по произвольной клавише */
                                out_request = true;
                            }
                        }

                        /* останавливаем сбор данных */
                        stop_err = hltr11.Stop();
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
