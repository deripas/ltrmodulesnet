using System;
using System.Runtime.InteropServices;

namespace ltrModulesNet
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct LTR114_LCHANNEL
    {
        public byte MeasMode;       /*режим измерения*/
        public byte Channel;       /*физический канал*/
        public byte Range;         /*диапазон измерения*/

        public LTR114_LCHANNEL(byte MeasMode, byte Channel, byte Range)
        {
            this.MeasMode = MeasMode;
            this.Channel = Channel;
            this.Range = Range;
        }
    }

    //информация о модуле
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct LTR114_TINFO
    {

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        char[] name;                          /* название модуля (строка) */
        public string Name { get { return (name != null) ? new string(name) : null; } }
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        char[] serial;                        /* серийный номер модуля (строка) */
        public string Serial { get { return (serial!=null)?new string(serial) : null; } }

        ushort verMCU;
        public string VerMCU { get { return ((byte)((verMCU &0xFF00)>>8)).ToString() + '.' + ((byte)(verMCU&0xFF)).ToString(); } } 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
        char[] date;                          /* дата создания ПО (строка) */
        public string Date { get { return (date != null) ? new string(date) : null; } }

        byte verPLD;  //версия прошивки ПЛИС
        public byte VerPLD { get { return verPLD; } }

        public _ltr114api.CbrCoefStruct CbrCoef;              /* калибровочные коэффициенты для каждого диапазона */
    };


    public class _ltr114api
    {
        [DllImport("ltr114api.dll")]
        public static extern _LTRNative.LTRERROR LTR114_Init(ref TLTR114 hnd);

        [DllImport("ltr114api.dll")]
        public static extern _LTRNative.LTRERROR LTR114_Open(ref TLTR114 hnd, uint saddr, ushort sport, char[] csn, int slot_num);

        [DllImport("ltr114api.dll")]
        public static extern _LTRNative.LTRERROR LTR114_Close(ref TLTR114 hnd);

        [DllImport("ltr114api.dll")]
        public static extern _LTRNative.LTRERROR LTR114_GetConfig(ref TLTR114 hnd);
        
        [DllImport("ltr114api.dll")]
        public static extern _LTRNative.LTRERROR LTR114_Calibrate(ref TLTR114 hnd);

        [DllImport("ltr114api.dll")]
        public static extern _LTRNative.LTRERROR LTR114_SetADC(ref TLTR114 hnd);

        [DllImport("ltr114api.dll")]
        public static extern _LTRNative.LTRERROR LTR114_Start(ref TLTR114 hnd);

        [DllImport("ltr114api.dll")]
        public static extern _LTRNative.LTRERROR LTR114_Stop(ref TLTR114 hnd);

        [DllImport("ltr114api.dll")]
        public static extern _LTRNative.LTRERROR LTR114_Recv(ref TLTR114 hnd, uint[] buf, uint[] tmark, uint size, uint timeout); //Прием данных от модуля		


        [DllImport("ltr114api.dll")]
        public static extern _LTRNative.LTRERROR LTR114_GetFrame(ref TLTR114 hnd, uint[] buf);

        [DllImport("ltr114api.dll")]
        public static extern _LTRNative.LTRERROR LTR114_ProcessData(ref TLTR114 hnd, uint[] src, double[] dest, ref int size, CorrectionMode correction_mode, ProcFlags flags);

        [DllImport("ltr114api.dll")]
        public static extern _LTRNative.LTRERROR LTR114_ProcessDataTherm(ref TLTR114 hnd, uint[] src, double[] dest, double[] therm, ref int size, out int tcnt, CorrectionMode correction_mode, ProcFlags flags);



        public enum CorrectionMode : int
        {
            None = 0,
            Init = 1,
            Auto = 2
        }

        [Flags]
        public enum ProcFlags : int
        {
            None = 0,
            Value = 1,
            AvgR = 2
        }

        [Flags]
        public enum Features : byte
        {
            None = 0,
            StopSW = 1,
            Therm = 2
        }

        public enum Sync : byte
        {
            None = 0,
            Internal = 1,
            Master = 2,
            External = 4
        }

        // функции вспомагательного характера
        [DllImport("ltr114api.dll")]
        public static extern string LTR114_GetErrorString(int ErrorCode);

        public const int LTR114_ADC_RANGEQNT = 3;
        public const int LTR114_R_RANGEQNT = 3;
        public const int LTR114_MAX_LCHANNEL = 128;
        const int LTR114_SCALE_INTERVALS = 3;


        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct AutoCoefs
        {
            public double Offset;                      /* смещение нуля */
            public double Gain;                        /* масштабный коэффициент */
        }

        /*измеренные значения шкалы на этампе автокалибровки*/
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TSCALE_LTR114
        {
            public int Null;        //значение нуля                    
            public int Ref;         //значение +шкала
            public int NRef;       //значение -шкала
            public int Interm;
            public int NInterm;
        }


        /*информация для автокалибровки модуля по одному диапазону*/
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TCBRINFO
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR114_SCALE_INTERVALS)]
            public AutoCoefs[] Coefs;  /*вычисленные на этапе автокалибровки значения Gain и Offset*/


            public IntPtr TempScale;       /*массив временных измерений шкалы/нуля */
            //public TSCALE_LTR114 Index;           /*количество измерений в TempScale*/
            //public TSCALE_LTR114 LastVals;       /*последнее измерение*/
            public int Null;        //значение нуля                    
            public int Ref;         //значение +шкала
            public int NRef;       //значение -шкала
            public int Interm;
            public int NInterm;

            public int LastNull;        //значение нуля                    
            public int LastRef;         //значение +шкала
            public int LastNRef;       //значение -шкала
            public int LastInterm;
            public int LastNInterm;

            public int HVal;
            public int LVal;   
        }
        
        
        
        


        //калибровочные коэффициенты из TINFO_LTR114
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct CbrCoefStruct
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR114_ADC_RANGEQNT)]
            public float[] U;                      /*значения ИОН для диапазонов измерения напряжений*/
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR114_R_RANGEQNT)]
            public float[] I;                       /*значения токов для диапазонов измерения сопротивлений*/
        
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR114_ADC_RANGEQNT)]
            public float[] UIntr;        
        }

        

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TLTR114
        {
            public int size;                               /* размер структуры в байтах */
            public _LTRNative.TLTR Channel;                           /* описатель канала связи с модулем */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR114_ADC_RANGEQNT)]
            public TCBRINFO[] AutoCalibrInfo;      /* данные для вычисления калибровочных коэф. для каждого диапазона */
            public int LChQnt;  // количество активных логических каналов 

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR114_MAX_LCHANNEL)]
            public LTR114_LCHANNEL[] LChTbl;        // управляющая таблица с настройками логических каналов

            public UInt16 Interval;                          //длина межкадрового интервала
            public Features SpecialFeatures;                   //дополнительные возможности модуля (подключение термометра, блокировка коммутации)

            //значение передискр. АЦП - вычисляется в соответствии с частотой дискретизации
            public byte AdcOsr;

            public Sync SyncMode;               /*режим синхронизации 
                                                  000 - нет синхронизации
                                                  001 - внутренняя синхронизация
                                                  010 - внутренняя синхронизация - ведущий
                                                  100 - внешняя синхронизация (ведомый)
                                                  */

            public int FreqDivider;                       // делитель частоты АЦП (2..8000)
                                           // частота дискретизации равна F = LTR114_CLOCK/(LTR114_ADC_DIVIDER*FreqDivider)

            public int FrameLength;                       //размер данных, передаваемых модулем за один кадр 
                                           //устанавливается после вызова LTR114_SetADC
            public bool Active;
            public IntPtr Reserved;

            public LTR114_TINFO ModuleInfo;                 /* информация о модуле LTR114 */
        };

        public TLTR114 NewTLTR114
        {
            get
            {
                TLTR114 NewModule = new TLTR114();
                LTR114_Init(ref NewModule);
                return NewModule;
            }
        }


        public TLTR114 module;

        public _ltr114api()
        {
            module = NewTLTR114;
        }


        public virtual _LTRNative.LTRERROR Init()
        {
            return LTR114_Init(ref module);
        }

        public virtual _LTRNative.LTRERROR Open(uint saddr, ushort sport, char[] csn, int slot_num)
        {
            return LTR114_Open(ref module, saddr, sport, csn, slot_num);
        }
        
        public virtual _LTRNative.LTRERROR Close()
        {
            return LTR114_Close(ref module);
        }

        public virtual _LTRNative.LTRERROR GetConfig()
        {
            return LTR114_GetConfig(ref module);
        }

        public virtual _LTRNative.LTRERROR Calibrate()
        {
            return LTR114_Calibrate(ref module);
        }

        public virtual _LTRNative.LTRERROR GetFrame(uint[] buf)
        {
            return LTR114_GetFrame(ref module, buf);
        }

        public virtual _LTRNative.LTRERROR SetADC()
        {
            return LTR114_SetADC(ref module);
        }

        public virtual _LTRNative.LTRERROR Start()
        {
            return LTR114_Start(ref module);
        }

        public virtual _LTRNative.LTRERROR Stop()
        {
            return LTR114_Stop(ref module);
        }

        public virtual _LTRNative.LTRERROR Recv(uint[] buf, uint[] tmark, uint size, uint timeout)
        {
             return LTR114_Recv(ref module, buf, tmark, size, timeout);
        }

        public virtual _LTRNative.LTRERROR ProcessData(uint[] src, double[] dest,
            ref int size, CorrectionMode correction_mode, ProcFlags flags)            
        {
            return LTR114_ProcessData(ref module, src, dest, ref size, correction_mode, flags);
        }

        public virtual _LTRNative.LTRERROR ProcessDataTherm(uint[] src, double[] dest, double[] therm,
            ref int size, out int tcnt, CorrectionMode correction_mode, ProcFlags flags)
        {
            return LTR114_ProcessDataTherm(ref module, src, dest,  therm, ref size, out tcnt, correction_mode, flags);
        }

        public virtual string GetErrorString(int err)
        {
            return _ltr114api.LTR114_GetErrorString(err);
        }

        
    }
}
