using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ltrModulesNet
{
    public class _ltr24api
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
        public static extern _LTRNative.LTRERROR LTR24_Phase(ref TLTR24 module);
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
        public static extern int LTR24_Recv(ref TLTR24 hnd, uint[] buf, uint[] tmark, uint size, uint timeout); //Прием данных от модуля		
        [DllImport("ltr24api.dll")]
        public static extern _LTRNative.LTRERROR LTR24_ProcessData(ref TLTR24 hnd, uint[] src_data, double[] dst_data, ref int size,
                  bool calib, bool volt, bool[] ovload);
        [DllImport("ltr24api.dll")]
        public static extern IntPtr LTR24_GetErrorString(int err);
      

        [DllImport("ltr24api.dll")]
        public static extern _LTRNative.LTRERROR LTR24_StoreMcs(ref TLTR24 module);
        [DllImport("ltr24api.dll")]
        public static extern _LTRNative.LTRERROR LTR24_RestoreMcs(ref TLTR24 module, uint saddr, ushort sport, string csn, int slot_num);
        [DllImport("ltr24api.dll")]
        public static extern _LTRNative.LTRERROR LTR24_ClearMcsSlot(ref TLTR24 module);
        [DllImport("ltr24api.dll")]
        public static extern _LTRNative.LTRERROR LTR24_InvalidateMcsSlot(ref TLTR24 module);
        [DllImport("ltr24api.dll")]
        public static extern _LTRNative.LTRERROR LTR24_FindFrameStart(ref TLTR24 module, uint[] data, int size, out int index);

        public const int LTR24_CHANNEL_NUM = 4;
        public const int LTR24_RANGE_NUM = 2;
        public const int LTR24_FREQ_NUM = 8;
        public const int LTR24_NAME_SIZE = 8;
        public const int LTR24_SERIAL_SIZE = 16;


        /* Частоты сбора данных. */
        public enum FreqCode : byte 
        {
            Freq_117=0,
            Freq_78=1,
            Freq_58=2,
            Freq_39=3,
            Freq_29=4,
            Freq_19=5,
            Freq_14=6,
            Freq_9=7
        }
        
        public enum Sync : byte 
        {
            None = 0,
            Master = 1,
            Slave = 2
        }

        public enum AdcRange : byte 
        {
            Range_2=0,
            Range_10=1
        }

        /* Разрядность данных. */
        public enum DataFormat : byte 
        {
            Format20=0,
            Format24=1
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct Channel
        {
            bool _Enable;
            AdcRange _Range;
            bool _AC;

            public Channel(bool enable, AdcRange range, bool ac)
            {
                _Enable = enable; _Range = range; _AC = ac;
            }

            public AdcRange Range { get { return _Range; } set { _Range = value; } }
            public bool Enable { get { return _Enable; } set { _Enable = value; } }
            public bool AC { get { return _AC; } set { _AC = value; } }            

        };

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct CbrCoef
        {
            float Offset;
            float Scale;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct Info
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR24_NAME_SIZE)]
            char[] _name;  
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR24_NAME_SIZE)]
            public char[] _serial; 
            byte _VerPLD;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR24_CHANNEL_NUM*LTR24_RANGE_NUM*LTR24_FREQ_NUM)]
            CbrCoef[] CalibCoef;

            public string Name { get { return new string(_name).TrimEnd('\0'); } }
            public string Serial { get { return new string(_serial).TrimEnd('\0'); } }
            public byte VerPLD { get { return _VerPLD; } }
        };


        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TLTR24
        {
            int Size;
            public _LTRNative.TLTR Channel;
            public bool Run;
            public FreqCode ADCFreqCode;
            public double ADCFreq;
            public DataFormat DataFmt;
            public Sync SyncMode;
            public bool ZeroMode;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR24_CHANNEL_NUM)]
            public Channel[] ChannelMode;

            public Info ModuleInfo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR24_CHANNEL_NUM*LTR24_RANGE_NUM*LTR24_FREQ_NUM)]
            public CbrCoef[] CalibCoef;
            public IntPtr       Reserved;
        };


        public TLTR24 module;


        public _ltr24api()
        {
            LTR24_Init(ref module);
        }

        /* в финализаторе убеждаемся, что остановили поток и 
         * закрыли модуль */
        ~_ltr24api()
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

        public virtual _LTRNative.LTRERROR Phase()
        {
            return LTR24_Phase(ref module);
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

        public virtual _LTRNative.LTRERROR ProcessData(uint[] src, double[] dest, ref int size, bool calib, bool volt, bool[] ovload)
        {
            return LTR24_ProcessData(ref module, src, dest, ref size, calib, volt, ovload);
        }

        public virtual _LTRNative.LTRERROR StoreMcs()
        {
            return LTR24_StoreMcs(ref module);
        }

        public virtual _LTRNative.LTRERROR RestoreMcs(uint saddr, ushort sport, string serial, int slot_num)
        {
            return LTR24_RestoreMcs(ref module, saddr, sport, serial, slot_num);
        }

        public virtual _LTRNative.LTRERROR StoreMcs(string serial, int slot_num)
        {
            return RestoreMcs(_LTRNative.SADDR_DEFAULT, _LTRNative.SPORT_DEFAULT, serial, slot_num);
        }

        public virtual _LTRNative.LTRERROR StoreMcs(int slot_num)
        {
            return RestoreMcs(_LTRNative.SADDR_DEFAULT, _LTRNative.SPORT_DEFAULT, "", slot_num);
        }

        public virtual _LTRNative.LTRERROR ClearMcsSlot()
        {
            return LTR24_ClearMcsSlot(ref module);
        }

        public virtual _LTRNative.LTRERROR InvalidateMcsSlot()
        {
            return LTR24_InvalidateMcsSlot(ref module);
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

        public Sync SyncMode
        {
            get { return module.SyncMode; }
            set { module.SyncMode = value; }
        }

        public bool ZeroMode
        {
            get { return module.ZeroMode; }
            set
            {
                if (!module.Run)
                    module.ZeroMode = value;
                else
                {
                    if (SetZeroMode(value) != _LTRNative.LTRERROR.OK)
                        throw new SystemException();
                }
            }
        }

        public Channel[] ChannelMode
        {
            get { return module.ChannelMode; }
        }

        public Info ModuleInfo
        {
            get {return module.ModuleInfo;}
        }
    }	
}
