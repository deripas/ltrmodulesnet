using System;
using System.Runtime.InteropServices;

namespace ltrModulesNet
{
    public class _ltr43api
    {
		[DllImport("ltr43api.dll")]
		public static extern _LTRNative.LTRERROR LTR43_Init (ref TLTR43 module);

		[DllImport("ltr43api.dll")]
		public static extern _LTRNative.LTRERROR LTR43_Open(ref TLTR43 module, uint net_addr, ushort net_port,
			char[] crate_sn, int slot_num);

        [DllImport("ltr43api.dll")]
        public static extern _LTRNative.LTRERROR LTR43_IsOpened(ref TLTR43 module);

		[DllImport("ltr43api.dll")]
		public static extern _LTRNative.LTRERROR LTR43_Close(ref TLTR43 module);

		[DllImport("ltr43api.dll")]
		public static extern _LTRNative.LTRERROR LTR43_WritePort(ref TLTR43 module, uint OutputData);

		[DllImport("ltr43api.dll")]
		public static extern _LTRNative.LTRERROR LTR43_ReadPort(ref TLTR43 module, ref uint InputData);

		[DllImport("ltr43api.dll")]
		public static extern _LTRNative.LTRERROR LTR43_WriteArray(ref TLTR43 module, uint[] OutputArray, 
			byte ArraySize);  

		[DllImport("ltr43api.dll")]
		public static extern _LTRNative.LTRERROR LTR43_StartStreamRead(ref TLTR43 module);

		[DllImport("ltr43api.dll")]
		public static extern _LTRNative.LTRERROR LTR43_StopStreamRead(ref TLTR43 module);

		[DllImport("ltr43api.dll")]
		public static extern _LTRNative.LTRERROR LTR43_Recv(ref TLTR43 module, uint [] data, uint[] tmark, uint size, uint timeout); 		

		[DllImport("ltr43api.dll")]
		static extern _LTRNative.LTRERROR LTR43_Recv(ref TLTR43 module, uint [] data, uint tmark, uint size, uint timeout); 

		[DllImport("ltr43api.dll")]
		public static extern _LTRNative.LTRERROR LTR43_ProcessData(ref TLTR43 module, uint [] src, uint [] dest, ref int size);   

		[DllImport("ltr43api.dll")]
		public static extern _LTRNative.LTRERROR LTR43_Config(ref TLTR43 module);

		[DllImport("ltr43api.dll")]
		public static extern _LTRNative.LTRERROR LTR43_StartSecondMark(ref TLTR43 module);

		[DllImport("ltr43api.dll")]
		public static extern _LTRNative.LTRERROR LTR43_StopSecondMark(ref TLTR43 module);

		[DllImport("ltr43api.dll")]
		public static extern _LTRNative.LTRERROR LTR43_MakeStartMark(ref TLTR43 module);

		[DllImport("ltr43api.dll")]
		public static extern _LTRNative.LTRERROR LTR43_RS485_Exchange(ref TLTR43 module, short [] PackToSend, 
			short [] ReceivedPack, int OutPackSize, 
			int InPackSize);            
		[DllImport("ltr43api.dll")]
		public static extern _LTRNative.LTRERROR LTR43_WriteEEPROM(ref TLTR43 module, int Address, byte val); 

		[DllImport("ltr43api.dll")]
		public static extern _LTRNative.LTRERROR LTR43_ReadEEPROM(ref TLTR43 module, int Address, byte [] val); 

		[DllImport("ltr43api.dll")]
		public static extern _LTRNative.LTRERROR LTR43_RS485_TestReceiveByte(ref TLTR43 module, int OutBytesQnt, int InBytesQnt);    

		[DllImport("ltr43api.dll")]
		public static extern _LTRNative.LTRERROR LTR43_RS485_TestStopReceive(ref TLTR43 module);

		// функции вспомагательного характера
		[DllImport("ltr43api.dll")]
		public static extern string LTR43_GetErrorString(int ErrorCode);
              

        [StructLayout(LayoutKind.Sequential, Pack=4)]
        public struct TINFO_LTR43
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

        [StructLayout(LayoutKind.Sequential, Pack=4)]
        public struct IO_PortsStruct
        {

            public int Port1;	   // направление линий ввода/вывода группы 1
            public int Port2;	   // направление линий ввода/вывода группы 2 
            public int Port3;    // направление линий ввода/вывода группы 3 
            public int Port4;	   // направление линий ввода/вывода группы 4 

        };

        [StructLayout(LayoutKind.Sequential, Pack=4)]
        public struct RS485Struct
        {

            public int FrameSize;	  // Кол-во бит в кадре
            public int Baud;		  // Скорость обмена в бодах
            public int StopBit;	  // Кол-во стоп-бит
            public int Parity;		  // Включение бита четности
            public int SendTimeoutMultiplier; // Множитель таймаута отправки
            public int ReceiveTimeoutMultiplier; // Множитель таймаута приема подтверждения

        }; // Структура для конфигурации RS485

        [StructLayout(LayoutKind.Sequential, Pack=4)]
        public struct MarksStruct
        {

            public int SecondMark_Mode; // Режим меток. 0 - внутр., 1-внутр.+выход, 2-внешн
            public int StartMark_Mode; // 

        };  // Структура для работы с временными метками		

        [StructLayout(LayoutKind.Sequential, Pack=4)]
        public struct TLTR43
        {

            public int size;   // размер структуры    
            public _LTRNative.TLTR Channel;

            public double StreamReadRate;

            public IO_PortsStruct IO_Ports;
            public RS485Struct RS485;
            public MarksStruct Marks;

            public TINFO_LTR43 ModuleInfo; 
        }; // Структура описания модуля


        public TLTR43 NewTLTR43
        {
            get
            {
                TLTR43 NewModule = new TLTR43();                 
                LTR43_Init(ref NewModule);
                return NewModule;
            }
        }
        

        public TLTR43 module;

        public _ltr43api()
        {
            module = NewTLTR43;		
        }

		
		public virtual _LTRNative.LTRERROR Init ()
		{ 
			return LTR43_Init(ref module);
		}

		
		public virtual _LTRNative.LTRERROR Open(uint net_addr, ushort net_port,
			char[] crate_sn, int slot_num)
		{ 
			if (net_addr ==0) net_addr = NewTLTR43.Channel.saddr;
			if (net_port ==0) net_port = NewTLTR43.Channel.sport;
			return LTR43_Open(ref module, net_addr, net_port, crate_sn, slot_num);
		}
		
		public virtual _LTRNative.LTRERROR Close()
		{ 
			return LTR43_Close(ref module);
		}

		
		public virtual _LTRNative.LTRERROR WritePort(uint OutputData)
		{ 
			return LTR43_WritePort(ref module, OutputData);
		}


        public virtual _LTRNative.LTRERROR ReadPort(ref uint InputData)
		{ 
			return LTR43_ReadPort(ref module, ref InputData);
		}

		
		public virtual _LTRNative.LTRERROR WriteArray(uint []OutputArray, 
			byte ArraySize)
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

		
		public virtual _LTRNative.LTRERROR Recv(uint [] data, uint size, uint [] tstamp, uint timeout)
		{ 			
			return LTR43_Recv(ref module, data, tstamp, size, timeout);
		} 

		
		public virtual _LTRNative.LTRERROR ProcessData(uint [] src, uint [] dest, ref int size)
		{ 
			return LTR43_ProcessData(ref module, src, dest, ref size);
		}   

		
		public virtual _LTRNative.LTRERROR Config()
		{ 
			return LTR43_Config(ref module);
		}

		
		public virtual _LTRNative.LTRERROR StartSecondMark()
		{ 
			return LTR43_StopSecondMark(ref module);
		}

		
		public virtual _LTRNative.LTRERROR StopSecondMark()
		{ 
			return LTR43_StopSecondMark(ref module);
		}

		
		public virtual _LTRNative.LTRERROR MakeStartMark()
		{ 
			return LTR43_MakeStartMark(ref module);
		}

		
		public virtual _LTRNative.LTRERROR RS485_Exchange(short [] PackToSend, 
			short [] ReceivedPack, int OutPackSize, 
			int InPackSize)
		{ 
			return LTR43_RS485_Exchange(ref module, PackToSend, ReceivedPack, OutPackSize, InPackSize);
		}            
		
		public virtual _LTRNative.LTRERROR WriteEEPROM(int Address, byte val)
		{ 
			return LTR43_WriteEEPROM(ref module, Address, val);
		} 

		
		public virtual _LTRNative.LTRERROR ReadEEPROM(int Address, byte [] val)
		{ 
			return LTR43_ReadEEPROM(ref module, Address, val);
		} 

		
		public virtual _LTRNative.LTRERROR RS485_TestReceiveByte(int OutBytesQnt, int InBytesQnt)
		{ 
			return LTR43_RS485_TestReceiveByte(ref module, OutBytesQnt, InBytesQnt);
		}    

		
		public virtual _LTRNative.LTRERROR RS485_TestStopReceive()
		{ 
			return LTR43_RS485_TestStopReceive(ref module);
		}

        public virtual string GetErrorString(int err)
        {
            return _ltr43api.LTR43_GetErrorString(err);
        }

    }
}
