using System;
using System.Runtime.InteropServices;
using System.Net;
using ltrModulesNet;



/* Данный пример демонстрирует получение списка всех IP-записей */
namespace ltrapi_iprec
{
    class ltrapi_iprec
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
                ltrsrvcon.IpCrateEntry[] ipEntries;
                /* получаем все записи */
                err = srvcon.GetListOfIPCrates(0, 0, out ipEntries);
                if (err != _LTRNative.LTRERROR.OK)
                {
                    Console.WriteLine("Не получить список IP-записей. Ошибка {0}: {1}",
                                      err, ltrsrvcon.GetErrorString(err));
                }
                else
                {
                    Console.WriteLine("Найдено IP-записей: {0}", ipEntries.Length);
                    for (int i = 0; i < ipEntries.Length; i++)
                    {
                        Console.WriteLine("Запись {0}: {1}, {2}, S/N: {3}, Auto: {4}", i + 1, ipEntries[i].IpAddr, 
                            ipEntries[i].Status, ipEntries[i].CrateSerial, ipEntries[i].Autoconnect);                        
                    }
                    

                }
            }



            if (srvcon.IsOpened() == _LTRNative.LTRERROR.OK)
                srvcon.Close();

            return (int)err;
        }
    }
}
