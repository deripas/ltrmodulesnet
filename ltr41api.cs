using System;
using System.Runtime.InteropServices;

namespace ltrModulesNet
{
    public class _ltr41api
    {
        [DllImport("ltr41api.dll")]
        public static extern _LTRNative.LTRERROR LTR41_Init(ref TLTR41 module);

        [DllImport("ltr41api.dll")]
        public static extern _LTRNative.LTRERROR LTR41_Open(ref TLTR41 module, uint net_addr, ushort net_port,
            char[] crate_sn, int slot_num);
        [DllImport("ltr41api.dll")]
        public static extern _LTRNative.LTRERROR LTR41_Close(ref TLTR41 module);

        [DllImport("ltr41api.dll")]
        public static extern _LTRNative.LTRERROR LTR41_IsOpened(ref TLTR41 module);

        [DllImport("ltr41api.dll")]
        public static extern _LTRNative.LTRERROR LTR41_ReadPort(ref TLTR41 module, ref ushort InputData);

        [DllImport("ltr41api.dll")]
        public static extern _LTRNative.LTRERROR LTR41_StartStreamRead(ref TLTR41 module);

        [DllImport("ltr41api.dll")]
        public static extern _LTRNative.LTRERROR LTR41_StopStreamRead(ref TLTR41 module);

        [DllImport("ltr41api.dll")]
        public static extern _LTRNative.LTRERROR LTR41_Recv(ref TLTR41 module, uint[] data, uint[] tmark, uint size, uint timeout);

        [DllImport("ltr41api.dll")]
        public static extern _LTRNative.LTRERROR LTR41_ProcessData(ref TLTR41 module, uint[] src, ushort[] dest, ref int size);

        [DllImport("ltr41api.dll")]
        public static extern _LTRNative.LTRERROR LTR41_Config(ref TLTR41 module);

        [DllImport("ltr41api.dll")]
        public static extern _LTRNative.LTRERROR LTR41_StartSecondMark(ref TLTR41 module);

        [DllImport("ltr41api.dll")]
        public static extern _LTRNative.LTRERROR LTR41_StopSecondMark(ref TLTR41 module);

        [DllImport("ltr41api.dll")]
        public static extern _LTRNative.LTRERROR LTR41_MakeStartMark(ref TLTR41 module);


        [DllImport("ltr41api.dll")]
        public static extern _LTRNative.LTRERROR LTR41_WriteEEPROM(ref TLTR41 module, int Address, byte val);

        [DllImport("ltr41api.dll")]
        public static extern _LTRNative.LTRERROR LTR41_ReadEEPROM(ref TLTR41 module, int Address, byte[] val);

        // функции вспомагательного характера
        [DllImport("ltr41api.dll")]
        public static extern string LTR41_GetErrorString(int ErrorCode);


        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TINFO_LTR41
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
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TLTR41
        {

            public int size;   // размер структуры    
            public _LTRNative.TLTR Channel;
            public double StreamReadRate;

            public _Marks Marks;  // Структура для работы с временными метками

            public TINFO_LTR41 ModuleInfo;
        }

        public TLTR41 NewTLTR41
        {
            get
            {
                TLTR41 NewModule = new TLTR41();
                LTR41_Init(ref NewModule);
                return NewModule;
            }
        }


        public TLTR41 module;

        public _ltr41api()
        {
            module = NewTLTR41;
        }


        public virtual _LTRNative.LTRERROR Init()
        {
            return LTR41_Init(ref module);
        }


        public virtual _LTRNative.LTRERROR Open( uint net_addr, ushort net_port,
            char[] crate_sn, int slot_num)
        {
			if (net_addr ==0) net_addr = NewTLTR41.Channel.saddr;
			if (net_port ==0) net_port = NewTLTR41.Channel.sport;

            return LTR41_Open(ref module, net_addr, net_port, crate_sn, slot_num);
        }

        public virtual _LTRNative.LTRERROR Close()
        {
            return LTR41_Close(ref module);
        }


        public virtual _LTRNative.LTRERROR IsOpened()
        {
            return LTR41_IsOpened(ref module);
        }


        public virtual _LTRNative.LTRERROR ReadPort(ref ushort InputData)
        {
            return LTR41_ReadPort(ref module, ref InputData);
        }


        public virtual _LTRNative.LTRERROR StartStreamRead()
        {
            return LTR41_StartStreamRead(ref module);
        }


        public virtual _LTRNative.LTRERROR StopStreamRead()
        {
            return LTR41_StartStreamRead(ref module);
        }


        public virtual _LTRNative.LTRERROR Recv( uint[] data, uint[] tmark, uint size, uint timeout) 
        {
            return LTR41_Recv(ref module, data, tmark, size, timeout);
        }


        public virtual _LTRNative.LTRERROR ProcessData( uint[] src, ushort[] dest, ref int size) 
        {
            return LTR41_ProcessData(ref module, src, dest, ref size);
        }


        public virtual _LTRNative.LTRERROR Config() 
        {
            return LTR41_Config(ref module);
        }


        public virtual _LTRNative.LTRERROR StartSecondMark() 
        {
            return LTR41_StartSecondMark(ref module);
        }


        public virtual _LTRNative.LTRERROR StopSecondMark() 
        {
            return LTR41_StopSecondMark(ref module);
        }

        public virtual _LTRNative.LTRERROR MakeStartMark() 
        {
            return LTR41_MakeStartMark(ref module);
        }
        
        public virtual _LTRNative.LTRERROR WriteEEPROM(int Address, byte val)
        {
            return LTR41_WriteEEPROM(ref module, Address, val);
        }


        public virtual _LTRNative.LTRERROR ReadEEPROM( int Address, byte[] val) 
        {
            return LTR41_ReadEEPROM(ref module, Address, val);
        }        

        public virtual string GetErrorString(int ErrorCode) 
        {
            return LTR41_GetErrorString(ErrorCode);
        }
    }
}
