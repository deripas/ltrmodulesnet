using System;
using System.Runtime.InteropServices;

namespace ltrModulesNet
{
    /// <summary>
    /// Summary description for LTR10Native.
    /// </summary>
    public class _ltr010api
    {

        public const int COMMENT_LENGTH = 256;
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TLTR10
        {
            public _LTRNative.TLTR Ltr;
        }

        [StructLayout(LayoutKind.Sequential, Pack=4)]
        public struct TDESCRIPTION_MODULE								//
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] CompanyName;                                  //
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] DeviceName;                                   // название изделия
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] SerialNumber;                                 // серийный номер изделия
            public byte Revision;                                       // ревизия изделия
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = COMMENT_LENGTH)]
            public char[] Comment;
        };
        // описание процессора и програмного обеспечения
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TDESCRIPTION_CPU									//
        {																//
            public byte Active;                                         // флаг достоверности остальных полей структуры
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] Name;                                         // название            
            public double ClockRate;                                    //
            public uint FirmwareVersion;                                //
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = COMMENT_LENGTH)]
            public char[] Comment;
        } ;
        // описание плис
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TDESCRIPTION_FPGA
        {																//
            public byte Active;                                         // флаг достоверности остальных полей структуры
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] Name;                                         // название
            public double ClockRate;                                    //
            public uint FirmwareVersion;                                //
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = COMMENT_LENGTH)]
            public char[] Comment;
        } ;
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TDESCRIPTION_INTERFACE							//
        {																//
            public byte Active;                                         // флаг достоверности остальных полей структуры
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] Name;                                         // название
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = COMMENT_LENGTH)]
            public char[] Comment;
        }; 
        
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TDESCRIPTION_MEZZANINE							//
        {																//
            public byte Active;                                         // флаг достоверности остальных полей структуры
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] Name;                                         // название изделия
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] SerialNumber;                                 // серийный номер изделия            
            public byte Revision;                                       // ревизия изделия
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public double []Calibration;                                       // корректировочные коэффициенты
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = COMMENT_LENGTH)]
            public char[] Comment;
        };                  
     

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TDESCRIPTION_LTR010
        {
            public TDESCRIPTION_MODULE Module;
            public TDESCRIPTION_CPU Cpu;
            public TDESCRIPTION_FPGA Fpga;
            public TDESCRIPTION_INTERFACE Interface;
        };

        //
        // загрузка AVR
        //
        [DllImport("ltr010api.dll")]
        public static extern _LTRNative.LTRERROR LTR010_Init(ref TLTR10 module);

        [DllImport("ltr010api.dll")]
        public static extern _LTRNative.LTRERROR LTR010_Open(ref TLTR10 module, uint saddr, ushort sport, byte [] csn);

        [DllImport("ltr010api.dll")]
        public static extern _LTRNative.LTRERROR LTR010_Close(ref TLTR10 module);

        [DllImport("ltr010api.dll")]
        public static extern _LTRNative.LTRERROR LTR010_GetArray(ref TLTR10 module, byte[] buf, uint size, uint address);

        [DllImport("ltr010api.dll")]
        public static extern _LTRNative.LTRERROR LTR010_PutArray(ref TLTR10 module, byte[] buf, uint size, uint address);

        [DllImport("ltr010api.dll")]
        public static extern _LTRNative.LTRERROR LTR010_GetDescription(ref TLTR10 module, ref TDESCRIPTION_LTR010 description);

        [DllImport("ltr010api.dll")]
        public static extern _LTRNative.LTRERROR LTR010_SetDescription(ref TLTR10 module, ref TDESCRIPTION_LTR010 description);

        /// <summary>
        /// Работа с сервером LTR10
        /// </summary>
        [DllImport("ltr010api.dll")]
        static extern _LTRNative.LTRERROR LTR010_LoadFPGA(ref TLTR10 module, char[] fname, byte rdma, byte wdma);

        public TLTR10 module;

        public TLTR10 NewModule
        {
            get
            {
                TLTR10 module1 = new TLTR10();
                LTR010_Init(ref module1);
                return module1;
            }
        }

        public _ltr010api()
        {
            module = NewModule;
        }
        
        public virtual _LTRNative.LTRERROR Init()
        {
            return LTR010_Init(ref module);
        }

        
        public virtual _LTRNative.LTRERROR Open(uint saddr, ushort sport, byte[] csn)
        {
            return LTR010_Open(ref module, saddr, sport, csn);
        }

        
        public virtual _LTRNative.LTRERROR Close()
        {
            return LTR010_Close(ref module);
        }

        
        public virtual _LTRNative.LTRERROR GetArray( byte[] buf, uint size, uint address)
        {
            return LTR010_GetArray(ref module, buf, size, address);
        }

        
        public virtual _LTRNative.LTRERROR PutArray( byte[] buf, uint size, uint address)
        {
            return LTR010_PutArray(ref module, buf, size, address);
        }

        
        public virtual _LTRNative.LTRERROR GetDescription(ref TDESCRIPTION_LTR010 description)
        {
            return LTR010_GetDescription(ref module, ref description);
        }
        
        public virtual _LTRNative.LTRERROR SetDescription(ref TDESCRIPTION_LTR010 description)
        {
            return LTR010_SetDescription(ref module, ref description);
        }

		public virtual _LTRNative.LTRERROR LoadFPGA(string Filename)
		{
			return LTR010_LoadFPGA(ref module, Filename.ToCharArray(),7,0);
		}

        

        public byte[] GetCrateRegisters()
        {           
            byte[] ReadData = new byte[16];            
            _ltr010api.LTR010_GetArray(ref module, ReadData, (uint)ReadData.Length, 0x8000 | 0x82000000);                           
            return ReadData;
        }

        public _LTRNative.LTRERROR SetServerPriority(_LTRNative.ServerPriority Priority)
        {            
            return _LTRNative.LTR_SetServerProcessPriority(ref module.Ltr, Priority);
        }
    }
}
