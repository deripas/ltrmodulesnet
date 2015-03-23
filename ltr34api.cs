using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ltrModulesNet
{
    public class ltr34api
    {
        [DllImport("ltr34api.dll")]
        static extern _LTRNative.LTRERROR LTR34_Init(ref TLTR34 module);

        [DllImport("ltr34api.dll")]
        static extern _LTRNative.LTRERROR LTR34_Open(ref TLTR34 module, uint net_addr, ushort net_port,
                                                     string crate_sn, int slot_num);
        [DllImport("ltr34api.dll")]
        static extern _LTRNative.LTRERROR LTR34_Close(ref TLTR34 module);

        [DllImport("ltr34api.dll")]
        static extern _LTRNative.LTRERROR LTR34_IsOpened(ref TLTR34 module);

        [DllImport("ltr34api.dll")]
        static extern int LTR34_Recv(ref TLTR34 module, uint[] data, uint[] tmark, uint size, uint timeout);

        [DllImport("ltr34api.dll")]
        static extern uint LTR34_CreateLChannel(byte PhysChannel, bool ScaleFlag);

        [DllImport("ltr34api.dll")]
        static extern int LTR34_Send(ref TLTR34 module, uint[] data, uint size, uint timeout);

        [DllImport("ltr34api.dll")]
        static extern _LTRNative.LTRERROR LTR34_ProcessData(ref TLTR34 module, double[] source, uint[] dest, uint size, bool volt);

        [DllImport("ltr34api.dll")]
        static extern _LTRNative.LTRERROR LTR34_Config(ref TLTR34 module);

        [DllImport("ltr34api.dll")]
        static extern _LTRNative.LTRERROR LTR34_DACStart(ref TLTR34 module);

        [DllImport("ltr34api.dll")]
        static extern _LTRNative.LTRERROR LTR34_DACStop(ref TLTR34 module);

        [DllImport("ltr34api.dll")]
        static extern _LTRNative.LTRERROR LTR34_Reset(ref TLTR34 module);


        [DllImport("ltr34api.dll")]
        static extern _LTRNative.LTRERROR LTR34_GetCalibrCoeffs(ref TLTR34 module);

        [DllImport("ltr34api.dll")]
        static extern _LTRNative.LTRERROR LTR34_TestEEPROM(ref TLTR34 module);

        [DllImport("ltr34api.dll")]
        static extern _LTRNative.LTRERROR LTR34_ReadFlash(ref TLTR34 module, byte[] data, ushort size, ushort Address);

        [DllImport("ltr34api.dll")]
        static extern IntPtr LTR34_GetErrorString(int ErrorCode);


        public const int LTR34_MAX_BUFFER_SIZE = 2097151;
        public const int LTR34_EEPROM_SIZE = 2048;
        public const int LTR34_USER_EEPROM_SIZE = 1024;
        public const int LTR34_DAC_NUMBER_MAX = 8;

        public enum AckType : byte
        {
            ECHO = 0,
            STATUS = 1
        } ;

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct DAC_CHANNEL_CALIBRATION
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (2 * LTR34_DAC_NUMBER_MAX))]
            float[] FactoryCalibrOffset_;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (2 * LTR34_DAC_NUMBER_MAX))]
            float[] FactoryCalibrScale_;

            public float[] FactoryCalibrOffset { get { return FactoryCalibrOffset_; } }
            public float[] FactoryCalibrScale { get { return FactoryCalibrScale_; } }
        };


        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct INFO
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            char[] Name_;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            char[] Serial_;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            char[] FPGA_Version_;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            char[] CalibrVersion_;
            byte MaxChannelQnt_;

            public string Name { get { return new string(Name_).Split('\0')[0]; } }
            public string Serial { get { return new string(Serial_).Split('\0')[0]; } }
            public string FPGA_Version { get { return new string(FPGA_Version_).Split('\0')[0]; } }
            public string CalibrVersion { get { return new string(CalibrVersion_).Split('\0')[0]; } }
            public byte MaxChannelQnt { get { return MaxChannelQnt_; } }
        };


        //**** конфигурация модуля
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        struct TLTR34
        {
            int Size;// Размер структуры 
            _LTRNative.TLTR Channel;   // структура описывающая модуль в крейте – описание в ltrapi.pdf 		    
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] LChTbl;                  // Таблица логических каналов
            //**** настройки модуля           
            byte FrequencyDivisor_;            // делитель частоты дискретизации 0..60 (31.25..500 кГц)
            byte ChannelQnt_;             // число каналов (1, 2, 4, 8)
            [MarshalAs(UnmanagedType.U1)]
            bool UseClb_;
            [MarshalAs(UnmanagedType.U1)]
            AckType AcknowledgeType_;             // тип подтверждения true - высылать подтверждение каждого слова, false- высылать состояние буффера каждые 100 мс
            [MarshalAs(UnmanagedType.U1)]
            bool ExternalStart_;               // внешний старт true - внешний старт, false - внутренний
            [MarshalAs(UnmanagedType.U1)]
            bool RingMode_;                    // режим кольца  true - режим кольца, false - потоковый режим
            [MarshalAs(UnmanagedType.U1)]
            bool BufferFull_;					// статус - буффер переполнен - ошибка
            [MarshalAs(UnmanagedType.U1)]
            bool BufferEmpty_;					// статус - буффер пуст - ошибка
            [MarshalAs(UnmanagedType.U1)]
            bool DACRunning_;					// статус - запущена ли генерация

            float FrequencyDAC_;				// статус - частота - на которую настроен ЦАП в текущей конфигурации
            DAC_CHANNEL_CALIBRATION DacCalibration_;
            INFO ModuleInfo_;

            public byte FrequencyDivisor { get { return FrequencyDivisor_; } set { FrequencyDivisor_ = value; } }
            public byte ChannelQnt { get { return ChannelQnt_; } set { ChannelQnt_ = value; } }
            public bool UseClb { get { return UseClb_; } set { UseClb_ = value; } }
            public AckType AcknowledgeType { get { return AcknowledgeType_; } set { AcknowledgeType_ = value; } }
            public bool ExternalStart { get { return ExternalStart_; } set { ExternalStart_ = value; } }
            public bool RingMode { get { return RingMode_; } set { RingMode_ = value; } }
            public bool BufferFull { get { return BufferFull_; } }
            public bool BufferEmpty { get { return BufferEmpty_; } }
            public bool DACRunning { get { return DACRunning_; } }
            public float FrequencyDAC { get { return FrequencyDAC_; } }
            public DAC_CHANNEL_CALIBRATION DacCalibration { get { return DacCalibration_; } }
            public INFO ModuleInfo { get { return ModuleInfo_; } }
        }

        TLTR34 module;

        public ltr34api() 
        {
            LTR34_Init(ref module);	
        }

        ~ltr34api()
        {
            LTR34_Close(ref module);
        }

		
		public virtual _LTRNative.LTRERROR Init()
		{
			return LTR34_Init(ref module);
		}

        public virtual _LTRNative.LTRERROR Open (uint saddr, ushort sport, string csn, ushort cc)
		{
			return LTR34_Open(ref module, saddr, sport, csn, cc);
		}

        public virtual _LTRNative.LTRERROR Open(string csn, ushort cc)
        {
            return Open(_LTRNative.SADDR_DEFAULT, _LTRNative.SPORT_DEFAULT, csn, cc);
        }

        public virtual _LTRNative.LTRERROR Open(ushort cc)
        {
            return Open("", cc);
        }

        public virtual _LTRNative.LTRERROR Close ()
		{
			return LTR34_Close(ref module);
		}        

        public virtual _LTRNative.LTRERROR IsOpened ()
		{
			return LTR34_IsOpened(ref module);
		}

        public virtual int Recv(uint[] Data, uint size, uint[] tstamp, uint timeout)
        {
            return LTR34_Recv(ref module, Data, tstamp, size, timeout);
        }

        public virtual int Recv(uint[] Data, uint size, uint timeout)
        {
            return LTR34_Recv(ref module, Data, null, size, timeout);
        }

        public virtual void SetLChannel(byte lch, byte PhysChannel, bool ScaleFlag)
        {
            module.LChTbl[lch] = LTR34_CreateLChannel(PhysChannel, ScaleFlag);
        }


        public virtual int Send(uint[] data, uint size, uint timeout)
        {
            return LTR34_Send(ref module, data, size, timeout);
        }

        public virtual _LTRNative.LTRERROR ProcessData(double[] source, uint[] dest, uint size, bool volt)
        {
            return LTR34_ProcessData(ref module, source, dest, size, volt);
        }

        public virtual _LTRNative.LTRERROR Config()
        {
            return LTR34_Config(ref module);
        }

        public virtual _LTRNative.LTRERROR DACStart()
        {
            return LTR34_DACStart(ref module);
        }

        public virtual _LTRNative.LTRERROR DACStop()
        {
            return LTR34_DACStop(ref module);
        }

        public virtual  _LTRNative.LTRERROR Reset()
        {
            return LTR34_Reset(ref module);
        }

        public virtual _LTRNative.LTRERROR TestEEPROM()
        {
            return LTR34_TestEEPROM(ref module);
        }

        public virtual _LTRNative.LTRERROR ReadFlash(byte[] data, ushort size, ushort Address)
        {
            return LTR34_ReadFlash(ref module, data, size, Address);
        }

        public static string GetErrorString(_LTRNative.LTRERROR err)
        {
            IntPtr ptr = LTR34_GetErrorString((int)err);
            string str = Marshal.PtrToStringAnsi(ptr);
            Encoding srcEncodingFormat = Encoding.GetEncoding("windows-1251");
            Encoding dstEncodingFormat = Encoding.UTF8;
            return dstEncodingFormat.GetString(Encoding.Convert(srcEncodingFormat, dstEncodingFormat, srcEncodingFormat.GetBytes(str)));
        }

        public byte FrequencyDivisor { get { return module.FrequencyDivisor; } set { module.FrequencyDivisor = value; } }
        public byte ChannelQnt { get { return module.ChannelQnt; } set { module.ChannelQnt = value; } }
        public bool UseClb { get { return module.UseClb; } set { module.UseClb = value; } }
        public AckType AcknowledgeType { get { return module.AcknowledgeType; } set { module.AcknowledgeType = value; } }
        public bool ExternalStart { get { return module.ExternalStart; } set { module.ExternalStart = value; } }
        public bool RingMode { get { return module.RingMode; } set { module.RingMode = value; } }
        public bool BufferFull { get { return module.BufferFull; } }
        public bool BufferEmpty { get { return module.BufferEmpty; } }
        public bool DACRunning { get { return module.DACRunning; } }
        public float FrequencyDAC { get { return module.FrequencyDAC; } }
        public DAC_CHANNEL_CALIBRATION DacCalibration { get { return module.DacCalibration; } }
        public INFO ModuleInfo { get { return module.ModuleInfo; } }
    }
}
