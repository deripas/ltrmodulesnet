using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ltrModulesNet
{
    public class ltr22api
    {
        [DllImport("ltr22api.dll")]
        static extern _LTRNative.LTRERROR LTR22_Init(ref TLTR22 module);
        [DllImport("ltr22api.dll")]
        static extern _LTRNative.LTRERROR LTR22_Close(ref TLTR22 module);
        [DllImport("ltr22api.dll")]
        static extern _LTRNative.LTRERROR LTR22_Open(ref TLTR22 hnd, uint saddr, ushort sport, string csn, ushort slot_num);
        [DllImport("ltr22api.dll")]
        static extern _LTRNative.LTRERROR LTR22_IsOpened(ref TLTR22 module);
        [DllImport("ltr22api.dll")]
        static extern _LTRNative.LTRERROR LTR22_GetConfig(ref TLTR22 module);
        [DllImport("ltr22api.dll")]
        static extern _LTRNative.LTRERROR LTR22_SetConfig(ref TLTR22 module);
        [DllImport("ltr22api.dll")]
        static extern _LTRNative.LTRERROR LTR22_ClearBuffer(ref TLTR22 module, bool wait_response);
        [DllImport("ltr22api.dll")]
        static extern _LTRNative.LTRERROR LTR22_StartADC(ref TLTR22 module, bool WaitSync);
        [DllImport("ltr22api.dll")]
        static extern _LTRNative.LTRERROR LTR22_StopADC(ref TLTR22 module);
        [DllImport("ltr22api.dll")]
        static extern _LTRNative.LTRERROR LTR22_SetSyncPriority(ref TLTR22 module, bool SyncMaster);
        [DllImport("ltr22api.dll")]
        static extern _LTRNative.LTRERROR LTR22_SyncPhaze(ref TLTR22 module, uint timeout);
        [DllImport("ltr22api.dll")]
        static extern _LTRNative.LTRERROR LTR22_SwitchMeasureADCZero(ref TLTR22 module, bool SetMeasure);
        [DllImport("ltr22api.dll")]
        static extern _LTRNative.LTRERROR LTR22_SetFreq(ref TLTR22 module, bool adc384, byte Freq_dv);
        [DllImport("ltr22api.dll")]
        static extern _LTRNative.LTRERROR LTR22_SwitchACDCState(ref TLTR22 module, bool ACDCState);
        [DllImport("ltr22api.dll")]
        static extern _LTRNative.LTRERROR LTR22_SetADCRange(ref TLTR22 module, byte ADCChannel, byte ADCChannelRange);
        [DllImport("ltr22api.dll")]
        static extern _LTRNative.LTRERROR LTR22_SetADCChannel(ref TLTR22 module, byte ADCChannel, bool EnableADC);
        [DllImport("ltr22api.dll")]
        static extern _LTRNative.LTRERROR LTR22_GetCalibrCoeffs(ref TLTR22 module);
        [DllImport("ltr22api.dll")]
        static extern int LTR22_Recv(ref TLTR22 module, uint[] data, uint[] tstamp, uint size, uint timeout);
        [DllImport("ltr22api.dll")]
        static extern _LTRNative.LTRERROR LTR22_GetModuleDescription(ref TLTR22 module);
        [DllImport("ltr22api.dll")]
        static extern _LTRNative.LTRERROR LTR22_ProcessData(ref TLTR22 module, uint[] src_data, double[] dst_data,
            uint size, bool calibrMainPset, bool calibrExtraVolts, byte[] OverflowFlags);
        [DllImport("ltr22api.dll"),]
        static extern _LTRNative.LTRERROR LTR22_ReadAVREEPROM(ref TLTR22 module, byte[] Data, uint BeginAddress, uint size);
        [DllImport("ltr22api.dll")]
        static extern _LTRNative.LTRERROR LTR22_WriteAVREEPROM(ref TLTR22 module, byte[] Data, uint BeginAddress, uint size);
        [DllImport("ltr22api.dll")]
        static extern _LTRNative.LTRERROR LTR22_TestHardwareInterface(ref TLTR22 module);
        [DllImport("ltr22api.dll")]
        static extern _LTRNative.LTRERROR LTR22_ReopenModule(ref TLTR22 module);

        [DllImport("ltr22api.dll")]
        static extern _LTRNative.LTRERROR LTR22_FindAdcFreqParams(double AdcFreq, out byte divider, out byte adc384, out int adcFreqIndex, out double resultAdcFreq);

        [DllImport("ltr22api.dll")]
        static extern double LTR22_CalcAdcFreq(byte divider, bool adc384);

        [DllImport("ltr22api.dll")]
        static extern IntPtr LTR22_GetErrorString(int ErrorCode);

        public const int LTR22_CHANNEL_CNT = 4;
        public const int LTR22_RANGE_CNT = 6;
        public const int LTR22_ADC_FREQ_CNT = 25;
        

        public enum AdcRange : byte 
        {
            Range_1     = 0,
            Range_0_3   = 1,
            Range_0_1   = 2,
            Range_0_03  = 3,
            Range_10    = 4,
            Range_3     = 5
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TINFO_LTR22
        {
            public _LTRNative.TDESCRIPTION_MODULE Description;	// описание модуля
            public _LTRNative.TDESCRIPTION_CPU CPU;				// описание AVR

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
            _LTRNative.TLTR Channel;

            // настройки модуля            
            byte _Fdiv_rg;						// дивайзер частоты клоков 1..15
            [MarshalAs(UnmanagedType.U1)]
            bool _Adc384;						// дополнительный дивайзер частоты сэмплов true =3 false =2
            [MarshalAs(UnmanagedType.U1)]
            bool _AC_DC_State;					// состояние true =AC+DC false=AC 
            [MarshalAs(UnmanagedType.U1)]
            bool _MeasureADCZero;				// измерение Zero true - включено false - выключено
            [MarshalAs(UnmanagedType.U1)]
            bool _DataReadingProcessed;		// состояние считывания АЦП true-АЦП считывается false - нет

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            AdcRange[] _ADCChannelRange;// предел имзерений АЦП по каналам 0 - 1В 1 - 0.3В 2 - 0.1В 3 - 0.03В 4 - 10В 5 - 3В    

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            byte[] _ChannelEnabled;		// Состояние каналов, включен - true выключен - false
            int _FreqDiscretizationIndex;

            byte _SyncType;		// Тип синхронизации 0 - внутренний старт по сигналу Go 
            //1 - фазировка модуля
            //2 - внешний старт
            //3 - резервировано  
            [MarshalAs(UnmanagedType.U1)]
            bool _SyncMaster;		// true - модуль генерит сигнал, false - модуль принимает синхросигнал

            TINFO_LTR22 _ModuleInfo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4 * 25)]
            ADC_CHANNEL_CALIBRATION[] _ADCCalibration;


            public byte Fdiv { get { return _Fdiv_rg; } set { _Fdiv_rg = value; } }
            public bool Adc384 { get { return _Adc384; } set { _Adc384 = value; } }
            public bool ACDCState { get { return _AC_DC_State; } set { _AC_DC_State = value; } }
            public bool MeasureADCZero { get { return _MeasureADCZero; } set { _MeasureADCZero = value; } }
            public bool DataReadingProcessed { get { return _DataReadingProcessed; } }
            public AdcRange channelRange(int ch) { return _ADCChannelRange[ch]; }
            public void fillChannelRange(int ch, AdcRange range) { _ADCChannelRange[ch] = range; }
            public bool channelEnabled(int ch) { return _ChannelEnabled[ch] != 0; }
            public void fillChannelEnabled(int ch, bool en) { _ChannelEnabled[ch] = (byte)(en ? 1 : 0); }
            public int FreqDiscretizationIndex { get { return _FreqDiscretizationIndex; } set { _FreqDiscretizationIndex = value; } }
            public bool SyncMaster { get { return SyncMaster; } set { _SyncMaster = value; } }
            public TINFO_LTR22 ModuleInfo { get { return _ModuleInfo; } }


        }


        TLTR22 module;

        public ltr22api()
        {
            LTR22_Init(ref module);
        }


        public virtual _LTRNative.LTRERROR Close()
        {
            return LTR22_Close(ref module);
        }

        public virtual _LTRNative.LTRERROR Open(uint saddr, ushort sport, string csn, ushort slot_num)
        {
            return LTR22_Open(ref module, saddr, sport, csn, slot_num);
        }

        public virtual _LTRNative.LTRERROR Open(string csn, ushort slot_num)
        {
            return Open(_LTRNative.SADDR_DEFAULT, _LTRNative.SPORT_DEFAULT, csn, slot_num);
        }

        public virtual _LTRNative.LTRERROR Open(ushort slot_num)
        {
            return Open("", slot_num);
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

        public virtual _LTRNative.LTRERROR GetCalibrCoeffs()
        {
            return LTR22_GetCalibrCoeffs(ref module);
        }

        public int Recv(uint[] data, uint[] tmark, uint size, uint timeout)
        {
            return LTR22_Recv(ref module, data, tmark, size, timeout);
        }

        public int Recv(uint[] data, uint size, uint timeout)
        {
            return LTR22_Recv(ref module, data, null, size, timeout);
        }

        public virtual _LTRNative.LTRERROR GetModuleDescription()
        {
            return LTR22_GetModuleDescription(ref module);
        }

        public virtual _LTRNative.LTRERROR ProcessData(uint[] src_data, double[] dst_data,
             uint size, bool calibrMainPset, bool calibrExtraVolts, byte[] OverflowFlags)
        {
            return LTR22_ProcessData(ref module, src_data, dst_data, size, calibrMainPset, calibrExtraVolts,
                OverflowFlags);
        }

        public virtual _LTRNative.LTRERROR ProcessData(uint[] src_data, double[] dst_data,
             uint size, bool calibrMainPset, bool calibrExtraVolts)
        {
            return LTR22_ProcessData(ref module, src_data, dst_data, size, calibrMainPset, calibrExtraVolts,
                null);
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
               

        public virtual _LTRNative.LTRERROR ReopenModule()
        {
            return LTR22_ReopenModule(ref module);
        }

        public virtual _LTRNative.LTRERROR FindAdcFreqParams(double adcFreq, out double resultAdcFreq)
        {
            byte divider, adc384;
            int freq_idx;

            _LTRNative.LTRERROR err = LTR22_FindAdcFreqParams(adcFreq, out divider, out adc384, out freq_idx, out resultAdcFreq);
            if (err == _LTRNative.LTRERROR.OK)
            {
                module.Adc384 = adc384 != 0;
                module.Fdiv = divider;
            }
            return err;
        }

        public static string GetErrorString(_LTRNative.LTRERROR err)
        {
            IntPtr ptr = LTR22_GetErrorString((int)err);
            string str = Marshal.PtrToStringAnsi(ptr);
            Encoding srcEncodingFormat = Encoding.GetEncoding("windows-1251");
            Encoding dstEncodingFormat = Encoding.UTF8;
            return dstEncodingFormat.GetString(Encoding.Convert(srcEncodingFormat, dstEncodingFormat, srcEncodingFormat.GetBytes(str)));
        }


        public bool ACDCState { get { return module.ACDCState; } set { module.ACDCState = value; } }
        public bool MeasureADCZero { get { return module.MeasureADCZero; } set { module.MeasureADCZero = value; } }
        public bool DataReadingProcessed { get { return module.DataReadingProcessed; } }
        public AdcRange channelRange(int ch) { return module.channelRange(ch); }
        public void fillChannelRange(int ch, AdcRange range) { module.fillChannelRange(ch, range); }
        public bool channelEnabled(int ch) { return module.channelEnabled(ch); }
        public void fillChannelEnabled(int ch, bool en) { module.fillChannelEnabled(ch, en) ; }
        public int FreqDiscretizationIndex { get { return module.FreqDiscretizationIndex; } }
        public bool SyncMaster { get { return module.SyncMaster; } set { module.SyncMaster = value; } }
        public TINFO_LTR22 ModuleInfo { get { return module.ModuleInfo; } }

        public double AdcFreq
        {
            get
            {
                return LTR22_CalcAdcFreq(module.Fdiv, module.Adc384);
            }
            set
            {
                double resFreq;
                FindAdcFreqParams(value, out resFreq);
            }
        }

    }	
}
