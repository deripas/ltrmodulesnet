using System;
using System.Runtime.InteropServices;

namespace ltrModulesNet
{
    public class _ltr11api
    {
        [DllImport("ltr11api.dll")]
        public static extern _LTRNative.LTRERROR LTR11_Init (ref TLTR11 module);

        [DllImport("ltr11api.dll")]
        public static extern _LTRNative.LTRERROR LTR11_Close (ref TLTR11 module);

        [DllImport("ltr11api.dll")]
        public static extern _LTRNative.LTRERROR LTR11_GetConfig (ref TLTR11 module);

        [DllImport("ltr11api.dll")]
        public static extern _LTRNative.LTRERROR LTR11_GetFrame (ref TLTR11 module, uint [] buf);

        [DllImport("ltr11api.dll")]
        public static extern _LTRNative.LTRERROR LTR11_Open (ref TLTR11 module, uint saddr, ushort sport, char[] csn, int slot_num);

                [DllImport("ltr11api.dll")]
        public static extern _LTRNative.LTRERROR LTR11_ProcessData (ref TLTR11 module, uint [] src, double [] dest, 
                                                ref uint size, bool calibr,
                                                bool valueConvert); 

        [DllImport("ltr11api.dll")]
        public static extern _LTRNative.LTRERROR LTR11_SetADC(ref TLTR11 module);

        [DllImport("ltr11api.dll")]
        public static extern _LTRNative.LTRERROR LTR11_Start(ref TLTR11 module);

        [DllImport("ltr11api.dll")]
        public static extern _LTRNative.LTRERROR LTR11_Stop(ref TLTR11 module);

        // ������� ���������������� ���������
        [DllImport("ltr11api.dll")]
        public static extern string LTR11_GetErrorString(int ErrorCode);   

        const int LTR11_ADC_RANGEQNT=4;
        const int LTR11_MAX_LCHANNEL=128;

        public const int LTR11_STARTADCMODE_INT     = 0;
        public const int LTR11_STARTADCMODE_EXTRISE = 1;
        public const int LTR11_STARTADCMODE_EXTFALL = 2;

        public const int LTR11_INPMODE_EXTRISE = 0;
        public const int LTR11_INPMODE_EXTFALL = 1;
        public const int LTR11_INPMODE_INT     = 2;

        public const int LTR11_ADCMODE_ACQ         = 0x00;
        public const int LTR11_ADCMODE_TEST_U1P    = 0x04;
        public const int LTR11_ADCMODE_TEST_U1N    = 0x05;
        public const int LTR11_ADCMODE_TEST_U2N    = 0x06;
        public const int LTR11_ADCMODE_TEST_U2P    = 0x07;
 
        
        
        /* 
        ��������� ��� ������� ������� ������������� ���
        ������� �������������� �� �������:
        F = LTR11_CLOCK/(prescaler*(divider+1)
        ��������!!! ������� 400 ��� �������� ������ �������:
        ��� �� ��������� ����������� � �������� ������ �����
        ��������� ��������:
        prescaler = 1
        divider   = 36
        */

        [StructLayout(LayoutKind.Sequential, Pack=4)]
        public struct CbrCoefStruct
        {
            public double Offset;                      /* �������� ���� */
            public double Gain;                        /* ���������� ����������� */
        }

        [StructLayout(LayoutKind.Sequential, Pack=4)]
        public struct TINFO_LTR11
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] Name;                          /* �������� ������ (������) */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            public char[] Serial;                        /* �������� ����� ������ (������) */

            public ushort Ver;                               /* ������ �� ������ (������� ���� - ��������,
                                             * ������� - ��������
                                             */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
            public char[] Date;                          /* ���� �������� �� (������) */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR11_ADC_RANGEQNT)]
            public CbrCoefStruct[] CbrCoef;	/* ������������� ������������ ��� ������� ��������� */
        };

        [StructLayout(LayoutKind.Sequential, Pack=4)]
        public struct ADCRateStruct
        {
            public int divider;                        // �������� �������� ������� ������, ��������: 2..65535
            public int prescaler;                      // ����������� �������� ������� ������: * 1, 8, 64, 256, 1024
        } ;    

        [StructLayout(LayoutKind.Sequential, Pack=4)]
        public struct TLTR11
        {
            public int size;                               /* ������ ��������� � ������ */
            public _LTRNative.TLTR Channel;                           /* ��������� ������ ����� � ������� */

            public int StartADCMode;                       /* ����� ������ ����� ������:
                                             * LTR11_STARTADCMODE_INT     - ���������� ����� (��
                                             *                              ������� �����);
                                             * LTR11_STARTADCMODE_EXTRISE - �� ������ ��������
                                             *                              �������;
                                             * LTR11_STARTADCMODE_EXTFALL - �� ����� ��������
                                             *                              �������.
                                             */
            public int InpMode;                            /* ����� ����� ������ � ���
                                             *  LTR11_INPMODE_INT     - ���������� ������ ���
                                             *                          (������� �������� AdcRate)
                                             *  LTR11_INPMODE_EXTRISE - �� ������ �������� �������
                                             *  LTR11_INPMODE_EXTFALL - �� ����� �������� �������
                                             */
            public int LChQnt;                             /* ����� �������� ���������� ������� (������ �����) */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR11_MAX_LCHANNEL)]
            public byte[] LChTbl;                          /* ����������� ������� � ��������� ����������� ��������
                                             * ��������� ������ ����� �������: MsbGGMMCCCCLsb
                                             *   GG   - ������� ��������:
                                             *          0 - +-10 �;
                                             *          1 - +-2.5 �;
                                             *          2 - +-0.625 �;
                                             *          3 - +-0.156�;
                                             *   MM   - �����:
                                             *          0 - 16-���������, ������ 1-16;
                                             *          1 - ��������� ������������ ����������
                                             *              �������� ����;
                                             *          2 - 32-���������, ������ 1-16;
                                             *          3 - 32-���������, ������ 17-32;
                                             *   CCCC - ����� ����������� ������:
                                             *          0 - ����� 1 (17);
                                             *          . . .
                                             *          15 - ����� 16 (32).
                                             */
            public int ADCMode;                            /* ����� ����� ������ ��� ��� ��������� ������ */
            public ADCRateStruct ADCRate;
            public double ChRate;                          /* ������� ������ ������ � ��� (������ �����) ���
                                             * ���������� ������� ���
                                             */
            public TINFO_LTR11 ModuleInfo;                 /* ���������� � ������ LTR11 */
        };

        public TLTR11 NewTLTR11
        {
            get
            {
                TLTR11 NewModule = new TLTR11();                 
                LTR11_Init(ref NewModule);
                return NewModule;
            }
        }
        

        public TLTR11 module;

        public _ltr11api()
        {
            module = NewTLTR11;
        }

		
		public virtual _LTRNative.LTRERROR Init ()
		{
			return LTR11_Init(ref module);
		}
		
		public virtual _LTRNative.LTRERROR Close ()
		{
			return LTR11_Close(ref module);
		}
		
		public virtual _LTRNative.LTRERROR GetConfig ()
		{
			return LTR11_GetConfig(ref module);
		}
		
		public virtual _LTRNative.LTRERROR GetFrame (uint [] buf)
		{
			return LTR11_GetFrame(ref module, buf);
		}
		
		public virtual _LTRNative.LTRERROR Open (uint saddr, ushort sport, char[] csn, int slot_num)
		{
			return LTR11_Open(ref module, saddr, sport, csn, slot_num);
		}

        public virtual int Recv(uint[] data,
                uint[] tmark, uint size, uint timeout)
        {
            return  _LTRNative.LTR_Recv(ref module.Channel, data, tmark, size, timeout);
        }
		
		public virtual _LTRNative.LTRERROR ProcessData (uint [] src, double [] dest, 
			ref uint size, bool calibr,
			bool valueMain)
		{
			return LTR11_ProcessData(ref module, src, dest, ref size, calibr, valueMain);
		}
		
		public virtual _LTRNative.LTRERROR SetADC()
		{
			return LTR11_SetADC(ref module);
		}
		
		public virtual _LTRNative.LTRERROR Start()
		{
			return LTR11_Start(ref module);
		}
		
		public virtual _LTRNative.LTRERROR Stop()
		{
			return LTR11_Stop(ref module);
		}

        public virtual string GetErrorString(int err)
        {
            return _ltr11api.LTR11_GetErrorString(err);
        }
	}
}
