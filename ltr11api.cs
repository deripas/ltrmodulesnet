using System;
using System.Runtime.InteropServices;

namespace ltrModulesNet
{
    public class _ltr11api
    {
        [DllImport("ltr11api.dll")]
        public static extern _LTRNative.LTRERROR LTR11_Init (ref TLTR11 module);

        [DllImport("ltr11api.dll")]
        public static extern _LTRNative.LTRERROR LTR11_Close (ref TLTR11 module);

        [DllImport("ltr11api.dll")]
        public static extern _LTRNative.LTRERROR LTR11_GetConfig (ref TLTR11 module);

        [DllImport("ltr11api.dll")]
        public static extern _LTRNative.LTRERROR LTR11_GetFrame (ref TLTR11 module, uint [] buf);

        [DllImport("ltr11api.dll")]
        public static extern _LTRNative.LTRERROR LTR11_Open (ref TLTR11 module, uint saddr, ushort sport, char[] csn, int slot_num);

                [DllImport("ltr11api.dll")]
        public static extern _LTRNative.LTRERROR LTR11_ProcessData (ref TLTR11 module, uint [] src, double [] dest, 
                                                ref uint size, bool calibr,
                                                bool valueConvert); 

        [DllImport("ltr11api.dll")]
        public static extern _LTRNative.LTRERROR LTR11_SetADC(ref TLTR11 module);

        [DllImport("ltr11api.dll")]
        public static extern _LTRNative.LTRERROR LTR11_Start(ref TLTR11 module);

        [DllImport("ltr11api.dll")]
        public static extern _LTRNative.LTRERROR LTR11_Stop(ref TLTR11 module);

        // функции вспомагательного характера
        [DllImport("ltr11api.dll")]
        public static extern string LTR11_GetErrorString(int ErrorCode);   

        const int LTR11_ADC_RANGEQNT=4;
        const int LTR11_MAX_LCHANNEL=128;

        public const int LTR11_STARTADCMODE_INT     = 0;
        public const int LTR11_STARTADCMODE_EXTRISE = 1;
        public const int LTR11_STARTADCMODE_EXTFALL = 2;

        public const int LTR11_INPMODE_EXTRISE = 0;
        public const int LTR11_INPMODE_EXTFALL = 1;
        public const int LTR11_INPMODE_INT     = 2;

        public const int LTR11_ADCMODE_ACQ         = 0x00;
        public const int LTR11_ADCMODE_TEST_U1P    = 0x04;
        public const int LTR11_ADCMODE_TEST_U1N    = 0x05;
        public const int LTR11_ADCMODE_TEST_U2N    = 0x06;
        public const int LTR11_ADCMODE_TEST_U2P    = 0x07;
 
        
        
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
            public double Offset;                      /* смещение нуля */
            public double Gain;                        /* масштабный коэффициент */
        }

        [StructLayout(LayoutKind.Sequential, Pack=4)]
        public struct TINFO_LTR11
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] Name;                          /* название модуля (строка) */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            public char[] Serial;                        /* серийный номер модуля (строка) */

            public ushort Ver;                               /* версия ПО модуля (младший байт - минорная,
                                             * старший - мажорная
                                             */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
            public char[] Date;                          /* дата создания ПО (строка) */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR11_ADC_RANGEQNT)]
            public CbrCoefStruct[] CbrCoef;	/* калибровочные коэффициенты для каждого диапазона */
        };

        [StructLayout(LayoutKind.Sequential, Pack=4)]
        public struct ADCRateStruct
        {
            public int divider;                        // делитель тактовой частоты модуля, значения: 2..65535
            public int prescaler;                      // пределитель тактовой частоты модуля: * 1, 8, 64, 256, 1024
        } ;    

        [StructLayout(LayoutKind.Sequential, Pack=4)]
        public struct TLTR11
        {
            public int size;                               /* размер структуры в байтах */
            public _LTRNative.TLTR Channel;                           /* описатель канала связи с модулем */

            public int StartADCMode;                       /* режим начала сбора данных:
                                             * LTR11_STARTADCMODE_INT     - внутренний старт (по
                                             *                              команде хоста);
                                             * LTR11_STARTADCMODE_EXTRISE - по фронту внешнего
                                             *                              сигнала;
                                             * LTR11_STARTADCMODE_EXTFALL - по спаду внешнего
                                             *                              сигнала.
                                             */
            public int InpMode;                            /* режим ввода данных с АЦП
                                             *  LTR11_INPMODE_INT     - внутренний запуск АЦП
                                             *                          (частота задается AdcRate)
                                             *  LTR11_INPMODE_EXTRISE - по фронту внешнего сигнала
                                             *  LTR11_INPMODE_EXTFALL - по спаду внешнего сигнала
                                             */
            public int LChQnt;                             /* число активных логических каналов (размер кадра) */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR11_MAX_LCHANNEL)]
            public byte[] LChTbl;                          /* управляющая таблица с активными логическими каналами
                                             * структура одного байта таблицы: MsbGGMMCCCCLsb
                                             *   GG   - входной диапазон:
                                             *          0 - +-10 В;
                                             *          1 - +-2.5 В;
                                             *          2 - +-0.625 В;
                                             *          3 - +-0.156В;
                                             *   MM   - режим:
                                             *          0 - 16-канальный, каналы 1-16;
                                             *          1 - измерение собственного напряжения
                                             *              смещения нуля;
                                             *          2 - 32-канальный, каналы 1-16;
                                             *          3 - 32-канальный, каналы 17-32;
                                             *   CCCC - номер физического канала:
                                             *          0 - канал 1 (17);
                                             *          . . .
                                             *          15 - канал 16 (32).
                                             */
            public int ADCMode;                            /* режим сбора данных или тип тестового режима */
            public ADCRateStruct ADCRate;
            public double ChRate;                          /* частота одного канала в кГц (период кадра) при
                                             * внутреннем запуске АЦП
                                             */
            public TINFO_LTR11 ModuleInfo;                 /* информация о модуле LTR11 */
        };

        public TLTR11 NewTLTR11
        {
            get
            {
                TLTR11 NewModule = new TLTR11();                 
                LTR11_Init(ref NewModule);
                return NewModule;
            }
        }
        

        public TLTR11 module;

        public _ltr11api()
        {
            module = NewTLTR11;
        }

		
		public virtual _LTRNative.LTRERROR Init ()
		{
			return LTR11_Init(ref module);
		}
		
		public virtual _LTRNative.LTRERROR Close ()
		{
			return LTR11_Close(ref module);
		}
		
		public virtual _LTRNative.LTRERROR GetConfig ()
		{
			return LTR11_GetConfig(ref module);
		}
		
		public virtual _LTRNative.LTRERROR GetFrame (uint [] buf)
		{
			return LTR11_GetFrame(ref module, buf);
		}
		
		public virtual _LTRNative.LTRERROR Open (uint saddr, ushort sport, char[] csn, int slot_num)
		{
			return LTR11_Open(ref module, saddr, sport, csn, slot_num);
		}

        public virtual int Recv(uint[] data,
                uint[] tmark, uint size, uint timeout)
        {
            return  _LTRNative.LTR_Recv(ref module.Channel, data, tmark, size, timeout);
        }
		
		public virtual _LTRNative.LTRERROR ProcessData (uint [] src, double [] dest, 
			ref uint size, bool calibr,
			bool valueMain)
		{
			return LTR11_ProcessData(ref module, src, dest, ref size, calibr, valueMain);
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

        public virtual string GetErrorString(int err)
        {
            return _ltr11api.LTR11_GetErrorString(err);
        }
	}
}
