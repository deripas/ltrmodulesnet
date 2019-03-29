using System;
using System.Runtime.InteropServices;

namespace ltrModulesNet
{
    public class _ltr22api
    {
        [DllImport("ltr22api.dll")]
        public static extern _LTRNative.LTRERROR LTR22_Init(ref TLTR22 module);
        [DllImport("ltr22api.dll")]
        public static extern _LTRNative.LTRERROR LTR22_Close(ref TLTR22 module);
        [DllImport("ltr22api.dll")]
        public static extern _LTRNative.LTRERROR LTR22_Open(ref TLTR22 module, uint saddr, ushort sport, [In, Out] byte[] csn, ushort cc);
        [DllImport("ltr22api.dll")]
        public static extern _LTRNative.LTRERROR LTR22_IsOpened(ref TLTR22 module);
        [DllImport("ltr22api.dll")]
        public static extern _LTRNative.LTRERROR LTR22_GetConfig(ref TLTR22 module);
        [DllImport("ltr22api.dll")]
        public static extern _LTRNative.LTRERROR LTR22_SetConfig(ref TLTR22 module);
        [DllImport("ltr22api.dll")]
        public static extern _LTRNative.LTRERROR LTR22_ClearBuffer(ref TLTR22 module, bool wait_response);
        [DllImport("ltr22api.dll")]
        public static extern _LTRNative.LTRERROR LTR22_StartADC(ref TLTR22 module, bool WaitSync);
        [DllImport("ltr22api.dll")]
        public static extern _LTRNative.LTRERROR LTR22_StopADC(ref TLTR22 module);
        [DllImport("ltr22api.dll")]
        public static extern _LTRNative.LTRERROR LTR22_SetSyncPriority(ref TLTR22 module, bool SyncMaster);
        [DllImport("ltr22api.dll")]
        public static extern _LTRNative.LTRERROR LTR22_SyncPhaze(ref TLTR22 module, uint timeout);
        [DllImport("ltr22api.dll")]
        public static extern _LTRNative.LTRERROR LTR22_SwitchMeasureADCZero(ref TLTR22 module, bool SetMeasure);
        [DllImport("ltr22api.dll")]
        public static extern _LTRNative.LTRERROR LTR22_SetFreq(ref TLTR22 module, bool adc384, byte Freq_dv);
        [DllImport("ltr22api.dll")]
        public static extern _LTRNative.LTRERROR LTR22_SwitchACDCState(ref TLTR22 module, bool ACDCState);
        [DllImport("ltr22api.dll")]
        public static extern _LTRNative.LTRERROR LTR22_SetADCRange(ref TLTR22 module, byte ADCChannel, byte ADCChannelRange);
        [DllImport("ltr22api.dll")]
        public static extern _LTRNative.LTRERROR LTR22_SetADCChannel(ref TLTR22 module, byte ADCChannel, bool EnableADC);
        [DllImport("ltr22api.dll")]
        public static extern _LTRNative.LTRERROR LTR22_GetCalibrovka(ref TLTR22 module);
        [DllImport("ltr22api.dll")]
        public static extern _LTRNative.LTRERROR LTR22_Recv(ref TLTR22 module, uint[] data, uint[] tstamp, uint size, uint timeout);
        [DllImport("ltr22api.dll")]
        public static extern _LTRNative.LTRERROR LTR22_GetModuleDescription(ref TLTR22 module);
        [DllImport("ltr22api.dll")]
        public static extern _LTRNative.LTRERROR LTR22_ProcessData(ref TLTR22 module, uint[] src_data, double[] dst_data,
            uint size, bool calibrMainPset, bool calibrExtraVolts, byte[] OverflowFlags);
        [DllImport("ltr22api.dll"),]
        public static extern _LTRNative.LTRERROR LTR22_ReadAVREEPROM(ref TLTR22 module, byte[] Data, uint BeginAddress, uint size);
        [DllImport("ltr22api.dll")]
        public static extern _LTRNative.LTRERROR LTR22_WriteAVREEPROM(ref TLTR22 module, byte[] Data, uint BeginAddress, uint size);
        [DllImport("ltr22api.dll")]
        public static extern _LTRNative.LTRERROR LTR22_TestHardwareInterface(ref TLTR22 module);
        [DllImport("ltr22api.dll")]
        public static extern _LTRNative.LTRERROR LTR22_GetADCData(ref TLTR22 module, double[] Data, uint Size, uint time,
            bool calibrMainPset, bool calibrExtraVolts);
        [DllImport("ltr22api.dll")]
        public static extern _LTRNative.LTRERROR LTR22_ReopenModule(ref TLTR22 module);

        [DllImport("ltr22api.dll")]
        public static extern string LTR22_GetErrorString(int ErrorCode);

        public const int LTR22_ADC_NUMBERS = 4;
        public const int LTR22_ADC_CHANNELS = LTR22_ADC_NUMBERS;
        public const int LTR22_RANGE_NUMBER = 6;
        public const int LTR22_RANGE_OVERFLOW = 7;
        public const int LTR22_MAX_DISC_FREQ_NUMBER = 25;
        public int[] LTR22_DISK_FREQ_ARRAY = new int[LTR22_MAX_DISC_FREQ_NUMBER]
		{
			3472,  3720,   4006,  4340,  4734,  5208,  5580,  5787,
			6009,  6510,   7102,  7440,  7812,  8680,  9765,  10416, 
			11160, 13020, 15625,  17361, 19531, 26041, 39062, 52083, 
			78125
		};

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TINFO_LTR22
        {
            public _ltr010api.TDESCRIPTION_MODULE Description;	// описание модуля
            public _ltr010api.TDESCRIPTION_CPU CPU;				// описание AVR

        };

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct ADC_CHANNEL_CALIBRATION
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public float[] FactoryCalibOffset;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public float[] FactoryCalibScale;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public float[] UserCalibOffset;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public float[] UserCalibScale;
        } ;

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TLTR22
        {
            int Size;
            public _LTRNative.TLTR Channel;

            // настройки модуля            
            public byte Fdiv_rg;						// дивайзер частоты клоков 1..15
            [MarshalAs(UnmanagedType.U1)]
            public bool Adc384;						// дополнительный дивайзер частоты сэмплов true =3 false =2
            [MarshalAs(UnmanagedType.U1)]
            public bool AC_DC_State;					// состояние true =AC+DC false=AC 
            [MarshalAs(UnmanagedType.U1)]
            public bool MeasureADCZero;				// измерение Zero true - включено false - выключено
            [MarshalAs(UnmanagedType.U1)]
            public bool DataReadingProcessed;		// состояние считывания АЦП true-АЦП считывается false - нет

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] ADCChannelRange;// предел имзерений АЦП по каналам 0 - 1В 1 - 0.3В 2 - 0.1В 3 - 0.03В 4 - 10В 5 - 3В    

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] ChannelEnabled;		// Состояние каналов, включен - true выключен - false
            public int FreqDiscretizationIndex;

            public byte SyncType;		// Тип синхронизации 0 - внутренний старт по сигналу Go 
            //1 - фазировка модуля
            //2 - внешний старт
            //3 - резервировано  
            [MarshalAs(UnmanagedType.U1)]
            public bool SyncMaster;		// true - модуль генерит сигнал, false - модуль принимает синхросигнал

            public TINFO_LTR22 ModuleInfo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4 * 25)]
            public ADC_CHANNEL_CALIBRATION[] ADCCalibration;
        }

        public TLTR22 NewTLTR22
        {
            get
            {
                TLTR22 NewModule = new TLTR22();
                LTR22_Init(ref NewModule);
                return NewModule;
            }
        }

        public TLTR22 module;


        public _ltr22api()
        {
            module = NewTLTR22;
        }

        public virtual _LTRNative.LTRERROR Init()
        {
            return LTR22_Init(ref module);
        }


        public virtual _LTRNative.LTRERROR Close()
        {
            return LTR22_Close(ref module);
        }


        public virtual _LTRNative.LTRERROR Open(uint saddr, ushort sport, [In, Out] byte[] csn, ushort cc)
        {
            return LTR22_Open(ref module, saddr, sport, csn, cc);
        }

        public virtual _LTRNative.LTRERROR IsOpened()
        {
            return LTR22_IsOpened(ref module);
        }

        public virtual _LTRNative.LTRERROR GetConfig()
        {
            return LTR22_GetConfig(ref module);
        }

        public virtual _LTRNative.LTRERROR SetConfig()
        {
            return LTR22_SetConfig(ref module);
        }

        public virtual _LTRNative.LTRERROR ClearBuffer(bool wait_response)
        {
            return LTR22_ClearBuffer(ref module, wait_response);
        }

        public virtual _LTRNative.LTRERROR StartADC(bool WaitSync)
        {
            return LTR22_StartADC(ref module, WaitSync);
        }

        public virtual _LTRNative.LTRERROR StopADC()
        {
            return LTR22_StopADC(ref module);
        }

        public virtual _LTRNative.LTRERROR SetSyncPriority(bool SyncMaster)
        {
            return LTR22_SetSyncPriority(ref module, SyncMaster);
        }

        public virtual _LTRNative.LTRERROR SyncPhaze(uint timeout)
        {
            return LTR22_SyncPhaze(ref module, timeout);
        }

        public virtual _LTRNative.LTRERROR SwitchMeasureADCZero(bool SetMeasure)
        {
            return LTR22_SwitchMeasureADCZero(ref module, SetMeasure);
        }

        public virtual _LTRNative.LTRERROR SetFreq(bool adc384, byte Freq_dv)
        {
            return LTR22_SetFreq(ref module, adc384, Freq_dv);
        }

        public virtual _LTRNative.LTRERROR SwitchACDCState(bool ACDCState)
        {
            return LTR22_SwitchACDCState(ref module, ACDCState);
        }

        public virtual _LTRNative.LTRERROR SetADCRange(byte ADCChannel, byte ADCChannelRange)
        {
            return LTR22_SetADCRange(ref module, ADCChannel, ADCChannelRange);
        }

        public virtual _LTRNative.LTRERROR SetADCChannel(byte ADCChannel, bool EnableADC)
        {
            return LTR22_SetADCChannel(ref module, ADCChannel, EnableADC);
        }

        public virtual _LTRNative.LTRERROR GetCalibrovka()
        {
            return LTR22_GetCalibrovka(ref module);
        }

        public virtual _LTRNative.LTRERROR Recv([In, Out] uint[] data, uint size, uint[] tstamp, uint timeout)
        {
            return LTR22_Recv(ref module, data, tstamp, size, timeout);
        }

        public virtual _LTRNative.LTRERROR GetModuleDescription()
        {
            return LTR22_GetModuleDescription(ref module);
        }

        public virtual _LTRNative.LTRERROR ProcessData([In, Out]uint[] src_data, [In, Out] double[] dst_data,
             uint size, bool calibrMainPset, bool calibrExtraVolts, [In, Out] byte[] OverflowFlags)
        {
            return LTR22_ProcessData(ref module, src_data, dst_data, size, calibrMainPset, calibrExtraVolts,
                OverflowFlags);
        }

        public virtual _LTRNative.LTRERROR ReadAVREEPROM([In, Out] byte[] Data, uint BeginAddress, uint size)
        {
            return LTR22_ReadAVREEPROM(ref module, Data, BeginAddress, size);
        }

        public virtual _LTRNative.LTRERROR WriteAVREEPROM([In, Out] byte[] Data, uint BeginAddress, uint size)
        {
            return LTR22_WriteAVREEPROM(ref module, Data, BeginAddress, size);
        }

        public virtual _LTRNative.LTRERROR TestHardwareInterface()
        {
            return LTR22_TestHardwareInterface(ref module);
        }

        public virtual _LTRNative.LTRERROR GetADCData([In, Out] double[] Data, uint Size, uint time,
            bool calibrMainPset, bool calibrExtraVolts)
        {
            return LTR22_GetADCData(ref module, Data, Size, time, calibrMainPset, calibrExtraVolts);
        }

        public virtual _LTRNative.LTRERROR ReopenModule()
        {
            return LTR22_ReopenModule(ref module);
        }

        public virtual string GetErrorString(int err)
        {
            return _ltr22api.LTR22_GetErrorString(err);
        }
    }	
}
