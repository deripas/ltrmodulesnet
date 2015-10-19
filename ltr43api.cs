using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ltrModulesNet
{
    public class ltr43api
    {
        [DllImport("ltr43api.dll")]
        static extern _LTRNative.LTRERROR LTR43_Init(ref TLTR43 module);

        [DllImport("ltr43api.dll")]
        static extern _LTRNative.LTRERROR LTR43_Open(ref TLTR43 module, uint net_addr, ushort net_port,
                                                            string crate_sn, int slot_num);

        [DllImport("ltr43api.dll")]
        static extern _LTRNative.LTRERROR LTR43_IsOpened(ref TLTR43 module);

        [DllImport("ltr43api.dll")]
        static extern _LTRNative.LTRERROR LTR43_Close(ref TLTR43 module);

        [DllImport("ltr43api.dll")]
        static extern _LTRNative.LTRERROR LTR43_WritePort(ref TLTR43 module, uint OutputData);

        [DllImport("ltr43api.dll")]
        static extern _LTRNative.LTRERROR LTR43_ReadPort(ref TLTR43 module, out uint InputData);

        [DllImport("ltr43api.dll")]
        static extern _LTRNative.LTRERROR LTR43_WriteArray(ref TLTR43 module, uint[] OutputArray,
            byte ArraySize);

        [DllImport("ltr43api.dll")]
        static extern _LTRNative.LTRERROR LTR43_StartStreamRead(ref TLTR43 module);

        [DllImport("ltr43api.dll")]
        static extern _LTRNative.LTRERROR LTR43_StopStreamRead(ref TLTR43 module);

        [DllImport("ltr43api.dll")]
        static extern int LTR43_Recv(ref TLTR43 module, uint[] data, uint[] tmark, uint size, uint timeout);

 
        [DllImport("ltr43api.dll")]
        static extern _LTRNative.LTRERROR LTR43_ProcessData(ref TLTR43 module, uint[] src, uint[] dest, ref int size);

        [DllImport("ltr43api.dll")]
        static extern _LTRNative.LTRERROR LTR43_Config(ref TLTR43 module);

        [DllImport("ltr43api.dll")]
        static extern _LTRNative.LTRERROR LTR43_StartSecondMark(ref TLTR43 module);

        [DllImport("ltr43api.dll")]
        static extern _LTRNative.LTRERROR LTR43_StopSecondMark(ref TLTR43 module);

        [DllImport("ltr43api.dll")]
        static extern _LTRNative.LTRERROR LTR43_MakeStartMark(ref TLTR43 module);


        [DllImport("ltr43api.dll")]
        static extern _LTRNative.LTRERROR LTR43_RS485_SetResponseTout(ref TLTR43 hnd, uint tout);

        [DllImport("ltr43api.dll")]
        static extern _LTRNative.LTRERROR LTR43_RS485_SetIntervalTout(ref TLTR43 hnd, uint tout);
        [DllImport("ltr43api.dll")]
        static extern _LTRNative.LTRERROR LTR43_RS485_SetTxActiveInterval(ref TLTR43 hnd, uint start_of_packet, uint end_of_packet);

        [DllImport("ltr43api.dll")]
        static extern _LTRNative.LTRERROR LTR43_RS485_Exchange(ref TLTR43 module, short[] PackToSend,
                                                                      short[] ReceivedPack, int OutPackSize,
                                                                      int InPackSize);
        [DllImport("ltr43api.dll")]
        static extern _LTRNative.LTRERROR LTR43_RS485_ExchangeEx(ref TLTR43 hnd, short[] PackToSend,
                                                                 short[] ReceivedPack, int OutPackSize, int InPackSize,
                                                                 RS485Flags flags, out int ReceivedSize);
        
        [DllImport("ltr43api.dll")]
        static extern _LTRNative.LTRERROR LTR43_WriteEEPROM(ref TLTR43 module, int Address, byte val);

        [DllImport("ltr43api.dll")]
        static extern _LTRNative.LTRERROR LTR43_ReadEEPROM(ref TLTR43 module, int Address, byte[] val);

        [DllImport("ltr43api.dll")]
        static extern IntPtr LTR43_GetErrorString(int ErrorCode);


        public const int LTR43_EEPROM_SIZE = 512;
        public const double LTR43_STREAM_READ_RATE_MIN = 100;
        public const double LTR43_STREAM_READ_RATE_MAX = 100000;

        public enum RS485Parity : int
        {
            NONE          = 0,
            EVEN          = 1,
            ODD           = 2
        }

        public enum MarkMode : int
        {
            INTERNAL = 0,
            MASTER = 1,
            EXTERNAL = 2
        }


        public enum PortDir : int
        {
            IN = 0,
            OUT = 1
        }

        [Flags]
        public enum RS485Flags : uint
        {
            UseIntervalTout = 1
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct INFO
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            char[] Name_;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            char[] Serial_;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            char[] FirmwareVersion_;// Версия БИОСа
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            char[] FirmwareDate_;  // Дата создания данной версии БИОСа

            public string Name { get { return new string(Name_).Split('\0')[0]; } }
            public string Serial { get { return new string(Serial_).Split('\0')[0]; } }
            public string FirmwareVersionStr { get { return new string(FirmwareVersion_).Split('\0')[0]; } }
            public string FirmwareDateStr { get { return new string(FirmwareDate_).Split('\0')[0]; } }
        };

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct IOPortsCfg
        {
            PortDir Port1_;	   // направление линий ввода/вывода группы 1
            PortDir Port2_;	   // направление линий ввода/вывода группы 2 
            PortDir Port3_;    // направление линий ввода/вывода группы 3 
            PortDir Port4_;	   // направление линий ввода/вывода группы 4 

            public PortDir Port1 { get { return Port1_; } set { Port1_ = value; } }
            public PortDir Port2 { get { return Port2_; } set { Port2_ = value; } }
            public PortDir Port3 { get { return Port3_; } set { Port3_ = value; } }
            public PortDir Port4 { get { return Port4_; } set { Port4_ = value; } }

        };

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct RS485Cfg
        {
            int FrameSize_;	  // Кол-во бит в кадре
            int Baud_;		  // Скорость обмена в бодах
            int StopBit_;	  // Кол-во стоп-бит
            RS485Parity Parity_;		  // Включение бита четности
            int SendTimeoutMultiplier_; // Множитель таймаута отправки
            int ReceiveTimeoutMultiplier_; // Множитель таймаута приема подтверждения

            public int FrameSize { get { return FrameSize_; } set { FrameSize_ = value; } }
            public int Baud { get { return Baud_; } set { Baud_ = value; } }
            public int StopBit { get { return StopBit_; } set { StopBit_ = value; } }
            public RS485Parity Parity { get { return Parity_; } set { Parity_ = value; } }
            public int SendTimeoutMultiplier { get { return SendTimeoutMultiplier_; } set { SendTimeoutMultiplier_ = value; } }
            public int ReceiveTimeoutMultiplier { get { return ReceiveTimeoutMultiplier_; } set { ReceiveTimeoutMultiplier_ = value; } }

        }; // Структура для конфигурации RS485

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct MarksCfg
        {
            MarkMode SecondMark_Mode_; // Режим меток. 0 - внутр., 1-внутр.+выход, 2-внешн
            MarkMode StartMark_Mode_; // 

            public MarkMode SecondMark_Mode { get { return SecondMark_Mode_; } set { SecondMark_Mode_ = value; } }
            public MarkMode StartMark_Mode { get { return StartMark_Mode_; } set { StartMark_Mode_ = value; } }
        };  // Структура для работы с временными метками		

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        struct TLTR43
        {
            int size;   // размер структуры    
            _LTRNative.TLTR Channel;

            double StreamReadRate_;

            IOPortsCfg IO_Ports_;
            RS485Cfg RS485_;
            MarksCfg Marks_;

            INFO ModuleInfo_;

            public double StreamReadRate { get { return StreamReadRate_; } set { StreamReadRate_ = value; } }
            public IOPortsCfg IO_Ports { get { return IO_Ports_; } set { IO_Ports_ = value; } }
            public RS485Cfg RS485 { get { return RS485_; } set { RS485_ = value; } }
            public MarksCfg Marks { get { return Marks_; } set { Marks_ = value; } }
            public INFO ModuleInfo { get { return ModuleInfo_; } }
        }; // Структура описания модуля



        TLTR43 module;

        public ltr43api() 
        {
            LTR43_Init(ref module);	
        }

        ~ltr43api()
        {
            LTR43_Close(ref module);
        }

		
		public virtual _LTRNative.LTRERROR Init()
		{
			return LTR43_Init(ref module);
		}

        public virtual _LTRNative.LTRERROR Open (uint saddr, ushort sport, string csn, ushort cc)
		{
			return LTR43_Open(ref module, saddr, sport, csn, cc);
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
			return LTR43_Close(ref module);
		}        

        public virtual _LTRNative.LTRERROR IsOpened ()
		{
			return LTR43_IsOpened(ref module);
		}


        public virtual _LTRNative.LTRERROR WritePort(uint OutputData)
        {
            return LTR43_WritePort(ref module, OutputData);
        }

        public virtual _LTRNative.LTRERROR ReadPort(out uint InputData)
        {
            return LTR43_ReadPort(ref module, out InputData);
        }

        public virtual _LTRNative.LTRERROR WriteArray(uint[] OutputArray, byte ArraySize)
        {
            return LTR43_WriteArray(ref module, OutputArray, ArraySize);
        }

        public virtual _LTRNative.LTRERROR StartStreamRead()
        {
            return LTR43_StartStreamRead(ref module);
        }

        public virtual _LTRNative.LTRERROR StopStreamRead()
        {
            return LTR43_StopStreamRead(ref module);
        }

        public virtual int Recv (uint [] Data, uint[] tstamp, uint size, uint timeout)
		{			
			return LTR43_Recv(ref module, Data, tstamp, size, timeout);
		}

        public virtual int Recv(uint[] Data, uint size, uint timeout)
        {
            return LTR43_Recv(ref module, Data, null, size, timeout);
        }

        public virtual _LTRNative.LTRERROR ProcessData(uint[] src, uint[] dest, ref int size)
        {
            return LTR43_ProcessData(ref module, src, dest, ref size);
        }

        public virtual _LTRNative.LTRERROR Config()
        {
            return LTR43_Config(ref module);
        }

        public virtual _LTRNative.LTRERROR StartSecondMark()
        {
            return LTR43_StartSecondMark(ref module);
        }

        public virtual _LTRNative.LTRERROR StopSecondMark()
        {
            return LTR43_StopSecondMark(ref module);
        }
                
        public virtual _LTRNative.LTRERROR MakeStartMark()
        {
            return LTR43_MakeStartMark(ref module);
        }

        public virtual _LTRNative.LTRERROR RS485_SetResponseTout(uint tout)
        {
            return LTR43_RS485_SetResponseTout(ref module, tout);
        }

        public virtual _LTRNative.LTRERROR RS485_SetIntervalTout(uint tout)
        {
            return LTR43_RS485_SetIntervalTout(ref module, tout);
        }
        
        public virtual _LTRNative.LTRERROR RS485_SetTxActiveInterval(uint start_of_packet, uint end_of_packet)
        {
            return LTR43_RS485_SetTxActiveInterval(ref module, start_of_packet, end_of_packet);
        }

        public virtual _LTRNative.LTRERROR RS485_Exchange(short[] PackToSend, short[] ReceivedPack, int OutPackSize, int InPackSize) 
        {
            return LTR43_RS485_Exchange(ref module, PackToSend, ReceivedPack, OutPackSize, InPackSize);
        }

        public virtual _LTRNative.LTRERROR RS485_ExchangeEx(short[] PackToSend, short[] ReceivedPack, int OutPackSize, int InPackSize,
                                                            RS485Flags flags, out int ReceivedSize)
        {
            return LTR43_RS485_ExchangeEx(ref module, PackToSend, ReceivedPack, OutPackSize, InPackSize,
                                          flags, out ReceivedSize);
        }

        public virtual _LTRNative.LTRERROR WriteEEPROM(int Address, byte val)
        {
            return LTR43_WriteEEPROM(ref module, Address, val);
        }

        public virtual _LTRNative.LTRERROR ReadEEPROM(int Address, byte[] val)
        {
            return LTR43_ReadEEPROM(ref module, Address, val);
        }

        public static string GetErrorString(_LTRNative.LTRERROR err)
        {
            IntPtr ptr = LTR43_GetErrorString((int)err);
            string str = Marshal.PtrToStringAnsi(ptr);
            Encoding srcEncodingFormat = Encoding.GetEncoding("windows-1251");
            Encoding dstEncodingFormat = Encoding.UTF8;
            return dstEncodingFormat.GetString(Encoding.Convert(srcEncodingFormat, dstEncodingFormat, srcEncodingFormat.GetBytes(str)));
        }

        public double StreamReadRate { get { return module.StreamReadRate; } set { module.StreamReadRate = value; } }
        public IOPortsCfg IO_Ports { get { return module.IO_Ports; } set { module.IO_Ports = value; } }
        public RS485Cfg RS485 { get { return module.RS485; } set { module.RS485 = value; } }
        public MarksCfg Marks { get { return module.Marks; } set { module.Marks = value; } }
        public INFO ModuleInfo { get { return module.ModuleInfo; } }
    }
}
