using System;
using System.Runtime.InteropServices;

namespace ltrModulesNet
{
    public class _ltr51api
    {

        [DllImport("ltr51api.dll")]
        public static extern _LTRNative.LTRERROR LTR51_Init (ref TLTR51 module);

        [DllImport("ltr51api.dll")]
        public static extern _LTRNative.LTRERROR LTR51_Open(ref TLTR51 module, uint net_addr, ushort net_port,
                                                    char[] crate_sn, int slot_num, char [] ttf);

        [DllImport("ltr51api.dll")]
        public static extern _LTRNative.LTRERROR LTR51_IsOpened (ref TLTR51 module);

        [DllImport("ltr51api.dll")]
        public static extern _LTRNative.LTRERROR LTR51_Close (ref TLTR51 module);

        [DllImport("ltr51api.dll")]
        public static extern _LTRNative.LTRERROR LTR51_WriteEEPROM(ref TLTR51 module, int Address, byte val); 

        [DllImport("ltr51api.dll")]
        public static extern _LTRNative.LTRERROR LTR51_ReadEEPROM(ref TLTR51 module, int Address, byte[] val); 

        [DllImport("ltr51api.dll")]
        public static extern _LTRNative.LTRERROR LTR51_CreateLChannel(int PhysChannel, ref double HighThreshold, 
                                                            ref double LowThreshold, int ThresholdRange, int EdgeMode); 

        [DllImport("ltr51api.dll")]
        public static extern _LTRNative.LTRERROR LTR51_Config (ref TLTR51 module);

        [DllImport("ltr51api.dll")]
        public static extern _LTRNative.LTRERROR LTR51_Start (ref TLTR51 module);

        [DllImport("ltr51api.dll")]
        public static extern _LTRNative.LTRERROR LTR51_Stop (ref TLTR51 module);

        [DllImport("ltr51api.dll")]
        public static extern _LTRNative.LTRERROR LTR51_Recv(ref TLTR51 module, uint [] data, uint [] tmark, uint size, uint timeout);     		
        
        [DllImport("ltr51api.dll")]
        public static extern _LTRNative.LTRERROR LTR51_ProcessData(ref TLTR51 module, uint [] src, uint [] dest, double [] Frequency, ref int size);   

        [DllImport("ltr51api.dll")]
        public static extern _LTRNative.LTRERROR LTR51_GetThresholdVals(ref TLTR51 module, int LChNumber, 
                                                    double [] HighThreshold, double [] LowThreshold, int ThresholdRange);

        [DllImport("ltr51api.dll")]
        public static extern uint LTR51_CalcTimeOut(ref TLTR51 module, int n);

        [DllImport("ltr51api.dll")]
        public static extern _LTRNative.LTRERROR LTR51_EvaluateFrequencies(ref TLTR51 module);

         // функции вспомагательного характера
        [DllImport("ltr51api.dll")]
        public static extern string LTR51_GetErrorString(int ErrorCode);
        
        [StructLayout(LayoutKind.Sequential, Pack=4)]
        public struct TINFO_LTR51
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] Name;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            public char[] Serial;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] FirmwareVersion;// Версия БИОСа
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] FirmwareDate;  // Дата создания данной версии БИОСа
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] FPGA_Version;  // Версия прошивки ПЛИС
        };

		[StructLayout(LayoutKind.Sequential, Pack=4)]
        public struct TLTR51
        {

            public int size;               // размер структуры

            public _LTRNative.TLTR Channel;
            public ushort ChannelsEna;       // Маска доступных каналов (показывает, какие субмодули подкл.)

            public int SetUserPars;	   // Указывает, задаются ли Fs и Base пользователем

            public int LChQnt;             // Количество логических каналов    
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public uint[] LChTbl;       // Таблица логических каналов

            public double Fs;                // Частота выборки сэмплов
            public ushort Base;                // Делитель частоты измерения
            public double F_Base;			 // Частота измерений F_Base=Fs/Base


            public int AcqTime;            // Время сбора в миллисекундах       
            public int TbaseQnt;		   // Количество периодов измерений, необходимое для обеспечения указанного интревала измерения

            public TINFO_LTR51 ModuleInfo;
        }; // Структура описания модуля

        public TLTR51 NewTLTR51
        {
            get
            {
                TLTR51 NewModule = new TLTR51();                 
                LTR51_Init(ref NewModule);
                return NewModule;
            }
        }
        

        public TLTR51 module;

        public _ltr51api()
        {
            module = NewTLTR51;
        }

		
		public virtual _LTRNative.LTRERROR Init ()
		{
			return LTR51_Init(ref module);
		}

		
		public virtual _LTRNative.LTRERROR Open( uint net_addr, ushort net_port,
			char[] crate_sn, int slot_num, string ttf)
		{
			if (net_addr ==0) net_addr = NewTLTR51.Channel.saddr;
			if (net_port ==0) net_port = NewTLTR51.Channel.sport;
			return LTR51_Open(ref module, net_addr, net_port, crate_sn, slot_num, ttf.ToCharArray());
		}

		
		public virtual _LTRNative.LTRERROR IsOpened ()
		{
			return LTR51_IsOpened(ref module);
		}

		
		public virtual _LTRNative.LTRERROR Close ()
		{
			return LTR51_Close(ref module);
		}
		
		public virtual _LTRNative.LTRERROR WriteEEPROM(int Address, byte val)
		{
			return LTR51_WriteEEPROM(ref module, Address, val);
		} 

		
		public virtual _LTRNative.LTRERROR ReadEEPROM(int Address, byte[] val)
		{
			return LTR51_ReadEEPROM(ref module, Address, val);
		} 

		
		public virtual _LTRNative.LTRERROR CreateLChannel(int PhysChannel, ref double HighThreshold, 
			ref double LowThreshold, int ThresholdRange, int EdgeMode)
		{
			return LTR51_CreateLChannel(PhysChannel, ref HighThreshold, ref LowThreshold, ThresholdRange,
				EdgeMode);
		} 

		
		public  virtual _LTRNative.LTRERROR Config ()
		{
			return LTR51_Config(ref module);
		}

		
		public virtual _LTRNative.LTRERROR Start ()
		{
			return LTR51_Start(ref module);
		}

		
		public virtual _LTRNative.LTRERROR Stop ()
		{
			return LTR51_Stop(ref module);
		}

		
		public virtual _LTRNative.LTRERROR Recv(uint [] data, uint [] tmark, uint size, uint timeout)
		{
			return LTR51_Recv(ref module, data, tmark, size, timeout);
		}     
        
		
		public virtual _LTRNative.LTRERROR ProcessData(uint [] src, uint [] dest, double [] Frequency, ref int size)
		{
			return LTR51_ProcessData(ref module, src, dest, Frequency, ref size);
		}   

		
		public virtual _LTRNative.LTRERROR GetThresholdVals(int LChNumber, 
			double [] HighThreshold, double [] LowThreshold, int ThresholdRange)
		{
			return LTR51_GetThresholdVals(ref module, LChNumber, HighThreshold, LowThreshold, ThresholdRange);
		}

		
		public virtual uint CalcTimeOut(int n)
		{
			return LTR51_CalcTimeOut(ref module, n);
		}

		
		public virtual _LTRNative.LTRERROR EvaluateFrequencies()
		{
			return LTR51_EvaluateFrequencies(ref module);
		}

        public virtual string GetErrorString(int err)
        {
            return _ltr51api.LTR51_GetErrorString(err);
        }
    }	
}
