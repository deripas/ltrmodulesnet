using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ltrModulesNet
{
    public class ltr24api
    {
        [DllImport("ltr24api.dll")]
        public static extern _LTRNative.LTRERROR LTR24_Init(ref TLTR24 module);
        [DllImport("ltr24api.dll")]
        public static extern _LTRNative.LTRERROR LTR24_Close(ref TLTR24 module);
        [DllImport("ltr24api.dll")]
        public static extern _LTRNative.LTRERROR LTR24_Open(ref TLTR24 module, uint saddr, ushort sport, string csn, int slot_num);
        [DllImport("ltr24api.dll")]
        public static extern _LTRNative.LTRERROR LTR24_IsOpened(ref TLTR24 module);
        [DllImport("ltr24api.dll")]
        public static extern _LTRNative.LTRERROR LTR24_GetConfig(ref TLTR24 module);
        [DllImport("ltr24api.dll")]
        public static extern _LTRNative.LTRERROR LTR24_SetADC(ref TLTR24 module);
        [DllImport("ltr24api.dll")]
        public static extern _LTRNative.LTRERROR LTR24_Start(ref TLTR24 module);
        [DllImport("ltr24api.dll")]
        public static extern _LTRNative.LTRERROR LTR24_Stop(ref TLTR24 module);
        [DllImport("ltr24api.dll")]
        public static extern _LTRNative.LTRERROR LTR24_SetZeroMode(ref TLTR24 module, bool enable);
        [DllImport("ltr24api.dll")]
        public static extern _LTRNative.LTRERROR LTR24_SetACMode(ref TLTR24 module, byte chan, bool ac_mode);
        [DllImport("ltr24api.dll")]
        public static extern int LTR24_Recv(ref TLTR24 hnd, uint[] buf, uint[] tmark, uint size, uint timeout); //ѕрием данных от модул€		
        [DllImport("ltr24api.dll")]
        public static extern int LTR24_RecvEx(ref TLTR24 ltr, uint[] data, uint[] tmark, uint size, uint timeout,
                                  UInt64[] unixtime);
        [DllImport("ltr24api.dll")]
        public static extern _LTRNative.LTRERROR LTR24_ProcessData(ref TLTR24 hnd, uint[] src_data, double[] dst_data, ref int size,
                  ProcFlags flags, bool[] ovload);
        [DllImport("ltr24api.dll")]
        public static extern IntPtr LTR24_GetErrorString(int err);
      
        [DllImport("ltr24api.dll")]
        public static extern _LTRNative.LTRERROR LTR24_FindFrameStart(ref TLTR24 module, uint[] data, int size, out int index);

        public const uint LTR24_VERSION_CODE = 0x02000000;
        public const int LTR24_CHANNEL_NUM = 4;
        public const int LTR24_RANGE_NUM = 2;
        public const int LTR24_ICP_RANGE_NUM = 2;
        public const int LTR24_FREQ_NUM = 16;
        public const int LTR24_I_SRC_VALUE_NUM = 2;
        public const int LTR24_NAME_SIZE = 8;
        public const int LTR24_SERIAL_SIZE = 16;


        /* „астоты сбора данных. */
        public enum FreqCode : byte 
        {
            Freq_117K = 0, //117.1875 к√ц
            Freq_78K = 1, // 78.125 к√ц
            Freq_58K = 2, // 58.59375 к√ц
            Freq_39K = 3, // 39.0625 к√ц
            Freq_29K = 4, // 29.296875 к√ц
            Freq_19K = 5, // 19.53125 к√ц
            Freq_14K = 6, // 14.6484375 к√ц
            Freq_9K7 = 7, // 9.765625 к√ц
            Freq_7K3 = 8, // 7.32421875 к√ц
            Freq_4K8 = 9, // 4.8828125 к√ц
            Freq_3K6 = 10, // 3.662109375 к√ц
            Freq_2K4 = 11, // 2.44140625 к√ц
            Freq_1K8 = 12, // 1.8310546875 к√ц
            Freq_1K2 = 13, // 1.220703125 к√ц
            Freq_915 = 14, // 915.52734375 √ц
            Freq_610 = 15  // 610.3515625 √ц
        }
        

        public enum AdcRange : byte 
        {
            Range_2=0,
            Range_10=1,
            ICP_Range_1 = Range_2,
            ICP_Range_5 = Range_10
        }

        public enum ISrcValues : byte
        {
            I_2_86 = 0,
            I_10   = 1
        }

        /* –азр€дность данных. */
        public enum DataFormat : byte 
        {
            Format20=0,
            Format24=1
        }

        [Flags]
        public enum ProcFlags : uint
        {
            Calibr = 0x00000001,
            Volt = 0x00000002,
            AfcCor = 0x00000004,
            NoncontData = 0x00000100
        }


        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct AFC_IIR_COEF
        {
            double _a0;
            double _a1;
            double _b0;

            public double a0 { get { return a0; } }
            public double a1 { get { return a1; } }
            public double b0 { get { return b0; } }
        };

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct AFC_COEFS
        {
            double _afc_freq;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR24_CHANNEL_NUM * LTR24_RANGE_NUM)]
            double[] _fir_coef;
            AFC_IIR_COEF _afc_iir_coef;
        }


        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct CHANNEL_MODE
        {
            bool _Enable;
            AdcRange _Range;
            bool _AC;
            bool _ICP;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            uint[] Reserved;

            public CHANNEL_MODE(bool enable, AdcRange range, bool ac, bool icp)
            {
                _Enable = enable; _Range = range; _AC = ac; _ICP = icp;
                Reserved = new uint[4];
                for (int i = 0; i < Reserved.Length; i++)
                    Reserved[i] = 0;
            }

            public AdcRange Range { get { return _Range; } set { _Range = value; } }
            public bool Enable { get { return _Enable; } set { _Enable = value; } }
            public bool AC { get { return _AC; } set { _AC = value; } }
            public bool ICPMode { get { return _ICP; } set { _ICP = value; } }
        };

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct CbrCoef
        {
            float Offset;
            float Scale;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct INFO
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR24_NAME_SIZE)]
            char[] _name;  
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR24_SERIAL_SIZE)]
            char[] _serial; 
            byte _VerPLD;
            bool _SupportICP;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            uint[] Reserved;


            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR24_CHANNEL_NUM*LTR24_RANGE_NUM*LTR24_FREQ_NUM)]
            CbrCoef[] CalibCoef;
            AFC_COEFS _AfcCoef;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR24_CHANNEL_NUM * LTR24_I_SRC_VALUE_NUM)]
            double[] _ISrcVals;

            public string Name { get { return new string(_name).TrimEnd('\0'); } }
            public string Serial { get { return new string(_serial).TrimEnd('\0'); } }
            public byte VerPLD { get { return _VerPLD; } }
            public bool SupportICP { get { return _SupportICP; } }

            public double getISrcValue(int ch, ISrcValues val) {return _ISrcVals[ch*LTR24_I_SRC_VALUE_NUM + (int)val];}
        };


        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TLTR24
        {
            int Size;
            _LTRNative.TLTR _channel;
            bool _Run;
            FreqCode _ADCFreqCode;
            double _ADCFreq;
            DataFormat _DataFmt;
            ISrcValues _ISrcValue;            
            bool _TestMode;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            uint[] Reserved;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR24_CHANNEL_NUM)]
            CHANNEL_MODE[] _ChannelMode;

            INFO _ModuleInfo;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR24_CHANNEL_NUM*LTR24_RANGE_NUM*LTR24_FREQ_NUM)]
            public CbrCoef[] CalibCoef;
            public AFC_COEFS AfcCoef;
            IntPtr Internal;

            
            public bool Run { get { return _Run; } }
            public FreqCode ADCFreqCode { get { return _ADCFreqCode; } set { _ADCFreqCode = value; } }
            public double ADCFreq { get { return _ADCFreq; } }
            public DataFormat DataFmt { get { return _DataFmt; } set { _DataFmt = value; } }
            public ISrcValues ISrcValue { get { return _ISrcValue; } set { _ISrcValue = value; } }
            public bool TestMode { get { return _TestMode; } set { _TestMode = value; } }
            public CHANNEL_MODE[] ChannelMode { get { return _ChannelMode; } set { _ChannelMode = value; } }
            public INFO ModuleInfo { get { return _ModuleInfo; } }
                        
            public _LTRNative.TLTR Channel { get { return _channel; } }
            
        };


        public TLTR24 module;


        public ltr24api()
        {
            LTR24_Init(ref module);
        }

        /* в финализаторе убеждаемс€, что остановили поток и 
         * закрыли модуль */
        ~ltr24api()
        {
            if (IsOpened() == _LTRNative.LTRERROR.OK)
            {
                if (Run)
                    Stop();
                Close();
            }
        }

       

        public virtual _LTRNative.LTRERROR Close()
        {
            return LTR24_Close(ref module);
        }

        public virtual _LTRNative.LTRERROR Open(uint saddr, ushort sport, string csn, int slot_num)
        {
            return LTR24_Open(ref module, saddr, sport, csn, slot_num);
        }

        public virtual _LTRNative.LTRERROR Open(string csn, int slot_num)
        {
            return Open(_LTRNative.SADDR_DEFAULT, _LTRNative.SPORT_DEFAULT, csn, slot_num);
        }

        public virtual _LTRNative.LTRERROR Open(int slot_num)
        {
            return Open("", slot_num);
        }

        public virtual _LTRNative.LTRERROR IsOpened()
        {
            return LTR24_IsOpened(ref module);
        }

        public virtual _LTRNative.LTRERROR GetConfig()
        {
            return LTR24_GetConfig(ref module);
        }

        public virtual _LTRNative.LTRERROR SetADC()
        {
            return LTR24_SetADC(ref module);
        }

        public virtual _LTRNative.LTRERROR Start()
        {
            return LTR24_Start(ref module);
        }

        public virtual _LTRNative.LTRERROR Stop()
        {
            return LTR24_Stop(ref module);
        }


        public virtual _LTRNative.LTRERROR SetZeroMode(bool enable)
        {
            return LTR24_SetZeroMode(ref module, enable);
        }

        public virtual _LTRNative.LTRERROR SetACMode(byte chan, bool ac_mode)
        {
            return LTR24_SetACMode(ref module, chan, ac_mode);
        }

        public virtual int Recv(uint[] buffer, uint size, uint timeout)
        {
            return LTR24_Recv(ref module, buffer, null, size, timeout);
        }

  
        public virtual int Recv(uint[] buffer, uint[] tmark, uint size, uint timeout)
        {
            return LTR24_Recv(ref module, buffer, tmark, size, timeout);
        }

        public virtual int RecvEx(uint[] buffer, uint[] tmark, uint size, uint timeout, UInt64[] unixtime)
        {
            return LTR24_RecvEx(ref module, buffer, tmark, size, timeout, unixtime);
        }

        public virtual _LTRNative.LTRERROR ProcessData(uint[] src, double[] dest, ref int size, ProcFlags flags, bool[] ovload)
        {
            return LTR24_ProcessData(ref module, src, dest, ref size, flags, ovload);
        }

        public virtual _LTRNative.LTRERROR ProcessData(uint[] src, double[] dest, ref int size, ProcFlags flags)
        {
            return LTR24_ProcessData(ref module, src, dest, ref size, flags, null);
        }
               

        public virtual _LTRNative.LTRERROR FindFrameStart(uint[] data, int size, out int index)
        {
            return LTR24_FindFrameStart(ref module, data, size, out index);
        }

        public static string GetErrorString(_LTRNative.LTRERROR err)
        {
            IntPtr ptr = LTR24_GetErrorString((int)err);
            string str = Marshal.PtrToStringAnsi(ptr);
            Encoding srcEncodingFormat = Encoding.GetEncoding("windows-1251");
            Encoding dstEncodingFormat = Encoding.UTF8;
            return dstEncodingFormat.GetString(Encoding.Convert(srcEncodingFormat, dstEncodingFormat, srcEncodingFormat.GetBytes(str)));
        }

        public bool Run {get {return module.Run;}}

        public FreqCode AdcFreqCode
        {
            get { return module.ADCFreqCode; }
            set { module.ADCFreqCode = value; }
        }

        public double AdcFreq
        {
            get { return module.ADCFreq; }
        }

        public DataFormat DataFmt
        {
            get { return module.DataFmt; }
            set { module.DataFmt = value; }
        }

        public ISrcValues ISrcValue
        {
            get { return module.ISrcValue; }
            set { module.ISrcValue = value; }
        }

        public bool TestMode
        {
            get { return module.TestMode; }
            set
            {
                if (!module.Run)
                    module.TestMode = value;
                else
                {
                    if (SetZeroMode(value) != _LTRNative.LTRERROR.OK)
                        throw new SystemException();
                }
            }
        }

        public CHANNEL_MODE[] ChannelMode
        {
            get { return module.ChannelMode; }
            set { module.ChannelMode = value; }
        }

        public INFO ModuleInfo
        {
            get {return module.ModuleInfo;}
        }

        public int EnabledChannelCnt
        {
            get
            {
                int ch_cnt = 0;
                for (int i = 0; i < LTR24_CHANNEL_NUM; i++)
                {
                    if (ChannelMode[i].Enable)
                        ch_cnt++;
                }
                return ch_cnt;
            }
        }
    }

    //дл€ совместимости со старым объ€влением
    public class _ltr24api : ltr24api
    {

    }
}
