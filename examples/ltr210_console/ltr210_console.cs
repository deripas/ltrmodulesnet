using System;
using System.Text;

using ltrModulesNet;

/* Данный пример демонстрирует работы с модулем LTR210 из программы на языке C#.
 * Пример представляет собой консольную программу, которая устанавливает связь с модулем,
 * выводит информацию, устанавливает настройки и собирает заданное кол-во кадров
 * в режиме покадрового сбора, выводя на экран только первые 2 значения из каждого кадра.
 * 
 * Необходимо установить номер слота, в котором вставлен модуль (константа SLOT).
 * Настройки сбора задаются в коде при конфигурации модуля.
 */

namespace ltr210_console
{
    class ltr210_console
    {
        /* кол-во кадров для приема */
        const int READ_FRAMES = 20;
        /* таймаут на прием кадра в мс */
        const int RECV_TOUT = 10000;
        /* Если за данное время не придет ни одного слова от модуля, то считаем его неисаравным */
        const int KEEPALIVE_TOUT = 10000;
        /* Номер слота в крейте, где вставлен модуль */
        const int SLOT = 1;

        static void loadProgr(IntPtr cb_data, ref ltr210api.TLTR210 hnd, uint doneSize, uint fullSize)
        {
            Console.Write(".");            
        }



        static void Main(string[] args)
        {
            /* LTR210_Init() вызывается уже в конструкторе */
            ltr210api hltr210 = new ltr210api();
            /* Отрываем модуль. Есть вариант функции как с только с номером слота, 
             * так и с серийным крейта и слотом + полный */
            _LTRNative.LTRERROR err = hltr210.Open(SLOT);
            if (err != _LTRNative.LTRERROR.OK)
            {
                Console.WriteLine("Не удалось открыть модуль. Ошибка {0}: {1}",
                    err, ltr210api.GetErrorString(err));
            }
            else
            {
                /* после открытия модуля доступна информация, кроме версии прошивки ПЛИС */
                Console.WriteLine("Модуль открыт успешно!");
                Console.WriteLine("  Название модуля    = {0}", hltr210.ModuleInfo.Name);
                Console.WriteLine("  Серийный номер     = {0}", hltr210.ModuleInfo.Serial);
                Console.WriteLine("  Версия PLD         = {0}", hltr210.ModuleInfo.VerPLD);

                /* Проверяем, загружена ли прошивка. Если нет - загружаем */
                if (hltr210.FPGAIsLoaded() != _LTRNative.LTRERROR.OK)
                {
                    Console.WriteLine("Начало записи прошивки модуля");
                    /* Загружаем прошивку из dll, используем callback для отображения прогресса загрузки */
                    err = hltr210.LoadFPGA(loadProgr);
                    Console.WriteLine("");
                    if (err != _LTRNative.LTRERROR.OK)
                    {
                        Console.WriteLine("Не удалось загрузить прошивку ПЛИС. Ошибка {0}: {1}",
                                          err, ltr210api.GetErrorString(err));
                    }
                    else
                    {
                        Console.WriteLine("Прошивка ПЛИС загружена успешно");
                    }
                }

                if (err == _LTRNative.LTRERROR.OK)
                {
                    Console.WriteLine("Версия прошивки ПЛИС = {0}\n", hltr210.ModuleInfo.VerFPGA);
                }

                if (err == _LTRNative.LTRERROR.OK)
                {
                    ltr210api.CONFIG cfg = hltr210.Cfg;
                    /* настройки каналов АЦП */
                    cfg.Ch[0].Enabled = true;
                    cfg.Ch[0].Range = ltr210api.AdcRanges.Range_10;
                    cfg.Ch[0].Mode = ltr210api.ChModes.ACDC;
                    cfg.Ch[1].Enabled = true;
                    cfg.Ch[1].Range = ltr210api.AdcRanges.Range_0_5;
                    cfg.Ch[1].Mode = ltr210api.ChModes.AC;

                    cfg.SyncMode = ltr210api.SyncModes.PERIODIC;

                    /* Размер кадра */
                    
                    cfg.FrameSize = 10000;
                    /* Размер предыстории */
                    cfg.HistSize = 100;

                    /* Устанавливаем максимальную частоту отсчетов - 10 МГц */
                    cfg.AdcFreq = 10000000;
                    /* Частота следования кадров - раз в секунду */
                    cfg.FrameFreq = 1;
                    /* Разрешаем посылку сигнала жизни и автоматическую
                        приостановку записи */
                    cfg.Flags = ltr210api.CfgFlags.KEEPALIVE_EN | ltr210api.CfgFlags.WRITE_AUTO_SUSP;
                    hltr210.Cfg = cfg;

                    err = hltr210.SetADC();
                    if (err != _LTRNative.LTRERROR.OK)
                    {
                        Console.WriteLine("Не удалось установить настройки АЦП: Ошибка {0}: {1}",
                                          err, ltr210api.GetErrorString(err));
                    }

                    if (err == _LTRNative.LTRERROR.OK)
                    {
                        err = hltr210.Start();
                        if (err != _LTRNative.LTRERROR.OK)
                        {
                            Console.WriteLine("Не удалось запустить сбор данных! Ошибка {0}: {1}",
                                err, ltr210api.GetErrorString(err));
                        }
                    }

                    if (err == _LTRNative.LTRERROR.OK)
                    {
                        
                        uint[] wrds = new uint[hltr210.State.RecvFrameSize];
                        double[] data = new double[hltr210.State.RecvFrameSize];
                        ltr210api.DATA_INFO[] info = new ltr210api.DATA_INFO[hltr210.State.RecvFrameSize];
                        int frames_cnt = 0;

                        while ((frames_cnt < READ_FRAMES) && (err == _LTRNative.LTRERROR.OK))
                        {
                            ltr210api.RecvEvents evt;
                            int recvCnt;

                            err = hltr210.WaitEvent(out evt, 100);
                            if (err == _LTRNative.LTRERROR.OK)
                            {
                                switch (evt)
                                {
                                    case ltr210api.RecvEvents.SOF:
                                        /* Пришел новый кадр. Для простоты принимаем его за один Recv, однако при желании можно
                                         * разбить прием на несколько блоков.
                                         * Примечание: в С# есть две версии функции Recv() - с метками и без */
                                        recvCnt = hltr210.Recv(wrds, hltr210.State.RecvFrameSize, RECV_TOUT);
                                        if (recvCnt < 0)
                                        {
                                            err = (_LTRNative.LTRERROR)recvCnt;
                                            Console.WriteLine("Ошибка при приеме кадра: Ошибка {0}: {1}",
                                                    err, ltr210api.GetErrorString(err));

                                        }
                                        else if (recvCnt < hltr210.State.RecvFrameSize)
                                        {
                                            Console.WriteLine("Принято меньше слов, чем было в кадре! запрашивали {0}, приняли {1}\n",
                                                                hltr210.State.RecvFrameSize, recvCnt);
                                            err = _LTRNative.LTRERROR.ERROR_RECV_INSUFFICIENT_DATA;
                                        }
                                        else
                                        {
                                            ltr210api.FRAME_STATUS frame_st;
                                            /* переводим данные в Вольты */
                                            err = hltr210.ProcessData(wrds, data, ref recvCnt,
                                                                      ltr210api.ProcFlags.VOLT,
                                                                      out frame_st, info);
                                            if (err != _LTRNative.LTRERROR.OK)
                                            {
                                                Console.WriteLine("Ошибка обработки данных! Ошибка {0}: {1}",
                                                                   err, ltr210api.GetErrorString(err));
                                            }
                                            else
                                            {
                                                frames_cnt++;
                                                Console.Write("Успешно приняли кадр {0}: первые отсчеты: ", frames_cnt);
                                                for (int i = 0; (i < 2) && (i < recvCnt); i++)
                                                {
                                                    Console.Write("{0} ", data[i].ToString("F7"));
                                                }
                                                Console.Write("\n");
                                            }
                                        }
                                        break;
                                    case ltr210api.RecvEvents.TIMEOUT:
                                        if ((hltr210.Cfg.Flags & ltr210api.CfgFlags.KEEPALIVE_EN) != 0)
                                        {
                                            /* если ничего не пришло, то проверяем, не превышен ли таймаут на ожидание
                                             * сигналов жизни */
                                            uint interval;
                                            err = hltr210.GetLastWordInterval(out interval);
                                            if (err == _LTRNative.LTRERROR.OK)
                                            {
                                                if (interval > KEEPALIVE_TOUT)
                                                {
                                                    Console.WriteLine("Не было периодических статусов от модуля за заданный интервал. Модуль не исправен!");
                                                    err = _LTRNative.LTRERROR.LTR210_ERR_KEEPALIVE_TOUT_EXCEEDED;
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Не удалось получить интервал времени с момента приема последнего слова. Ошибка Ошибка {0}: {1}",
                                                                   err, ltr210api.GetErrorString(err));
                                            }
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Ошибка при ожидании данных от модуля! Ошибка {0}: {1}",
                                                    err, ltr210api.GetErrorString(err));
                            }                           
                        }

                        /* Останов сбора по выходу из цикла */
                        _LTRNative.LTRERROR stop_err = hltr210.Stop();
                        if (stop_err != _LTRNative.LTRERROR.OK)
                        {
                            Console.WriteLine("Сбор остановлен с ошибокй. Ошибка {0}: {1}",
                                err, ltr210api.GetErrorString(stop_err));
                            if (err == _LTRNative.LTRERROR.OK)
                                err = stop_err;
                        }
                    }

                }             


                hltr210.Close();
            }

        }
    }
}
