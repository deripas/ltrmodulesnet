using System;
using System.Runtime.InteropServices;

namespace ltrModulesNet
{
    public class ltr212apiLabView : _ltr212api
    {
        public double fs;
        public byte decimation;
        public byte taps;
        public short[] koeff;

        public int IIR;         // ���� ������������� ���-�������
        public int FIR;         // ���� ������������� ���-�������
        public int Decimation;  // �������� ������������ ��������� ��� ���-�������
        public int TAP;		 // ������� ���-������� 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512 + 1)]
        public char[] IIR_Name; // ������ ���� � ����� � �����-�� ������������ ���-������� 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512 + 1)]
        public char[] FIR_Name; // ������ ���� � ����� � �����-�� ������������ ���-�������


        public int AcqMode; // ����� ����� ������
        public int UseClb;  // ���� ������������� ������������� �����-���
        public int UseFabricClb;// ���� ������������� ��������� ������������� �����-���
        public int LChQnt;	 // ���-�� ������������ ����������� �������		
        public int[] LChTbl;  //������� ����������� �������
        public int REF;		 // ���� �������� �������� ����������
        public int AC;		 // ���� ���������������� �������� ����������
        public double Fs;     // ������� ������������� ���


        public uint StartMarks { get { return (module.Channel.tmark >> 16) & 0xFFFF; } }
        public uint SecondMarks { get { return module.Channel.tmark & 0xFFFF; } }

        public uint StartMarksAfterOpen { get { return StartMarks - init_start_mark; } }
        public uint SecondMarksAfterOpen { get { return SecondMarks - init_second_mark; } }

        uint init_start_mark;
        uint init_second_mark;



        public ltr212apiLabView()
        {            
            GetConfigFromModule();
        }

        void GetConfigFromModule()
        {
            AcqMode = module.AcqMode; // ����� ����� ������
            UseClb = module.UseClb;  // ���� ������������� ������������� �����-���
            UseFabricClb = module.UseFabricClb;// ���� ������������� ��������� ������������� �����-���
            LChQnt = module.LChQnt;	 // ���-�� ������������ ����������� �������		
            LChTbl = module.LChTbl;  //������� ����������� �������
            REF = module.REF;		 // ���� �������� �������� ����������
            AC = module.AC;		 // ���� ���������������� �������� ����������
            Fs = module.Fs;     // ������� ������������� ���

            IIR = module.filter.IIR;         // ���� ������������� ���-�������
            FIR = module.filter.FIR;         // ���� ������������� ���-�������
            Decimation = module.filter.Decimation;  // �������� ������������ ��������� ��� ���-�������
            TAP = module.filter.TAP;		 // ������� ���-������� 

            IIR_Name = module.filter.IIR_Name; // ������ ���� � ����� � �����-�� ������������ ���-������� 

            FIR_Name = module.filter.FIR_Name; // ������ ���� � ����� � �����-�� ������������ ���-�������
        }

        void SetConfigToModule()
        {
            module.AcqMode = AcqMode; // ����� ����� ������
            module.UseClb = UseClb;  // ���� ������������� ������������� �����-���
            module.UseFabricClb = UseFabricClb;// ���� ������������� ��������� ������������� �����-���
            module.LChQnt = LChQnt;	 // ���-�� ������������ ����������� �������		
            module.LChTbl = LChTbl;  //������� ����������� �������
            module.REF = REF;		 // ���� �������� �������� ����������
            module.AC = AC;		 // ���� ���������������� �������� ����������
            module.Fs = Fs;     // ������� ������������� ���

            module.filter.IIR = IIR;         // ���� ������������� ���-�������
            module.filter.FIR = FIR;         // ���� ������������� ���-�������
            module.filter.Decimation = Decimation;  // �������� ������������ ��������� ��� ���-�������
            module.filter.TAP = TAP;		 // ������� ���-������� 

            module.filter.IIR_Name = IIR_Name; // ������ ���� � ����� � �����-�� ������������ ���-������� 

            module.filter.FIR_Name = FIR_Name; // ������ ���� � ����� � �����-�� ������������ ���-�������
        }

        public override _LTRNative.LTRERROR SetADC()
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR212_SetADC(ref module);
            GetConfigFromModule();
            return res;
        }


        public _LTRNative.LTRERROR Recv(uint[] data, uint size, uint timeout)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR212_Recv(ref module, data, null, size, timeout);
            GetConfigFromModule();
            return res;
        }

        public _LTRNative.LTRERROR RecvWithTmark(uint[] data, uint[] start_mark, uint[] second_mark, uint size, uint timeout)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR212_Recv(ref module, data, start_mark, size, timeout);
            for (int i = 0; i < start_mark.Length; i++)
            {
                second_mark[i] = start_mark[i] & 0xFFFF;
                start_mark[i] = (start_mark[i] >> 16) & 0xFFFF;
            }
            GetConfigFromModule();
            return res;
        }


        public _LTRNative.LTRERROR ProcessData(uint[] src, double[] dest,
            int size, bool volt/*,INT *bad_num*/)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR212_ProcessData(ref module, src, dest, ref size, volt);
            GetConfigFromModule();
            return res;
        }


        public _LTRNative.LTRERROR CalcFS(double[] fsBase, double[] fs)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR212_CalcFS(ref module, ref fsBase[0], ref fs[0]);
            GetConfigFromModule();
            return res;
        }


        public _LTRNative.LTRERROR ProcessDataTest(uint[] src, double[] dest, int size, bool volt, uint[] bad_num)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR212_ProcessDataTest(ref module, src, dest, ref size, volt, ref bad_num[0]);
            GetConfigFromModule();
            return res;
        }


        public _LTRNative.LTRERROR ReadFilter(string fname, ltr212filter[] filter)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR212_ReadFilter(fname.ToCharArray(), ref filter[0]);
            GetConfigFromModule();

            decimation = filter[0].decimation;
            fs = filter[0].fs;
            koeff = filter[0].koeff;
            taps = filter[0].taps;
            return res;
        }

        public override _LTRNative.LTRERROR Init()
        {            
            _LTRNative.LTRERROR res = LTR212_Init(ref module);
            GetConfigFromModule();
            return res;
        }

        public override _LTRNative.LTRERROR Open(uint net_addr, ushort net_port,
            char[] crate_sn, int slot_num, string biosname)
        {
			if (net_addr ==0) net_addr = NewTLTR212.Channel.saddr;
			if (net_port ==0) net_port = NewTLTR212.Channel.sport;

            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR212_Open(ref module, net_addr, net_port, crate_sn, slot_num, biosname.ToCharArray());
            GetConfigFromModule(); 
            init_start_mark = (module.Channel.tmark >> 16) & 0xFFFF;
            init_second_mark = module.Channel.tmark & 0xFFFF;
            return res;
        }

        public override _LTRNative.LTRERROR Close()
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR212_Close(ref module);
            GetConfigFromModule(); 
            return res;
        }

        public override _LTRNative.LTRERROR CreateLChannel(int PhysChannel, int Scale)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR212_CreateLChannel(PhysChannel, Scale);
            GetConfigFromModule(); 
            return res;
        }

        public override _LTRNative.LTRERROR CreateLChannel2(uint PhysChannel, uint Scale, uint BridgeType)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR212_CreateLChannel2(PhysChannel, Scale, BridgeType);
            GetConfigFromModule();
            return res;
        }

        public override _LTRNative.LTRERROR Start()
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR212_Start(ref module);
            GetConfigFromModule();
            return res;
        }

        public override _LTRNative.LTRERROR Stop()
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR212_Stop(ref module);
            GetConfigFromModule();
            return res;
        }

        public override _LTRNative.LTRERROR Calibrate(byte[] LChannel_Mask, int mode, int reset)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR212_Calibrate(ref module, LChannel_Mask, mode, reset);
            GetConfigFromModule();
            return res;
        }

        public override _LTRNative.LTRERROR TestEEPROM()
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR212_TestEEPROM(ref module);
            GetConfigFromModule(); 
            return res;
        }

        public _LTRNative.LTRERROR WriteSerialNumber(string sn, ushort Code)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR212_WriteSerialNumber(ref module, sn.ToCharArray(), Code);
            GetConfigFromModule(); 
            return res;
        }


        public override _LTRNative.LTRERROR TestInterfaceStart(int PackDelay)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR212_TestInterfaceStart(ref module, PackDelay);
            GetConfigFromModule(); 
            return res;
        }


        public override uint CalcTimeOut(int n)
        {
            SetConfigToModule();
            uint res = LTR212_CalcTimeOut(ref module, n);
            GetConfigFromModule(); 
            return res;
        }



    }
}
