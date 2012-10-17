using System;
using System.Runtime.InteropServices;

namespace ltrModulesNet
{
    

    public class ltr114apiLabView : _ltr114api
    {

        public int LChQnt { get { return module.LChQnt; } set { module.LChQnt = value; } }                            /* число активных логических каналов (размер кадра) */
        public Sync SyncMode { get { return module.SyncMode; } set { module.SyncMode = value; } }
        
        public ushort Interval { get { return module.Interval; } set { module.Interval = value; } }                          //длина межкадрового интервала
        public Features SpecialFeatures { get { return module.SpecialFeatures; } set { module.SpecialFeatures = value; } }                   //дополнительные возможности модуля (подключение термометра, блокировка коммутации)

        public int FreqDivider { get { return module.FreqDivider; } set { module.FreqDivider = value; } }                      // делитель частоты АЦП (2..8000)
                                           // частота дискретизации равна F = LTR114_CLOCK/(LTR114_ADC_DIVIDER*FreqDivider)
        public int FrameLength { get { return module.FrameLength; } }

        public double Freq { get { return 8000 / module.FreqDivider; } }

        public LTR114_TINFO ModuleInfo { get { return module.ModuleInfo; } }
        
        public LTR114_LCHANNEL[] LChTbl
        {
            get { return module.LChTbl; }
            set
            {
                for (int i = 0; (i < value.Length) && (i<128); i++)
                    module.LChTbl[i] = value[i];
            }
        }

        public ltr114apiLabView()
        {
            
        }

        
        public override _LTRNative.LTRERROR GetConfig()
        {
            _LTRNative.LTRERROR res = base.GetConfig();
            return res;
        }

        public _LTRNative.LTRERROR Recv(uint[] buffer, uint size, uint timeout)
        {
            _LTRNative.LTRERROR res = LTR114_Recv(ref module, buffer, null, size, timeout);
            return res;
        }

        public override _LTRNative.LTRERROR ProcessData(uint[] src, double[] dest,
             ref int size, CorrectionMode correction_mode, ProcFlags flags)
        {
            _LTRNative.LTRERROR res = base.ProcessData(src, dest, ref size, correction_mode, flags);
            return res;
        }


        public override _LTRNative.LTRERROR ProcessDataTherm(uint[] src, double[] dest, double[] therm,
            ref int size, out int tcnt, CorrectionMode correction_mode, ProcFlags flags)
        {
            _LTRNative.LTRERROR res = base.ProcessDataTherm(src, dest, therm, ref size, out tcnt,
            correction_mode, flags);
            return res;
        }


        public override _LTRNative.LTRERROR SetADC()
        {
            _LTRNative.LTRERROR res = base.SetADC();
            return res;
        }

        public override _LTRNative.LTRERROR Init()
        {
            _LTRNative.LTRERROR res = base.Init();
            return res;
        }

        public override _LTRNative.LTRERROR Close()
        {
            _LTRNative.LTRERROR res = base.Close();
            return res;
        }

        public override _LTRNative.LTRERROR GetFrame(uint[] buf)
        {
            _LTRNative.LTRERROR res = base.GetFrame(buf);
            return res;
        }

        public override _LTRNative.LTRERROR Open(uint saddr, ushort sport, char[] csn, int slot_num)
        {
            if (saddr == 0) saddr = NewTLTR114.Channel.saddr;
            if (sport == 0) sport = NewTLTR114.Channel.sport;
            _LTRNative.LTRERROR res = base.Open(saddr, sport, csn, slot_num);
            return res;
        }

        public override _LTRNative.LTRERROR Start()
        {
            _LTRNative.LTRERROR res = base.Start();
            return res;
        }

        public override _LTRNative.LTRERROR Stop()
        {
            _LTRNative.LTRERROR res = base.Stop();
            return res;
        }


    }
}