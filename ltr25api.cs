using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ltrModulesNet
{
    public class ltr25api
    {
        [DllImport("ltr25api.dll")]
        public static extern _LTRNative.LTRERROR LTR25_Init(ref TLTR25 hnd);
        [DllImport("ltr25api.dll")]
        public static extern _LTRNative.LTRERROR LTR25_Close(ref TLTR25 hnd);
        [DllImport("ltr25api.dll")]
        public static extern _LTRNative.LTRERROR LTR25_Open(ref TLTR25 hnd, uint saddr, ushort sport, string csn, int slot_num);
        [DllImport("ltr25api.dll")]
        public static extern _LTRNative.LTRERROR LTR25_OpenEx(ref TLTR25 module, uint saddr, ushort sport, string csn, ushort cc,
                   _LTRNative.OpenInFlags in_flags, out _LTRNative.OpenOutFlags out_flags);
        [DllImport("ltr25api.dll")]
        public static extern _LTRNative.LTRERROR LTR25_IsOpened(ref TLTR25 hnd);
        [DllImport("ltr25api.dll")]
        public static extern _LTRNative.LTRERROR LTR25_GetConfig(ref TLTR25 hnd);
        [DllImport("ltr25api.dll")]
        public static extern _LTRNative.LTRERROR LTR25_SetADC(ref TLTR25 hnd);
        [DllImport("ltr25api.dll")]
        public static extern _LTRNative.LTRERROR LTR25_Start(ref TLTR25 hnd);
        [DllImport("ltr25api.dll")]
        public static extern _LTRNative.LTRERROR LTR25_Stop(ref TLTR25 hnd);
        [DllImport("ltr25api.dll")]
        public static extern int LTR25_Recv(ref TLTR25 hnd, uint[] data, uint[] tmark, uint size, uint timeout);
        [DllImport("ltr25api.dll")]
        public static extern _LTRNative.LTRERROR LTR25_ProcessData(ref TLTR25 hnd, uint[] src,
                                                            double[] dest, ref int size, ProcFlags flags,
                                                            ChStatus[] ch_status);

        [DllImport("ltr25api.dll")]
        public static extern _LTRNative.LTRERROR LTR25_SearchFirstFrame(ref TLTR25 hnd, uint[] data, uint size,
                                                                 out uint index);
        [DllImport("ltr25api.dll")]
        public static extern IntPtr LTR25_GetErrorString(int err);

        [DllImport("ltr25api.dll")]
        public static extern _LTRNative.LTRERROR LTR25_SetLowPowMode(ref TLTR25 hnd, bool lowPowState);

        [DllImport("ltr25api.dll")]
        public static extern _LTRNative.LTRERROR LTR25_FPGAIsEnabled(ref TLTR25 hnd, out bool enabled);
        [DllImport("ltr25api.dll")]
        static extern _LTRNative.LTRERROR LTR25_FPGAEnable(ref TLTR25 hnd, bool enable);

        [DllImport("ltr25api.dll")]
        public static extern _LTRNative.LTRERROR LTR25_FlashRead(ref TLTR25 hnd, uint addr, byte[] data, uint size);
        [DllImport("ltr25api.dll")]
        public static extern _LTRNative.LTRERROR LTR25_FlashWrite(ref TLTR25 hnd, uint addr, byte[] data, uint size);
        [DllImport("ltr25api.dll")]
        public static extern _LTRNative.LTRERROR LTR25_FlashErase(ref TLTR25 hnd, uint addr, uint size);

        [DllImport("ltr25api.dll")]
        public static extern _LTRNative.LTRERROR LTR25_StoreConfig(ref TLTR25 module, _LTRNative.StartMode start_mode);




        public const int LTR25_CHANNEL_CNT = 8;
        public const int LTR25_FREQ_CNT = 8;
        public const int LTR25_CBR_FREQ_CNT = 2;
        public const int LTR25_I_SRC_VALUE_CNT = 2;
        public const int LTR25_NAME_SIZE = 8;
        public const int LTR25_SERIAL_SIZE = 16;

        /** Максимальное пиковое значение в Вольтах для диапазона измерения модуля */
        public const double LTR25_ADC_RANGE_PEAK = 10;
        /** Код АЦП, соответствующее максимальному пиковому значению */
        public const int LTR25_ADC_SCALE_CODE_MAX = 2000000000;
        /** Адрес, с которой начинается пользовательская область flash-памяти */
        public const int LTR25_FLASH_USERDATA_ADDR = 0x0;
        /** Размер пользовательской области Flash-памяти */
        public const int LTR25_FLASH_USERDATA_SIZE = 0x100000;

        /* Частоты сбора данных. */
        public enum FreqCode : byte
        {
            Freq_78K = 0, // 78.125 кГц
            Freq_39K = 1, // 39.0625 кГц
            Freq_19K = 2, // 19.53125 кГц
            Freq_9K7 = 3, // 9.765625 кГц
            Freq_4K8 = 4, // 4.8828125 кГц
            Freq_2K4 = 5, // 2.44140625 кГц
            Freq_1K2 = 6, // 1.220703125 кГц
            Freq_610 = 7  // 610.3515625 Гц
        }

        public enum ISrcValues : byte
        {
            I_2_86 = 0,
            I_10 = 1
        }

        /* Разрядность данных. */
        public enum DataFormat : byte
        {
            Format20 = 0,
            Format32 = 1
        }

        [Flags]
        public enum ProcFlags : uint
        {
            Volt = 0x00000001,
            NoncontData = 0x00000100
        }

        /** Состояние входного канала. Возвращается LTR25_ProcessData() */
        public enum ChStatus : uint
        {
            OK = 0,
            SHORT = 1, /**< Было обнаружено короткое замыкание */
            OPEN = 2, /**< Был обнаружен разрыв цепи */
        }





        /** Калибровочные коэффициенты */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct CBR_COEF
        {
            float _offset;
            float _scale;

            public float Offset { get { return _offset; } } /**< Код смещения */
            public float Scale { get { return _scale; } }  /**< Коэффициент шкалы */
        }

        /** Информация о модуле */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct INFO
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR25_NAME_SIZE)]
            char[] _name;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR25_SERIAL_SIZE)]
            char[] _serial;
            ushort _verFPGA;
            byte _verPLD;
            byte _boardRev;
            bool _industrial;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            uint[] Reserved;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR25_CHANNEL_CNT * LTR25_CBR_FREQ_CNT)]
            CBR_COEF[] CbrCoef;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32 * LTR25_CHANNEL_CNT)]
            double[] Reserved2;

            /* Название модуля */
            public string Name { get { return new string(_name).Split('\0')[0]; } }
            /* Серийный номер модуля */
            public string Serial { get { return new string(_serial).Split('\0')[0]; } }
            /* Версия прошивки ПЛИС модуля (действительна только после ее загрузки) */
            public ushort VerFPGA { get { return _verFPGA; } }
            /* Версия прошивки PLD */
            public byte VerPLD { get { return _verPLD; } }
            /* Ревизия платы */
            public byte BoardRev { get { return _verPLD; } }
            /* Признак, это индустриальный вариант модуля или нет */
            public bool Industrial { get { return _industrial; } }
        }


        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct CHANNEL_CONFIG
        {
            bool _enabled;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
            uint[] Reserved;

            public bool Enabled { get { return _enabled; } set { _enabled = value; } }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct CONFIG
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR25_CHANNEL_CNT)]
            CHANNEL_CONFIG[] _ch;
            FreqCode _freq_code;
            DataFormat _data_fmt;
            ISrcValues _i_src_val;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
            uint[] Reserved;

            /** Настройки каналов АЦП */
            public CHANNEL_CONFIG[] Ch { get { return _ch; } set { _ch = value; } }
            public FreqCode FreqCode { get { return _freq_code; } set { _freq_code = value; } }
            public DataFormat DataFmt { get { return _data_fmt; } set { _data_fmt = value; } }
            public ISrcValues ISrcValue { get { return _i_src_val; } set { _i_src_val = value; } }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct STATE
        {
            _LTRNative.FpgaState _fpga_state;
            byte _enabled_ch_cnt;
            bool _run;
            double _adc_freq;
            bool _low_pow_mode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)]
            uint[] Reserved;

            public _LTRNative.FpgaState FpgaState { get { return _fpga_state; } }
            public byte EnabledChCnt { get { return _enabled_ch_cnt; } }
            public bool Run { get { return _run; } }
            public double AdcFreq { get { return _adc_freq; } }
            public bool LowPowMode { get {return _low_pow_mode; } }
        }


        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TLTR25
        {
            int _size;
            _LTRNative.TLTR _channel;
            IntPtr _internal;
            CONFIG _cfg;
            STATE _stat;
            INFO _info;

            /* Структура, содержащая состояние соединения с сервером. */
            public _LTRNative.TLTR Channel { get { return _channel; } }
            /** Настройки модуля. Заполняются пользователем перед вызовом LTR210_SetADC(). */
            public CONFIG Cfg { get { return _cfg; } set { _cfg = value; } }
            /** Состояние модуля и рассчитанные параметры. Поля изменяются функциями
                библиотеки. Пользовательской программой могут использоваться
                только для чтения. */
            public STATE State { get { return _stat; } }
            /** Информация о модуле */
            public INFO ModuleInfo { get { return _info; } }
        }



        public TLTR25 module;

        public CONFIG Cfg { get { return module.Cfg; } set { module.Cfg = value; } }
        public STATE State { get { return module.State; } }
        public INFO ModuleInfo { get { return module.ModuleInfo; } }



        public ltr25api()
        {
            LTR25_Init(ref module);
        }

        /* в финализаторе убеждаемся, что остановили поток и 
         * закрыли модуль */
        ~ltr25api()
        {
            if (IsOpened() == _LTRNative.LTRERROR.OK)
            {              
                Close();
            }
        }

        public _LTRNative.LTRERROR Open(uint saddr, ushort sport, string csn, int slot_num)
        {
            return LTR25_Open(ref module, saddr, sport, csn, slot_num);
        }

        public _LTRNative.LTRERROR Open(string csn, int slot_num)
        {
            return Open(_LTRNative.SADDR_DEFAULT, _LTRNative.SPORT_DEFAULT, csn, slot_num);
        }

        public _LTRNative.LTRERROR Open(int slot_num)
        {
            return Open("", slot_num);
        }

        public virtual _LTRNative.LTRERROR OpenEx(uint saddr, ushort sport, string csn, ushort cc,
                                                 _LTRNative.OpenInFlags in_flags, out _LTRNative.OpenOutFlags out_flags)
        {
            return LTR25_OpenEx(ref module, saddr, sport, csn, cc, in_flags, out out_flags);
        }

        public virtual _LTRNative.LTRERROR OpenEx(string csn, ushort cc,
                                          _LTRNative.OpenInFlags in_flags, out _LTRNative.OpenOutFlags out_flags)
        {
            return OpenEx(_LTRNative.SADDR_DEFAULT, _LTRNative.SPORT_DEFAULT, csn, cc, in_flags, out out_flags);
        }

        public virtual _LTRNative.LTRERROR OpenEx(ushort cc, _LTRNative.OpenInFlags in_flags, out _LTRNative.OpenOutFlags out_flags)
        {
            return OpenEx(_LTRNative.SADDR_DEFAULT, _LTRNative.SPORT_DEFAULT, "", cc, in_flags, out out_flags);
        }

        public _LTRNative.LTRERROR Close()
        {
            return LTR25_Close(ref module);
        }

        public _LTRNative.LTRERROR IsOpened()
        {
            return LTR25_IsOpened(ref module);
        }

        public _LTRNative.LTRERROR GetConfig() 
        {
            return LTR25_GetConfig(ref module);
        }

        public _LTRNative.LTRERROR SetADC() 
        {
            return LTR25_SetADC(ref module);
        }

        public _LTRNative.LTRERROR Start() 
        {
            return LTR25_Start(ref module);
        }

        public _LTRNative.LTRERROR Stop() 
        {
            return LTR25_Stop(ref module);
        }

        public int Recv(uint[] data, uint[] tmark, uint size, uint timeout)
        {
            return LTR25_Recv(ref module, data, tmark, size, timeout);
        }

        public int Recv(uint[] data, uint size, uint timeout)
        {
            return LTR25_Recv(ref module, data, null, size, timeout);
        }

        public _LTRNative.LTRERROR ProcessData(uint[] src, double[] dest, ref int size, ProcFlags flags,
                                                    ChStatus[] ch_status)
        {
            return LTR25_ProcessData(ref module, src, dest, ref size, flags, ch_status);
        }

        public _LTRNative.LTRERROR ProcessData(uint[] src, double[] dest, ref int size, ProcFlags flags)
        {
            return LTR25_ProcessData(ref module, src, dest, ref size, flags, null);
        }

        public _LTRNative.LTRERROR SearchFirstFrame(uint[] data, uint size, out uint index) 
        {
            return LTR25_SearchFirstFrame(ref module, data, size, out index);
        }


        public static string GetErrorString(_LTRNative.LTRERROR err)
        {
            IntPtr ptr = LTR25_GetErrorString((int)err);
            string str = Marshal.PtrToStringAnsi(ptr);
            Encoding srcEncodingFormat = Encoding.GetEncoding("windows-1251");
            Encoding dstEncodingFormat = Encoding.UTF8;
            return dstEncodingFormat.GetString(Encoding.Convert(srcEncodingFormat, dstEncodingFormat, srcEncodingFormat.GetBytes(str)));
        }

        public _LTRNative.LTRERROR SetLowPowMode(bool lowPowState)
        {
            return LTR25_SetLowPowMode(ref module, lowPowState);
        }

        public _LTRNative.LTRERROR FPGAIsEnabled(out bool enabled) 
        {
            return LTR25_FPGAIsEnabled(ref module, out enabled);
        }

        public _LTRNative.LTRERROR FPGAEnable(bool enable) 
        {
            return LTR25_FPGAEnable(ref module, enable);
        }

        public _LTRNative.LTRERROR FlashRead(uint addr, byte[] data, uint size)
        {
            return LTR25_FlashRead(ref module, addr, data, size);
        }

        public _LTRNative.LTRERROR FlashWrite(uint addr, byte[] data, uint size)
        {
            return LTR25_FlashWrite(ref module, addr, data, size);
        }

        public _LTRNative.LTRERROR FlashErase(uint addr, uint size)
        {
            return LTR25_FlashErase(ref module, addr, size);
        }

        public virtual _LTRNative.LTRERROR StoreConfig(_LTRNative.StartMode start_mode)
        {
            return LTR25_StoreConfig(ref module, start_mode);
        }
    }
}
