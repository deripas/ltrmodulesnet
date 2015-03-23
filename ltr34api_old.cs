using System;
using System.Runtime.InteropServices;

namespace ltrModulesNet
{
    public class _ltr34api
    {
        [DllImport("ltr34api.dll")]
        public static extern _LTRNative.LTRERROR LTR34_Init(ref TLTR34 module);

        [DllImport("ltr34api.dll")]
        public static extern _LTRNative.LTRERROR LTR34_Open(ref TLTR34 module, uint net_addr, ushort net_port,
            char[] crate_sn, int slot_num);
        [DllImport("ltr34api.dll")]
        public static extern _LTRNative.LTRERROR LTR34_Close(ref TLTR34 module);

        [DllImport("ltr34api.dll")]
        public static extern _LTRNative.LTRERROR LTR34_IsOpened(ref TLTR34 module);

        [DllImport("ltr34api.dll")]
        public static extern _LTRNative.LTRERROR LTR34_Recv(ref TLTR34 module, uint[] data, uint[] tmark, uint size, uint timeout);

        [DllImport("ltr34api.dll")]
        public static extern uint LTR34_CreateLChannel(byte PhysChannel, bool ScaleFlag);

        [DllImport("ltr34api.dll")]
        public static extern _LTRNative.LTRERROR LTR34_Send(ref TLTR34 module, uint[] data, uint size, uint timeout);

        [DllImport("ltr34api.dll")]
        public static extern _LTRNative.LTRERROR LTR34_ProcessData(ref TLTR34 module, double[] source, uint[] dest, uint size, bool volt);

        [DllImport("ltr34api.dll")]
        public static extern _LTRNative.LTRERROR LTR34_Config(ref TLTR34 module);

        [DllImport("ltr34api.dll")]
        public static extern _LTRNative.LTRERROR LTR34_DACStart(ref TLTR34 module);

        [DllImport("ltr34api.dll")]
        public static extern _LTRNative.LTRERROR LTR34_DACStop(ref TLTR34 module);

        [DllImport("ltr34api.dll")]
        public static extern _LTRNative.LTRERROR LTR34_Reset(ref TLTR34 module);

        [DllImport("ltr34api.dll")]
        public static extern _LTRNative.LTRERROR LTR34_SetDescription(ref TLTR34 module);

        [DllImport("ltr34api.dll")]
        public static extern _LTRNative.LTRERROR LTR34_GetDescription(ref TLTR34 module);

        [DllImport("ltr34api.dll")]
        public static extern _LTRNative.LTRERROR LTR34_GetCalibrCoeffs(ref TLTR34 module);

        [DllImport("ltr34api.dll")]
        public static extern _LTRNative.LTRERROR LTR34_WriteCalibrCoeffs(ref TLTR34 module);

        [DllImport("ltr34api.dll")]
        public static extern _LTRNative.LTRERROR LTR34_TestEEPROM(ref TLTR34 module);

        [DllImport("ltr34api.dll")]
        public static extern _LTRNative.LTRERROR LTR34_ReadFlash(ref TLTR34 module, byte[] data, ushort size, ushort Address);

        [DllImport("ltr34api.dll")]
        public static extern string LTR34_GetErrorString(int ErrorCode);


        public const int LTR34_MAX_BUFFER_SIZE = 2097151;
        public const int LTR34_EEPROM_SIZE = 2048;
        public const int LTR34_USER_EEPROM_SIZE = 1024;
        public const int LTR34_DAC_NUMBER_MAX = 8;

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct DAC_CHANNEL_CALIBRATION
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (2 * LTR34_DAC_NUMBER_MAX))]
            public float[] FactoryCalibrOffset;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (2 * LTR34_DAC_NUMBER_MAX))]
            public float[] FactoryCalibrScale;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TINFO_LTR34
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] Name;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            public char[] Serial;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] FPGA_Version;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] CalibrVersion;
            public byte MaxChannelQnt;
        };


        //**** конфигурация модуля
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TLTR34
        {
            public int Size;// Размер структуры 
            public _LTRNative.TLTR Channel;   // структура описывающая модуль в крейте – описание в ltrapi.pdf 		    
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] LChTbl;                  // Таблица логических каналов
            //**** настройки модуля           
            public byte FrequencyDivisor;            // делитель частоты дискретизации 0..60 (31.25..500 кГц)
            public byte ChannelQnt;             // число каналов 0, 1, 2, 3 соотвествнно (1, 2, 4, 8)
            [MarshalAs(UnmanagedType.U1)]
            public bool UseClb;
            [MarshalAs(UnmanagedType.U1)]
            public bool AcknowledgeType;             // тип подтверждения true - высылать подтверждение каждого слова, false- высылать состояние буффера каждые 100 мс
            [MarshalAs(UnmanagedType.U1)]
            public bool ExternalStart;               // внешний старт true - внешний старт, false - внутренний
            [MarshalAs(UnmanagedType.U1)]
            public bool RingMode;                    // режим кольца  true - режим кольца, false - потоковый режим
            [MarshalAs(UnmanagedType.U1)]
            public bool BufferFull;					// статус - буффер переполнен - ошибка
            [MarshalAs(UnmanagedType.U1)]
            public bool BufferEmpty;					// статус - буффер пуст - ошибка
            [MarshalAs(UnmanagedType.U1)]
            public bool DACRunning;					// статус - запущена ли генерация

            public float FrequencyDAC;				// статус - частота - на которую настроен ЦАП в текущей конфигурации
            public DAC_CHANNEL_CALIBRATION DacCalibration;
            public TINFO_LTR34 ModuleInfo;
        }

        public TLTR34 NewTLTR34
        {
            get
            {
                TLTR34 NewModule = new TLTR34();
                LTR34_Init(ref NewModule);
                return NewModule;
            }
        }

        public TLTR34 module;

        public _ltr34api()
        {
            module = NewTLTR34;
        }


        public virtual _LTRNative.LTRERROR Init()
        {
            return LTR34_Init(ref module);
        }


        public virtual _LTRNative.LTRERROR Open( uint net_addr, ushort net_port,
            char[] crate_sn, int slot_num)
        {
			if (net_addr ==0) net_addr = NewTLTR34.Channel.saddr;
			if (net_port ==0) net_port = NewTLTR34.Channel.sport;

            return LTR34_Open(ref module, net_addr, net_port, crate_sn, slot_num);
        }

        public virtual _LTRNative.LTRERROR Close()
        {
            return LTR34_Close(ref module);
        }


        public virtual _LTRNative.LTRERROR IsOpened()
        {
            return LTR34_IsOpened(ref module);
        }


        public virtual _LTRNative.LTRERROR Recv([In, Out]uint[] data, [In, Out] uint[] tmark, uint size, uint timeout)
        {
            return LTR34_Recv(ref module, data, tmark, size,timeout);
        }


        public virtual uint CreateLChannel(byte PhysChannel, bool ScaleFlag)
        {
            return LTR34_CreateLChannel(PhysChannel, ScaleFlag);
        }


        public virtual _LTRNative.LTRERROR Send([In]uint[] data, uint size, uint timeout)
        {
            return LTR34_Send(ref module, data, size, timeout);
        }


        public virtual _LTRNative.LTRERROR ProcessData([In]double[] source, [In, Out]uint[] dest, uint size, bool volt)
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


        public virtual _LTRNative.LTRERROR Reset()
        {
            return LTR34_Reset(ref module);
        }


        public virtual _LTRNative.LTRERROR SetDescription()
        {
            return LTR34_SetDescription(ref module);
        }


        public virtual _LTRNative.LTRERROR GetDescription()
        {
            return LTR34_GetDescription(ref module);
        }


        public virtual _LTRNative.LTRERROR GetCalibrCoeffs()
        {
            return LTR34_GetCalibrCoeffs(ref module);
        }


        public virtual _LTRNative.LTRERROR WriteCalibrCoeffs()
        {
            return LTR34_WriteCalibrCoeffs(ref module);
        }


        public virtual _LTRNative.LTRERROR TestEEPROM()
        {
            return LTR34_TestEEPROM(ref module);
        }

        public virtual _LTRNative.LTRERROR ReadFlash([In, Out] byte[] data, ushort size, ushort Address)
        {
            return LTR34_ReadFlash(ref module, data, size, Address);
        }

        public virtual string GetErrorString(int ErrorCode)
        {
            return LTR34_GetErrorString(ErrorCode);
        }
    }
}
