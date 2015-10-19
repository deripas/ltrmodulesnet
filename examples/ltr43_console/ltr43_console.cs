using System;
using ltrModulesNet;


/* Данный пример демонстрирует работу с модулем ltr43 из программы на языке C#.
 * Пример представляет собой консольную программу, которая устанавливает связь с модулем,
 * выводит информацию о модуле, настраивает направления портов, выводит тестовое значение
 * на выходы и читает значения со входов.
 * 
 * Необходимо установить номер слота, в котором вставлен модуль (константа SLOT). 
 */


namespace ltr43_console
{
    class ltr43_console
    {
         /* Номер слота в крейте, где вставлен модуль */
        const int SLOT = 1;

        static int Main(string[] args)
        {          
            /* ltr43_Init() вызывается уже в конструкторе */
            ltr43api hltr43 = new ltr43api();
            /* отрываем модуль. Используем упрощенный вариант функции с указанием только слота.
             * (есть вариант как с только со слотом, так и с серийным крейта и слотом 
             *  + полный) */
            _LTRNative.LTRERROR err = hltr43.Open(SLOT);
            if (err != _LTRNative.LTRERROR.OK)
            {
                Console.WriteLine("Не удалось открыть модуль. Ошибка {0}: {1}",
                    err, ltr43api.GetErrorString(err));
            }
            else
            {
                /* выводим информацию из hltr43.ModuleInfo */
                Console.WriteLine("Информация о модуле: ");
                Console.WriteLine("  Название модуля: {0}", hltr43.ModuleInfo.Name);
                Console.WriteLine("  Серийный номер : {0}", hltr43.ModuleInfo.Serial);
                Console.WriteLine("  Версия прошивки: {0}", hltr43.ModuleInfo.FirmwareVersionStr);
                Console.WriteLine("  Дата прошивки  : {0}", hltr43.ModuleInfo.FirmwareDateStr);

                /* --------------- задание параметров работы модуля ------------ */


                /* направление портов ввода-вывода */
                ltr43api.IOPortsCfg ioports = new ltr43api.IOPortsCfg();
                ioports.Port1 = ltr43api.PortDir.OUT;
                ioports.Port2 = ltr43api.PortDir.OUT;
                ioports.Port3 = ltr43api.PortDir.IN;
                ioports.Port4 = ltr43api.PortDir.IN;
                hltr43.IO_Ports = ioports;

                err = hltr43.Config();
                if (err != _LTRNative.LTRERROR.OK)
                {
                    Console.WriteLine("Не удалось установить настройки модуля. Ошибка {0}: {1}",
                            err, ltr43api.GetErrorString(err));                    
                }

                if (err == _LTRNative.LTRERROR.OK)
                {
                    uint wrd = 0x00005A42;
                    err = hltr43.WritePort(wrd);
                    if (err != _LTRNative.LTRERROR.OK)
                    {
                        Console.WriteLine("Не удалось записать значения портов модуля. Ошибка {0}: {1}",
                                err, ltr43api.GetErrorString(err));
                    }
                    else
                    {
                        Console.WriteLine("Значения портов записаны успешно!");
                    }
                }

                if (err == _LTRNative.LTRERROR.OK)
                {
                    uint wrd;
                    err = hltr43.ReadPort(out wrd);
                    if (err != _LTRNative.LTRERROR.OK)
                    {
                        Console.WriteLine("Не удалось прочитать значения портов модуля. Ошибка {0}: {1}",
                                err, ltr43api.GetErrorString(err));
                    }
                    else
                    {
                        uint msk;
                        Console.Write("Успешно считано значение (биты 31-16): ");
                        for (msk = (1U << 31); msk != (1 << 15); msk >>= 1)
                            Console.Write("{0} ", (wrd & msk) != 0 ? 1 : 0);
                        Console.WriteLine("");
                    }                
                }
            }

            /* закрываем соединение */
            if (hltr43.IsOpened() == _LTRNative.LTRERROR.OK)
                hltr43.Close();

            return (int)err;
        }
    }
}
