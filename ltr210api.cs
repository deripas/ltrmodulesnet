using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ltrModulesNet
{
    public class ltr210api
    {
        [DllImport("ltr210api.dll")]
        static extern _LTRNative.LTRERROR LTR210_Init(ref TLTR210 hnd);
        [DllImport("ltr210api.dll")]
        static extern _LTRNative.LTRERROR LTR210_Close(ref TLTR210 hnd);
        [DllImport("ltr210api.dll")]
        static extern _LTRNative.LTRERROR LTR210_Open(ref TLTR210 hnd, uint saddr, ushort sport, string csn, int slot_num);
        [DllImport("ltr210api.dll")]
        static extern _LTRNative.LTRERROR LTR210_IsOpened(ref TLTR210 hnd);
        [DllImport("ltr210api.dll")]
        static extern _LTRNative.LTRERROR LTR210_FPGAIsLoaded(ref TLTR210 hnd);
        [DllImport("ltr210api.dll")]
        static extern _LTRNative.LTRERROR LTR210_LoadFPGA(ref TLTR210 hnd, string filename, IntPtr progr_cb, IntPtr cb_data);
        [DllImport("ltr210api.dll")]
        static extern _LTRNative.LTRERROR LTR210_SetADC(ref TLTR210 hnd);
        [DllImport("ltr210api.dll")]
        static extern _LTRNative.LTRERROR LTR210_FillAdcFreq(ref CONFIG cfg, double freq, uint flags, out double set_freq);
        [DllImport("ltr210api.dll")]
        static extern _LTRNative.LTRERROR LTR210_Start(ref TLTR210 hnd);
        [DllImport("ltr210api.dll")]
        static extern _LTRNative.LTRERROR LTR210_Stop(ref TLTR210 hnd);
        [DllImport("ltr210api.dll")]
        static extern _LTRNative.LTRERROR LTR210_FrameStart(ref TLTR210 hnd);
        [DllImport("ltr210api.dll")]
        static extern _LTRNative.LTRERROR LTR210_WaitEvent(ref TLTR210 hnd, out RecvEvents recvEvent, out uint status, uint timeout);
        [DllImport("ltr210api.dll")]
        static extern _LTRNative.LTRERROR LTR210_WaitEvent(ref TLTR210 hnd, out RecvEvents recvEvent, IntPtr status, uint timeout);
        [DllImport("ltr210api.dll")]
        static extern int LTR210_Recv(ref TLTR210 hnd, uint[] data, uint[] tmark, uint size, uint timeout);
        [DllImport("ltr210api.dll")]
        static extern _LTRNative.LTRERROR LTR210_ProcessData(ref TLTR210 hnd,  uint[] src,
                                                             double[] dest, ref int size, ProcFlags flags,
                                                             out FRAME_STATUS frame_status,
                                                             DATA_INFO[] data_info);
        [DllImport("ltr210api.dll")]
        static extern _LTRNative.LTRERROR LTR210_GetLastWordInterval(ref TLTR210 hnd, out uint interval);
        [DllImport("ltr210api.dll")]
        static extern IntPtr LTR210_GetErrorString(int err);
        [DllImport("ltr210api.dll")]
        static extern _LTRNative.LTRERROR LTR210_LoadCbrCoef(ref TLTR210 hnd);



        public const int LTR210_NAME_SIZE   = 8;
        public const int LTR210_SERIAL_SIZE = 16;
        /** Количество каналов АЦП в одном модуле */
        public const int LTR210_CHANNEL_CNT = 2;
        /** Количество диапазонов измерения АЦП */
        public const int LTR210_RANGE_CNT   = 5;
        /** Количество диапазонов, для которых нужно выполнять дополнительную коррекцию
            АЧХ с помощью IIR фильтра */
        public const int LTR210_AFC_IIR_COR_RANGE_CNT = 2;

        /** Код принятого отсчета АЦП, соответствующий максимальному напряжению
            заданного диапазона */
        public const int LTR210_ADC_SCALE_CODE_MAX = 13000;
        /** Максимальное значение делителя частоты АЦП */
        public const int LTR210_ADC_FREQ_DIV_MAX   = 10;
        /** Максимальное значение коэффициента прореживания данных от АЦП */
        public const int LTR210_ADC_DCM_CNT_MAX    = 256;

        /** Частота в Герцах, относительно которой задается частота отсчетов АЦП */
        public const int LTR210_ADC_FREQ_HZ    = 10000000;
        /** Частота в Герцах, относительно которой задается частота следования кадров
            в режиме #LTR210_SYNC_MODE_PERIODIC */
        public const int LTR210_FRAME_FREQ_HZ  = 1000000;

        /** Размер внутреннего циклического буфера модуля в отсчетах АЦП */
        public const int LTR210_INTERNAL_BUFFER_SIZE = 16777216;

        /** Максимальный размер кадра, который можно установить в одноканальном режиме */
        public const int LTR210_FRAME_SIZE_MAX = (16777216 - 512);

        /** Диапазоны канала АЦП */
        public enum AdcRanges : byte
        {
            Range_10 = 0, /* Диапазон +/- 10 В */
            Range_5 = 1,  /* Диапазон +/- 5 В */
            Range_2 = 2,  /* Диапазон +/- 2 В */
            Range_1 = 3,  /* Диапазон +/- 1 В */
            Range_0_5 = 4  /* Диапазон +/- 0.5 В */           
        }

        /** Режим измерения канала АЦП */
        public enum ChModes : byte
        {
            ACDC = 0, /**< Измерение переменной и постоянной составляющей(открытый вход) */
            AC = 1, /**< Отсечка постоянной составляющей (закрытый вход) */
            ZERO = 2  /**< Режим измерения собственного нуля*/
        }

        /** Режим работы и события синхронизации. */
        public enum SyncModes : byte
        {
            INTERNAL = 0, /**< Режим сбора кадра по программной команде,
                                   передаваемой вызовом LTR210_FrameStart()*/
            CH1_RISE = 1, /**< Режим сбора кадра по фронту сигнала относительно
                                   уровня синхронизации на первом аналоговом канале*/
            CH1_FALL = 2, /**< Режим сбора кадра по спаду сигнала относительно
                                   уровня синхронизации на первом аналоговом канале*/
            CH2_RISE = 3, /**< Режим сбора кадра по фронту сигнала относительно
                                   уровня синхронизации на втором аналоговом канале*/
            CH2_FALL = 4, /**< Режим сбора кадра по спаду сигнала относительно
                                   уровня синхронизации на втором аналоговом канале*/
            SYNC_IN_RISE = 5, /**< Режим сбора кадра по фронту цифрового сигнала
                                   на входе SYNC (не от другого модуля!)*/
            SYNC_IN_FALL = 6, /**< Режим сбора кадра по спаду цифрового сигнала
                                   на входе SYNC (не от другого модуля!)*/
            PERIODIC = 7, /**< Режим периодического сбора кадров с
                                   установленной частотой следования кадров*/
            CONTINUOUS = 8  /**< Режим непрерывного сбора данных*/
        }

        /** Режим работы модуля в группе */
        public enum GroupModes : byte
        {
            INDIVIDUAL = 0, /**< Модуль работает независимо от остальных */
            MASTER = 1, /**< Режим мастера --- при возникновении заданного 
                                 события синхронизации модуль выдает сигнал 
                                 на выход SYNC. */
            SLAVE = 2 /**< Режим подчиненного модуля --- модуль запускает
                                сбор кадра от сигнала на входе SYNC,
                                который должен сгенерировать другой LTR210,
                                настроенный на режим мастера.
                                Значение SyncMode при этом не учитывается */
        }

        /** Коды асинхронных событий */
        public enum RecvEvents : uint
        {
            TIMEOUT = 0, /**< Не пришло никакого события от модуля за указанное время */
            KEEPALIVE = 1, /**< Пришел корректный сигнал статуса от модуля (сигнал жизни) */
            SOF = 2  /**< Пришло начало записанного кадра */
        }

        /** Коды, определяющие правильность принятого кадра */
        public enum FrameResult : byte
        {
            OK = 0, /**< Кадр принят без ошибок. Данные кадра действительны */
            PENDING = 1, /**< В обрабатываемых данных не было признака конца кадра. */
            ERROR = 2  /**< Кадр принят с ошибкой. Данные кадра недействительны.
                               Причину ошибки можно узнать по флагам статуса */
        }

        /** Флаги статуса */
        [Flags]
        public enum StatusFlags : ushort
        {
            PLL_LOCK = 0x0001, /**< Признак захвата PLL в момент передачи статуса. */
            PLL_LOCK_HOLD = 0x0002, /**< Признак, что захват PLL не пропадал с момента предыдущей передачи статуса. */
            OVERLAP = 0x0004, /**< Признак, что процесс записи обогнал процесс чтения. Часть данных в кадре
                                         может быть недействительна */
            SYNC_SKIP = 0x0008, /**< Признак, что во время записи кадра возникло хотя бы одно
                                         событие синхронизации, которое было пропущено.
                                         Не влияет на правильность самого кадра.*/
            INVALID_HIST = 0x0010, /**< Признак того, что предыстория принятого кадра недействительна
                                         (событие наступило меньше чем через HistSize отсчетов после разрешения записи) */
            CH1_EN = 0x0040, /**< Признак, что разрешена запись по первому каналу */
            CH2_EN = 0x0080  /**< Признак, что разрешена запись по второму каналу */
        }

        /** Дополнительные флаги настроек. */
        [Flags]
        public enum CfgFlags : uint
        {
            KEEPALIVE_EN = 0x001, /**< Разрешение периодической передачи статуса модуля при запущенном сборе */
             /** Разрешение режима автоматической приостановки записи на время, пока
                 кадр выдается по интерфейсу в крейт. Данный режим позволяет установить
                 максимальный размер кадра независимо от частоты сбора АЦП */
            WRITE_AUTO_SUSP = 0x002,
            /** Включение тестого режима, в котором вместо данных передается счетчик */
            TEST_CNTR_MODE = 0x100
        }

        /** Флаги обработки данных */
        [Flags]
        public enum ProcFlags : uint
        {
            /** Признак, что нужно перевести коды АЦП в Вольты. Если данный флаг не указан,
                то будут возвращены коды АЦП. При этом код #LTR210_ADC_SCALE_CODE_MAX
                соответствует максимальному напряжению для установленного диапазона. */
            VOLT = 0x0001,
            /** Признак, что необходимо выполнить коррекцию АЧХ на основании записанных
                во Flash-памяти модуля коэффициентов */
            AFC_COR = 0x0002,
            /** По умолчанию LTR210_ProcessData() предполагает, что ей на обработку
                передаются все принятые данные и проверяет непрерывность счетчика не только
                внутри переданного блока данных, но и между вызовами.
                Если обрабатываются не все данные или одни и те же данные обрабатываются
                повторно, то нужно указать данный флаг, чтобы счетчик проверялся только
                внутри обрабатываемого блока */
            NONCONT_DATA = 0x0100,
        }

        /** Скорость выдачи данных в интерфейс */
        public enum IntfTransfRates : byte
        {
            Rate_500K = 0, /**< 500 КСлов/c */
            Rate_200K = 1, /**< 200 КСлов/c */
            Rate_100K = 2, /**< 100 КСлов/c */
            Rate_50K = 3, /**< 50  КСлов/c */
            Rate_25K = 4, /**< 25  КСлов/c */
            Rate_10K = 5  /**< 10  КСлов/c */
        }

        /** Режим работы дополнительного бита во входном потоке */
        public enum DigBitModes : byte
        {
            /** Всегда нулевое значение бита */
            ZERO = 0,
            /** Бит отражает состояние цифрового входа SYNC модуля */
            SYNC_IN = 1,
            /** Бит равен "1", если уровень сигнала для 1-го канала АЦП превысил верхний
                уровень синхронизации и не опустился ниже нижнего */
            CH1_LVL = 2,
            /** Бит равен "1", если уровень сигнала для 2-го канала АЦП превысил верхний
                уровень синхронизации и не опустился ниже нижнего  */
            CH2_LVL = 3,
            /** Бит равен "1" для одного отсчета в момент срабатывания программной
                 или периодической синхронизации */
            INTERNAL_SYNC = 4
        }

        
        /** Калибровочные коэффициенты */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct CBR_COEF
        {
            float   Offset; /**< 15-битный код смещения */
            float   Scale;  /**< Коэффициент шкалы */
        } 

        /** Параметры БИХ-фильтра. */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct AFC_IIR_COEF
        {
            double R; /**< Сопротивление эквивалентной цепи фильтра */
            double C; /**< Емкость эквивалентной цепи фильтра */
        } 

        /** Информация о модуле */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct INFO
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR210_NAME_SIZE)]
            char[] _name;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR210_SERIAL_SIZE)]
            char[] _serial;
            ushort _verFPGA;
            byte _verPLD;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR210_CHANNEL_CNT*8)]
            CBR_COEF[] CbrCoef;
            double AfcCoefFreq;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR210_CHANNEL_CNT * 8)]
            double[] AfcCoef;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR210_CHANNEL_CNT * 8)]
            AFC_IIR_COEF[] AfcIirParam;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            uint[] Reserved;

            /* Название модуля */
            public string Name { get { return new string(_name).TrimEnd('\0'); } }
            /* Серийный номер модуля */
            public string Serial { get { return new string(_serial).TrimEnd('\0'); } }
            /* Версия прошивки ПЛИС модуля (действительна только после ее загрузки) */
            public ushort VerFPGA { get { return _verFPGA; } }
            /* Версия прошивки PLD */
            public byte VerPLD { get { return _verPLD; } }            
        }

        /**  Настройки канала АЦП */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct CHANNEL_CONFIG
        {
            [MarshalAs(UnmanagedType.U1)]
            bool _enabled;
            AdcRanges _range;
            ChModes _mode;
            DigBitModes _dig_bit_mode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            byte[] reserved;
            double _sync_lvl_l;
            double _sync_lvl_h;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            uint[] reserved2;

            /* Признак, разрешен ли сбор по данному каналу  */
            public bool Enabled { set { _enabled = value; } get { return _enabled; } }
            /* Установленный диапазон --- константа из #e_LTR210_ADC_RANGE */
            public AdcRanges Range { set { _range = value; } get { return _range; } }
            /* Режим измерения --- константа из #e_LTR210_CH_MODE */
            public ChModes Mode { set { _mode = value; } get { return _mode; } }
            /** Режим работы дополнительного бита во входном потоке данных данного канала.
                Константа из #e_LTR210_DIG_BIT_MODE */
            public DigBitModes DigBitMode { set { _dig_bit_mode = value; } get { return _dig_bit_mode; } }
            /** Нижний порог гистерезиса при аналоговой синхронизации в Вольтах. */
            public double SyncLevelL { set { _sync_lvl_l = value; } get { return _sync_lvl_l; } }
            /** Верхний порог гистерезиса при аналоговой синхронизации в Вольтах */
            public double SyncLevelH { set { _sync_lvl_h = value; } get { return _sync_lvl_h; } }
        }

        /** Настройки модуля */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct CONFIG
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTR210_CHANNEL_CNT)]
            CHANNEL_CONFIG[] _ch;  
            uint _frame_size;             
            uint _hist_size;            
            SyncModes _sync_mode;        
            GroupModes _group_mode;
            ushort _adc_freq_div;
            uint _adc_dcm_cnt;                       
            uint _frame_freq_div;
            CfgFlags _flags; 
            IntfTransfRates _intf_transf_rate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 39)]
            uint[] Reserved; 

            /** Настройки каналов АЦП */
            public CHANNEL_CONFIG[] Ch { get { return _ch; } set { _ch = value; } }
            /** Размер точек на канал в кадре при покадровом сборе */
            public uint FrameSize { get { return _frame_size; } set { _frame_size = value; } }
            /** Размер предыстории (количество точек в кадре на канал,
                измеренных до возникновения события синхронизации) */
            public uint HistSize { get { return _hist_size; } set { _hist_size = value; } }
            /** Условие сбора кадра (событие синхронизации) */
            public SyncModes SyncMode { get { return _sync_mode; } set { _sync_mode = value; } }
            /** Режим работы в составе группы модулей. */
            public GroupModes GroupMode { get { return _group_mode; } set { _group_mode = value; } }
            /** Значение делителя частоты АЦП минус 1. Может быть в диапазоне от 0
                до LTR210_ADC_FREQ_DIV_MAX-1*/
            public ushort AdcFreqDiv { get { return _adc_freq_div; } set { _adc_freq_div = value; } }
            /** Значение коэфициент прореживания данных АЦП минус 1. Может быть в диапазоне
                от 0 до LTR210_ADC_DCM_CNT_MAX-1.*/
            public uint AdcDcmCnt { get { return _adc_dcm_cnt; } set { _adc_dcm_cnt = value; } }
            /** Делитель частоты запуска сбора кадров для SyncMode = SyncModes.PERIODIC */
            public uint FrameFreqDiv { get { return _frame_freq_div; } set { _frame_freq_div = value; } }
            /** Флаги конфигурации */
            public CfgFlags Flags { get { return _flags; } set { _flags = value; } }
            /** Скорость выдачи данных в интерфейс
                По-умолчанию устанавливается максимальная скорость (500 КСлов/с).
                Если установленная скорость превышает максимальную скорость интерфейса для крейта,
                в который установлен модуль, то будет установлена максимальная скорость,
                поддерживаемая данным крейтом */
            public IntfTransfRates IntfTransfRate { get { return _intf_transf_rate; } set { _intf_transf_rate = value; } }           

            public _LTRNative.LTRERROR FillAdcFreq(double freq, uint flags, out double set_freq)
            {
                return LTR210_FillAdcFreq(ref this, freq, flags, out set_freq);
            }
        }

        /** Параметры состояния модуля */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct STATE
        {
            [MarshalAs(UnmanagedType.U1)]
            bool _run;                     
            uint _recv_frame_size;            
            double _adc_freq;

            double _frame_freq;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            uint[] Reserved; /**< Резервные поля */

            /** Признак, запущен ли сбор данных */
            public bool Run { get { return _run; } }
            /** Количество слов в принимаемом кадре, включая статус.
                (устанавливается после вызова LTR210_SetADC()) */
            public uint RecvFrameSize { get { return _recv_frame_size; } }
            /** Рассчитанная частота сбора АЦП в Гц (устанавливается после вызова LTR210_SetADC()) */
            public double AdcFreq { get { return _adc_freq; } }
            /** Рассчитанная частота следования кадров для режима синхронизации
                #LTR210_SYNC_MODE_PERIODIC (устанавливается после вызова
                LTR210_SetADC()) */
            public double FrameFreq { get { return _frame_freq; } }
        }

        /** Описатель модуля */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TLTR210
        {
            int _size;
            _LTRNative.TLTR _channel;
            IntPtr _internal;
            CONFIG _cfg;
            STATE _stat;
            INFO _info;

            /* Структура, содержащая состояние соединения с сервером. */
            public _LTRNative.TLTR Channel { get { return _channel; } }
            /** Настройки модуля. Заполняются пользователем перед вызовом LTR210_SetADC(). */
            public CONFIG Cfg { get { return _cfg; } set { _cfg = value; } }
            /** Состояние модуля и рассчитанные параметры. Поля изменяются функциями
                библиотеки. Пользовательской программой могут использоваться
                только для чтения. */
            public STATE State { get { return _stat; } }
            /** Информация о модуле */
            public INFO ModuleInfo { get { return _info; } }
        }

        /* Дополнительная информация о принятом отсчете. */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct DATA_INFO
        {
            byte _dig_bit_state;
            
            byte _ch;
            AdcRanges _range;       
            byte Reserved;

            /* Значение дополнительного бита, передаваемого вместе с потоком данных */
            public byte DigBitState { get { return (byte)(_dig_bit_state & 1); } }
            /** Номер канала, которому соответствует принятое слово
                 (0 --- первый, 1 --- второй) */
            public byte Ch { get { return _ch; } }
            /** Диапазон канала, установленный во время измерения данного отсчета */
            public AdcRanges Range { get { return _range; } }
        }

        /* Информация о статусе обработанного кадра. */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct FRAME_STATUS
        {
            FrameResult _result;
            byte _reserved;
            StatusFlags _flags;

            /* Код результата обработки кадра */
            public FrameResult Result { get { return _result; } }
            /** Дополнительные флаги, представляющие собой информацию 
                о статусе самого модуля и принятого кадра */
            public StatusFlags Flags { get { return _flags; } }
        }





        public TLTR210 hnd;

        public CONFIG Cfg { get { return hnd.Cfg; } set { hnd.Cfg = value; } }
        public STATE State { get { return hnd.State; } }
        public INFO ModuleInfo { get { return hnd.ModuleInfo; } }



        public ltr210api()
        {
            LTR210_Init(ref hnd);
        }

        /* в финализаторе убеждаемся, что остановили поток и 
         * закрыли модуль */
        ~ltr210api()
        {
            if (IsOpened() == _LTRNative.LTRERROR.OK)
            {
                if (hnd.State.Run)
                    Stop();
                Close();
            }
        }

        public _LTRNative.LTRERROR Open(uint saddr, ushort sport, string csn, int slot_num)
        {
            return LTR210_Open(ref hnd, saddr, sport, csn, slot_num);
        }

        public _LTRNative.LTRERROR Open(string csn, int slot_num)
        {
            return Open(_LTRNative.SADDR_DEFAULT, _LTRNative.SPORT_DEFAULT, csn, slot_num);
        }

        public _LTRNative.LTRERROR Open(int slot_num)
        {
            return Open("", slot_num);
        }

        public _LTRNative.LTRERROR Close()
        {
            return LTR210_Close(ref hnd);
        }

        public _LTRNative.LTRERROR IsOpened()
        {
            return LTR210_IsOpened(ref hnd);
        }

        public _LTRNative.LTRERROR FPGAIsLoaded()
        { 
            return LTR210_FPGAIsLoaded(ref hnd);
        }

        public _LTRNative.LTRERROR LoadFPGA(string FileName)
        {
            return LTR210_LoadFPGA(ref hnd, FileName, IntPtr.Zero, IntPtr.Zero);
        }

        public _LTRNative.LTRERROR LoadFPGA()
        {
            return LTR210_LoadFPGA(ref hnd, "", IntPtr.Zero, IntPtr.Zero);
        }

        public _LTRNative.LTRERROR SetADC()
        {
            return LTR210_SetADC(ref hnd);
        }

        public _LTRNative.LTRERROR FillAdcFreq(double freq, out double setFreq)
        {
            return hnd.Cfg.FillAdcFreq(freq, 0, out setFreq);
        }

        public _LTRNative.LTRERROR Start()
        {
            return LTR210_Start(ref hnd);
        }

        public _LTRNative.LTRERROR Stop()
        {
            return LTR210_Stop(ref hnd);
        }

        public _LTRNative.LTRERROR FrameStart()
        {
            return LTR210_FrameStart(ref hnd);
        }

        public _LTRNative.LTRERROR WaitEvent(out RecvEvents recvEvent, out StatusFlags status, uint timeout)
        {
            uint val=0;
            _LTRNative.LTRERROR err = LTR210_WaitEvent(ref hnd, out recvEvent, out val, timeout);
            status = (StatusFlags)val;
            return err;
        }

        public _LTRNative.LTRERROR WaitEvent( out RecvEvents recvEvent, uint timeout)
        {
            return LTR210_WaitEvent(ref hnd, out recvEvent, IntPtr.Zero, timeout);
        }

        public int Recv(uint[] data, uint[] tmark, uint size, uint timeout)
        {
            return LTR210_Recv(ref hnd, data, tmark, size, timeout);
        }

        public int Recv(uint[] data, uint size, uint timeout)
        {
            return LTR210_Recv(ref hnd, data, null, size, timeout);
        }

        public _LTRNative.LTRERROR ProcessData(uint[] src, double[] dest, ref int size, ProcFlags flags,
                                               out FRAME_STATUS frame_status,
                                               DATA_INFO[] data_info)
        {
            return LTR210_ProcessData(ref hnd, src, dest, ref size, flags, out frame_status, data_info);
        }

        public _LTRNative.LTRERROR ProcessData(uint[] src, double[] dest, ref int size, ProcFlags flags,
                                               out FRAME_STATUS frame_status)
        {
            return LTR210_ProcessData(ref hnd, src, dest, ref size, flags, out frame_status, null);
        }

        public _LTRNative.LTRERROR GetLastWordInterval(out uint interval)
        {
            return LTR210_GetLastWordInterval(ref hnd, out interval);
        }

        public _LTRNative.LTRERROR LoadCbrCoef()
        {
            return LTR210_LoadCbrCoef(ref hnd);
        }

        public static string GetErrorString(_LTRNative.LTRERROR err)
        {
            IntPtr ptr = LTR210_GetErrorString((int)err);
            string str = Marshal.PtrToStringAnsi(ptr);
            Encoding srcEncodingFormat = Encoding.GetEncoding("windows-1251");
            Encoding dstEncodingFormat = Encoding.UTF8;
            return dstEncodingFormat.GetString(Encoding.Convert(srcEncodingFormat, dstEncodingFormat, srcEncodingFormat.GetBytes(str)));
        }
    }
}
