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

        public int IIR;         // Флаг использования БИХ-фильтра
        public int FIR;         // Флаг использования КИХ-фильтра
        public int Decimation;  // Значение коэффициента децимации для КИХ-фильтра
        public int TAP;		 // Порядок КИХ-фильтра 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512 + 1)]
        public char[] IIR_Name; // Полный путь к файлу с коэфф-ми программного БИХ-фильтра 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512 + 1)]
        public char[] FIR_Name; // Полный путь к файлу с коэфф-ми программного КИХ-фильтра


        public int AcqMode; // Режим сбора данных
        public int UseClb;  // Флаг использования калибровочных коэфф-тов
        public int UseFabricClb;// Флаг использования заводских калибровочных коэфф-тов
        public int LChQnt;	 // Кол-во используемых виртуальных каналов		
        public int[] LChTbl;  //Таблица виртуальных каналов
        public int REF;		 // Флаг высокого опорного напряжения
        public int AC;		 // Флаг знакопеременного опорного напряжения
        public double Fs;     // Частота дискретизации АЦП


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
            AcqMode = module.AcqMode; // Режим сбора данных
            UseClb = module.UseClb;  // Флаг использования калибровочных коэфф-тов
            UseFabricClb = module.UseFabricClb;// Флаг использования заводских калибровочных коэфф-тов
            LChQnt = module.LChQnt;	 // Кол-во используемых виртуальных каналов		
            LChTbl = module.LChTbl;  //Таблица виртуальных каналов
            REF = module.REF;		 // Флаг высокого опорного напряжения
            AC = module.AC;		 // Флаг знакопеременного опорного напряжения
            Fs = module.Fs;     // Частота дискретизации АЦП

            IIR = module.filter.IIR;         // Флаг использования БИХ-фильтра
            FIR = module.filter.FIR;         // Флаг использования КИХ-фильтра
            Decimation = module.filter.Decimation;  // Значение коэффициента децимации для КИХ-фильтра
            TAP = module.filter.TAP;		 // Порядок КИХ-фильтра 

            IIR_Name = module.filter.IIR_Name; // Полный путь к файлу с коэфф-ми программного БИХ-фильтра 

            FIR_Name = module.filter.FIR_Name; // Полный путь к файлу с коэфф-ми программного КИХ-фильтра
        }

        void SetConfigToModule()
        {
            module.AcqMode = AcqMode; // Режим сбора данных
            module.UseClb = UseClb;  // Флаг использования калибровочных коэфф-тов
            module.UseFabricClb = UseFabricClb;// Флаг использования заводских калибровочных коэфф-тов
            module.LChQnt = LChQnt;	 // Кол-во используемых виртуальных каналов		
            module.LChTbl = LChTbl;  //Таблица виртуальных каналов
            module.REF = REF;		 // Флаг высокого опорного напряжения
            module.AC = AC;		 // Флаг знакопеременного опорного напряжения
            module.Fs = Fs;     // Частота дискретизации АЦП

            module.filter.IIR = IIR;         // Флаг использования БИХ-фильтра
            module.filter.FIR = FIR;         // Флаг использования КИХ-фильтра
            module.filter.Decimation = Decimation;  // Значение коэффициента децимации для КИХ-фильтра
            module.filter.TAP = TAP;		 // Порядок КИХ-фильтра 

            module.filter.IIR_Name = IIR_Name; // Полный путь к файлу с коэфф-ми программного БИХ-фильтра 

            module.filter.FIR_Name = FIR_Name; // Полный путь к файлу с коэфф-ми программного КИХ-фильтра
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
