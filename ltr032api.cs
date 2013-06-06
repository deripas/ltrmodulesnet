using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ltrModulesNet
{  
    public class ltr032
    {
        /** Значения токов по умолчанию для порогов NAMUR. */
        public const float LTR032_NAMUR_DEFAULT_LOWER = 1.3146F;
        public const float LTR032_NAMUR_DEFAULT_UPPER = 1.9466F;

        /** Индексы температурных сенсоров. */
        public enum TempSens : byte
        {
            LTR032_TEMP_SENS_LOWER,
            LTR032_TEMP_SENS_UPPER
        };

        /** Режим работы выхода M1S_OUT. */
        public enum M1SOutMode :byte
        {
            LTR032_M1SOUT_MODE_OPEN,
            LTR032_M1SOUT_MODE_CLOSE,
            LTR032_M1SOUT_MODE_CLOSE_PULSE,
            LTR032_M1SOUT_MODE_OPEN_PULSE = 4,
        };



        [DllImport("ltr032api.dll")]
        public static extern UInt32 ltr032_get_version();
        
        /** Возвращает описатель интерфейса для работы с крейт-контроллером. */
        [DllImport("ltr032api.dll")]
        public static extern IntPtr ltr032_init();

        /** Освобождает выделенные ресурсы для работы с описателем. */
        [DllImport("ltr032api.dll")]
        public static extern _LTRNative.LTRERROR ltr032_exit(IntPtr hnd);

        /** Открывает интерфейс с крейт-контроллером. */
        [DllImport("ltr032api.dll")]
        public static extern _LTRNative.LTRERROR ltr032_open(IntPtr hnd, uint ip_addr,
            ushort port, string serial);

        /** Возвращает 1, если интерфейс с крейт-контроллером открыт,
         *  или 0, если закрыт.
         */
        [DllImport("ltr032api.dll")]
        public static extern _LTRNative.LTRERROR ltr032_is_open(IntPtr hnd);

        /** Закрывает интерфейс с крейт-контроллером. */
        [DllImport("ltr032api.dll")]
        public static extern _LTRNative.LTRERROR ltr032_close(IntPtr hnd);

        /** Устанавливает режим работы выхода M1S_OUT. */
        [DllImport("ltr032api.dll")]
        public static extern _LTRNative.LTRERROR ltr032_set_m1sout_mode(IntPtr hnd, M1SOutMode mode);

        /** Возвращает режим работы выхода M1S_OUT. */
        [DllImport("ltr032api.dll")]
        public static extern _LTRNative.LTRERROR ltr032_get_m1sout_mode(IntPtr hnd, out M1SOutMode mode);

        /** Изменяет параметры интерфейса NAMUR. */
        [DllImport("ltr032api.dll")]
        public static extern _LTRNative.LTRERROR ltr032_set_startin_namur(IntPtr hnd, float lower, float upper);

        /** Читает параметры интерфейса NAMUR. */
        [DllImport("ltr032api.dll")]
        public static extern _LTRNative.LTRERROR ltr032_get_startin_namur(IntPtr hnd, out float lower, out float upper);

        /** Читает текущую температуру. */
        [DllImport("ltr032api.dll")]
        public static extern _LTRNative.LTRERROR ltr032_get_temperature(IntPtr hnd, TempSens sens, out float temp);

        /** Возвращает текущее значение напряжения питания. */
        [DllImport("ltr032api.dll")]
        public static extern _LTRNative.LTRERROR ltr032_get_supply_voltage(IntPtr hnd, out float volt);

        /** Устанавливает текущее время крейта в POSIX формате. */
        [DllImport("ltr032api.dll")]
        public static extern _LTRNative.LTRERROR ltr032_set_time(IntPtr hnd, UInt64 ptime);

        /** Возвращает текущее время крейта в POSIX формате. */
        [DllImport("ltr032api.dll")]
        public static extern _LTRNative.LTRERROR ltr032_get_time(IntPtr hnd, out UInt64 ptime);
        
        /** Получить текстовое описание ошибки. */
        [DllImport("ltr032api.dll")]
        public static extern IntPtr ltr032_get_error_string(IntPtr hnd);

        /** Возвращает структуру TLTR для работы через интерфейс LTRAPI.
         *  Используется, например, для включения секундных меток.
         */
        [DllImport("ltr032api.dll")]
        public static extern _LTRNative.LTRERROR ltr032_get_legacy_interface(IntPtr hnd, out _LTRNative.TLTR ltr);

        /** Выполняет полный сброс крейт-контроллера. */
        [DllImport("ltr032api.dll")]
        public static extern _LTRNative.LTRERROR ltr032_reset(IntPtr hnd);

        /** Запускает загрузчик крейт-контроллера. */
        [DllImport("ltr032api.dll")]
        public static extern _LTRNative.LTRERROR ltr032_start_boot(IntPtr hnd);



        IntPtr hnd;


        public ltr032()
        {
            hnd = ltr032_init();
        }
        
        ~ltr032()
        {
            ltr032_exit(hnd);
        }

        public virtual _LTRNative.LTRERROR Close()
        {
            return ltr032_close(hnd);
        }

        public virtual _LTRNative.LTRERROR Open(uint saddr, ushort sport, string csn)
        {
            return ltr032_open(hnd, saddr, sport, csn);
        }

        public virtual _LTRNative.LTRERROR Open(string csn)
        {
            return Open(_LTRNative.SADDR_DEFAULT, _LTRNative.SPORT_DEFAULT, csn);
        }

        public virtual _LTRNative.LTRERROR IsOpened()
        {
            return ltr032_is_open(hnd);
        }


        /** Устанавливает режим работы выхода M1S_OUT. */
        public virtual _LTRNative.LTRERROR SetM1SOutMode(M1SOutMode mode)
        {
            return ltr032_set_m1sout_mode(hnd, mode);
        }

        public virtual _LTRNative.LTRERROR GetM1SOutMode(out M1SOutMode mode)
        {
            return ltr032_get_m1sout_mode(hnd, out mode);
        }

        /** Изменяет параметры интерфейса NAMUR. */
        public virtual _LTRNative.LTRERROR SetStartinNamur(float lower, float upper)
        {
            return ltr032_set_startin_namur(hnd, lower, upper);
        }

        /** Читает параметры интерфейса NAMUR. */
        public virtual _LTRNative.LTRERROR GetStartinNamur(out float lower, out float upper)
        {
            return ltr032_get_startin_namur(hnd, out lower, out upper);
        }

        /** Читает текущую температуру. */
        public virtual _LTRNative.LTRERROR GetTemperature(TempSens sens, out float temp)
        {
            return ltr032_get_temperature(hnd, sens, out temp);
        }

        /** Возвращает текущее значение напряжения питания. */
        public virtual _LTRNative.LTRERROR GetSupplyVoltage(out float volt)
        {
            return ltr032_get_supply_voltage(hnd, out volt);
        }

        /** Устанавливает текущее время крейта в POSIX формате. */
        public virtual _LTRNative.LTRERROR SetTime(UInt64 ptime)
        {
            return ltr032_set_time(hnd, ptime);
        }

        /** Возвращает текущее время крейта в POSIX формате. */
        public virtual _LTRNative.LTRERROR GetTime(out UInt64 ptime)
        {
            return ltr032_get_time(hnd, out ptime);
        }

        /** Получить текстовое описание ошибки. */
        public virtual string GetErrorString(_LTRNative.LTRERROR err)
        {
            IntPtr ptr = ltr032_get_error_string(hnd);
            string str = Marshal.PtrToStringAnsi(ptr);
            Encoding srcEncodingFormat = Encoding.GetEncoding("windows-1251");
            Encoding dstEncodingFormat = Encoding.UTF8;
            return dstEncodingFormat.GetString(Encoding.Convert(srcEncodingFormat, dstEncodingFormat, srcEncodingFormat.GetBytes(str)));
        }

        /** Возвращает структуру TLTR для работы через интерфейс LTRAPI.
         *  Используется, например, для включения секундных меток.
         */
        public virtual _LTRNative.LTRERROR GetLegacyInterface(out _LTRNative.TLTR ltr)
        {
            return ltr032_get_legacy_interface(hnd, out ltr);
        }

        /** Выполняет полный сброс крейт-контроллера. */
        public virtual _LTRNative.LTRERROR Reset()
        {            
            return ltr032_reset(hnd);
        }

        /** Запускает загрузчик крейт-контроллера. */
        public virtual _LTRNative.LTRERROR StartBoot()
        {
            return ltr032_start_boot(hnd);
        }
    }
}
