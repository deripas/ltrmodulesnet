using System;
using System.Runtime.InteropServices;

namespace ltrModulesNet
{
    public class ltr51apiLabView : _ltr51api
    {

        public ushort ChannelsEna;       // Маска доступных каналов (показывает, какие субмодули подкл.)

        public int SetUserPars;	   // Указывает, задаются ли Fs и Base пользователем

        public int LChQnt;             // Количество логических каналов    

        public uint[] LChTbl;       // Таблица логических каналов

        public double Fs;                // Частота выборки сэмплов
        public ushort Base;                // Делитель частоты измерения
        public double F_Base;			 // Частота измерений F_Base=Fs/Base


        public int AcqTime;            // Время сбора в миллисекундах       
        public int TbaseQnt;		   // Количество периодов измерений, необходимое для обеспечения указанного интревала измерения



        public ltr51apiLabView()
        {           
            GetConfigFromModule();
        }

        void GetConfigFromModule()
        {
            ChannelsEna = module.ChannelsEna;       // Маска доступных каналов (показывает, какие субмодули подкл.)

            SetUserPars = module.SetUserPars;	   // Указывает, задаются ли Fs и Base пользователем

            LChQnt = module.LChQnt;             // Количество логических каналов    

            LChTbl = module.LChTbl;       // Таблица логических каналов

            Fs = module.Fs;                // Частота выборки сэмплов
            Base = module.Base;                // Делитель частоты измерения
            F_Base = module.F_Base;			 // Частота измерений F_Base=Fs/Base


            AcqTime = module.AcqTime;            // Время сбора в миллисекундах       
            TbaseQnt = module.TbaseQnt;		   // Количество периодов измерений, необходимое для обеспечения указанного интревала измерения			
        }

        void SetConfigToModule()
        {
            module.ChannelsEna = ChannelsEna;       // Маска доступных каналов (показывает, какие субмодули подкл.)

            module.SetUserPars = SetUserPars;	   // Указывает, задаются ли Fs и Base пользователем

            module.LChQnt = LChQnt;             // Количество логических каналов    

            module.LChTbl = LChTbl;       // Таблица логических каналов

            module.Fs = Fs;                // Частота выборки сэмплов
            module.Base = Base;                // Делитель частоты измерения
            module.F_Base = F_Base;			 // Частота измерений F_Base=Fs/Base


            module.AcqTime = AcqTime;            // Время сбора в миллисекундах       
            module.TbaseQnt = TbaseQnt;		   // Количество периодов измерений, необходимое для обеспечения указанного интревала измерения			        
        }

        public override _LTRNative.LTRERROR Config()
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR51_Config(ref module);
            GetConfigFromModule(); 
            return res;
        }

        public _LTRNative.LTRERROR Recv(uint[] data, uint size, uint timeout)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR51_Recv(ref module, data, null, size, timeout);
            GetConfigFromModule(); 
            return res;
        }


        public _LTRNative.LTRERROR ProcessData(uint[] src, uint[] dest, double[] Frequency, int size)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR51_ProcessData(ref module, src, dest, Frequency, ref size);
            GetConfigFromModule(); 
            return res;
        }

        public override _LTRNative.LTRERROR Init()
        {           
            _LTRNative.LTRERROR res = LTR51_Init(ref module);
            GetConfigFromModule(); 
            return res;
        }


        public override _LTRNative.LTRERROR Open(uint net_addr, ushort net_port,
            char[] crate_sn, int slot_num, string ttf)
        {
			if (net_addr ==0) net_addr = NewTLTR51.Channel.saddr;
			if (net_port ==0) net_port = NewTLTR51.Channel.sport;

			char [] ttf1 = ttf.ToCharArray();
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR51_Open(ref module, net_addr, net_port, crate_sn, slot_num, ttf1);
            GetConfigFromModule(); 
            return res;
        }


        public override _LTRNative.LTRERROR IsOpened()
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR51_IsOpened(ref module);
            GetConfigFromModule(); 
            return res;
        }


        public override _LTRNative.LTRERROR Close()
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR51_Close(ref module);
            GetConfigFromModule(); 
            return res;
        }

        public override _LTRNative.LTRERROR WriteEEPROM(int Address, byte val)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR51_WriteEEPROM(ref module, Address, val);
            GetConfigFromModule(); 
            return res;
        }


        public override _LTRNative.LTRERROR ReadEEPROM(int Address, byte[] val)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR51_ReadEEPROM(ref module, Address, val);
            GetConfigFromModule(); 
            return res;
        }


        public  _LTRNative.LTRERROR CreateLChannel(int PhysChannel, double HighThreshold,
            double LowThreshold, int ThresholdRange, int EdgeMode)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR51_CreateLChannel(PhysChannel, ref HighThreshold, ref LowThreshold, ThresholdRange,
                EdgeMode);
            GetConfigFromModule(); 
            return res;
        }       

        public override _LTRNative.LTRERROR Start()
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR51_Start(ref module);
            GetConfigFromModule(); 
            return res;
        }


        public override _LTRNative.LTRERROR Stop()
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR51_Stop(ref module);
            GetConfigFromModule(); 
            return res;
        }

        public override _LTRNative.LTRERROR GetThresholdVals(int LChNumber,
            double[] HighThreshold, double[] LowThreshold, int ThresholdRange)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR51_GetThresholdVals(ref module, LChNumber, HighThreshold, LowThreshold, ThresholdRange);
            GetConfigFromModule(); 
            return res;
        }


        public override uint CalcTimeOut(int n)
        {
            SetConfigToModule();
            uint res = LTR51_CalcTimeOut(ref module, n);
            GetConfigFromModule(); 
            return res;
        }


        public override _LTRNative.LTRERROR EvaluateFrequencies()
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR51_EvaluateFrequencies(ref module);
            GetConfigFromModule(); 
            return res;
        }

      
    }
}
