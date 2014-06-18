using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ltrModulesNet
{
    public class ltr27api
    {
        public const int LTR27_MEZZANINE_NUMBER = 8;

    

        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_Init(ref TLTR27 module);

        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_Open(ref TLTR27 module, uint saddr, ushort sport, string csn, ushort cc);

        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_OpenEx(ref TLTR27 module, uint saddr, ushort sport, string csn, ushort cc,
                   _LTRNative.OpenInFlags in_flags, out _LTRNative.OpenOutFlags out_flags);

        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_Close(ref TLTR27 module);

        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_IsOpened (ref TLTR27 module);

        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_Echo(ref TLTR27 module);

        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_GetConfig(ref TLTR27 module);

        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_SetConfig(ref TLTR27 module);

        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_StoreConfig(ref TLTR27 module, _LTRNative.StartMode start_mode);

        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_ADCStart(ref TLTR27 module);

        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_ADCStop(ref TLTR27 module);

        [DllImport("ltr27api.dll")]
        public static extern int LTR27_Recv(ref TLTR27 module, uint[] Data, uint[] tstamp, uint size, uint timeout);		

        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_ProcessData(ref TLTR27 module, uint[] src_data, double[] dst_data,
                                                                    ref uint size, bool calibr,
                                                                     bool valueMain);
        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_SearchFirstFrame(ref TLTR27 module, uint[] src_data, uint size,
                                                                        out uint frame_idx);
                                                                        

        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_GetDescription(ref TLTR27 module, ushort flags);

        [DllImport("ltr27api.dll")]
        public static extern _LTRNative.LTRERROR LTR27_GetDescription(ref TLTR27 module, Descriptions flags);

        // функции вспомагательного характера
        [DllImport("ltr27api.dll")]
        public static extern IntPtr LTR27_GetErrorString(int ErrorCode);
        

        [Flags]
        public enum Descriptions : ushort
        {
            MODULE = (1 << 0),
            MEZZANINE1 = (1 << 1),
            MEZZANINE2 = (1 << 2),
            MEZZANINE3 = (1 << 3),
            MEZZANINE4 = (1 << 4),
            MEZZANINE5 = (1 << 5),
            MEZZANINE6 = (1 << 6),
            MEZZANINE7 = (1 << 7),
            MEZZANINE8 = (1 << 8),
            ALL_MEZZANINE = MEZZANINE1 | MEZZANINE2 | MEZZANINE3 | MEZZANINE4
                | MEZZANINE5 | MEZZANINE6 | MEZZANINE7 | MEZZANINE8,
            ALL = MODULE | ALL_MEZZANINE
        };
         



        [StructLayout(LayoutKind.Sequential, Pack=4)]
        public struct TINFO_LTR27
        {
            public _LTRNative.TDESCRIPTION_MODULE Module;	// описание модуля
            public _LTRNative.TDESCRIPTION_CPU Cpu;				// описание AVR
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR27_MEZZANINE_NUMBER)]
            public _LTRNative.TDESCRIPTION_MEZZANINE[] Mezzanine;	// описание мезанинов
        };       

        [StructLayout(LayoutKind.Sequential, Pack=4)]
        public struct TMezzanine 
        {    
          [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]  
          char[] Name_;                // название субмодуля
          [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
          char[] Unit_;                // измеряемая субмодулем физ.величина
          [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
          public double[] ConvCoeff;          // масштаб и смещение для пересчета кода в физ.величину
          [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
          public double[] CalibrCoeff;        // калибровочные коэффициенты

          public string Name { get { return new string(Name_).TrimEnd('\0'); } }
          public string Unit { get { return new string(Unit_).TrimEnd('\0'); } }
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




        public TLTR27 module;

        public ltr27api()
        {
            LTR27_Init(ref module);	
        }

        ~ltr27api()
        {
            LTR27_Close(ref module);
        }

		
		public virtual _LTRNative.LTRERROR Init ()
		{
			return LTR27_Init(ref module);
		}

		
		public virtual _LTRNative.LTRERROR Open (uint saddr, ushort sport, string csn, ushort cc)
		{
			return LTR27_Open(ref module, saddr, sport, csn, cc);
		}

        public virtual _LTRNative.LTRERROR Open(string csn, ushort cc)
        {
            return Open(_LTRNative.SADDR_DEFAULT, _LTRNative.SPORT_DEFAULT, csn, cc);
        }

        public virtual _LTRNative.LTRERROR Open(ushort cc)
        {
            return Open("", cc);
        }

        public virtual _LTRNative.LTRERROR OpenEx(uint saddr, ushort sport, string csn, ushort cc,
                                                  _LTRNative.OpenInFlags in_flags, out _LTRNative.OpenOutFlags out_flags)
        {
            return LTR27_OpenEx(ref module, saddr, sport, csn, cc, in_flags, out out_flags);
        }

        public virtual _LTRNative.LTRERROR OpenEx(string csn, ushort cc,
                                          _LTRNative.OpenInFlags in_flags, out _LTRNative.OpenOutFlags out_flags)
        {
            return OpenEx(_LTRNative.SADDR_DEFAULT, _LTRNative.SPORT_DEFAULT, csn, cc, in_flags, out out_flags);
        }

        public virtual _LTRNative.LTRERROR OpenEx(ushort cc, _LTRNative.OpenInFlags in_flags, out _LTRNative.OpenOutFlags out_flags)
        {
            return OpenEx(_LTRNative.SADDR_DEFAULT, _LTRNative.SPORT_DEFAULT, "", cc, in_flags, out out_flags);
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

        public virtual _LTRNative.LTRERROR StoreConfig(_LTRNative.StartMode start_mode)
        {
            return LTR27_StoreConfig(ref module, start_mode);
        }

		
		public virtual _LTRNative.LTRERROR ADCStart ()
		{
			return LTR27_ADCStart(ref module);
		}

		
		public virtual _LTRNative.LTRERROR ADCStop ()
		{
			return LTR27_ADCStop(ref module);
		}

		
		public virtual int Recv (uint [] Data, uint size, uint[] tstamp ,uint timeout)
		{			
			return LTR27_Recv(ref module, Data, tstamp, size, timeout);
		}

        public virtual int Recv(uint[] Data, uint size, uint timeout)
        {
            return LTR27_Recv(ref module, Data, null, size, timeout);
        }

		
		public virtual _LTRNative.LTRERROR ProcessData(uint [] src_data, double [] dst_data,
			uint size, bool calibr,
			 bool valueMain)
		{
			return LTR27_ProcessData(ref module, src_data, dst_data, ref size, calibr,valueMain);
		}

        public virtual _LTRNative.LTRERROR SearchFirstFrame(uint[] src_data, uint size, out uint frame_idx)
        {
            return LTR27_SearchFirstFrame(ref module, src_data, size, out frame_idx);
        }

        public virtual _LTRNative.LTRERROR GetDescription(Descriptions flags)
        {
            return GetDescription((ushort)flags);
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

        public static string GetErrorString(_LTRNative.LTRERROR err)
        {
            IntPtr ptr = LTR27_GetErrorString((int)err);
            string str = Marshal.PtrToStringAnsi(ptr);
            Encoding srcEncodingFormat = Encoding.GetEncoding("windows-1251");
            Encoding dstEncodingFormat = Encoding.UTF8;
            return dstEncodingFormat.GetString(Encoding.Convert(srcEncodingFormat, dstEncodingFormat, srcEncodingFormat.GetBytes(str)));
        }


        public TMezzanine[] Mezzanine { get { return module.Mezzanine; } }
        public TINFO_LTR27 ModuleInfo { get { return module.ModuleInfo; } }
        public _LTRNative.TLTR Channel { get { return module.Channel; } }

        public byte FrequencyDivisor { 
            get { return module.FrequencyDivisor; } 
            set { module.FrequencyDivisor = value; } 
        }
        public byte SubChannel { get { return module.SubChannel; } }
    }
}
