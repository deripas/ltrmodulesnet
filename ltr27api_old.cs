using System;
using System.Runtime.InteropServices;

namespace ltrModulesNet
{
    public class _ltr27api
    {
        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_Init (ref TLTR27 module);

        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_Open (ref TLTR27 module, uint saddr, ushort sport, char[] csn, ushort cc);

        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_Close (ref TLTR27 module);

        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_IsOpened (ref TLTR27 module);

        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_Echo (ref TLTR27 module);

        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_GetConfig (ref TLTR27 module);

        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_SetConfig (ref TLTR27 module);

        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_ADCStart (ref TLTR27 module);

        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_ADCStop (ref TLTR27 module);

        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_Recv (ref TLTR27 module, uint [] Data, uint [] tstamp, uint size, uint timeout);		

        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_ProcessData(  ref TLTR27 module, uint [] src_data, double [] dst_data,
                                                                    ref uint size, bool calibr,
                                                                     bool valueMain); 
        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_GetDescription(ref TLTR27 module, ushort flags);        

        // функции вспомагательного характера
        [DllImport("ltr27api.dll")]
        public static extern string LTR27_GetErrorString(int ErrorCode);
        public const int LTR27_MEZZANINE_NUMBER=8;

		public const int LTR27_MODULE_DESCRIPTION  =       (1<<0);
		public const int LTR27_MEZZANINE1_DESCRIPTION =    (1<<1);
		public const int LTR27_MEZZANINE2_DESCRIPTION =    (1<<2);
		public const int LTR27_MEZZANINE3_DESCRIPTION =    (1<<3);
		public const int LTR27_MEZZANINE4_DESCRIPTION =    (1<<4);
		public const int LTR27_MEZZANINE5_DESCRIPTION =    (1<<5);
		public const int LTR27_MEZZANINE6_DESCRIPTION =    (1<<6);
		public const int LTR27_MEZZANINE7_DESCRIPTION =    (1<<7);
		public const int LTR27_MEZZANINE8_DESCRIPTION =    (1<<8);


        [StructLayout(LayoutKind.Sequential, Pack=4)]
        public struct TINFO_LTR27
        {
            public _ltr010api.TDESCRIPTION_MODULE Description;	// описание модуля
            public _ltr010api.TDESCRIPTION_CPU CPU;				// описание AVR
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR27_MEZZANINE_NUMBER)]
            public _ltr010api.TDESCRIPTION_MEZZANINE[] Mezzanine;	// описание мезанинов
        };       

        [StructLayout(LayoutKind.Sequential, Pack=4)]
        public struct TMezzanine 
        {    
          [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]  
          public char[] Name;                // название субмодуля
          [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
          public char[] Unit;                // измеряемая субмодулем физ.величина
          [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
          public double[] ConvCoeff;          // масштаб и смещение для пересчета кода в физ.величину
          [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
          public double[] CalibrCoeff;        // калибровочные коэффициенты
        } 		

        [StructLayout(LayoutKind.Sequential, Pack=4)]
        public struct TLTR27
        {            
            public int Size;            
            public _LTRNative.TLTR Channel;

            public byte SubChannel;
            public byte FrequencyDivisor;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR27_MEZZANINE_NUMBER)]
            public TMezzanine [] Mezzanine;// установленные мзонины

            public TINFO_LTR27 ModuleInfo;                        
        }

        public TLTR27 NewTLTR27
        {
            get
            {
                TLTR27 NewModule = new TLTR27();                 
                LTR27_Init(ref NewModule);
                return NewModule;
            }
        }
        

        public TLTR27 module;

        public _ltr27api()
        {
            module = NewTLTR27;			
        }

		
		public virtual _LTRNative.LTRERROR Init ()
		{
			return LTR27_Init(ref module);
		}

		
		public virtual _LTRNative.LTRERROR Open (uint saddr, ushort sport, char[] csn, ushort cc)
		{
			return LTR27_Open(ref module, saddr, sport, csn, cc);
		}

		
		public virtual _LTRNative.LTRERROR Close ()
		{
			return LTR27_Close(ref module);
		}

		
		public virtual _LTRNative.LTRERROR IsOpened ()
		{
			return LTR27_IsOpened(ref module);
		}
		
		public virtual _LTRNative.LTRERROR Echo ()
		{
			return LTR27_Echo(ref module);
		}

		
		public virtual _LTRNative.LTRERROR GetConfig ()
		{
			return LTR27_GetConfig(ref module);						
		}

		
		public virtual _LTRNative.LTRERROR SetConfig ()
		{			
			return LTR27_SetConfig(ref module);
		}

		
		public virtual _LTRNative.LTRERROR ADCStart ()
		{
			return LTR27_ADCStart(ref module);
		}

		
		public virtual _LTRNative.LTRERROR ADCStop ()
		{
			return LTR27_ADCStop(ref module);
		}

		
		public virtual _LTRNative.LTRERROR Recv (uint [] Data, uint size, uint[] tstamp ,uint timeout)
		{			
			return LTR27_Recv(ref module, Data, tstamp, size, timeout);
		}

		
		public virtual _LTRNative.LTRERROR ProcessData(uint [] src_data, double [] dst_data,
			uint size, bool calibr,
			 bool valueMain)
		{
			return LTR27_ProcessData(ref module, src_data, dst_data, ref size, calibr,valueMain);
		} 
		
		public virtual _LTRNative.LTRERROR GetDescription(ushort flags)
		{
			_LTRNative.LTRERROR Result = LTR27_GetDescription(ref module, flags);
			if (Result==_LTRNative.LTRERROR.OK)
			{
				for (int i = 0; i < LTR27_MEZZANINE_NUMBER; i++)
				{
					if ((flags & (1 << (i + 1)))!=0)
					{
						for (int j = 0; j < 4; j++)
						{
							module.Mezzanine[i].CalibrCoeff[j] = module.ModuleInfo.Mezzanine[i].Calibration[j];
						}
					}
				}
			}

			return Result;
		}

        public virtual string GetErrorString(int err)
        {
            return _ltr27api.LTR27_GetErrorString(err);
        }
    }	
}
