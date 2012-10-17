using System;
using System.Runtime.InteropServices;

namespace ltrModulesNet
{
    public class ltr11apiLabView : _ltr11api
    {

        public int StartADCMode;
        public int InpMode;
        public int LChQnt;                             /* ����� �������� ���������� ������� (������ �����) */

        public byte[] LChTbl;
        public int ADCMode;                            /* ����� ����� ������ ��� ��� ��������� ������ */
        public int divider;                        // �������� �������� ������� ������, ��������: 2..65535
        public int prescaler;                      // ����������� �������� ������� ������: * 1, 8, 64, 256, 1024
        public double ChRate;                          /* ������� ������ ������ � ��� (������ �����) ���*/

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
            ADCMode = module.ADCMode;                            /* ����� ����� ������ ��� ��� ��������� ������ */
            divider = module.ADCRate.divider;                        // �������� �������� ������� ������, ��������: 2..65535
            prescaler = module.ADCRate.prescaler;                      // ����������� �������� ������� ������: * 1, 8, 64, 256, 1024
            ChRate = module.ChRate;                          /* ������� ������ ������ � ��� (������ �����) ���*/
        }

        void SetConfigToModule()
        {
            module.StartADCMode = StartADCMode;

            module.InpMode = InpMode;
            module.LChQnt = LChQnt;

			module.LChTbl = LChTbl;		
            
            module.ADCMode = ADCMode;                            /* ����� ����� ������ ��� ��� ��������� ������ */
            module.ADCRate.divider = divider;                        // �������� �������� ������� ������, ��������: 2..65535
            module.ADCRate.prescaler = prescaler;                      // ����������� �������� ������� ������: * 1, 8, 64, 256, 1024
            module.ChRate = ChRate;                          /* ������� ������ ������ � ��� (������ �����) ���*/
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
