using System;
using ltrModulesNet;

namespace ltr27_open_ex
{
    class ltr27_open_ex
    {
        /* Номер слота в крейте, где вставлен модуль */
        const int SLOT = 15;
        /* количество отсчетов на каждый мезонин, принимаемых за раз (блок) */
        const uint RECV_FRAMES = 4;
        /* количество принятых блоков по RECV_FRAMES отсчетов */
        const uint RECV_BLOCK_CNT = 3;
        /* таймаут в мс на прием блока */
        const uint RECV_BLOCK_TOUT = 10000;

        const bool stop_module = false;
        const bool store_config = true;
        const bool autorun = true;
        const _LTRNative.StartMode start_mode = _LTRNative.StartMode.RUN;

        static _LTRNative.LTRERROR RecvData(ltr27api hltr27, uint[] data, uint size)
        {
            _LTRNative.LTRERROR err = _LTRNative.LTRERROR.OK;
            int recv_res = hltr27.Recv(data, size, RECV_BLOCK_TOUT);

            if (recv_res < 0)
            {
                err = (_LTRNative.LTRERROR)recv_res;
                Console.WriteLine("Ошибка приема данных. Ошибка {0}: {1}",
                        err, ltr27api.GetErrorString(err));
            }
            else if (recv_res != size)
            {
                err = _LTRNative.LTRERROR.ERROR_RECV_INSUFFICIENT_DATA;
                Console.WriteLine("Приняли недостаточно данных: запрашивали {0}, приняли {1}",
                                  size, recv_res);
            }
            return err;
        }

        static void Main(string[] args)
        {
            _LTRNative.OpenOutFlags out_flags=0;
            /* LTR24_Init() вызывается уже в конструкторе */
            ltr27api hltr27 = new ltr27api();
            /* отрываем модуль. Входной флаг REOPEN указывает, что разрешено подключение 
               к модулю с запущенным сбором без его сброса. В out_flags указывается,
               в действительности установленно соединение с модулем, по которому уже
               был запущен сбор (установлен флаг REOPEN) или открыто новое соединение
               (флаг REOPEN сброшен) */
            _LTRNative.LTRERROR err = hltr27.OpenEx(SLOT, _LTRNative.OpenInFlags.REOPEN, out out_flags);
            if (err != _LTRNative.LTRERROR.OK)
            {
                Console.WriteLine("Не удалось открыть модуль. Ошибка {0}: {1}",
                        err, ltr27api.GetErrorString(err));
            }
            else
            {
                Console.WriteLine("Модуль открыт успешно!");
                if ((out_flags & _LTRNative.OpenOutFlags.REOPEN)!=0)
                {
                    Console.WriteLine("Было осуществлено подключение к работающему модулю");
                }
            }

            if (err == _LTRNative.LTRERROR.OK)
            {
                /* Считываем информацию о конфигурации модуля и о мезанинах, только
                 * если это новое соединение. Если восстановили старое - то 
                 * это информация уже восстановлена и идет сбор - т.е. явно считать нельзя */
                if ((out_flags & _LTRNative.OpenOutFlags.REOPEN) == 0)
                {
                    err = hltr27.GetConfig();
                    if (err != _LTRNative.LTRERROR.OK)
                    {
                        Console.WriteLine("Не удалось прочитать конфигурацию модуля. Ошибка {0}: {1}",
                                err, ltr27api.GetErrorString(err));
                    }
                    else
                    {
                        err = hltr27.GetDescription(ltr27api.Descriptions.MODULE);


                        for (int i = 0; (i < ltr27api.LTR27_MEZZANINE_NUMBER) &&
                            (err == _LTRNative.LTRERROR.OK); i++)
                        {
                            if (hltr27.Mezzanine[i].Name != "EMPTY")
                            {
                                err = hltr27.GetDescription((ushort)(1 << i));
                            }
                        }

                        if (err != _LTRNative.LTRERROR.OK)
                        {
                            Console.WriteLine("Не удалось прочитать описание модуля или мезонинов. Ошибка {0}: {1}",
                                              err, ltr27api.GetErrorString(err));
                        }
                    }                        
                }
            }

            /* выводим информацию о модуле */
            if (err == _LTRNative.LTRERROR.OK)
            {
                Console.WriteLine("  Информация о модуле :");
                Console.WriteLine("      Название        : {0}", hltr27.ModuleInfo.Module.DeviceName);
                Console.WriteLine("      Серийный номер  : {0}", hltr27.ModuleInfo.Module.SerialNumber);

                if (hltr27.ModuleInfo.Cpu.Active)
                {
                    Console.WriteLine("  Информация о процессоре :");
                    Console.WriteLine("     Название        : {0}", hltr27.ModuleInfo.Cpu.Name);
                    Console.WriteLine("     Частота клока   : {0} Hz", hltr27.ModuleInfo.Cpu.ClockRate);
                    Console.WriteLine("     Версия прошивки : {0}.{1}.{2}.{3}",
                        (hltr27.ModuleInfo.Cpu.FirmwareVersion >> 24) & 0xFF,
                        (hltr27.ModuleInfo.Cpu.FirmwareVersion >> 16) & 0xFF,
                        (hltr27.ModuleInfo.Cpu.FirmwareVersion >> 8) & 0xFF,
                        hltr27.ModuleInfo.Cpu.FirmwareVersion & 0xFF);
                    Console.WriteLine("     Комментарий     : {0}", hltr27.ModuleInfo.Cpu.Comment);
                }
                else
                {
                    Console.WriteLine("  Не найдено действительное описание процессора!!!");
                }

                for (int i = 0; (i < ltr27api.LTR27_MEZZANINE_NUMBER); i++)
                {
                    if (hltr27.ModuleInfo.Mezzanine[i].Active)
                    {
                        Console.WriteLine("    Информация о мезонине в слоте {0} :", i + 1);
                        Console.WriteLine("      Название        : {0}", hltr27.ModuleInfo.Mezzanine[i].Name);
                        Console.WriteLine("      Серийный номер  : {0}", hltr27.ModuleInfo.Mezzanine[i].SerialNumber);
                        Console.WriteLine("      Ревизия         : {0}", hltr27.ModuleInfo.Mezzanine[i].Revision);
                        for (int j = 0; (j < 4); j++)
                        {
                            Console.WriteLine("      Калибр. коэф. {0} : {1}", j,
                                              hltr27.ModuleInfo.Mezzanine[i].Calibration[j].ToString("F5"));
                        }
                    }
                }
            }

            /* Запись настроек (только если не был уже запущен сбор) */
            if ((err == _LTRNative.LTRERROR.OK) &&
                ((out_flags & _LTRNative.OpenOutFlags.REOPEN) == 0))
            {
                /* выбираем частоту дискретизции 1000Гц */
                hltr27.FrequencyDivisor = 255;

                err = hltr27.SetConfig();
                if (err != _LTRNative.LTRERROR.OK)
                {
                    Console.WriteLine("Не удалось записать настройки модуля. Ошибка {0}: {1}",
                        err, ltr27api.GetErrorString(err));
                }
            }

            /* Запуск сбора данных (если не был запущен) */
            if ((err == _LTRNative.LTRERROR.OK) &&
                ((out_flags & _LTRNative.OpenOutFlags.REOPEN) == 0))
            {
                err = hltr27.ADCStart();
                if (err != _LTRNative.LTRERROR.OK)
                {
                    Console.WriteLine("Не удалось запустить сбор данных. Ошибка {0}: {1}",
                        err, ltr27api.GetErrorString(err));
                }
            }


            if (err == _LTRNative.LTRERROR.OK)
            {
                uint size = RECV_FRAMES*2*ltr27api.LTR27_MEZZANINE_NUMBER;
                uint[] buf = new uint[size];
                double[] data = new double[size];
                bool search_start_req = (out_flags & _LTRNative.OpenOutFlags.REOPEN)!=0; 

                Console.WriteLine("Сбор данных запущен успешно");

                for (int b = 0; b < RECV_BLOCK_CNT; b++)
                {     
                    err = RecvData(hltr27, buf, size);
                    if ((err==_LTRNative.LTRERROR.OK) && search_start_req)
                    {
                     /* если подсоединились к уже запущенному сбору данных, то
                      * нужно найти начало кадра в принятых словах */
                        uint frame_index;
                        err = hltr27.SearchFirstFrame(buf, size, out frame_index);
                        if (err != _LTRNative.LTRERROR.OK)
                        {
                            Console.WriteLine("Ошибка поиска начала кадра. Ошибка {0}: {1}",
                                              err, ltr27api.GetErrorString(err));
                        }
                        else if (frame_index > 0)
                        {     
                            /* если начала кадра не совпадает с началом принятого буфера,
                             * то нужно откинуть из начала лишние данные и допринять
                             * в конец столько же */
                            uint[] end_buf = new uint[frame_index];
                            err = RecvData(hltr27, end_buf, frame_index);

                            if (err == _LTRNative.LTRERROR.OK)
                            {
                                Array.Copy(buf, frame_index, buf, 0, size - frame_index);
                                Array.Copy(end_buf, 0, buf, size - frame_index, frame_index);
                            }
                        }
                        search_start_req = false;                    
                    }

                    if (err == _LTRNative.LTRERROR.OK)
                    {
                        err = hltr27.ProcessData(buf, data, size, true, true);
                        if (err == _LTRNative.LTRERROR.OK)
                        {
                            /* отображаем данные */
                            Console.WriteLine("Успешно приняли блок данных {0}:", b + 1);
                            for (int i = 0; (i < RECV_FRAMES); i++)
                            {
                                for (int j = 0; (j < 2 * ltr27api.LTR27_MEZZANINE_NUMBER); j++)
                                {
                                    if (hltr27.Mezzanine[j >> 1].Name != "EMPTY")
                                    {
                                        Console.Write(" {0} {1}", data[i * 2 * ltr27api.LTR27_MEZZANINE_NUMBER]
                                            .ToString("F4"), hltr27.Mezzanine[j >> 1].Unit);
                                    }
                                }
                                Console.Write("\n");
                            }
                            
                        }
                        else
                        {
                            Console.WriteLine("Ошибка обработки данных! Ошибка {0}: {1}",
                                             err, ltr27api.GetErrorString(err));
                        }

                    }
                }


                /* останов сбора данных */
                if (stop_module)
                {
                    _LTRNative.LTRERROR stoperr = hltr27.ADCStop();
                    if (stoperr != _LTRNative.LTRERROR.OK)
                    {
                        Console.WriteLine("Не удалось остановить сбор данных. Ошибка {0}: {1}",
                            stoperr, ltr27api.GetErrorString(stoperr));
                        if (err == _LTRNative.LTRERROR.OK)
                            err = stoperr;
                    }
                    else
                    {
                        Console.WriteLine("Сбор данных остановлен успешно");
                    }
                }
            } /*if (err == LTR_OK)*/

            /* сохранение настроек модуля во flash-память крейта */
            if ((err == _LTRNative.LTRERROR.OK) && store_config)
            {
                err = hltr27.StoreConfig(start_mode);
                if (err == _LTRNative.LTRERROR.OK)
                    Console.WriteLine("Конфигурация модуля успешно сохранена");
                else
                    Console.WriteLine("Не удалось сохранить конфигурацию модуля. Ошибка {0}: {1}",
                            err, ltr27api.GetErrorString(err));
            }

            /* сохранение настроек крейта (автономная работа) во flash-память */
            if ((err == _LTRNative.LTRERROR.OK) && store_config)
            {
                _LTRNative.TLTR hltr = new _LTRNative.TLTR();
                err = _LTRNative.LTR_Init(ref hltr);
                if (err == _LTRNative.LTRERROR.OK)
                {
                    

                    hltr.cc = 0; /* управляющее соединение */
                    /* устанавливаем соединение с тем крейтом, в котором
                     * установлен ltr27 */
                    hltr.saddr = hltr27.Channel.saddr;
                    hltr.sport = hltr27.Channel.sport;
                    hltr.Serial = hltr27.Channel.Serial;

                    err = _LTRNative.LTR_Open(ref hltr);
                }
                if (err == _LTRNative.LTRERROR.OK)
                {
                    _LTRNative.TLTR_SETTINGS set = new _LTRNative.TLTR_SETTINGS();
                    
                    set.Init();
                    set.AutorunIsOn = autorun;
                    err = _LTRNative.LTR_PutSettings(ref hltr, ref set);
                    if (err == _LTRNative.LTRERROR.OK)
                        Console.WriteLine("Кофигурация крейта успешно сохранена");
                    _LTRNative.LTRERROR closerr = _LTRNative.LTR_Close(ref hltr);
                    if (closerr != _LTRNative.LTRERROR.OK)
                    {
                        Console.WriteLine("Не удалось закрыть соединение с крейтом. Ошибка {0}: {1}",
                            closerr, ltr27api.GetErrorString(closerr));
                        if (err == _LTRNative.LTRERROR.OK)
                            err = closerr;
                    }
                }

                if (err != _LTRNative.LTRERROR.OK)
                {
                    Console.WriteLine("Не удалось сохранить конфигурацию крейта. Ошибка {0}: {1}",
                            err, ltr27api.GetErrorString(err));
                }

            }



            /* разрываем соединение */
            _LTRNative.LTRERROR closeerr = hltr27.Close();
            if (closeerr != _LTRNative.LTRERROR.OK)
            {
                Console.WriteLine("Не удалось закрыть соединение с модулем. Ошибка {0}: {1}",
                    closeerr, ltr27api.GetErrorString(closeerr));
                if (err == _LTRNative.LTRERROR.OK)
                    err = closeerr;
            }
            else
            {
                Console.WriteLine("Соединение с модулем закрыто успешно");
            }
        }
    }
}
