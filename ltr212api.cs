using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ltrModulesNet
{
    public class ltr212api
    {

        [DllImport("ltr212api.dll")]
        static extern void LTR212_GetDllVersion(out DLL_VERSION DllVersion);

        [DllImport("ltr212api.dll")]
        static extern _LTRNative.LTRERROR LTR212_Init(ref TLTR212 module);

        [DllImport("ltr212api.dll")]
        static extern _LTRNative.LTRERROR LTR212_Open(ref TLTR212 module, uint net_addr, ushort net_port,
                                                             string crate_sn, int slot_num, string biosname);

        [DllImport("ltr212api.dll")]
        static extern _LTRNative.LTRERROR LTR212_IsOpened(ref TLTR212 module);

        [DllImport("ltr212api.dll")]
        static extern _LTRNative.LTRERROR LTR212_Close(ref TLTR212 module);

        [DllImport("ltr212api.dll")]
        static extern int LTR212_CreateLChannel(uint PhysChannel, Scales Scale);

        [DllImport("ltr212api.dll")]
        static extern int LTR212_CreateLChannel2(uint PhysChannel, Scales Scale, BridgeTypes BridgeType);

        [DllImport("ltr212api.dll")]
        static extern _LTRNative.LTRERROR LTR212_SetADC(ref TLTR212 module);

        [DllImport("ltr212api.dll")]
        static extern _LTRNative.LTRERROR LTR212_Start(ref TLTR212 module);

        [DllImport("ltr212api.dll")]
        static extern _LTRNative.LTRERROR LTR212_Stop(ref TLTR212 module);

        [DllImport("ltr212api.dll")]
        static extern int LTR212_Recv(ref TLTR212 module, uint[] data, uint[] tmark, uint size, uint timeout);


        [DllImport("ltr212api.dll")]
        static extern _LTRNative.LTRERROR LTR212_ProcessData(ref TLTR212 module, uint[] src, double[] dest,
            ref int size, bool volt);

        [DllImport("ltr212api.dll")]
        static extern _LTRNative.LTRERROR LTR212_Calibrate(ref TLTR212 module, ref byte LChannel_Mask, CalibrModes mode, int reset);

        [DllImport("ltr212api.dll")]
        static extern _LTRNative.LTRERROR LTR212_CalcFS(ref TLTR212 module, out double fsBase, out double fs);

        [DllImport("ltr212api.dll")]
        static extern _LTRNative.LTRERROR LTR212_TestEEPROM(ref TLTR212 module);

        [DllImport("ltr212api.dll")]
        static extern _LTRNative.LTRERROR LTR212_ReadFilter(string fname, out FILTER filter);


        [DllImport("ltr212api.dll")]
        static extern uint LTR212_CalcTimeOut(ref TLTR212 module, int n);

        // функции вспомагательного характера
        [DllImport("ltr212api.dll")]
        static extern IntPtr LTR212_GetErrorString(int ErrorCode);



        public const int LTR212_LCH_CNT_MAX = 8;  // Макс. число логических. каналов
        public const int LTR212_FIR_ORDER_MAX = 255; // Максимальное значение порядка КИХ-фильтра
        public const int LTR212_FIR_ORDER_MIN = 3;   // Минимальное значение порядка КИХ-фильтра


        // модификации модуля
        public enum ModuleTypes : byte
        {
            OLD,     // старый модуль с поддержкой полно- и полу-мостовых подключений
            M_1,     // новый модуль с поддержкой полно-,  полу- и четверть-мостовых подключений
            M_2      // новый модуль с поддержкой полно- и полу-мостовых подключений
        };


        // типы возможных мостов
        public enum BridgeTypes : uint
        {
            FullOrHalf,
            Quarter_200_Ohm,
            Quarter_350_Ohm,
            Quarter_Custom_Ohm,
            UnbalancedQuarter_200_Ohm,
            UnbalancedQuarter_350_Ohm,
            UnbalancedQuarter_Custom_Ohm
        };

        // режимы сбора данных (AcqMode)
        public enum AcqModes : int
        {
            FourChannelsWithMediumResolution = 0,
            FourChannelsWithHighResolution = 1,
            EightChannelsWithHighResolution = 2
        };

        // значения опорного напряжения
        public enum RefVals : int
        {
            REF_2_5V = 0,  //2.5 В
            REF_5V = 1   //5   В
        };

        // диапазоны канало
        public enum Scales : int
        {
            B_10 = 0, /* диапазон -10мВ/+10мВ */
            B_20 = 1, /* диапазон -20мВ/+20мВ */
            B_40 = 2, /* диапазон -40мВ/+40мВ */
            B_80 = 3, /* диапазон -80мВ/+80мВ */
            U_10 = 4, /* диапазон -10мВ/+10мВ */
            U_20 = 5, /* диапазон -20мВ/+20мВ */
            U_40 = 6, /* диапазон -40мВ/+40мВ */
            U_80 = 7 /* диапазон -80мВ/+80мВ */
        };

        // режимы калибровки
        public enum CalibrModes : int
        {
            INT_ZERO = 0,
            INT_SCALE = 1,
            INT_FULL = 2,
            EXT_ZERO = 3,
            EXT_SCALE = 4,
            EXT_ZERO_INT_SCALE = 5,
            EXT_FULL_2ND_STAGE = 6, /* вторая стадия внешней калибровки */
            EXT_ZERO_SAVE_SCALE = 7  /* внешний ноль с сохранением до этого полученных коэф. масштаба */
        };

        [StructLayout(LayoutKind.Sequential, Pack=4)]
        public struct DLL_VERSION {
            byte revision_;                   
            byte build_;
            byte minor_;
            byte major_;

            public byte Revision { get { return revision_; } }
            public byte Build { get { return build_; } }
            public byte Minor { get { return minor_; } }
            public byte Major { get { return major_; } }
            public uint Value { get { return ((uint)major_ << 24) | ((uint)minor_ << 16) | ((uint)build_ << 8) | revision_; } }
            public String Str
            {
                get
                {
                    return major_.ToString() + "." + minor_.ToString() + "." +
                           build_.ToString() + "." + revision_.ToString();
                }
            }
        }
        

        // Структура, используеамая при загрузке фильтра
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct FILTER
        {
            double fs_;
            byte decimation_;
            byte taps_;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR212_FIR_ORDER_MAX)]
            short[] koeff_;

            public double fs { get { return fs_; } }
            public byte decimation { get { return decimation_; } }
            public byte taps { get { return taps_; } }
            public short[] koeff { get { return koeff_; } }
        };

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct INFO
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            char[] Name_;
            ModuleTypes Type_;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            char[] Serial_;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            char[] BiosVersion_;// Версия БИОСа
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            char[] BiosDate_;  // Дата создания данной версии БИОСа

            public string Name   { get { return new string(Name_).Split('\0')[0]; } }
            public ModuleTypes Type { get { return Type_; } }
            public string TypeStr
            {
                get
                {
                    return Type_ == ModuleTypes.M_1 ? "LTR212-M1" :
                        Type_ == ModuleTypes.M_2 ? "LTR212-M2" : "LTR212";
                }
            }
            public string Serial { get { return new string(Serial_).Split('\0')[0]; } }
            public string BiosVersion { get { return new string(BiosVersion_).Split('\0')[0]; } }
            public string BiosDate { get { return new string(BiosDate_).Split('\0')[0]; } }
        };

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        struct FILTER_CFG
        {
            bool IIR_;         // Флаг использования БИХ-фильтра
            bool FIR_;         // Флаг использования КИХ-фильтра
            int Decimation_;  // Значение коэффициента децимации для КИХ-фильтра
            int TAP_;		 // Порядок КИХ-фильтра 
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512 + 1)]
            char[] IIR_Name_; // Полный путь к файлу с коэфф-ми программного БИХ-фильтра 
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512 + 1)]
            char[] FIR_Name_; // Полный путь к файлу с коэфф-ми программного КИХ-фильтра

            public bool IIR { get { return IIR_; } set { IIR_ = value; } }
            public bool FIR { get { return FIR_; } set { FIR_ = value; } }
            public int Decimation { get { return Decimation_; } }
            public int TAP { get { return TAP_; } }

            public string IIR_Name
            {
                get { return new string(IIR_Name_).Split('\0')[0]; }
                set
                {
                    char[] arr = value.ToCharArray();
                    int i;
                    int len = Math.Min(arr.Length, 512);
                    for (i = 0; i < len; i++)
                        IIR_Name_[i] = arr[i];
                    IIR_Name_[len] = '\0';
                }
            }

            public string FIR_Name
            {
                get { return new string(FIR_Name_).Split('\0')[0]; }
                set
                {
                    char[] arr = value.ToCharArray();
                    int i;
                    int len = Math.Min(arr.Length, 512);
                    for (i = 0; i < len; i++)
                        FIR_Name_[i] = arr[i];
                    FIR_Name_[len] = '\0';
                }
            }

        } ;   // Структура, хранящая данные о прогр. фильтрах и их коэфф-ты.

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        struct TLTR212
        {
            int size_;
            _LTRNative.TLTR Channel_;
            AcqModes AcqMode_; // Режим сбора данных
            bool UseClb_;  // Флаг использования калибровочных коэфф-тов
            bool UseFabricClb_;// Флаг использования заводских калибровочных коэфф-тов
            int LChQnt_;	 // Кол-во используемых виртуальных каналов
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR212_LCH_CNT_MAX)]
            public int[] LChTbl;  //Таблица виртуальных каналов
            RefVals REF_;		 // Флаг высокого опорного напряжения
            bool AC_;		 // Флаг знакопеременного опорного напряжения
            double Fs_;     // Частота дискретизации АЦП

            public FILTER_CFG filter;   // Структура, хранящая данные о прогр. фильтрах и их коэфф-ты.

            INFO ModuleInfo_;

            ushort CRC_PM; // для служебного пользования
            ushort CRC_Flash_Eval; // для служебного пользования
            ushort CRC_Flash_Read;   // для служебного пользования


            public AcqModes AcqMode { get { return AcqMode_; } set { AcqMode_ = value; } }
            public bool UseClb { get { return UseClb_; } set { UseClb_ = value; } }
            public bool UseFabricClb { get { return UseFabricClb_; } set { UseFabricClb_ = value; } }
            public int LChQnt { get { return LChQnt_; } set { LChQnt_ = value; } }

            public RefVals REF { get { return REF_; } set { REF_ = value; } }
            public bool AC { get { return AC_; } set { AC_ = value; } }
            public double Fs { get { return Fs_; } }
            
            public INFO ModuleInfo { get { return ModuleInfo_; } }
        } // структура описания модуля 


        TLTR212 module;

        public ltr212api() 
        {
            LTR212_Init(ref module);	
        }

        ~ltr212api()
        {
            LTR212_Close(ref module);
        }


        public virtual _LTRNative.LTRERROR Open(uint saddr, ushort sport, string csn, ushort cc, string bios_file)
        {
            return LTR212_Open(ref module, saddr, sport, csn, cc, bios_file);
        }

        public virtual _LTRNative.LTRERROR Open(string csn, ushort cc, string bios_file)
        {
            return Open(_LTRNative.SADDR_DEFAULT, _LTRNative.SPORT_DEFAULT, csn, cc, bios_file);
        }

        public virtual _LTRNative.LTRERROR Open(ushort cc, string bios_file)
        {
            return Open("", cc, bios_file);
        }

        public virtual _LTRNative.LTRERROR Close()
        {
            return LTR212_Close(ref module);
        }

        public virtual _LTRNative.LTRERROR IsOpened()
        {
            return LTR212_IsOpened(ref module);
        }


        public void SetLChannel(uint lch, uint phy_ch, Scales scale)
        {
            module.LChTbl[lch] = LTR212_CreateLChannel(phy_ch, scale);
        }

        public void SetLChannel(uint lch, uint phy_ch, Scales scale, BridgeTypes BridgeType)
        {
            module.LChTbl[lch] = LTR212_CreateLChannel2(phy_ch, scale, BridgeType);
        }

        public virtual _LTRNative.LTRERROR SetADC()
        {
            return LTR212_SetADC(ref module);
        }

        public virtual _LTRNative.LTRERROR Start()
        {
            return LTR212_Start(ref module);
        }

        public virtual _LTRNative.LTRERROR Stop()
        {
            return LTR212_Stop(ref module);
        }

        public virtual int Recv(uint[] Data, uint size, uint[] tstamp, uint timeout)
        {
            return LTR212_Recv(ref module, Data, tstamp, size, timeout);
        }

        public virtual int Recv(uint[] Data, uint size, uint timeout)
        {
            return LTR212_Recv(ref module, Data, null, size, timeout);
        }

        public virtual _LTRNative.LTRERROR ProcessData(uint[] src, double[] dest,
            ref int size, bool volt)
        {
            return LTR212_ProcessData(ref module, src, dest, ref size, volt);
        }

        public static string GetErrorString(_LTRNative.LTRERROR err)
        {
            IntPtr ptr = LTR212_GetErrorString((int)err);
            string str = Marshal.PtrToStringAnsi(ptr);
            Encoding srcEncodingFormat = Encoding.GetEncoding("windows-1251");
            Encoding dstEncodingFormat = Encoding.UTF8;
            return dstEncodingFormat.GetString(Encoding.Convert(srcEncodingFormat, dstEncodingFormat, srcEncodingFormat.GetBytes(str)));
        }

        public virtual _LTRNative.LTRERROR Calibrate(ref byte LChannel_Mask, CalibrModes mode, bool reset)
        {
            return LTR212_Calibrate(ref module, ref LChannel_Mask, mode, reset ? 1 : 0);
        }


        public virtual _LTRNative.LTRERROR CalcFS(out double fsBase, out double fs)
        {
            return LTR212_CalcFS(ref module, out fsBase, out fs);
        }


        public virtual _LTRNative.LTRERROR TestEEPROM()
        {
            return LTR212_TestEEPROM(ref module);
        }

        public static _LTRNative.LTRERROR ReadFilter(string fname, out FILTER filter)
        {
            return LTR212_ReadFilter(fname, out filter);
        }


        public virtual uint CalcTimeOut(int n)
        {
            return LTR212_CalcTimeOut(ref module, n);
        }






        public AcqModes AcqMode { get { return module.AcqMode; } set { module.AcqMode = value; } }
        public bool UseClb { get { return module.UseClb; } set { module.UseClb = value; } }
        public bool UseFabricClb { get { return module.UseFabricClb; } set { module.UseFabricClb = value; } }
        public int LChQnt { get { return module.LChQnt; } set { module.LChQnt = value; } }

        public RefVals REF { get { return module.REF; } set { module.REF = value; } }
        public bool AC { get { return module.AC; } set { module.AC = value; } }
        public double Fs { get { return module.Fs; } }
        public INFO ModuleInfo { get { return module.ModuleInfo; } }


        public bool FilterIIR { get { return module.filter.IIR; } set { module.filter.IIR = value; } }
        public bool FilterFIR { get { return module.filter.FIR; } set { module.filter.FIR = value; } }
        public int FilterDecimation { get { return module.filter.Decimation; } }
        public int FilterTAP { get { return module.filter.TAP; } }
        public string FilterIIR_Name { get { return module.filter.IIR_Name; } set { module.filter.IIR_Name = value; } }
        public string FilterFIR_Name { get { return module.filter.FIR_Name; } set { module.filter.FIR_Name = value; } }

        public static DLL_VERSION DllVersion
        {
            get
            {
                DLL_VERSION ver;
                LTR212_GetDllVersion(out ver);
                return ver;
            }
        }
    }
}
