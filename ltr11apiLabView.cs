using System;
using System.Runtime.InteropServices;

namespace ltrModulesNet
{
    public class ltr11apiLabView : _ltr11api
    {

        public int StartADCMode;
        public int InpMode;
        public int LChQnt;                             /* число активных логических каналов (размер кадра) */

        public byte[] LChTbl;
        public int ADCMode;                            /* режим сбора данных или тип тестового режима */
        public int divider;                        // делитель тактовой частоты модуля, значения: 2..65535
        public int prescaler;                      // пределитель тактовой частоты модуля: * 1, 8, 64, 256, 1024
        public double ChRate;                          /* частота одного канала в кГц (период кадра) при*/

        public ltr11apiLabView()
        {
            GetConfigFromModule();
        }

        void GetConfigFromModule()
        {
            StartADCMode = module.StartADCMode;

            InpMode = module.InpMode;
            LChQnt = module.LChQnt;

            LChTbl = module.LChTbl;
            ADCMode = module.ADCMode;                            /* режим сбора данных или тип тестового режима */
            divider = module.ADCRate.divider;                        // делитель тактовой частоты модуля, значения: 2..65535
            prescaler = module.ADCRate.prescaler;                      // пределитель тактовой частоты модуля: * 1, 8, 64, 256, 1024
            ChRate = module.ChRate;                          /* частота одного канала в кГц (период кадра) при*/
        }

        void SetConfigToModule()
        {
            module.StartADCMode = StartADCMode;

            module.InpMode = InpMode;
            module.LChQnt = LChQnt;

			module.LChTbl = LChTbl;		
            
            module.ADCMode = ADCMode;                            /* режим сбора данных или тип тестового режима */
            module.ADCRate.divider = divider;                        // делитель тактовой частоты модуля, значения: 2..65535
            module.ADCRate.prescaler = prescaler;                      // пределитель тактовой частоты модуля: * 1, 8, 64, 256, 1024
            module.ChRate = ChRate;                          /* частота одного канала в кГц (период кадра) при*/
        }


        public override _LTRNative.LTRERROR GetConfig()
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR11_GetConfig(ref module);
            GetConfigFromModule();
            return res;
        }

        public int Recv(uint[] buffer, uint size, uint timeout)
        {
            SetConfigToModule();
            int res = _LTRNative.LTR_Recv(ref module.Channel, buffer, null, size, timeout);
            GetConfigFromModule();
            return res;
        }

        public _LTRNative.LTRERROR ProcessData(uint[] src, double[] dest,
            uint size, bool calibr,
            bool valueMain)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR11_ProcessData(ref module, src, dest, ref size, calibr, valueMain);
            GetConfigFromModule();

            return res;
        }		

        public override _LTRNative.LTRERROR SetADC()
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR11_SetADC(ref module);
            GetConfigFromModule(); 
            return res;
        }

        public override _LTRNative.LTRERROR Init()
        {            
            _LTRNative.LTRERROR res = LTR11_Init(ref module);
            GetConfigFromModule();
            return res;
        }

        public override _LTRNative.LTRERROR Close()
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR11_Close(ref module);
            GetConfigFromModule();
            return res;
        }

        public override _LTRNative.LTRERROR GetFrame(uint[] buf)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR11_GetFrame(ref module, buf);
            GetConfigFromModule();
            return res;
        }

        public override _LTRNative.LTRERROR Open(uint saddr, ushort sport, char[] csn, int slot_num)
        {
			if (saddr ==0) saddr = NewTLTR11.Channel.saddr;
			if (sport ==0) sport = NewTLTR11.Channel.sport;

            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR11_Open(ref module, saddr, sport, csn, slot_num);
            GetConfigFromModule();
            return res;
        }

        public override _LTRNative.LTRERROR Start()
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR11_Start(ref module);
            GetConfigFromModule(); 
            return res;
        }

        public override _LTRNative.LTRERROR Stop()
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR11_Stop(ref module);
            GetConfigFromModule();
            return res;
        }

       
    }
}
