using System;
using System.Runtime.InteropServices;

namespace ltrModulesNet
{
    public class _ltr42api
    {
        [DllImport("ltr42api.dll")]
        public static extern _LTRNative.LTRERROR LTR42_Init(ref TLTR42 module);

        [DllImport("ltr42api.dll")]
        public static extern _LTRNative.LTRERROR LTR42_Open(ref TLTR42 module, uint net_addr, ushort net_port,
            char[] crate_sn, int slot_num);

        [DllImport("ltr42api.dll")]
        public static extern _LTRNative.LTRERROR LTR42_Open(ref TLTR42 module, uint net_addr, ushort net_port,
            string crate_sn, int slot_num);

        [DllImport("ltr42api.dll")]
        public static extern _LTRNative.LTRERROR LTR42_Close(ref TLTR42 module);

        [DllImport("ltr42api.dll")]
        public static extern _LTRNative.LTRERROR LTR42_IsOpened(ref TLTR42 module);

        [DllImport("ltr42api.dll")]
        public static extern _LTRNative.LTRERROR LTR42_WritePort(ref TLTR42 module, ushort OutputData);

        [DllImport("ltr42api.dll")]
        public static extern _LTRNative.LTRERROR LTR42_WriteArray(ref TLTR42 module, ushort[] OutputData, int ArraySize);

        [DllImport("ltr42api.dll")]
        public static extern _LTRNative.LTRERROR LTR42_Config(ref TLTR42 module);

        [DllImport("ltr42api.dll")]
        public static extern _LTRNative.LTRERROR LTR42_StartSecondMark(ref TLTR42 module);

        [DllImport("ltr42api.dll")]
        public static extern _LTRNative.LTRERROR LTR42_StopSecondMark(ref TLTR42 module);

        [DllImport("ltr42api.dll")]
        public static extern _LTRNative.LTRERROR LTR42_MakeStartMark(ref TLTR42 module);


        [DllImport("ltr42api.dll")]
        public static extern _LTRNative.LTRERROR LTR42_WriteEEPROM(ref TLTR42 module, int Address, byte val);

        [DllImport("ltr42api.dll")]
        public static extern _LTRNative.LTRERROR LTR42_ReadEEPROM(ref TLTR42 module, int Address, byte[] val);

        // функции вспомагательного характера
        [DllImport("ltr42api.dll")]
        public static extern string LTR42_GetErrorString(int ErrorCode);

        [DllImport("ltr42api.dll")]
        public static extern _LTRNative.LTRERROR LTR42_ConfigAndStart(ref TLTR42 module);

        [DllImport("ltr42api.dll")]
        public static extern _LTRNative.LTRERROR LTR42_WritePortSaved(ref TLTR42 module, ushort OutputData);

        [DllImport("ltr42api.dll")]
        public static extern _LTRNative.LTRERROR LTR42_StoreConfig(ref TLTR42 module, _LTRNative.StartMode start_mode);


        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TINFO_LTR42
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] Name;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            public char[] Serial;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] FirmwareVersion;// Версия БИОСа
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] FirmwareDate;  // Дата создания данной версии БИОСа
        };

        public struct _Marks
        {
            public int SecondMark_Mode; // Режим меток. 0 - внутр., 1-внутр.+выход, 2-внешн
            public int StartMark_Mode; // 

        };  // Структура для работы с временными метками


        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TLTR42
        {            
            public _LTRNative.TLTR Channel;
            public int size;   // размер структуры    
            [MarshalAs(UnmanagedType.U1)]
            public bool AckEna;

            public _Marks Marks;  // Структура для работы с временными метками

            public TINFO_LTR42 ModuleInfo;
        }

        public TLTR42 NewTLTR42
        {
            get
            {
                TLTR42 NewModule = new TLTR42();
                LTR42_Init(ref NewModule);
                return NewModule;
            }
        }


        public TLTR42 module;

        public _ltr42api()
        {
            module = NewTLTR42;
        }



        public virtual _LTRNative.LTRERROR Init()
        {
            return LTR42_Init(ref module);
        }


        public virtual _LTRNative.LTRERROR Open(uint net_addr, ushort net_port,
            char[] crate_sn, int slot_num)
        {
			if (net_addr ==0) net_addr = NewTLTR42.Channel.saddr;
			if (net_port ==0) net_port = NewTLTR42.Channel.sport;

            return LTR42_Open(ref module, net_addr, net_port, crate_sn, slot_num);
        }

        public virtual _LTRNative.LTRERROR Close()
        {
            return LTR42_Close(ref module);
        }


        public virtual _LTRNative.LTRERROR IsOpened()
        {
            return LTR42_IsOpened(ref module);
        }


        public virtual _LTRNative.LTRERROR WritePort(ushort OutputData)
        {
            return LTR42_WritePort(ref module, OutputData);
        }


        public virtual _LTRNative.LTRERROR WriteArray(ushort[] OutputData, int ArraySize)
        {
            return LTR42_WriteArray(ref module, OutputData, ArraySize);
        }


        public virtual _LTRNative.LTRERROR Config()
        {
            return LTR42_Config(ref module);
        }


        public virtual _LTRNative.LTRERROR StartSecondMark()
        {
            return LTR42_StartSecondMark(ref module);
        }


        public virtual _LTRNative.LTRERROR StopSecondMark()
        {
            return LTR42_StopSecondMark(ref module);
        }


        public virtual _LTRNative.LTRERROR MakeStartMark()
        {
            return LTR42_MakeStartMark(ref module);
        }

        public virtual _LTRNative.LTRERROR WriteEEPROM(int Address, byte val)
        {
            return LTR42_WriteEEPROM(ref module, Address, val);
        }

        public virtual _LTRNative.LTRERROR ReadEEPROM(int Address, byte[] val)
        {
            return LTR42_ReadEEPROM(ref module, Address, val);
        }      

        public virtual string GetErrorString(int ErrorCode)
        {
            return LTR42_GetErrorString(ErrorCode);
        }


        public virtual _LTRNative.LTRERROR ConfigAndStart(ref TLTR42 module) 
        {
            return LTR42_ConfigAndStart(ref module);
        }

        public virtual _LTRNative.LTRERROR WritePortSaved(ref TLTR42 module, ushort OutputData)
        {
            return LTR42_WritePortSaved(ref module, OutputData);
        }

        public virtual _LTRNative.LTRERROR StoreConfig(ref TLTR42 module, _LTRNative.StartMode start_mode) 
        {
            return LTR42_StoreConfig(ref module, start_mode);
        }
    }
}
