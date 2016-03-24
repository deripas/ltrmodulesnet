using System;
using System.Runtime.InteropServices;

namespace ltrModulesNet
{
	/// <summary>
	/// Summary description for ltr22apiLabView.
	/// </summary>
	public class ltr22apiLabView:_ltr22api
	{	
		// настройки модуля            
		public byte Fdiv_rg;						// дивайзер частоты клоков 1..15		
		public bool Adc384;						// дополнительный дивайзер частоты сэмплов true =3 false =2		
		public bool AC_DC_State;					// состояние true =AC+DC false=AC 		
		public bool MeasureADCZero;				// измерение Zero true - включено false - выключено		
		public byte[] ADCChannelRange;// предел имзерений АЦП по каналам 0 - 1В 1 - 0.3В 2 - 0.1В 3 - 0.03В 4 - 10В 5 - 3В    
		
		public byte[] ChannelEnabled;		// Состояние каналов, включен - true выключен - false
		public int FreqDiscretizationIndex;	
		public byte SyncType;
		public bool SyncMaster;       

		public ltr22apiLabView()
		{   		
			GetConfigFromModule();
		}		
		
		void GetConfigFromModule()
		{
			Fdiv_rg=module.Fdiv_rg;						// дивайзер частоты клоков 1..15		
			Adc384=module.Adc384;						// дополнительный дивайзер частоты сэмплов true =3 false =2		
			AC_DC_State=module.AC_DC_State;					// состояние true =AC+DC false=AC 		
			MeasureADCZero = module.MeasureADCZero;				// измерение Zero true - включено false - выключено		
			ADCChannelRange = module.ADCChannelRange;// предел имзерений АЦП по каналам 0 - 1В 1 - 0.3В 2 - 0.1В 3 - 0.03В 4 - 10В 5 - 3В    
		
			ChannelEnabled = module.ChannelEnabled;		// Состояние каналов, включен - true выключен - false
			FreqDiscretizationIndex=module.FreqDiscretizationIndex;	
			SyncType=module.SyncType;
			SyncMaster=module.SyncMaster;            
		}

		void SetConfigToModule()
		{
			module.Fdiv_rg = Fdiv_rg;
			module.Adc384 = Adc384;
			module.AC_DC_State = AC_DC_State;
			module.MeasureADCZero = MeasureADCZero;
			
			module.ADCChannelRange = ADCChannelRange;
			module.ChannelEnabled = ChannelEnabled;			
            
			module.SyncType = SyncType;
			module.SyncMaster = SyncMaster;	
		}

        public float GetFactoryCalibOffset(uint DiscFreqNumber, uint Channel, uint Range)
        {
            if (Channel >= 0 && Channel < 4 &&
                DiscFreqNumber >= 0 && DiscFreqNumber < LTR22_DISK_FREQ_ARRAY.Length &&
                Range >= 0 && Range < LTR22_RANGE_NUMBER)
            {
                return module.ADCCalibration[(Channel * LTR22_MAX_DISC_FREQ_NUMBER) + DiscFreqNumber].FactoryCalibOffset[Range];
            }
            else return float.MinValue;
        }

        public float GetFactoryCalibScale(uint DiscFreqNumber, uint Channel, uint Range)
        {
            if (Channel >= 0 && Channel < 4 &&
                DiscFreqNumber >= 0 && DiscFreqNumber < LTR22_DISK_FREQ_ARRAY.Length &&
                Range >= 0 && Range < LTR22_RANGE_NUMBER)
            {
                return module.ADCCalibration[(Channel * LTR22_MAX_DISC_FREQ_NUMBER) + DiscFreqNumber].FactoryCalibScale[Range];
            }
            else return float.MinValue;
        }
		
		
		public _LTRNative.LTRERROR Recv([In,Out] uint[] data, uint size, uint timeout)
		{			
			_LTRNative.LTRERROR res=LTR22_Recv(ref module, data, null, size, timeout);
			GetConfigFromModule();
			return res;
		}	

		public override _LTRNative.LTRERROR Init()
		{			
			_LTRNative.LTRERROR res=LTR22_Init(ref module);
			GetConfigFromModule();
			return res;
		}

		
		public override _LTRNative.LTRERROR Close()
		{
			SetConfigToModule();
			_LTRNative.LTRERROR res=LTR22_Close(ref module);
			GetConfigFromModule();
			return res;
		}

        [DllImport("ltr22api.dll")]
        static extern _LTRNative.LTRERROR LTR22_Open(ref TLTR22 module, uint saddr, ushort sport, string csn, ushort slot_num);

        public virtual _LTRNative.LTRERROR Open2(uint saddr, ushort sport, string csn, ushort cc)
        {
            if (saddr == 0) saddr = NewTLTR22.Channel.saddr;
            if (sport == 0) sport = NewTLTR22.Channel.sport;

            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR22_Open(ref module, saddr, sport, csn, cc);
            GetConfigFromModule();
            return res;
        }


		
		public override _LTRNative.LTRERROR Open(uint saddr, ushort sport, [In,Out] byte[] csn, ushort cc)
		{
			if (saddr ==0) saddr = NewTLTR22.Channel.saddr;
			if (sport ==0) sport = NewTLTR22.Channel.sport;

			SetConfigToModule();
			_LTRNative.LTRERROR res=LTR22_Open(ref module, saddr, sport, csn, cc);
			GetConfigFromModule();
			return res;
		}
		
		public override _LTRNative.LTRERROR IsOpened()
		{
			SetConfigToModule();
			_LTRNative.LTRERROR res=LTR22_IsOpened(ref module);
			GetConfigFromModule();
			return res;
		}
		
		public override _LTRNative.LTRERROR GetConfig()
		{				
			_LTRNative.LTRERROR res=LTR22_GetConfig(ref module);
			GetConfigFromModule();
			return res;
		}
		
		public override _LTRNative.LTRERROR SetConfig()
		{
			SetConfigToModule();
			_LTRNative.LTRERROR res=LTR22_SetConfig(ref module);
			GetConfigFromModule();
			return res;
		}
		
		public override _LTRNative.LTRERROR ClearBuffer(bool wait_response)
		{
			SetConfigToModule();
			_LTRNative.LTRERROR res=LTR22_ClearBuffer(ref module, wait_response);
			GetConfigFromModule();
			return res;
		}
		
		public override _LTRNative.LTRERROR StartADC(bool WaitSync)
		{
			SetConfigToModule();
			_LTRNative.LTRERROR res=LTR22_StartADC(ref module, WaitSync);
			GetConfigFromModule();
			return res;
		}
		
		public override _LTRNative.LTRERROR StopADC()
		{
			SetConfigToModule();
			_LTRNative.LTRERROR res=LTR22_StopADC(ref module);
			GetConfigFromModule();
			return res;
		}
		
		public override _LTRNative.LTRERROR SetSyncPriority(bool SyncMaster)
		{
			SetConfigToModule();
			_LTRNative.LTRERROR res=LTR22_SetSyncPriority(ref module, SyncMaster);
			GetConfigFromModule();
			return res;
		}
		
		public override _LTRNative.LTRERROR SyncPhaze(uint timeout)
		{
			SetConfigToModule();
			_LTRNative.LTRERROR res=LTR22_SyncPhaze(ref module, timeout);
			GetConfigFromModule();
			return res;
		}
		
		public override _LTRNative.LTRERROR SwitchMeasureADCZero(bool SetMeasure)
		{
			SetConfigToModule();
			_LTRNative.LTRERROR res=LTR22_SwitchMeasureADCZero(ref module, SetMeasure);
			GetConfigFromModule();
			return res;
		}
		
		public override _LTRNative.LTRERROR SetFreq(bool adc384, byte Freq_dv)
		{
			SetConfigToModule();
			_LTRNative.LTRERROR res=LTR22_SetFreq(ref module, adc384, Freq_dv);
			GetConfigFromModule();
			return res;
		}
		
		public override _LTRNative.LTRERROR SwitchACDCState(bool ACDCState)
		{
			SetConfigToModule();
			_LTRNative.LTRERROR res=LTR22_SwitchACDCState(ref module, ACDCState);
			GetConfigFromModule();
			return res;
		}
		
		public override _LTRNative.LTRERROR SetADCRange(byte ADCChannel, byte ADCChannelRange)
		{
			SetConfigToModule();
			_LTRNative.LTRERROR res=LTR22_SetADCRange(ref module, ADCChannel, ADCChannelRange);
			GetConfigFromModule();
			return res;
		}
		
		public override _LTRNative.LTRERROR SetADCChannel(byte ADCChannel, bool EnableADC)
		{
			SetConfigToModule();
			_LTRNative.LTRERROR res=LTR22_SetADCChannel(ref module, ADCChannel, EnableADC);
			GetConfigFromModule();
			return res;
		}
		
		public override _LTRNative.LTRERROR GetCalibrovka()
		{
			SetConfigToModule();
			_LTRNative.LTRERROR res=LTR22_GetCalibrovka(ref module);
			GetConfigFromModule();
			return res;
		}	
		
		public override _LTRNative.LTRERROR GetModuleDescription()
		{
			SetConfigToModule();
			_LTRNative.LTRERROR res=LTR22_GetModuleDescription(ref module);
			GetConfigFromModule();
			return res;
		}
		
		public override _LTRNative.LTRERROR ProcessData([In,Out]uint[] src_data, [In,Out] double[] dst_data,
			uint size, bool calibrMainPset, bool calibrExtraVolts, [In,Out] byte[] OverflowFlags)
		{
			SetConfigToModule();				
			return LTR22_ProcessData(ref module, src_data, dst_data, size, calibrMainPset, calibrExtraVolts, OverflowFlags);
		}
		
		public override _LTRNative.LTRERROR ReadAVREEPROM([In,Out] byte[] Data, uint BeginAddress, uint size)
		{
			SetConfigToModule();
			_LTRNative.LTRERROR res=LTR22_ReadAVREEPROM(ref module, Data, BeginAddress, size);
			GetConfigFromModule();
			return res;
		}
		
		public override _LTRNative.LTRERROR WriteAVREEPROM([In,Out] byte[] Data, uint BeginAddress, uint size)
		{
			SetConfigToModule();
			_LTRNative.LTRERROR res=LTR22_WriteAVREEPROM(ref module, Data, BeginAddress, size);
			GetConfigFromModule();
			return res;
		}
		
		public override _LTRNative.LTRERROR TestHardwareInterface()
		{
			SetConfigToModule();
			_LTRNative.LTRERROR res=LTR22_TestHardwareInterface(ref module);
			GetConfigFromModule();
			return res;
		}
		
		public override _LTRNative.LTRERROR GetADCData([In,Out] double[] Data, uint Size, uint time,
			bool calibrMainPset, bool calibrExtraVolts)
		{
			SetConfigToModule();
			_LTRNative.LTRERROR res=LTR22_GetADCData(ref module, Data, Size, time, calibrMainPset, calibrExtraVolts);
			GetConfigFromModule();
			return res;
		}
		
		public override _LTRNative.LTRERROR ReopenModule()
		{
			SetConfigToModule();
			_LTRNative.LTRERROR res=LTR22_ReopenModule(ref module);
			GetConfigFromModule();
			return res;
		}

   
	}
}
