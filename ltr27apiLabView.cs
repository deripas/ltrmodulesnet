using System;
using System.Runtime.InteropServices;

namespace ltrModulesNet
{
    /// <summary>
    /// Summary description for ltr27apiLabView.
    /// </summary>
    public class ltr27apiLabView : _ltr27api
    {
        public byte SubChannel;
        public byte FrequencyDivisor;

        public ltr27apiLabView()
        {            
            GetConfigFromModule();
        }

        void GetConfigFromModule()
        {
            SubChannel = module.SubChannel;
            FrequencyDivisor = module.FrequencyDivisor;
        }

        void SetConfigToModule()
        {
            module.SubChannel = SubChannel;
            module.FrequencyDivisor = FrequencyDivisor;
        }


        public override _LTRNative.LTRERROR GetConfig()
        {
            _LTRNative.LTRERROR res = LTR27_GetConfig(ref module);
            GetConfigFromModule();
            return res;
        }


        public override _LTRNative.LTRERROR SetConfig()
        {
            SetConfigToModule();            
            _LTRNative.LTRERROR res = LTR27_SetConfig(ref module);
            GetConfigFromModule();
            return res;
        }

        public _LTRNative.LTRERROR Recv(uint[] Data, uint size, uint timeout)
        {
            _LTRNative.LTRERROR res = LTR27_Recv(ref module, Data, null, size, timeout);
            GetConfigFromModule();
            return res;
        }

        public override _LTRNative.LTRERROR Init()
        {
            _LTRNative.LTRERROR res = LTR27_Init(ref module);
            GetConfigFromModule();
            return res;
        }


        public override _LTRNative.LTRERROR Open(uint saddr, ushort sport, char[] csn, ushort cc)
        {
			if (saddr ==0) saddr = NewTLTR27.Channel.saddr;
			if (sport ==0) sport = NewTLTR27.Channel.sport;
            SetConfigToModule(); 
            _LTRNative.LTRERROR res = LTR27_Open(ref module, saddr, sport, csn, cc);
            GetConfigFromModule();
            return res;
        }


        public override _LTRNative.LTRERROR Close()
        {
            SetConfigToModule(); 
            _LTRNative.LTRERROR res = LTR27_Close(ref module);
            GetConfigFromModule();
            return res;
        }


        public override _LTRNative.LTRERROR IsOpened()
        {
            SetConfigToModule(); 
            _LTRNative.LTRERROR res = LTR27_IsOpened(ref module);
            GetConfigFromModule(); 
            return res;
        }

        public override _LTRNative.LTRERROR Echo()
        {
            SetConfigToModule(); 
            _LTRNative.LTRERROR res = LTR27_Echo(ref module);
            GetConfigFromModule(); 
            return res;
        }       


        public override _LTRNative.LTRERROR ADCStart()
        {
            SetConfigToModule(); 
            _LTRNative.LTRERROR res = LTR27_ADCStart(ref module);
            GetConfigFromModule(); 
            return res;
        }


        public override _LTRNative.LTRERROR ADCStop()
        {
            SetConfigToModule(); 
            _LTRNative.LTRERROR res = LTR27_ADCStop(ref module);
            GetConfigFromModule();
            return res;
        }     


        public override _LTRNative.LTRERROR ProcessData(uint[] src_data, double[] dst_data,
            uint size, bool calibr,
            bool valueMain)
        {
            SetConfigToModule(); 
            _LTRNative.LTRERROR res = LTR27_ProcessData(ref module, src_data, dst_data, ref size, calibr, valueMain);
            GetConfigFromModule(); 
            return res;
        }

        public override _LTRNative.LTRERROR GetDescription(ushort flags)
        {
            SetConfigToModule(); 
            _LTRNative.LTRERROR res = LTR27_GetDescription(ref module, flags);
            if (res==_LTRNative.LTRERROR.OK)
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
            GetConfigFromModule(); 
            return res;
        }

 
    }
}
