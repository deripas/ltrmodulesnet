using System;
using System.Runtime.InteropServices;

namespace ltrModulesNet
{
    public class _ltr212api
    {        
		[DllImport("ltr212api.dll")]
		public static extern _LTRNative.LTRERROR LTR212_Init(ref TLTR212 module);

		[DllImport("ltr212api.dll")]
		public static extern _LTRNative.LTRERROR LTR212_Open(ref TLTR212 module, uint net_addr, ushort net_port,
			char[] crate_sn, int slot_num, char[] biosname);

		[DllImport("ltr212api.dll")]
		public static extern _LTRNative.LTRERROR LTR212_Close(ref TLTR212 module);

		[DllImport("ltr212api.dll")]
		public static extern _LTRNative.LTRERROR LTR212_CreateLChannel(int PhysChannel, int Scale);

        [DllImport("ltr212api.dll")]
        public static extern _LTRNative.LTRERROR LTR212_CreateLChannel2(uint PhysChannel, uint Scale, uint BridgeType);

		[DllImport("ltr212api.dll")]
		public static extern _LTRNative.LTRERROR LTR212_SetADC(ref TLTR212 module);

		[DllImport("ltr212api.dll")]
		public static extern _LTRNative.LTRERROR LTR212_Start(ref TLTR212 module);

		[DllImport("ltr212api.dll")]
		public static extern _LTRNative.LTRERROR LTR212_Stop(ref TLTR212 module);

		[DllImport("ltr212api.dll")]
		public static extern _LTRNative.LTRERROR LTR212_Recv(ref TLTR212 module, uint[] data,
			uint [] tmark, uint size, uint timeout);

		[DllImport("ltr212api.dll")]
		static extern _LTRNative.LTRERROR LTR212_Recv(ref TLTR212 module, uint[] data,
			uint tmark, uint size, uint timeout);

		[DllImport("ltr212api.dll")]
		public static extern _LTRNative.LTRERROR LTR212_ProcessData(ref TLTR212 module, uint[] src, double[] dest,
			ref int size, bool volt);

		[DllImport("ltr212api.dll")]
		public static extern _LTRNative.LTRERROR LTR212_Calibrate(ref TLTR212 module, byte[] LChannel_Mask, int mode, int reset);

		[DllImport("ltr212api.dll")]
		public static extern _LTRNative.LTRERROR LTR212_CalcFS(ref TLTR212 module, ref double fsBase, ref double fs);

		[DllImport("ltr212api.dll")]
		public static extern _LTRNative.LTRERROR LTR212_TestEEPROM(ref TLTR212 module);

		[DllImport("ltr212api.dll")]
		public static extern _LTRNative.LTRERROR LTR212_ProcessDataTest(ref TLTR212 module,
			uint[] src, double[] dest, ref int size, bool volt, ref uint bad_num);

		[DllImport("ltr212api.dll")]
		public static extern _LTRNative.LTRERROR LTR212_ReadFilter(char[] fname, ref ltr212filter filter);

		[DllImport("ltr212api.dll")]
		public static extern _LTRNative.LTRERROR LTR212_WriteSerialNumber(ref TLTR212 module, char[] sn, ushort Code);

		[DllImport("ltr212api.dll")]
		public static extern _LTRNative.LTRERROR LTR212_TestInterfaceStart(ref TLTR212 module, int PackDelay);

		[DllImport("ltr212api.dll")]
		public static extern uint LTR212_CalcTimeOut(ref TLTR212 module, int n);

		// ������� ���������������� ���������
		[DllImport("ltr212api.dll")]
		public static extern string LTR212_GetErrorString(int ErrorCode);

		public const int MAXTAPS = 255;

        public const int LTR212_LCH_CNT_MAX      = 8;  // ����. ����� ����������. �������
        public const int LTR212_FIR_ORDER_MAX    = 255; // ������������ �������� ������� ���-�������
        public const int LTR212_FIR_ORDER_MIN    = 3;   // ����������� �������� ������� ���-�������


        // ����������� ������
        public enum LTR212_MODULE_TYPES : byte
        {
            LTR212_OLD,     // ������ ������ � ���������� �����- � ����-�������� �����������
            LTR212_M_1,     // ����� ������ � ���������� �����-,  ����- � ��������-�������� �����������
            LTR212_M_2      // ����� ������ � ���������� �����- � ����-�������� �����������
        };


        // ���� ��������� ������
        public enum LTR212_BRIDGE_TYPES : uint
        {
            LTR212_FULL_OR_HALF_BRIDGE,
            LTR212_QUARTER_BRIDGE_WITH_200_Ohm,
            LTR212_QUARTER_BRIDGE_WITH_350_Ohm,
            LTR212_QUARTER_BRIDGE_WITH_CUSTOM_Ohm,
            LTR212_UNBALANCED_QUARTER_BRIDGE_WITH_200_Ohm,
            LTR212_UNBALANCED_QUARTER_BRIDGE_WITH_350_Ohm,
            LTR212_UNBALANCED_QUARTER_BRIDGE_WITH_CUSTOM_Ohm
        };

        // ������ ����� ������ (AcqMode)
        public enum LTR212_ACQ_MODE : int
        {
            LTR212_FOUR_CHANNELS_WITH_MEDIUM_RESOLUTION = 0,
            LTR212_FOUR_CHANNELS_WITH_HIGH_RESOLUTION = 1,
            LTR212_EIGHT_CHANNELS_WITH_HIGH_RESOLUTION = 2
        };

        // �������� �������� ����������
        public enum LTR212_REF_VAL : int
        {
            LTR212_REF_2_5V = 0,  //2.5 �
            LTR212_REF_5V = 1   //5   �
        };

        // ��������� ������
        public enum LTR212_SCALE : int
        {
            LTR212_SCALE_B_10 = 0, /* �������� -10��/+10�� */
            LTR212_SCALE_B_20 = 1, /* �������� -20��/+20�� */
            LTR212_SCALE_B_40 = 2, /* �������� -40��/+40�� */
            LTR212_SCALE_B_80 = 3, /* �������� -80��/+80�� */
            LTR212_SCALE_U_10 = 4, /* �������� -10��/+10�� */
            LTR212_SCALE_U_20 = 5, /* �������� -20��/+20�� */
            LTR212_SCALE_U_40 = 6, /* �������� -40��/+40�� */
            LTR212_SCALE_U_80 = 7 /* �������� -80��/+80�� */
        };

        // ������ ����������
        public enum LTR212_CALIBR_MODE : int
        {
            LTR212_CALIBR_MODE_INT_ZERO = 0,
            LTR212_CALIBR_MODE_INT_SCALE = 1,
            LTR212_CALIBR_MODE_INT_FULL = 2,
            LTR212_CALIBR_MODE_EXT_ZERO = 3,
            LTR212_CALIBR_MODE_EXT_SCALE = 4,
            LTR212_CALIBR_MODE_EXT_ZERO_INT_SCALE = 5,
            LTR212_CALIBR_MODE_EXT_FULL_2ND_STAGE = 6, /* ������ ������ ������� ���������� */
            LTR212_CALIBR_MODE_EXT_ZERO_SAVE_SCALE = 7  /* ������� ���� � ����������� �� ����� ���������� ����. �������� */
        };






		// ���������, ������������� ��� �������� �������
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
			public struct ltr212filter
		{
			public double fs;
			public byte decimation;
			public byte taps;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXTAPS)]
			public short[] koeff;
		};

		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		public struct TINFO_LTR212
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
			public char[] Name;
            public byte Type;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
			public char[] Serial;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			public char[] BiosVersion;// ������ �����
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public char[] BiosDate;  // ���� �������� ������ ������ �����
		};

		[StructLayout(LayoutKind.Sequential, Pack = 4)]
			public struct filterStruct
		{
			public int IIR;         // ���� ������������� ���-�������
			public int FIR;         // ���� ������������� ���-�������
			public int Decimation;  // �������� ������������ ��������� ��� ���-�������
			public int TAP;		 // ������� ���-������� 
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512 + 1)]
			public char[] IIR_Name; // ������ ���� � ����� � �����-�� ������������ ���-������� 
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512 + 1)]
			public char[] FIR_Name; // ������ ���� � ����� � �����-�� ������������ ���-�������
		} ;   // ���������, �������� ������ � �����. �������� � �� �����-��.

		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		public struct TLTR212
		{
			public int size;
			public _LTRNative.TLTR Channel;
			public int AcqMode; // ����� ����� ������
			public int UseClb;  // ���� ������������� ������������� �����-���
			public int UseFabricClb;// ���� ������������� ��������� ������������� �����-���
			public int LChQnt;	 // ���-�� ������������ ����������� �������
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			public int[] LChTbl;  //������� ����������� �������
			public int REF;		 // ���� �������� �������� ����������
			public int AC;		 // ���� ���������������� �������� ����������
			public double Fs;     // ������� ������������� ���

			public filterStruct filter;   // ���������, �������� ������ � �����. �������� � �� �����-��.

			public TINFO_LTR212 ModuleInfo;

			public ushort CRC_PM; // ��� ���������� �����������
			public ushort CRC_Flash_Eval; // ��� ���������� �����������
			public ushort CRC_Flash_Read;   // ��� ���������� �����������

		} // ��������� �������� ������ 


		public TLTR212 NewTLTR212
		{
			get
			{
				TLTR212 NewModule = new TLTR212();
				LTR212_Init(ref NewModule);
				return NewModule;
			}
		}


		public TLTR212 module;

		public _ltr212api()
		{
			module = NewTLTR212;
		}

		
		public virtual _LTRNative.LTRERROR Init()
		{
			return LTR212_Init(ref module);
		}

		
		public virtual _LTRNative.LTRERROR Open(uint net_addr, ushort net_port,
			char[] crate_sn, int slot_num, string biosname)
		{
			return LTR212_Open(ref module, net_addr, net_port, crate_sn, slot_num, biosname.ToCharArray());
		}

		
		public virtual _LTRNative.LTRERROR Close()
		{
			return LTR212_Close(ref module);
		}

		
		public virtual _LTRNative.LTRERROR CreateLChannel(int PhysChannel, int Scale)
		{
			return LTR212_CreateLChannel(PhysChannel, Scale);
		}

        public virtual _LTRNative.LTRERROR CreateLChannel2(uint PhysChannel, uint Scale, uint BridgeType)
        {
            return LTR212_CreateLChannel2(PhysChannel, Scale, BridgeType);
        }

		
		public virtual _LTRNative.LTRERROR SetADC()
		{
			return LTR212_SetADC(ref module);
		}

		
		public virtual _LTRNative.LTRERROR Start()
		{
			return LTR212_Start(ref module);
		}

		
		public virtual _LTRNative.LTRERROR Stop()
		{
			return LTR212_Stop(ref module);
		}

		
		public virtual _LTRNative.LTRERROR Recv(uint[] data,
			uint [] tmark, uint size, uint timeout)
		{
			return LTR212_Recv(ref module, data, tmark, size, timeout);
		}

		
		public virtual _LTRNative.LTRERROR ProcessData(uint[] src, double[] dest,
			ref int size, bool volt/*,INT *bad_num*/)
		{
			return LTR212_ProcessData(ref module, src, dest, ref size, volt);
		}

		
		public virtual _LTRNative.LTRERROR Calibrate(byte[] LChannel_Mask, int mode, int reset)
		{
			return LTR212_Calibrate(ref module, LChannel_Mask, mode, reset);
		}

		
		public virtual _LTRNative.LTRERROR CalcFS(ref double fsBase, ref double fs)
		{
			return LTR212_CalcFS(ref module, ref fsBase, ref fs);
		}

		
		public virtual _LTRNative.LTRERROR TestEEPROM()
		{
			return LTR212_TestEEPROM(ref module);
		}

		
		public virtual _LTRNative.LTRERROR ProcessDataTest(uint[] src, double[] dest,ref int size, bool volt, ref uint bad_num)
		{
			return LTR212_ProcessDataTest(ref module, src, dest, ref size, volt, ref bad_num);
		}

		
		public virtual _LTRNative.LTRERROR ReadFilter(char[] fname, ref ltr212filter filter)
		{
			return LTR212_ReadFilter(fname, ref filter);
		}

		
		public virtual _LTRNative.LTRERROR WriteSerialNumber(char[] sn, ushort Code)
		{
			return LTR212_WriteSerialNumber(ref module, sn, Code);
		}

		
		public virtual _LTRNative.LTRERROR TestInterfaceStart(int PackDelay)
		{
			return LTR212_TestInterfaceStart(ref module, PackDelay);
		}

		
		public virtual uint CalcTimeOut(int n)
		{
			return LTR212_CalcTimeOut(ref module, n);
		}

        public virtual string GetErrorString(int err)
        {
            return _ltr212api.LTR212_GetErrorString(err);
        }
    }
}
