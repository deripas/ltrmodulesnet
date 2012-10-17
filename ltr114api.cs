using System;
using System.Runtime.InteropServices;

namespace ltrModulesNet
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct LTR114_LCHANNEL
    {
        public byte MeasMode;       /*����� ���������*/
        public byte Channel;       /*���������� �����*/
        public byte Range;         /*�������� ���������*/

        public LTR114_LCHANNEL(byte MeasMode, byte Channel, byte Range)
        {
            this.MeasMode = MeasMode;
            this.Channel = Channel;
            this.Range = Range;
        }
    }

    //���������� � ������
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct LTR114_TINFO
    {

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        char[] name;                          /* �������� ������ (������) */
        public string Name { get { return (name != null) ? new string(name) : null; } }
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        char[] serial;                        /* �������� ����� ������ (������) */
        public string Serial { get { return (serial!=null)?new string(serial) : null; } }

        ushort verMCU;
        public string VerMCU { get { return ((byte)((verMCU &0xFF00)>>8)).ToString() + '.' + ((byte)(verMCU&0xFF)).ToString(); } } 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
        char[] date;                          /* ���� �������� �� (������) */
        public string Date { get { return (date != null) ? new string(date) : null; } }

        byte verPLD;  //������ �������� ����
        public byte VerPLD { get { return verPLD; } }

        public _ltr114api.CbrCoefStruct CbrCoef;              /* ������������� ������������ ��� ������� ��������� */
    };


    public class _ltr114api
    {
        [DllImport("ltr114api.dll")]
        public static extern _LTRNative.LTRERROR LTR114_Init(ref TLTR114 hnd);

        [DllImport("ltr114api.dll")]
        public static extern _LTRNative.LTRERROR LTR114_Open(ref TLTR114 hnd, uint saddr, ushort sport, char[] csn, int slot_num);

        [DllImport("ltr114api.dll")]
        public static extern _LTRNative.LTRERROR LTR114_Close(ref TLTR114 hnd);

        [DllImport("ltr114api.dll")]
        public static extern _LTRNative.LTRERROR LTR114_GetConfig(ref TLTR114 hnd);
        
        [DllImport("ltr114api.dll")]
        public static extern _LTRNative.LTRERROR LTR114_Calibrate(ref TLTR114 hnd);

        [DllImport("ltr114api.dll")]
        public static extern _LTRNative.LTRERROR LTR114_SetADC(ref TLTR114 hnd);

        [DllImport("ltr114api.dll")]
        public static extern _LTRNative.LTRERROR LTR114_Start(ref TLTR114 hnd);

        [DllImport("ltr114api.dll")]
        public static extern _LTRNative.LTRERROR LTR114_Stop(ref TLTR114 hnd);

        [DllImport("ltr114api.dll")]
        public static extern _LTRNative.LTRERROR LTR114_Recv(ref TLTR114 hnd, uint[] buf, uint[] tmark, uint size, uint timeout); //����� ������ �� ������		


        [DllImport("ltr114api.dll")]
        public static extern _LTRNative.LTRERROR LTR114_GetFrame(ref TLTR114 hnd, uint[] buf);

        [DllImport("ltr114api.dll")]
        public static extern _LTRNative.LTRERROR LTR114_ProcessData(ref TLTR114 hnd, uint[] src, double[] dest, ref int size, CorrectionMode correction_mode, ProcFlags flags);

        [DllImport("ltr114api.dll")]
        public static extern _LTRNative.LTRERROR LTR114_ProcessDataTherm(ref TLTR114 hnd, uint[] src, double[] dest, double[] therm, ref int size, out int tcnt, CorrectionMode correction_mode, ProcFlags flags);



        public enum CorrectionMode : int
        {
            None = 0,
            Init = 1,
            Auto = 2
        }

        [Flags]
        public enum ProcFlags : int
        {
            None = 0,
            Value = 1,
            AvgR = 2
        }

        [Flags]
        public enum Features : byte
        {
            None = 0,
            StopSW = 1,
            Therm = 2
        }

        public enum Sync : byte
        {
            None = 0,
            Internal = 1,
            Master = 2,
            External = 4
        }

        // ������� ���������������� ���������
        [DllImport("ltr114api.dll")]
        public static extern string LTR114_GetErrorString(int ErrorCode);

        public const int LTR114_ADC_RANGEQNT = 3;
        public const int LTR114_R_RANGEQNT = 3;
        public const int LTR114_MAX_LCHANNEL = 128;
        const int LTR114_SCALE_INTERVALS = 3;


        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct AutoCoefs
        {
            public double Offset;                      /* �������� ���� */
            public double Gain;                        /* ���������� ����������� */
        }

        /*���������� �������� ����� �� ������ ��������������*/
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TSCALE_LTR114
        {
            public int Null;        //�������� ����                    
            public int Ref;         //�������� +�����
            public int NRef;       //�������� -�����
            public int Interm;
            public int NInterm;
        }


        /*���������� ��� �������������� ������ �� ������ ���������*/
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TCBRINFO
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR114_SCALE_INTERVALS)]
            public AutoCoefs[] Coefs;  /*����������� �� ����� �������������� �������� Gain � Offset*/


            public IntPtr TempScale;       /*������ ��������� ��������� �����/���� */
            //public TSCALE_LTR114 Index;           /*���������� ��������� � TempScale*/
            //public TSCALE_LTR114 LastVals;       /*��������� ���������*/
            public int Null;        //�������� ����                    
            public int Ref;         //�������� +�����
            public int NRef;       //�������� -�����
            public int Interm;
            public int NInterm;

            public int LastNull;        //�������� ����                    
            public int LastRef;         //�������� +�����
            public int LastNRef;       //�������� -�����
            public int LastInterm;
            public int LastNInterm;

            public int HVal;
            public int LVal;   
        }
        
        
        
        


        //������������� ������������ �� TINFO_LTR114
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct CbrCoefStruct
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR114_ADC_RANGEQNT)]
            public float[] U;                      /*�������� ��� ��� ���������� ��������� ����������*/
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR114_R_RANGEQNT)]
            public float[] I;                       /*�������� ����� ��� ���������� ��������� �������������*/
        
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR114_ADC_RANGEQNT)]
            public float[] UIntr;        
        }

        

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TLTR114
        {
            public int size;                               /* ������ ��������� � ������ */
            public _LTRNative.TLTR Channel;                           /* ��������� ������ ����� � ������� */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR114_ADC_RANGEQNT)]
            public TCBRINFO[] AutoCalibrInfo;      /* ������ ��� ���������� ������������� ����. ��� ������� ��������� */
            public int LChQnt;  // ���������� �������� ���������� ������� 

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR114_MAX_LCHANNEL)]
            public LTR114_LCHANNEL[] LChTbl;        // ����������� ������� � ����������� ���������� �������

            public UInt16 Interval;                          //����� ������������ ���������
            public Features SpecialFeatures;                   //�������������� ����������� ������ (����������� ����������, ���������� ����������)

            //�������� ���������. ��� - ����������� � ������������ � �������� �������������
            public byte AdcOsr;

            public Sync SyncMode;               /*����� ������������� 
                                                  000 - ��� �������������
                                                  001 - ���������� �������������
                                                  010 - ���������� ������������� - �������
                                                  100 - ������� ������������� (�������)
                                                  */

            public int FreqDivider;                       // �������� ������� ��� (2..8000)
                                           // ������� ������������� ����� F = LTR114_CLOCK/(LTR114_ADC_DIVIDER*FreqDivider)

            public int FrameLength;                       //������ ������, ������������ ������� �� ���� ���� 
                                           //��������������� ����� ������ LTR114_SetADC
            public bool Active;
            public IntPtr Reserved;

            public LTR114_TINFO ModuleInfo;                 /* ���������� � ������ LTR114 */
        };

        public TLTR114 NewTLTR114
        {
            get
            {
                TLTR114 NewModule = new TLTR114();
                LTR114_Init(ref NewModule);
                return NewModule;
            }
        }


        public TLTR114 module;

        public _ltr114api()
        {
            module = NewTLTR114;
        }


        public virtual _LTRNative.LTRERROR Init()
        {
            return LTR114_Init(ref module);
        }

        public virtual _LTRNative.LTRERROR Open(uint saddr, ushort sport, char[] csn, int slot_num)
        {
            return LTR114_Open(ref module, saddr, sport, csn, slot_num);
        }
        
        public virtual _LTRNative.LTRERROR Close()
        {
            return LTR114_Close(ref module);
        }

        public virtual _LTRNative.LTRERROR GetConfig()
        {
            return LTR114_GetConfig(ref module);
        }

        public virtual _LTRNative.LTRERROR Calibrate()
        {
            return LTR114_Calibrate(ref module);
        }

        public virtual _LTRNative.LTRERROR GetFrame(uint[] buf)
        {
            return LTR114_GetFrame(ref module, buf);
        }

        public virtual _LTRNative.LTRERROR SetADC()
        {
            return LTR114_SetADC(ref module);
        }

        public virtual _LTRNative.LTRERROR Start()
        {
            return LTR114_Start(ref module);
        }

        public virtual _LTRNative.LTRERROR Stop()
        {
            return LTR114_Stop(ref module);
        }

        public virtual _LTRNative.LTRERROR Recv(uint[] buf, uint[] tmark, uint size, uint timeout)
        {
             return LTR114_Recv(ref module, buf, tmark, size, timeout);
        }

        public virtual _LTRNative.LTRERROR ProcessData(uint[] src, double[] dest,
            ref int size, CorrectionMode correction_mode, ProcFlags flags)            
        {
            return LTR114_ProcessData(ref module, src, dest, ref size, correction_mode, flags);
        }

        public virtual _LTRNative.LTRERROR ProcessDataTherm(uint[] src, double[] dest, double[] therm,
            ref int size, out int tcnt, CorrectionMode correction_mode, ProcFlags flags)
        {
            return LTR114_ProcessDataTherm(ref module, src, dest,  therm, ref size, out tcnt, correction_mode, flags);
        }

        public virtual string GetErrorString(int err)
        {
            return _ltr114api.LTR114_GetErrorString(err);
        }

        
    }
}
