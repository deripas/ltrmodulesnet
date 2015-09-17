using System;
using System.Runtime.InteropServices;
using System.Net;
using ltrModulesNet;

/* Данный пример демонстрирует получение списка крейтов и 
   типов установленных модулей в каждом крейте */
namespace ltrapi_cratelist
{
    class ltrapi_cratelist
    {
        static int Main(string[] args)
        {
            /* для передачи управляющих команд устанавливаем управляющее соединение
             * с сервисом (ltrd/LtrServer). Для управляющего соединения существует
             * специальный класс ltrsrvcon с набором функций ltrapi, которые относятся
             * к командом управления сервисом */
            ltrsrvcon srvcon = new ltrsrvcon();
            _LTRNative.LTRERROR err = srvcon.Open();
            if (err != _LTRNative.LTRERROR.OK)
            {
                Console.WriteLine("Не удалось установить связь с сервисом. Ошибка {0}: {1}",
                                  err, ltrsrvcon.GetErrorString(err));
            }
            else
            {
                string srv_ver;
                err = srvcon.GetServerVersion(out srv_ver);
                if (err != _LTRNative.LTRERROR.OK)
                {
                    Console.WriteLine("Не удалось установить связь с сервисом. Ошибка {0}: {1}",
                                      err, ltrsrvcon.GetErrorString(err));
                }
                else
                {
                    Console.WriteLine("Установлено соединение с сервисом, версия сервиса {0}", srv_ver);
                }
            }


            if (err == _LTRNative.LTRERROR.OK)
            {
                /* получаем список серийных номеров крейтов, которые подключены */
                string[] crateSerialList;
                err = srvcon.GetCrates(out crateSerialList);
                if (err != _LTRNative.LTRERROR.OK)
                {
                    Console.WriteLine("Не удалось получить список крейтов. Ошибка {0}: {1}",
                                      err, ltrsrvcon.GetErrorString(err));
                }
                else
                {
                    Console.WriteLine("Найдено крейтов: {0}", crateSerialList.Length);
                    foreach (string csn in crateSerialList)
                    {
                        /* устанавливаем соединение с каждым крейтом. для этого
                         * используем класс ltrcrate, который реализует подмножество
                         * функций из ltrapi для работы с одним крейтом */
                        ltrcrate crate = new ltrcrate();
                        _LTRNative.LTRERROR crate_err = crate.Open(csn);
                        if (crate_err != _LTRNative.LTRERROR.OK)
                        {
                            Console.WriteLine("Не удалось установить связь с крейтом {2}. Ошибка {0}: {1}",
                                      err, ltrsrvcon.GetErrorString(err), csn);
                        }
                        else
                        {
                            Console.WriteLine("Крейт {0}, S/N: {1}, интерфейс {2}", crate.Type, crate.Serial, crate.Interface);

                            /* получаем список модулей */
                            _LTRNative.MODULETYPE[] mids;
                            crate_err = crate.GetModules(out mids);
                            if (crate_err != _LTRNative.LTRERROR.OK)
                            {
                                Console.WriteLine("Не удалось получить список модулей. Ошибка {0}: {1}",
                                          err, ltrsrvcon.GetErrorString(err));
                            }
                            else
                            {
                                for (int i = 0; i < mids.Length; i++)
                                {
                                    /* для наглядности при выводе для пустых слотов выводим ---, а не
                                     * название типа EMPTY, чтобы не сливалось и явно было видно где нет модулей */
                                    Console.WriteLine("  Слот {0}: {1}", i + 1, 
                                        mids[i] == _LTRNative.MODULETYPE.EMPTY ? "---" : mids[i].ToString());
                                }
                            }
                        }

                        if (crate.IsOpened() == _LTRNative.LTRERROR.OK)
                            crate.Close();
                    }
                }

            }


            if (srvcon.IsOpened() == _LTRNative.LTRERROR.OK)
                srvcon.Close();

            return (int)err;
        }
    }
}
