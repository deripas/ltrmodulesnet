using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ltrModulesNet
{
    public class ltr11api
    {
        [DllImport("ltr11api.dll")]
        static extern _LTRNative.LTRERROR LTR11_Init (ref TLTR11 module);

        [DllImport("ltr11api.dll")]
        static extern _LTRNative.LTRERROR LTR11_Close (ref TLTR11 module);

        [DllImport("ltr11api.dll")]
        static extern _LTRNative.LTRERROR LTR11_GetConfig (ref TLTR11 module);

        [DllImport("ltr11api.dll")]
        static extern _LTRNative.LTRERROR LTR11_GetFrame (ref TLTR11 module, uint[] buf);

        [DllImport("ltr11api.dll")]
        static extern _LTRNative.LTRERROR LTR11_Open (ref TLTR11 module, uint saddr, ushort sport, string csn, int slot_num);

        [DllImport("ltr11api.dll")]
        static extern _LTRNative.LTRERROR LTR11_IsOpened(ref TLTR11 module);

        [DllImport("ltr11api.dll")]
        static extern _LTRNative.LTRERROR LTR11_ProcessData (ref TLTR11 module, uint[] src, double[] dest, 
                                                                    ref int size, bool calibr,
                                                                    bool volt); 

        [DllImport("ltr11api.dll")]
        static extern _LTRNative.LTRERROR LTR11_SetADC(ref TLTR11 module);

        [DllImport("ltr11api.dll")]
        static extern _LTRNative.LTRERROR LTR11_Start(ref TLTR11 module);

        [DllImport("ltr11api.dll")]
        static extern _LTRNative.LTRERROR LTR11_Stop(ref TLTR11 module);

        [DllImport("ltr11api.dll")]
        static extern int LTR11_Recv(ref TLTR11 hnd, uint[] buf, uint[] tmark, uint size, uint timeout); //Прием данных от модуля

        [DllImport("ltr11api.dll")]
        static extern byte LTR11_CreateLChannel(byte phy_ch, ChModes mode, ChRanges range);
        
        [DllImport("ltr11api.dll")]
        static extern _LTRNative.LTRERROR LTR11_FindAdcFreqParams(double adcFreq, 
            out int prescaler, out int devider, out double resultAdcFreq);

        [DllImport("ltr11api.dll")]
        static extern _LTRNative.LTRERROR LTR11_SearchFirstFrame(ref TLTR11 hnd, uint[] data, uint size,
                                                                 out uint index);

        // функции вспомагательного характера
        [DllImport("ltr11api.dll")]
        static extern IntPtr LTR11_GetErrorString(int ErrorCode);   

        public const int LTR11_CLOCK =  15000;
        public const int LTR11_MAX_CHANNEL = 32;
        public const int LTR11_ADC_RANGEQNT=4;
        public const int LTR11_MAX_LCHANNEL=128;

        public const int LTR11_MAX_ADC_DIVIDER=65535;
        public const int LTR11_MIN_ADC_DIVIDER = 2;

        public enum StartAdcModes : int
        {
            INT = 0,
            EXTRISE = 1,
            EXTFALL = 2
        }

        public enum InpModes : int
        {
            EXTRISE = 0,
            EXTFALL = 1,
            INT = 2
        }

        public enum AdcModes : int
        {
            ACQ = 0x00,
            TEST_U1P = 0x04,
            TEST_U1N = 0x05,
            TEST_U2N = 0x06,
            TEST_U2P = 0x07
        }


        /* Входные дипазоны каналов */
        public enum ChRanges : byte
        {
            Range_10000MV = 0, /* +-10 В (10000 мВ) */
            Range_2500MV = 1, /* +-2.5 В (2500 мВ) */
            Range_625MV = 2, /* +-0.625 В (625 мВ) */
            Range_156MV = 3 /* +-0.156 В (156 мВ) */
        }


        /* Режимы работы каналов */
        public enum ChModes : byte
        {
            CH16 = 0,
            CH32 = 1,
            DIFF = CH16, /* диф. подкл., 16 каналов */
            COMM = CH32, /* общая земля, 32 каналов */
            ZERO = 2     /* измерение нуля */
        }
        
        
        /* 
        параметры для задания частоты дискретизации АЦП
        частота рассчитывается по формуле:
        F = LTR11_CLOCK/(prescaler*(divider+1)
        ВНИМАНИЕ!!! Частота 400 кГц является особым случаем:
        для ее установки пределитель и делитель должны иметь
        следующие значения:
        prescaler = 1
        divider   = 36
        */

        [StructLayout(LayoutKind.Sequential, Pack=4)]
        public struct CbrCoefStruct
        {
            double Offset_;                      /* смещение нуля */
            double Gain_;                        /* масштабный коэффициент */

            public double Offset { get { return Offset_; } }
            public double Gain { get { return Gain_; } }
        }

        [StructLayout(LayoutKind.Sequential, Pack=4)]
        public struct TINFO_LTR11
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            char[] Name_;                          /* название модуля (строка) */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            char[] Serial_;                        /* серийный номер модуля (строка) */

            ushort Ver_;                               /* версия ПО модуля (младший байт - минорная,
                                             * старший - мажорная
                                             */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
            char[] Date_;                          /* дата создания ПО (строка) */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR11_ADC_RANGEQNT)]
            CbrCoefStruct[] CbrCoef_;	/* калибровочные коэффициенты для каждого диапазона */


            public string Name { get { return new string(Name_).Split('\0')[0]; } }
            public string Serial { get { return new string(Serial_).Split('\0')[0]; } }
            public ushort Ver { get { return Ver_; } }
            public string VerStr { get { return ((byte)((Ver_ & 0xFF00) >> 8)).ToString() + '.' + ((byte)(Ver_ & 0xFF)).ToString(); } }
            public string Date { get { return new string(Date_).Split('\0')[0]; } }

        };

        [StructLayout(LayoutKind.Sequential, Pack=4)]
        public struct ADCRateStruct
        {
            int divider_;                        // делитель тактовой частоты модуля, значения: 2..65535
            int prescaler_;                      // пределитель тактовой частоты модуля: * 1, 8, 64, 256, 1024

            public int divider { get { return divider_; } set { divider_ = value; } }
            public int prescaler { get { return prescaler_; } set { prescaler_ = value; } }
        } ;    

        [StructLayout(LayoutKind.Sequential, Pack=4)]
        struct TLTR11
        {
            int size;                               /* размер структуры в байтах */
            _LTRNative.TLTR Channel_;               /* описатель канала связи с модулем */

            StartAdcModes StartADCMode_;     /* режим начала сбора данных */
            InpModes InpMode_;                  /* режим ввода данных с АЦП */                                             
            int LChQnt_;                             /* число активных логических каналов (размер кадра) */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR11_MAX_LCHANNEL)]
            public byte[] LChTbl;                  /* управляющая таблица с активными логическими каналами */
            AdcModes ADCMode_;                            /* режим сбора данных или тип тестового режима */
            public ADCRateStruct ADCRate;
            double ChRate_;                          /* частота одного канала в кГц (период кадра) при
                                             * внутреннем запуске АЦП
                                             */
            TINFO_LTR11 ModuleInfo_;                 /* информация о модуле LTR11 */


            public StartAdcModes StartADCMode { get { return StartADCMode_; } set { StartADCMode_ = value; } }
            public InpModes InpMode { get { return InpMode_; } set { InpMode_ = value; } }
            public int LChQnt { get { return LChQnt_; } set { LChQnt_ = value; } }

            public AdcModes ADCMode { get { return ADCMode_; } set { ADCMode_ = value; } }
            public double ChRate { get { return ChRate_; } }
            public TINFO_LTR11 ModuleInfo { get { return ModuleInfo_; } }
        };             

        TLTR11 module;

        public ltr11api() 
        {
            LTR11_Init(ref module);	
        }

        ~ltr11api()
        {
            LTR11_Close(ref module);
        }

		
		public virtual _LTRNative.LTRERROR Init()
		{
			return LTR11_Init(ref module);
		}

        public virtual _LTRNative.LTRERROR Open (uint saddr, ushort sport, string csn, ushort cc)
		{
			return LTR11_Open(ref module, saddr, sport, csn, cc);
		}

        public virtual _LTRNative.LTRERROR Open(string csn, ushort cc)
        {
            return Open(_LTRNative.SADDR_DEFAULT, _LTRNative.SPORT_DEFAULT, csn, cc);
        }

        public virtual _LTRNative.LTRERROR Open(ushort cc)
        {
            return Open("", cc);
        }

        public virtual _LTRNative.LTRERROR Close ()
		{
			return LTR11_Close(ref module);
		}        

        public virtual _LTRNative.LTRERROR IsOpened ()
		{
			return LTR11_IsOpened(ref module);
		}
		
		public virtual _LTRNative.LTRERROR GetConfig ()
		{
			return LTR11_GetConfig(ref module);
		}
		
        public virtual _LTRNative.LTRERROR SetADC()
		{
			return LTR11_SetADC(ref module);
		}
		
		public virtual _LTRNative.LTRERROR Start()
		{
			return LTR11_Start(ref module);
		}
		
		public virtual _LTRNative.LTRERROR Stop()
		{
			return LTR11_Stop(ref module);
		}
                
		
		public virtual int Recv (uint [] Data, uint[] tstamp, uint size, uint timeout)
		{			
			return LTR11_Recv(ref module, Data, tstamp, size, timeout);
		}

        /* такой вариант оставлен только для совместимости .... */
        public virtual int Recv(uint[] Data, uint size, uint[] tstamp, uint timeout)
        {
            return LTR11_Recv(ref module, Data, tstamp, size, timeout);
        }

        public virtual int Recv(uint[] Data, uint size, uint timeout)
        {
            return LTR11_Recv(ref module, Data, null, size, timeout);
        }
		
		public virtual _LTRNative.LTRERROR ProcessData (uint [] src, double [] dest,
            ref int size, bool calibr, bool volt)
		{
            return LTR11_ProcessData(ref module, src, dest, ref size, calibr, volt);
		}

        public virtual _LTRNative.LTRERROR GetFrame(uint[] buf)
        {
            return LTR11_GetFrame(ref module, buf);
        }

        public virtual _LTRNative.LTRERROR FindAdcFreqParams(double adcFreq, out double resultAdcFreq)
        {
            int presc, divider;
            _LTRNative.LTRERROR err = LTR11_FindAdcFreqParams(adcFreq, out presc,
                out divider, out resultAdcFreq);
            if (err == _LTRNative.LTRERROR.OK)
            {
                module.ADCRate.prescaler = presc;
                module.ADCRate.divider = divider;
            }
            return err;
        }

        public virtual _LTRNative.LTRERROR FindAdcFreqParams(double adcFreq)
        {
            double tmp;
            return FindAdcFreqParams(adcFreq, out tmp);
        }

        public static string GetErrorString(_LTRNative.LTRERROR err)
        {
            IntPtr ptr = LTR11_GetErrorString((int)err);
            string str = Marshal.PtrToStringAnsi(ptr);
            Encoding srcEncodingFormat = Encoding.GetEncoding("windows-1251");
            Encoding dstEncodingFormat = Encoding.UTF8;
            return dstEncodingFormat.GetString(Encoding.Convert(srcEncodingFormat, dstEncodingFormat, srcEncodingFormat.GetBytes(str)));
        }

        public static byte CreateLChannel(byte phy_ch, ChModes mode, ChRanges range)
        {
            return LTR11_CreateLChannel(phy_ch, mode, range);
        }


        public void SetLChannel(byte lch, byte phy_ch, ChModes mode, ChRanges range) {
            module.LChTbl[lch] = LTR11_CreateLChannel(phy_ch, mode, range);
        }

        public _LTRNative.LTRERROR SearchFirstFrame(uint[] data, uint size, out uint index)
        {
            return LTR11_SearchFirstFrame(ref module, data, size, out index);
        }


        public StartAdcModes StartADCMode { get { return module.StartADCMode; } set {  module.StartADCMode = value; } }
        public InpModes InpMode { get { return module.InpMode; } set { module.InpMode = value; } }
        public int LChQnt { get { return module.LChQnt; } set { module.LChQnt = value; } }

        public AdcModes ADCMode { get { return module.ADCMode; } set { module.ADCMode = value; } }
        public ADCRateStruct ADCRate { get { return module.ADCRate; } set { module.ADCRate = value; } }
        public double ChRate { get { return module.ChRate; } }
        public TINFO_LTR11 ModuleInfo { get { return module.ModuleInfo; } }

        
	}
}
