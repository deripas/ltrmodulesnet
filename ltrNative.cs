using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ltrModulesNet
{
    /// <summary>
    /// Summary description for _LTRNative.
    /// </summary>
    public class _LTRNative
    {
        public const int COMMENT_LENGTH = 256;

        public enum StartMode: int {
            OFF = 0,
            RUN = 1
        } 


        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TLTR
        {
            public uint saddr;                      // сетевой адрес сервера
            public ushort sport;                    // сетевой порт сервера
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] csn;                        // серийный номер крейта
            public ushort cc;                       // номер канала крейта
            public uint flags;                      // флаги состояния канала
            public uint tmark;                        // время синхронизации
            public UIntPtr Internal;                // указатель на канал


            public string Serial
            {
                get 
                {
                    char[] arr = new char[16];
                    for (int i = 0; i < 16; i++)
                        arr[i] = (char)csn[i];
                    return new string(arr).TrimEnd('\0'); 
                }
                set
                {
                    char[] arr = value.ToCharArray();
                    for (int i = 0; i < 16; i++)
                        csn[i] = i < arr.Length ? (byte)arr[i] : (byte)0;
                }
            }
        
        };
        // Информация о типе крейта
        public struct TCRATE_INFO
        {
            public byte CrateType;                                           // Тип крейта
            public byte CrateInterface;                                      // Тип подключения крейта
        }
        // элемент списка IP-крейтов
        public struct TIPCRATE_ENTRY
        {
            public uint ip_addr;                                          // IP адрес (host-endian)
            public uint flags;                                            // флаги режимов (CRATE_IP_FLAG_...)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] serial_number;                                 // серийный номер (если крейт подключен)
            public byte is_dynamic;                                        // 0 = задан пользователем, 1 = найден автоматически
            public byte status;                                            // состояние (CRATE_IP_STATUS_...)
        }
        public struct TLTR_CONFIG
        {
            // Настройка ножек процессора
            // [0] PF1 в рев. 0, PF1 в рев. 1+
            // [1] PG13
            // [2] PF3, только рев. 1+, только вход
            // [3] резерв
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt16[] userio;          // одно из значений LTR_USERIO_...
            // Настройка выходов DIGOUTx
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public UInt16[] digout;          // конфигурация выходов (LTR_DIGOUT_...)
            public UInt16 digout_en;          // разрешение выходов DIGOUT1, DIGOUT2
        }


        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TDESCRIPTION_MODULE                                //
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            char[] CompanyName_;                                  //
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            char[] DeviceName_;                                   // название изделия
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            char[] SerialNumber_;                                 // серийный номер изделия
            byte Revision_;                                       // ревизия изделия
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = COMMENT_LENGTH)]
            char[] Comment_;

            public string CompanyName { get { return new string(CompanyName_).TrimEnd('\0'); } }
            public string DeviceName { get { return new string(DeviceName_).TrimEnd('\0'); } }
            public string SerialNumber { get { return new string(SerialNumber_).TrimEnd('\0'); } }
            public string Comment { get { return new string(Comment_).TrimEnd('\0'); } }
            public byte Revision { get { return Revision_; } }
        };
        // описание процессора и програмного обеспечения
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TDESCRIPTION_CPU                                    //
        {                                                                //
            byte Active_;                                         // флаг достоверности остальных полей структуры
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            char[] Name_;                                         // название            
            double ClockRate_;                                    //
            uint FirmwareVersion_;                                //
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = COMMENT_LENGTH)]
            char[] Comment_;

            public string Name { get { return new string(Name_).TrimEnd('\0'); } }
            public string Comment { get { return new string(Comment_).TrimEnd('\0'); } }
            public bool Active { get { return Active_!=0; } }
            public double ClockRate { get { return ClockRate_; } }
            public uint FirmwareVersion { get { return FirmwareVersion_; } }
        } ;
        // описание плис
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TDESCRIPTION_FPGA
        {                                                                //
            byte Active_;                                         // флаг достоверности остальных полей структуры
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            char[] Name_;                                         // название
            double ClockRate_;                                    //
            uint FirmwareVersion_;                                //
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = COMMENT_LENGTH)]
            char[] Comment_;

            public string Name { get { return new string(Name_).TrimEnd('\0'); } }
            public string Comment { get { return new string(Comment_).TrimEnd('\0'); } }
            public bool Active { get { return Active_!=0; } }
            public double ClockRate { get { return ClockRate_; } }
            public uint FirmwareVersion { get { return FirmwareVersion_; } }
        } ;
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TDESCRIPTION_INTERFACE                            //
        {                                                                //
            byte Active_;                                         // флаг достоверности остальных полей структуры
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            char[] Name_;                                         // название
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = COMMENT_LENGTH)]
            char[] Comment_;

            public string Name { get { return new string(Name_).TrimEnd('\0'); } }
            public string Comment { get { return new string(Comment_).TrimEnd('\0'); } }
            public bool Active { get { return Active_!=0; } }
        };

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TDESCRIPTION_MEZZANINE                            //
        {                                                                //
            byte Active_;                                         // флаг достоверности остальных полей структуры
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            char[] Name_;                                         // название изделия
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            char[] SerialNumber_;                                 // серийный номер изделия            
            byte Revision_;                                       // ревизия изделия
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            double[] Calibration_;                                       // корректировочные коэффициенты
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = COMMENT_LENGTH)]
            char[] Comment_;

            public string Name { get { return new string(Name_).TrimEnd('\0'); } }
            public string SerialNumber { get { return new string(SerialNumber_).TrimEnd('\0'); } }
            public string Comment { get { return new string(Comment_).TrimEnd('\0'); } }
            public bool Active { get { return Active_!=0; } }
            public byte Revision { get { return Revision_; } }
            public double[] Calibration { get { return Calibration_; } }
        };   
        
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TLTR_SETTINGS
        {
            ushort _size;
            byte   _autorun_ison;
            public void Init() { _size = 4; }
            //public ushort Size { get { return _size; } set { _size = value; } }
            public bool AutorunIsOn { get { return _autorun_ison!=0; } set { _autorun_ison = (byte)(value ? 1 : 0); } }
        };
        
        // описание крейта (для TCRATE_INFO)
        public enum en_CrateType
        {
            CRATE_TYPE_UNKNOWN    =  0,
            CRATE_TYPE_LTR010     =  10,
            CRATE_TYPE_LTR021     =  21,
            CRATE_TYPE_LTR030     =  30,
            CRATE_TYPE_LTR031     =  31,
            CRATE_TYPE_LTR032     =  32,
            CRATE_TYPE_LTR_CU_1   =  40,
            CRATE_TYPE_LTR_CEU_1  =  41,
            CRATE_TYPE_BOOTLOADER =  99
        }
        // интерфейс крейта (для TCRATE_INFO)
        public enum en_CrateInterface
        {
            CRATE_IFACE_UNKNOWN = 0,
            CRATE_IFACE_USB     = 1,
            CRATE_IFACE_TCPIP   = 2
        }

        // Значения для управления ножками процессора, доступными для пользовательского программирования
        public enum en_LTR_UserIoCfg
        {
            LTR_USERIO_DIGIN1  = 1,    // ножка является входом и подключена к DIGIN1
            LTR_USERIO_DIGIN2  = 2,    // ножка является входом и подключена к DIGIN2
            LTR_USERIO_DIGOUT  = 0,    // ножка является выходом (подключение см. en_LTR_DigOutCfg)
            LTR_USERIO_DEFAULT = LTR_USERIO_DIGOUT
        }
        // Значения для управления выходами DIGOUTx
        public enum en_LTR_DigOutCfg
        {
            LTR_DIGOUT_CONST0  = 0x00, // постоянный уровень логического "0"
            LTR_DIGOUT_CONST1  = 0x01, // постоянный уровень логической "1"
            LTR_DIGOUT_USERIO0 = 0x02, // выход подключен к ножке userio0 (PF1 в рев. 0, PF1 в рев. 1)
            LTR_DIGOUT_USERIO1 = 0x03, // выход подключен к ножке userio1 (PG13)
            LTR_DIGOUT_DIGIN1  = 0x04, // выход подключен ко входу DIGIN1
            LTR_DIGOUT_DIGIN2  = 0x05, // выход подключен ко входу DIGIN2
            LTR_DIGOUT_START   = 0x06, // на выход подаются метки "СТАРТ"
            LTR_DIGOUT_SECOND  = 0x07, // на выход подаются метки "СЕКУНДА"
            LTR_DIGOUT_IRIG = 0x08, // контроль сигналов точного времени IRIG (digout1: готовность, digout2: секунда)
            LTR_DIGOUT_DEFAULT = LTR_DIGOUT_CONST0
        }

        // Значения для управления метками "СТАРТ" и "СЕКУНДА"
        public enum en_LTR_MarkMode : int
        {
            LTR_MARK_OFF = 0x00, // метка отключена
            LTR_MARK_EXT_DIGIN1_RISE = 0x01, // метка по фронту DIGIN1
            LTR_MARK_EXT_DIGIN1_FALL = 0x02, // метка по спаду DIGIN1
            LTR_MARK_EXT_DIGIN2_RISE = 0x03, // метка по фронту DIGIN2
            LTR_MARK_EXT_DIGIN2_FALL = 0x04, // метка по спаду DIGIN2
            LTR_MARK_INTERNAL        = 0x05, // внутренняя генерация метки
            // Режимы меток LTR-E-7/15
            LTR_MARK_NAMUR1_LO2HI = 0x08, // по сигналу NAMUR1, возрастание тока
            LTR_MARK_NAMUR1_HI2LO = 0x09, // по сигналу NAMUR1, спад тока
            LTR_MARK_NAMUR2_LO2HI = 0x0A, // по сигналу NAMUR2, возрастание тока
            LTR_MARK_NAMUR2_HI2LO = 0x0B, // по сигналу NAMUR2, спад тока

            /* Источник метки - декодер сигналов точного времени IRIG-B006
                IRIG может использоваться только для меток "СЕКУНДА", для "СТАРТ" игнорируется */
            LTR_MARK_SEC_IRIGB_DIGIN1 = 16,   // со входа DIGIN1, прямой сигнал
            LTR_MARK_SEC_IRIGB_nDIGIN1 = 17,   // со входа DIGIN1, инвертированный сигнал
            LTR_MARK_SEC_IRIGB_DIGIN2 = 18,   // со входа DIGIN2, прямой сигнал
            LTR_MARK_SEC_IRIGB_nDIGIN2 = 19,   // со входа DIGIN2, инвертированный сигнал
        };

        public enum LTRCC : ushort
        {
            CONTROL = 0,            // канал для работы c модулем в слоте 0, он же
            MODULE0 = 0,           // канал для управления крейтом и сервером
            MODULE1,              // канал для работы c модулем в слоте 1
            MODULE2,              // канал для работы c модулем в слоте 2
            MODULE3,              // канал для работы c модулем в слоте 3
            MODULE4,              // канал для работы c модулем в слоте 4
            MODULE5,              // канал для работы c модулем в слоте 5
            MODULE6,              // канал для работы c модулем в слоте 6
            MODULE7,              // канал для работы c модулем в слоте 7
            MODULE8,              // канал для работы c модулем в слоте 8
            MODULE9,              // канал для работы c модулем в слоте 9
            MODULE10,             // канал для работы c модулем в слоте 10
            MODULE11,             // канал для работы c модулем в слоте 11
            MODULE12,             // канал для работы c модулем в слоте 12
            MODULE13,             // канал для работы c модулем в слоте 13
            MODULE14,             // канал для работы c модулем в слоте 14
            MODULE15,             // канал для работы c модулем в слоте 15
            MODULE16,             // канал для работы c модулем в слоте 16
            DEBUG_FLAG = 0x8000   // флаг отладки - данные высылаются в крейт в
        }

        public enum LTRCOMMAND
        {
            CMD = 0x8000,
            CMD_STOP = 0x8000,
            CMD_PROGR = 0x8040,
            CMD_RESET = 0x8080,
            CMD_INSTR = 0x80C0,
            SLOT_MASK = (0xF << 8)
        }

        public enum LTRERROR
        {
            OK = 0,                                 /*Выполнено без ошибок.*/
            ERROR_UNKNOWN = -1,                     /*Неизвестная ошибка.*/
            ERROR_PARAMETRS = -2,                   /*Ошибка входных параметров.*/
            ERROR_MEMORY_ALLOC = -3,                /*Ошибка динамического выделения памяти.*/
            ERROR_OPEN_CHANNEL = -4,                /*Ошибка открытия канала обмена с сервером.*/
            ERROR_OPEN_SOCKET = -5,                 /*Ошибка открытия сокета.*/
            ERROR_CHANNEL_CLOSED = -6,              /*Ошибка. Канал обмена с сервером не создан.*/
            ERROR_SEND = -7,                        /*Ошибка отправления данных.*/
            ERROR_RECV = -8,                        /*Ошибка приема данных.*/
            ERROR_EXECUTE = -9,                     /*Ошибка обмена с крейт-контроллером.*/
            WARNING_MODULE_IN_USE = -10,            /* Канал обмена с сервером уже был создан и не закрыт*/
            ERROR_NOT_CTRL_CHANNEL = -11,           /* Номер канала для этой операции должен быть CC_CONTROL */
            ERROR_SRV_INVALID_CMD = -12,            /* Команда не поддерживается сервером */
            ERROR_SRV_INVALID_CMD_PARAMS = -13,     /* Сервер не поддерживает указанные параметры команды */
            ERROR_INVALID_CRATE = -14,              /* Указанный крейт не найден */
            ERROR_EMPTY_SLOT = -15,                 /* В указанном слоте отсутствует модуль */
            ERROR_UNSUP_CMD_FOR_SRV_CTL = -16,      /* Команда не поддерживается управляющим каналом сервера */
            ERROR_INVALID_IP_ENTRY = -17,           /* Неверная запись сетевого адреса крейта */
            ERROR_NOT_IMPLEMENTED = -18,            /* Данная возможность не реализована */
            ERROR_CONNECTION_CLOSED = -19,          /* Соединение было закрыто сервером */
            ERROR_LTRD_UNKNOWN_RETCODE = -20,       /* Неизвестный код ошибки службы ltrd */
            ERROR_LTRD_CMD_FAILED = -21,            /* Ошибка выполнения управляющей команды ltrd */
            ERROR_INVALID_CON_SLOT_NUM = -22,       /* Указан неверный номер слота при открытии соединения */

            ERROR_INVALID_MODULE_DESCR   = -40,    /* Неверный описатель модуля */
            ERROR_INVALID_MODULE_SLOT    = -41,    /* Указан неверный слот для модуля */
            ERROR_INVALID_MODULE_ID      = -42,    /* Неверный ID-модуля в ответе на сброс */
            ERROR_NO_RESET_RESPONSE      = -43,    /* Нет ответа на команду сброса */
            ERROR_SEND_INSUFFICIENT_DATA = -44,    /* Передано данных меньше, чем запрашивалось */
            ERROR_RECV_INSUFFICIENT_DATA = -45,    /* Принято данных меньше, чем запрашивалось */
            ERROR_NO_CMD_RESPONSE        = -46,    /* Нет ответа на команду */
            ERROR_INVALID_CMD_RESPONSE   = -47,    /* Пришел неверный ответ на команду */
            ERROR_INVALID_RESP_PARITY    = -48,    /* Неверный бит четности в пришедшем ответе */
            ERROR_INVALID_CMD_PARITY     = -49,    /* Ошибка четности переданной команды */
            ERROR_UNSUP_BY_FIRM_VER      = -50,    /* Возможность не поддерживается данной версией прошивки */
            ERROR_MODULE_STARTED         = -51,    /* Модуль уже запущен */
            ERROR_MODULE_STOPPED         = -52,    /* Модуль остановлен */
            ERROR_RECV_OVERFLOW          = -53,    /* Произошло переполнение буфера */
            ERROR_FIRM_FILE_OPEN         = -54,    /* Ошибка открытия файла прошивки */
            ERROR_FIRM_FILE_READ         = -55,    /* Ошибка чтения файла прошивки */
            ERROR_FIRM_FILE_FORMAT       = -56,    /* Ошибка формата файла прошивки */
            ERROR_FPGA_LOAD_READY_TOUT   = -57,    /* Превышен таймаут ожидания готовности ПЛИС к загрузке */
            ERROR_FPGA_LOAD_DONE_TOUT    = -58,    /* Превышен таймаут ожидания перехода ПЛИС в рабочий режим */
            ERROR_FPGA_IS_NOT_LOADED     = -59,    /* Прошивка ПЛИС не загружена */
            ERROR_FLASH_INVALID_ADDR     = -60,    /* Неверный адрес Flash-памяти */
            ERROR_FLASH_WAIT_RDY_TOUT    = -61,    /* Превышен таймаут ожидания завершения записи/стирания Flash-памяти */
            ERROR_FIRSTFRAME_NOTFOUND    = -62,    /* First frame in card data stream not found */
            ERROR_CARDSCONFIG_UNSUPPORTED = -63,
            ERROR_FLASH_OP_FAILED         = -64,    /* Ошибка выполненя операции flash-памятью */
            ERROR_FLASH_NOT_PRESENT       = -65,    /* Flash-память не обнаружена */
            ERROR_FLASH_UNSUPPORTED_ID    = -66,    /* Обнаружен неподдерживаемый тип flash-памяти */
            ERROR_FLASH_UNALIGNED_ADDR    = -67,    /* Невыровненный адрес flash-памяти */
            ERROR_FLASH_VERIFY            = -68,    /* Ошибка при проверки записанных данных во flash-память */
            ERROR_FLASH_UNSUP_PAGE_SIZE   = -69,    /* Установлен неподдерживаемый размер страницы flash-памяти */
            ERROR_FLASH_INFO_NOT_PRESENT  = -70,    /* Отсутствует информация о модуле во Flash-памяти */
            ERROR_FLASH_INFO_UNSUP_FORMAT = -71,    /* Неподдерживаемый формат информации о модуле во Flash-памяти */
            ERROR_FLASH_SET_PROTECTION    = -72,    /* Не удалось установить защиту Flash-памяти */
            ERROR_FPGA_NO_POWER           = -73,    /* Нет питания микросхемы ПЛИС */
            ERROR_FPGA_INVALID_STATE      = -74,    /* Не действительное состояние загрузки ПЛИС */
            ERROR_FPGA_ENABLE             = -75,    /* Не удалось перевести ПЛИС в разрешенное состояние */
            ERROR_FPGA_AUTOLOAD_TOUT      = -76,    /* Истекло время ожидания автоматической загрузки ПЛИС */
            ERROR_PROCDATA_UNALIGNED      = -77,    /* Обрабатываемые данные не выравнены на границу кадра */
            ERROR_PROCDATA_CNTR           = -78,    /* Ошибка счетчика в обрабатываемых данных */
            ERROR_PROCDATA_CHNUM          = -79,    /* Неверный номер канала в обрабатываемых данных */
            ERROR_PROCDATA_WORD_SEQ       = -80,    /* Неверная последовательность слов в обрабатываемых данных */
            ERROR_FLASH_INFO_CRC          = -81,    /* Неверная контрольная сумма в записанной информации о модуле */
            ERROR_PROCDATA_UNEXP_CMD      = -82,  /** Обнаружена неожиданная команда в потоке данных */
            ERROR_UNSUP_BY_BOARD_REV      = -83,  /** Возможность не поддерживается данной ревизией платы */
            ERROR_MODULE_NOT_CONFIGURED   = -84,  /** Не выполнена конфигурация модуля */

            LTR010_ERROR_GET_ARRAY        = -100, /*Ошибка выполнения команды GET_ARRAY.*/
            LTR010_ERROR_PUT_ARRAY        = -101, /*Ошибка выполнения команды PUT_ARRAY.*/
            LTR010_ERROR_GET_MODULE_NAME  = -102, /*Ошибка выполнения команды GET_MODULE_NAME.*/
            LTR010_ERROR_GET_MODULE_DESCR = -103, /*Ошибка выполнения команды GET_MODULE_DESCRIPTOR.*/
            LTR010_ERROR_INIT_FPGA        = -104, /*Ошибка выполнения команды INIT_FPGA.*/
            LTR010_ERROR_RESET_FPGA       = -105, /*Ошибка выполнения команды RESET_FPGA.*/
            LTR010_ERROR_INIT_DMAC        = -106, /*Ошибка выполнения команды INIT_DMAC.*/
            LTR_ERROR_LOAD_FPGA           = -107, /*Ошибка выполнения команды LOAD_FPGA.*/
            LTR010_ERROR_OPEN_FILE        = -108, /*Ошибка открытия файла.*/
            LTR010_ERROR_GET_INFO_FPGA    = -109, /*Ошибка выполнения команды GET_INFO_FPGA.*/

            LTR021_ERROR_GET_ARRAY        = -200, /*Ошибка выполнения команды GET_ARRAY.*/
            LTR021_ERROR_PUT_ARRAY        = -201, /*Ошибка выполнения команды PUT_ARRAY.*/
            LTR021_ERROR_GET_MODULE_NAME  = -202, /*Ошибка выполнения команды GET_MODULE_NAME.*/
            LTR021_ERROR_GET_MODULE_GESCR = -203, /*Ошибка выполнения команды GET_MODULE_DESCRIPTOR.*/
            LTR021_ERROR_CRATE_TYPE       = -204, /*Неверный тип крейта.*/
            LTR021_ERROR_TIMEOUT          = -205, /*Превышение таймаута */

            LTRAVR_ERROR_RECV_PRG_DATA_ECHO = -200,     /*Ошибка приема эхо ответа данных для программирования.*/
            LTRAVR_ERROR_SEND_PRG_DATA = -201,     /*Ошибка отправки данных команды програмирования avr.*/
            LTRAVR_ERROR_RECV_PRG_ENABLE_ACK = -202,     /*Ошибка приема подтверждения команды входа в режим программирования.*/
            LTRAVR_ERROR_SEND_PRG_ENB_CMD = -203,     /*Ошибка отправки команды входа в режим программирования.*/
            LTRAVR_ERROR_CHIP_ERASE = -204,     /*Ошибка стирания flash команд avr.*/
            LTRAVR_ERROR_READ_PRG_MEM = -205,     /*Ошибка считывания flash команд avr.*/
            LTRAVR_ERROR_WRITE_PRG_MEM = -206,     /*Ошибка программирования flash команд avr.*/
            LTRAVR_ERROR_READ_FUSE_BITS = -207,     /*Ошибка считывания fuse витов avr.*/
            LTRAVR_ERROR_WRITE_FUSE_BITS = -208,     /*Ошибка программирования fuse витов avr.*/
            LTRAVR_ERROR_READ_SIGN = -209,     /*Ошибка считывания сигнатуры avr.*/

            LTRBOOT_ERROR_GET_ARRAY = -300,            /*Ошибка выполнения команды GET_ARRAY.*/
            LTRBOOT_ERROR_PUT_ARRAY = -301,      /*Ошибка выполнения команды PUT_ARRAY.*/
            LTRBOOT_ERROR_CALL_APPL = -302,      /*Ошибка выполнения команды CALL_APPLICATION.*/
            LTRBOOT_ERROR_GET_DESCRIPTION = -303,      /*Ошибка выполнения команды GET_DESCRIPTION.*/
            LTRBOOT_ERROR_PUT_DESCRIPTION = -304,     /*Ошибка выполнения команды PUT_DESCRIPTION.*/


            LTR030_ERR_UNSUPORTED_CRATE_TYPE = -400, /* Данный тип крейта не поддерживается библиотекой */
            LTR030_ERR_FIRM_VERIFY           = -401, /* Ошибка проверки правильности записи прошивки */
            LTR030_ERR_FIRM_SIZE             = -402,  /* Неверный размер прошивки */

            // LTR11
            /* Коды ошибок, возвращаемые функциями библиотеки */
            LTR11_ERR_INVALID_DESCR = -1000, /* указатель на описатель модуля равен NULL */
            LTR11_ERR_INVALID_ADCMODE = -1001, /* недопустимый режим запуска АЦП */
            LTR11_ERR_INVALID_ADCLCHQNT = -1002, /* недопустимое количество логических каналов */
            LTR11_ERR_INVALID_ADCRATE = -1003, /* недопустимое значение частоты дискретизации АЦП модуля*/
            LTR11_ERR_INVALID_ADCSTROBE = -1004, /* недопустимый источник тактовой частоты для АЦП */
            LTR11_ERR_GETFRAME = -1005, /* ошибка получения кадра данных с АЦП */
            LTR11_ERR_GETCFG = -1006, /* ошибка чтения конфигурации */
            LTR11_ERR_CFGDATA = -1007, /* ошибка при получении конфигурации модуля */
            LTR11_ERR_CFGSIGNATURE = -1008, /* неверное значение первого байта конфигурационной записи модуля*/
            LTR11_ERR_CFGCRC = -1009, /* неверная контрольная сумма конфигурационной записи*/
            LTR11_ERR_INVALID_ARRPOINTER = -1010, /* указатель на массив равен NULL */
            LTR11_ERR_ADCDATA_CHNUM = -1011, /* неверный номер канала в массиве данных от АЦП */
            LTR11_ERR_INVALID_CRATESN = -1012, /* указатель на строку с серийным номером крейта равен NULL*/
            LTR11_ERR_INVALID_SLOTNUM = -1013, /* недопустимый номер слота в крейте */
            LTR11_ERR_NOACK = -1014, /* нет подтверждения от модуля */
            LTR11_ERR_MODULEID = -1015, /* попытка открытия модуля, отличного от LTR11 */
            LTR11_ERR_INVALIDACK = -1016, /* неверное подтверждение от модуля */
            LTR11_ERR_ADCDATA_SLOTNUM = -1017, /* неверный номер слота в данных от АЦП */
            LTR11_ERR_ADCDATA_CNT = -1018, /* неверный счетчик пакетов в данных от АЦП */
            LTR11_ERR_INVALID_STARTADCMODE = -1019, /* неверный режим старта сбора данных */

            // LTR212
            LTR212_ERR_INVALID_DESCR = -2001,
            LTR212_ERR_INVALID_CRATE_SN = -2002,
            LTR212_ERR_INVALID_SLOT_NUM = -2003,
            LTR212_ERR_CANT_INIT = -2004,
            LTR212_ERR_CANT_OPEN_CHANNEL = -2005,
            LTR212_ERR_CANT_CLOSE = -2006,
            LTR212_ERR_CANT_LOAD_BIOS = -2007,
            LTR212_ERR_CANT_SEND_COMMAND = -2008,
            LTR212_ERR_CANT_READ_EEPROM = -2009,
            LTR212_ERR_CANT_WRITE_EEPROM = -2010,
            LTR212_ERR_CANT_LOAD_IIR = -2011,
            LTR212_ERR_CANT_LOAD_FIR = -2012,
            LTR212_ERR_CANT_RESET_CODECS = -2013,
            LTR212_ERR_CANT_SELECT_CODEC = -2014,
            LTR212_ERR_CANT_WRITE_REG = -2015,
            LTR212_ERR_CANT_READ_REG = -2016,
            LTR212_ERR_WRONG_ADC_SETTINGS = -2017,
            LTR212_ERR_WRONG_VCH_SETTINGS = -2018,
            LTR212_ERR_CANT_SET_ADC = -2019,
            LTR212_ERR_CANT_CALIBRATE = -2020,
            LTR212_ERR_CANT_START_ADC = -2021,
            LTR212_ERR_INVALID_ACQ_MODE = -2022,
            LTR212_ERR_CANT_GET_DATA = -2023,
            LTR212_ERR_CANT_MANAGE_FILTERS = -2024,
            LTR212_ERR_CANT_STOP = -2025,
            LTR212_ERR_CANT_GET_FRAME = -2026,
            LTR212_ERR_INV_ADC_DATA = -2026,
            LTR212_ERR_TEST_NOT_PASSED = -2027,
            LTR212_ERR_CANT_WRITE_SERIAL_NUM = -2028,
            LTR212_ERR_CANT_RESET_MODULE = -2029,
            LTR212_ERR_MODULE_NO_RESPONCE = -2030,
            LTR212_ERR_WRONG_FLASH_CRC = -2031,
            LTR212_ERR_CANT_USE_FABRIC_AND_USER_CALIBR_SYM = -2032,
            LTR212_ERR_CANT_START_INTERFACE_TEST = -2033,
            LTR212_ERR_WRONG_BIOS_FILE = -2034,
            LTR212_ERR_CANT_USE_CALIBR_MODE = -2035,
            LTR212_ERR_PARITY_ERROR = -2036,
            LTR212_ERR_CANT_LOAD_CLB_COEFFS = -2037,
            LTR212_ERR_CANT_LOAD_FABRIC_CLB_COEFFS = -2038,
            LTR212_ERR_CANT_GET_VER = -2039,
            LTR212_ERR_CANT_GET_DATE = -2040,
            LTR212_ERR_WRONG_SN = -2041,
            LTR212_ERR_CANT_EVAL_DAC = -2042,
            LTR212_ERR_ERROR_OVERFLOW = -2043,
            LTR212_ERR_SOME_CHENNEL_CANT_CLB      = -2044,
            LTR212_ERR_CANT_GET_MODULE_TYPE       = -2045,
            LTR212_ERR_ERASE_OR_PROGRAM_FLASH                           =-2046,
            LTR212_ERR_CANT_SET_BRIDGE_CONNECTIONS                      =-2047,
            LTR212_ERR_CANT_SET_BRIDGE_CONNECTIONS_FOR_THIS_MODULE_TYPE =-2048,
            LTR212_ERR_QB_RESISTORS_IN_ALL_CHANNELS_MUST_BE_EQUAL       =-2049,            
            LTR212_ERR_INVALID_EEPROM_ADDR                              =-2050,
            LTR212_ERR_INVALID_VCH_CNT                                  =-2051,
            LTR212_ERR_FILTER_FILE_OPEN                                 =-2052,
            LTR212_ERR_FILTER_FILE_READ                                 =-2053,
            LTR212_ERR_FILTER_FILE_FORMAT                               =-2054,
            LTR212_ERR_FILTER_ORDER                                     =-2055,
            LTR212_ERR_UNSUPPORTED_MODULE_TYPE                          =-2056,

            // LTR27
            LTR27_ERROR_SEND_DATA = -3000,
            LTR27_ERROR_RECV_DATA = -3001,
            LTR27_ERROR_RESET_MODULE = -3002,
            LTR27_ERROR_NOT_LTR27 = -3003,
            LTR27_ERROR_PARITY = -3004,
            LTR27_ERROR_OVERFLOW = -3005,
            LTR27_ERROR_INDEX = -3006,
            LTR27_ERROR = -3007,
            LTR27_ERROR_EXCHANGE = -3008,
            LTR27_ERROR_FORMAT = -3008,
            LTR27_ERROR_CRC = -3010,

            // LTR43
            LTR43_ERR_WRONG_MODULE_DESCR = -4001,
            LTR43_ERR_CANT_OPEN = -4002,
            LTR43_ERR_INVALID_CRATE_SN = -4003,
            LTR43_ERR_INVALID_SLOT_NUM = -4004,
            LTR43_ERR_CANT_SEND_COMMAND = -4005,
            LTR43_ERR_CANT_RESET_MODULE = -4006,
            LTR43_ERR_MODULE_NO_RESPONCE = -4007,
            LTR43_ERR_CANT_SEND_DATA = -4008,
            LTR43_ERR_CANT_CONFIG = -4009,
            LTR43_ERR_CANT_RS485_CONFIG = -4010,
            LTR43_ERR_CANT_LAUNCH_SEC_MARK = -4011,
            LTR43_ERR_CANT_STOP_SEC_MARK = -4012,
            LTR43_ERR_CANT_LAUNCH_START_MARK = -4013,
            LTR43_ERR_CANT_STOP_RS485RCV = -4014,
            LTR43_ERR_RS485_CANT_SEND_BYTE = -4015,
            LTR43_ERR_RS485_FRAME_ERR_RCV = -4016,
            LTR43_ERR_RS485_PARITY_ERR_RCV = -4017,
            LTR43_ERR_WRONG_IO_GROUPS_CONF = -4018,
            LTR43_ERR_RS485_WRONG_BAUDRATE = -4019,
            LTR43_ERR_RS485_WRONG_FRAME_SIZE = -4020,
            LTR43_ERR_RS485_WRONG_PARITY_CONF = -4021,
            LTR43_ERR_RS485_WRONG_STOPBIT_CONF = -4022,
            LTR43_ERR_DATA_TRANSMISSON_ERROR = -4023,
            LTR43_ERR_RS485_CONFIRM_TIMEOUT = -4024,
            LTR43_ERR_RS485_SEND_TIMEOUT = -4025,
            LTR43_ERR_LESS_WORDS_RECEIVED = -4026,
            LTR43_ERR_PARITY_TO_MODULE = -4027,
            LTR43_ERR_PARITY_FROM_MODULE = -4028,
            LTR43_ERR_WRONG_IO_LINES_CONF = -4029,
            LTR43_ERR_WRONG_SECOND_MARK_CONF = -4030,
            LTR43_ERR_WRONG_START_MARK_CONF = -4031,
            LTR43_ERR_CANT_READ_DATA = -4032,
            LTR43_ERR_RS485_CANT_SEND_PACK = -4033,
            LTR43_ERR_RS485_CANT_CONFIGURE = -4034,
            LTR43_ERR_CANT_WRITE_EEPROM = -4035,
            LTR43_ERR_CANT_READ_EEPROM = -4036,
            LTR43_ERR_WRONG_EEPROM_ADDR = -4037,
            LTR43_ERR_RS485_WRONG_PACK_SIZE = -4038,
            LTR43_ERR_RS485_WRONG_OUT_TIMEOUT = -4039,
            LTR43_ERR_RS485_WRONG_IN_TIMEOUT = -4040,
            LTR43_ERR_CANT_READ_CONF_REC = -4041,
            LTR43_ERR_WRONG_CONF_REC = -4042,
            LTR43_ERR_RS485_CANT_STOP_TST_RCV = -4043,
            LTR43_ERR_CANT_START_STREAM_READ = -4044,
            LTR43_ERR_CANT_STOP_STREAM_READ = -4045,
            LTR43_ERR_WRONG_IO_DATA = -4046,
            LTR43_ERR_WRONG_STREAM_READ_FREQ_SETTINGS = -4047,
            LTR43_ERR_ERROR_OVERFLOW = -4048,

            // LTR51
            LTR51_ERR_WRONG_MODULE_DESCR = -5001,
            LTR51_ERR_CANT_OPEN = -5002,
            LTR51_ERR_CANT_LOAD_ALTERA = -5003,
            LTR51_ERR_INVALID_CRATE_SN = -5004,
            LTR51_ERR_INVALID_SLOT_NUM = -5005,
            LTR51_ERR_CANT_SEND_COMMAND = -5006,
            LTR51_ERR_CANT_RESET_MODULE = -5007,
            LTR51_ERR_MODULE_NO_RESPONCE = -5008,
            LTR51_ERR_CANT_OPEN_MODULE = -5009,
            LTR51_ERR_PARITY_TO_MODULE = -5010,
            LTR51_ERR_PARITY_FROM_MODULE = -5011,
            LTR51_ERR_ALTERA_TEST_FAILED = -5012,
            LTR51_ERR_CANT_START_DATA_AQC = -5013,
            LTR51_ERR_CANT_STOP_DATA_AQC = -5014,
            LTR51_ERR_CANT_SET_FS = -5015,
            LTR51_ERR_CANT_SET_BASE = -5016,
            LTR51_ERR_CANT_SET_EDGE_MODE = -5017,
            LTR51_ERR_CANT_SET_THRESHOLD = -5018,
            LTR51_WRONG_DATA = -5019,
            LTR51_ERR_WRONG_HIGH_THRESOLD_SETTINGS = -5020,
            LTR51_ERR_WRONG_LOW_THRESOLD_SETTINGS = -5021,
            LTR51_ERR_WRONG_FPGA_FILE = -5022,
            LTR51_ERR_CANT_READ_ID_REC = -5023,
            LTR51_ERR_WRONG_ID_REC = -5024,
            LTR51_ERR_WRONG_FS_SETTINGS = -5025,
            LTR51_ERR_WRONG_BASE_SETTINGS = -5026,
            LTR51_ERR_CANT_WRITE_EEPROM = -5027,
            LTR51_ERR_CANT_READ_EEPROM = -5028,
            LTR51_ERR_WRONG_EEPROM_ADDR = -5029,
            LTR51_ERR_WRONG_THRESHOLD_VALUES = -5030,
            LTR51_ERR_ERROR_OVERFLOW = -5031,
            LTR51_ERR_MODULE_WRONG_ACQ_TIME_SETTINGS = -5032,
            LTR51_ERR_NOT_ENOUGH_POINTS = -5033,
            LTR51_ERR_WRONG_SRC_SIZE = -5034,



            // LTR22
            LTR22_ERROR_SEND_DATA = -6000,
            LTR22_ERROR_RECV_DATA = -6001,
            LTR22_ERROR_NOT_LTR22 = -6002,
            LTR22_ERROR_OVERFLOW = -6003,
            LTR22_ERROR_CANNOT_DO_WHILE_ADC_RUNNING = -6004,
            LTR22_ERROR_MODULE_INTERFACE = -6005,
            LTR22_ERROR_INVALID_FREQ_DIV = -6006,
            LTR22_ERROR_INVALID_TEST_HARD_INTERFACE = -6007,
            LTR22_ERROR_INVALID_DATA_RANGE_FOR_THIS_CHANNEL = -6008,
            LTR22_ERROR_INVALID_DATA_COUNTER = -6009,
            LTR22_ERROR_PRERARE_TO_WRITE = -6010,
            LTR22_ERROR_WRITE_AVR_MEMORY = -6011,
            LTR22_ERROR_READ_AVR_MEMORY = -6012,
            LTR22_ERROR_PARAMETERS = -6013,
            LTR22_ERROR_CLEAR_BUFFER_TOUT = -6014,
            LTR22_ERROR_SYNC_FHAZE_NOT_STARTED = -6015,
            LTR22_ERROR_INVALID_CH_NUMBER = -6016,
            LTR22_ERROR_AVR_MEMORY_COMPARE = -6017,

            LTR34_ERROR_SEND_DATA = -3001,
            LTR34_ERROR_RECV_DATA = -3002,
            LTR34_ERROR_RESET_MODULE = -3003,
            LTR34_ERROR_NOT_LTR34 = -3004,
            LTR34_ERROR_CRATE_BUF_OWF = -3005,
            LTR34_ERROR_PARITY = -3006,
            LTR34_ERROR_OVERFLOW = -3007,
            LTR34_ERROR_INDEX = -3008,
            //
            LTR34_ERROR = -3009,
            LTR34_ERROR_EXCHANGE = -3010,
            LTR34_ERROR_FORMAT = -3011,
            LTR34_ERROR_PARAMETERS = -3012,
            LTR34_ERROR_ANSWER = -3013,
            LTR34_ERROR_WRONG_FLASH_CRC = -3014,
            LTR34_ERROR_CANT_WRITE_FLASH = -3015,
            LTR34_ERROR_CANT_READ_FLASH = -3016,
            LTR34_ERROR_CANT_WRITE_SERIAL_NUM = -3017,
            LTR34_ERROR_CANT_READ_SERIAL_NUM = -3018,
            LTR34_ERROR_CANT_WRITE_FPGA_VER = -3019,
            LTR34_ERROR_CANT_READ_FPGA_VER = -3020,
            LTR34_ERROR_CANT_WRITE_CALIBR_VER = -3021,
            LTR34_ERROR_CANT_READ_CALIBR_VER = -3022,
            LTR34_ERROR_CANT_STOP = -3023,
            LTR34_ERROR_SEND_CMD = -3024,
            LTR34_ERROR_CANT_WRITE_MODULE_NAME = -3025,
            LTR34_ERROR_CANT_WRITE_MAX_CH_QNT = -3026,
            LTR34_ERROR_CHANNEL_NOT_OPENED = -3027,
            LTR34_ERROR_WRONG_LCH_CONF = -3028,

            LTR41_ERR_WRONG_MODULE_DESCR = -7001,
            LTR41_ERR_CANT_OPEN = -7002,
            LTR41_ERR_INVALID_CRATE_SN = -7003,
            LTR41_ERR_INVALID_SLOT_NUM = -7004,
            LTR41_ERR_CANT_SEND_COMMAND = -7005,
            LTR41_ERR_CANT_RESET_MODULE = -7006,
            LTR41_ERR_MODULE_NO_RESPONCE = -7007,
            LTR41_ERR_CANT_CONFIG = -7008,
            LTR41_ERR_CANT_LAUNCH_SEC_MARK = -7009,
            LTR41_ERR_CANT_STOP_SEC_MARK = -7010,
            LTR41_ERR_CANT_LAUNCH_START_MARK = -7011,
            LTR41_ERR_LESS_WORDS_RECEIVED = -7012,
            LTR41_ERR_PARITY_TO_MODULE = -7013,
            LTR41_ERR_PARITY_FROM_MODULE = -7014,
            LTR41_ERR_WRONG_SECOND_MARK_CONF = -7015,
            LTR41_ERR_WRONG_START_MARK_CONF = -7016,
            LTR41_ERR_CANT_READ_DATA = -7017,
            LTR41_ERR_CANT_WRITE_EEPROM = -7018,
            LTR41_ERR_CANT_READ_EEPROM = -7019,
            LTR41_ERR_WRONG_EEPROM_ADDR = -7020,
            LTR41_ERR_CANT_READ_CONF_REC = -7021,
            LTR41_ERR_WRONG_CONF_REC = -7022,
            LTR41_ERR_CANT_START_STREAM_READ = -7023,
            LTR41_ERR_CANT_STOP_STREAM_READ = -7024,
            LTR41_ERR_WRONG_IO_DATA = -7025,
            LTR41_ERR_WRONG_STREAM_READ_FREQ_SETTINGS = -7026,
            LTR41_ERR_ERROR_OVERFLOW = -7027,

            LTR42_ERR_WRONG_MODULE_DESCR = -8001,
            LTR42_ERR_CANT_OPEN = -8002,
            LTR42_ERR_INVALID_CRATE_SN = -8003,
            LTR42_ERR_INVALID_SLOT_NUM = -8004,
            LTR42_ERR_CANT_SEND_COMMAND = -8005,
            LTR42_ERR_CANT_RESET_MODULE = -8006,
            LTR42_ERR_MODULE_NO_RESPONCE = -8007,
            LTR42_ERR_CANT_SEND_DATA = -8008,
            LTR42_ERR_CANT_CONFIG = -8009,
            LTR42_ERR_CANT_LAUNCH_SEC_MARK = -8010,
            LTR42_ERR_CANT_STOP_SEC_MARK = -8011,
            LTR42_ERR_CANT_LAUNCH_START_MARK = -8012,
            LTR42_ERR_DATA_TRANSMISSON_ERROR = -8013,
            LTR42_ERR_LESS_WORDS_RECEIVED = -8014,
            LTR42_ERR_PARITY_TO_MODULE = -8015,
            LTR42_ERR_PARITY_FROM_MODULE = -8016,
            LTR42_ERR_WRONG_SECOND_MARK_CONF = -8017,
            LTR42_ERR_WRONG_START_MARK_CONF = -8018,
            LTR42_ERR_CANT_READ_DATA = -8019,
            LTR42_ERR_CANT_WRITE_EEPROM = -8020,
            LTR42_ERR_CANT_READ_EEPROM = -8021,
            LTR42_ERR_WRONG_EEPROM_ADDR = -8022,
            LTR42_ERR_CANT_READ_CONF_REC = -8023,
            LTR42_ERR_WRONG_CONF_REC = -8024,



            LTR114_ERR_INVALID_DESCR       = -10000, /* указатель на описатель модуля равен NULL */
            LTR114_ERR_INVALID_SYNCMODE    = -10001, /* недопустимый режим синхронизации модуля АЦП */
            LTR114_ERR_INVALID_ADCLCHQNT   = -10002, /* недопустимое количество логических каналов */
            LTR114_ERR_INVALID_ADCRATE     = -10003, /* недопустимое значение частоты дискретизации АЦП
                                                      * модуля
                                                      */
            LTR114_ERR_GETFRAME            = -10004, /* ошибка получения кадра данных с АЦП */
            LTR114_ERR_GETCFG              = -10005, /* ошибка чтения конфигурации */
            LTR114_ERR_CFGDATA             = -10006, /* ошибка при получении конфигурации модуля */
            LTR114_ERR_CFGSIGNATURE        = -10007, /* неверное значение первого байта конфигурационной
                                                      * записи модуля
                                                      */
            LTR114_ERR_CFGCRC              = -10008, /* неверная контрольная сумма конфигурационной
                                                      * записи
                                                      */
            LTR114_ERR_INVALID_ARRPOINTER  = -10009, /* указатель на массив равен NULL */
            LTR114_ERR_ADCDATA_CHNUM       = -10010, /* неверный номер канала в массиве данных от АЦП */
            LTR114_ERR_INVALID_CRATESN     = -10011, /* указатель на строку с серийным номером крейта
                                                      * равен NULL
                                                      */
            LTR114_ERR_INVALID_SLOTNUM     = -10012, /* недопустимый номер слота в крейте */
            LTR114_ERR_NOACK               = -10013, /* нет подтверждения от модуля */
            LTR114_ERR_MODULEID            = -10014, /* попытка открытия модуля, отличного от LTR114 */
            LTR114_ERR_INVALIDACK          = -10015, /* неверное подтверждение от модуля */
            LTR114_ERR_ADCDATA_SLOTNUM     = -10016, /* неверный номер слота в данных от АЦП */
            LTR114_ERR_ADCDATA_CNT         = -10017, /* неверный счетчик пакетов в данных от АЦП */
            LTR114_ERR_INVALID_LCH         = -10018, /* неверный режим лог. канала*/
            LTR114_ERR_CORRECTION_MODE     = -10019, /* неверный режим коррекции данных*/


            //LTR24
            LTR24_ERR_INVAL_FREQ        = -10100,
            LTR24_ERR_INVAL_FORMAT      = -10101,
            LTR24_ERR_CFG_UNSUP_CH_CNT  = -10102,
            LTR24_ERR_INVAL_RANGE       = -10103,
            LTR24_ERR_WRONG_CRC         = -10104,
            LTR24_ERR_VERIFY_FAILED     = -10105,
            LTR24_ERR_DATA_FORMAT       = -10106,
            LTR24_ERR_UNALIGNED_DATA    = -10107,
            LTR24_ERR_DISCONT_DATA      = -10108,
            LTR24_ERR_CHANNELS_DISBL    = -10109,
            LTR24_ERR_UNSUP_VERS        = -10110,
            LTR24_ERR_FRAME_NOT_FOUND   = -10111,
            LTR24_ERR_UNSUP_FLASH_FMT   = -10116,
            LTR24_ERR_INVAL_I_SRC_VALUE = -10117,
            LTR24_ERR_UNSUP_ICP_MODE    = -10118,
                      
            //LTR032
            LTR032_ERR_NO_MEM           = -10300,
            LTR032_ERR_INVAL_PARAM      = -10301,
            LTR032_ERR_NOT_OPEN         = -10302,
            LTR032_ERR_OPEN             = -10303,
            LTR032_ERR_INVAL_CRATE_TYPE = -10304,
            LTR032_ERR_CMD_REJECTED     = -10305,
            LTR032_ERR_INVALID_M1S_OUT_MODE     = -10306,
            LTR032_ERR_INVALID_NAMUR_LEVELS     = -10307,
            LTR032_ERR_INVALID_THERM_SENS_IND   = -10308,

            //LTRT10
            LTRT10_ERR_INVALID_SWITCH_POS = -10400, /**< Задан неверный код состояния коммутатора*/
            LTRT10_ERR_INVALID_DDS_DIV = -10401, /**< Задан неверный код коэффициента передачи выходного делителя сигнала DDS */
            LTRT10_ERR_INVALID_DDS_GAIN = -10402, /**< Задан неверный код усиления для DDS */
            LTRT10_ERR_INVALID_FREQ_VAL = -10403, /**< Неверно задан код частоты сигнала DDS */
            LTRT10_ERR_INVALID_DDS_AMP = -10404, /**< Задан неверный код амплитуды сигнала DDS */
            LTRT10_ERR_GAIN2_EXCEED_GAIN1 = -10405, /**< Коэф. усиления второй ступени превышает коэф. первой ступени */

            LTR210_ERR_INVALID_SYNC_MODE            = -10500, /**< Задан неверный код условия сбора кадра*/
            LTR210_ERR_INVALID_GROUP_MODE           = -10501, /**< Задан неверный код режима работы модуля в составе группы */
            LTR210_ERR_INVALID_ADC_FREQ_DIV         = -10502, /**< Задано неверное значение делителя частоты АЦП*/
            LTR210_ERR_INVALID_CH_RANGE             = -10503, /**< Задан неверный код диапазона канала АЦП*/
            LTR210_ERR_INVALID_CH_MODE              = -10504, /**< Задан неверный режим измерения канала*/
            LTR210_ERR_SYNC_LEVEL_EXCEED_RANGE      = -10505, /**< Установленный уровень аналоговой синхронизации
                                                                выходит за границы установленного диапазона*/
            LTR210_ERR_NO_ENABLED_CHANNEL           = -10506, /**< Ни один канал АЦП не был разрешен*/
            LTR210_ERR_PLL_NOT_LOCKED               = -10507, /**< Ошибка захвата PLL*/
            LTR210_ERR_INVALID_RECV_DATA_CNTR       = -10508, /**< Неверное значение счетчика в принятых данных*/
            LTR210_ERR_RECV_UNEXPECTED_CMD          = -10509, /**< Прием неподдерживаемой команды в потоке данных*/
            LTR210_ERR_FLASH_INFO_SIGN              = -10510, /**< Неверный признак информации о модуле во Flash-памяти*/
            LTR210_ERR_FLASH_INFO_SIZE              = -10511, /**< Неверный размер прочитанной из Flash-памяти информации о модуле*/
            LTR210_ERR_FLASH_INFO_UNSUP_FORMAT      = -10512, /**< Неподдерживаемый формат информации о модуле из Flash-памяти*/
            LTR210_ERR_FLASH_INFO_CRC               = -10513, /**< Ошибка проверки CRC информации о модуле из Flash-памяти*/
            LTR210_ERR_FLASH_INFO_VERIFY            = -10514, /**< Ошибка проверки записи информации о модуле во Flash-память*/
            LTR210_ERR_CHANGE_PAR_ON_THE_FLY        = -10515, /**< Часть измененных параметров нельзя изменять на лету */
            LTR210_ERR_INVALID_ADC_DCM_CNT          = -10516, /**< Задан неверный коэффициент прореживания данных АЦП */
            LTR210_ERR_MODE_UNSUP_ADC_FREQ          = -10517, /**< Установленный режим не поддерживает заданную частоту АЦП */
            LTR210_ERR_INVALID_FRAME_SIZE           = -10518, /**< Неверно задан размер кадра */
            LTR210_ERR_INVALID_HIST_SIZE            = -10519, /**< Неверно задан размер предыстории */
            LTR210_ERR_INVALID_INTF_TRANSF_RATE     = -10520, /**< Неверно задано значение скорости выдачи данных в интерфейс */
            LTR210_ERR_INVALID_DIG_BIT_MODE         = -10521, /**< Неверно задан режим работы дополнительного бита */
            LTR210_ERR_SYNC_LEVEL_LOW_EXCEED_HIGH   = -10522, /**< Нижний порог аналоговой синхронизации превышает верхний */
            LTR210_ERR_KEEPALIVE_TOUT_EXCEEDED      = -10523, /**< Не пришло ни одного статуса от модуля за заданный интервал */
            LTR210_ERR_WAIT_FRAME_TIMEOUT           = -10524, /**< Не удалось дождаться прихода кадра за заданное время */
            LTR210_ERR_FRAME_STATUS                 = -10525, /**< Слово статуса в принятом кадре указывает на ошибку данных */


            LTR25_ERR_FPGA_FIRM_TEMP_RANGE      = -10600, /**< Загружена прошивка ПЛИС для неверного температурного диапазона */
            LTR25_ERR_I2C_ACK_STATUS            = -10601, /**< Ошибка обмена при обращении к регистрам АЦП по интерфейсу I2C */
            LTR25_ERR_I2C_INVALID_RESP          = -10602, /**< Неверный ответ на команду при обращении к регистрам АЦП по интерфейсу I2C */
            LTR25_ERR_INVALID_FREQ_CODE         = -10603, /**< Неверно задан код частоты АЦП */
            LTR25_ERR_INVALID_DATA_FORMAT       = -10604, /**< Неверно задан формат данных АЦП */
            LTR25_ERR_INVALID_I_SRC_VALUE       = -10605, /**< Неверно задано значение источника тока" */
            LTR25_ERR_CFG_UNSUP_CH_CNT          = -10606, /**< Для заданной частоты и формата не поддерживается заданное количество каналов АЦП */
            LTR25_ERR_NO_ENABLED_CH             = -10607, /**< Не был разрешен ни один канал АЦП */
            LTR25_ERR_ADC_PLL_NOT_LOCKED        = -10608, /**< Ошибка захвата PLL АЦП */
            LTR25_ERR_ADC_REG_CHECK             = -10609, /**< Ошибка проверки значения записанных регистров АЦП */
            LTR25_ERR_LOW_POW_MODE_NOT_CHANGED  = -10610, /**< Не удалось перевести АЦП из/в низкопотребляющее состояние */
            LTR25_ERR_LOW_POW_MODE              = -10611, /**< Модуль находится в низкопотребляющем режиме */
            LTR25_ERR_INVALID_SENSOR_POWER_MODE = -10612, /**< Неверное значение режима питания датчиков */
            LTR25_ERR_CHANGE_SENSOR_POWER_MODE = -10613, /**< Не удалось изменить режим питания датчиков */
            LTR25_ERR_INVALID_CHANNEL_NUMBER = -10614, /**< Указан неверный номер канала модуля */
            LTR25_ERR_ICP_MODE_REQUIRED = -10615, /**< Модуль не переведен в ICP-режим питания датчиков, необходимый для данной операции  */
            LTR25_ERR_TEDS_MODE_REQUIRED = -10616, /**< Модуль не переведен в TEDS режим питания датчиков, необходимый для данной операции */
            LTR25_ERR_TEDS_UNSUP_NODE_FAMILY = -10617, /**< Данное семейство устройств TEDS узла не поддерживается библиотекой */
            LTR25_ERR_TEDS_UNSUP_NODE_OP = -10618, /**< Данная операция не поддерживается библиотекой для обнаруженого типа узла TEDS */
            LTR25_ERR_TEDS_NODE_URN_CRC = -10624, /**< Неверное значение контрольной суммы в URN узла TEDS */
            LTR25_ERR_TEDS_DATA_CRC = -10619, /**< Неверное значение контрольной суммы в прочитанных данных TEDS */
            LTR25_ERR_TEDS_1W_NO_PRESENSE_PULSE = -10620, /**< Не обнаружено сигнала присутствия TEDS узла на однопроводной шине */
            LTR25_ERR_TEDS_1W_NOT_IDLE = -10621, /**< Однопроводная шина не была в незанятом состоянии на момент начала обмена */
            LTR25_ERR_TEDS_1W_UNKNOWN_ERR = -10622, /**< Неизвестная ошибка при обмене по однопроводной шине с узлом TEDS */
            LTR25_ERR_TEDS_MEM_STATUS = -10623, /**< Неверное состояние памяти TEDS узла */

            LTR216_ERR_ADC_ID_CHECK             = -10700, /**< Не удалось обнаружить микросхему АЦП */
            LTR216_ERR_ADC_RECV_SYNC_OVERRATE   = -10701, /**< Частота синхронизации АЦП превысила частоту преобразования */
            LTR216_ERR_ADC_RECV_INT_CYCLE_ERROR = -10702, /**< Ошибка внутреннего цикла чтения данных с АЦП */
            LTR216_ERR_ADC_REGS_INTEGRITY       = -10703, /**< Нарушена целостность регистров АЦП */
            LTR216_ERR_INVALID_ADC_SWMODE       = -10704, /**< Задано неверное значение режима опроса каналов АЦП */
            LTR216_ERR_INVALID_FILTER_TYPE      = -10705, /**< Задано неверное значение типа фильтра АЦП */
            LTR216_ERR_INVALID_ADC_ODR_CODE     = -10706, /**< Задано неверное значение кода, определяющего скорость преобразования АЦП */
            LTR216_ERR_INVALID_SYNC_FDIV        = -10707, /**< Задано неверное значение делителя частоты синхронизации АЦП */
            LTR216_ERR_INVALID_LCH_CNT          = -10708, /**< Задано неверное количество логических каналов */
            LTR216_ERR_INVALID_ISRC_CODE        = -10709, /**< Задан неверный код, определяющий силу тока питания датчиков */
            LTR216_ERR_CH_NOT_FOUND_IN_LTABLE   = -10710, /**< Указанный канал не был найден в логической таблице */
            LTR216_ERR_NO_CH_ENABLED            = -10711, /**< Ни один канал не был разрешен */
            LTR216_ERR_TARE_CHANNELS            = -10712, /**< Не удалось найти действительное значение при тарировке некоторых каналов */
            LTR216_ERR_TOO_MANY_LTABLE_CH       = -10713, /**< Превышено максимальное число каналов в основном цикле опроса АЦП */
            LTR216_ERR_TOO_MANY_LTABLE_BG_CH    = -10714, /**< Превышено максимальное число каналов в фоновом цикле опроса АЦП */
            LTR216_ERR_UNSUF_SW_TIME            = -10715, /**< Полученное время на коммутацию меньше заданного предела */
            LTR216_ERR_BAD_INIT_MEAS_STATUS     = -10716, /**< Измеренное значение начального параметра недействительно */
            LTR216_ERR_INVALID_CH_RANGE         = -10717, /**< Задан неверный код диапазона канала АЦП*/
            LTR216_ERR_INVALID_CH_NUM           = -10718, /**< Задан неверный номер физического канала АЦП */
            LTR216_ERR_UREF_MEAS_REQ            = -10719,  /**< Для выполнения операции требуется измерить действительное значение напряжения \f$U_{ref}\f$ */

            LPW25_ERROR_FD_NOT_SET = -10800, /** Не задана частота дискретизации сигнала */
            LPW25_ERROR_SENS_NOT_SET = -10801, /** Не задан коэффициент передачи преобразователя */
            LPW25_ERROR_PROC_NOT_STARTED = -10802, /** Не запущена обработка данных */
            LPW25_ERROR_TEDS_MANUFACTURER_ID = -10803, /** Неизвестный идентификатор производителя */
            LPW25_ERROR_TEDS_MODEL_ID = -10804, /** Неизвестная модель преобразователя */




            LTEDS_ERROR_INSUF_SIZE = -12000, /**< Недостаточно места в данных TEDS для выполнения операции */
            LTEDS_ERROR_CHECKSUM = -12001, /**< Неверное значение контрольной суммы в данных TEDS */
            LTEDS_ERROR_INVALID_BITSIZE = -12002, /**< Неверно задан битовый размер данных TEDS */
            LTEDS_ERROR_UNSUPPORTED_FORMAT = -12003, /**< Не поддерживается указанный формат данных TEDS */
            LTEDS_ERROR_ENCODE_VALUE = -12004, /**< Неверно указано значение для кодирования в TEDS */
            LTEDS_ERROR_UNKNOWN_SEL_CASE = -12005 /**< Неизвестный вариант выбора ветвления данных TEDS */
        }

        public enum MODULETYPE : ushort
        {
            EMPTY = 0,
            IDENTIFYING = 0xFFFF,
            LTR01 = 0x0101,
            LTR11 = 0x0B0B,
            LTR22 = 0x1616,
            LTR24 = 0x1818,
            LTR25 = 0x1919,
            LTR27 = 0x1B1B,
            LTR34 = 0x2222,
            LTR35 = 0x2323,
            LTR41 = 0x2929,
            LTR42 = 0x2A2A,
            LTR43 = 0x2B2B,
            LTR51 = 0x3333,
            LTR114 = 0x7272,
            LTR210 = 0xD2D2,
            LTR212 = 0xD4D4            
        }

        public enum ServerPriority
        {
            NORMAL_PRIORITY_CLASS = 0x20,
            IDLE_PRIORITY_CLASS = 0x40,
            HIGH_PRIORITY_CLASS = 0x80,
            REALTIME_PRIORITY_CLASS = 0x100,
            BELOW_NORMAL_PRIORITY_CLASS = 0x4000,
            ABOVE_NORMAL_PRIORITY_CLASS = 0x8000
        }

        [Flags]
        public enum OpenInFlags : uint
        {
            REOPEN = 1
        }

        [Flags]
        public enum OpenOutFlags : uint
        {
            REOPEN = 1
        }


        public enum FpgaState : byte 
        {
            NO_POWER       = 0x0, /**< Нет сигнала питания ПЛИС */
            NSTATUS_TOUT   = 0x1, /**< Истекло время ожидания готовности ПЛИС к загрузке */
            CONF_DONE_TOUT = 0x2, /**< Истекло время ожидания завершения загрузки ПЛИС (обычно означает,
                                       что во Flash нет действительной прошивки) */
            LOAD_PROGRESS  = 0x3, /**< Идет загрузка ПЛИС */
            LOAD_DONE      = 0x7, /**< Загрузка ПЛИС завершена, но работа ПЛИС еще не разрешена */
            WORK           = 0xF  /**< Нормальное рабочее состояние ПЛИС */
        }

        public const uint MODULE_MAX = 16;
        public const uint CRATE_MAX = 16;
        public const uint SERIAL_NUMBER_SIZE = 16;


        public const uint SADDR_DEFAULT = 0x7F000001U;
        public const ushort SPORT_DEFAULT = 11111;


        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_Init(ref TLTR ltr); //Инициализация полей структуры

        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_Open(ref TLTR ltr); //Открытие соединения с модулем, крейтом или сервером

        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_OpenEx(ref TLTR ltr, uint timeout);

        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_OpenSvcControl(ref TLTR ltr, uint saddr, ushort sport);

        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_OpenCrate(ref TLTR ltr, uint saddr, ushort sport, int iface, string csn); 
        
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_Close(ref TLTR ltr); //Разрыв соединения с модулем

        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_IsOpened(ref TLTR ltr); //Состояние соединения с модулем

        [DllImport("ltrapi.dll")]
        public static extern int LTR_Recv(ref TLTR ltr, uint[] buf, uint[] tmark, uint size, uint timeout); //Прием данных от модуля

        // Прием данных из крейта с поддержкой расширенных меток времени
        [DllImport("ltrapi.dll")]
        public static extern int LTR_RecvEx(ref TLTR ltr, uint[] data, uint[] tmark, uint size, uint timeout,
                                  UInt64[] unixtime);
        [DllImport("ltrapi.dll")]
        public static extern int LTR_Send(ref TLTR ltr, uint[] buf, uint size, uint timeout); //Передача данных модулю

        [DllImport("ltrapi.dll")]
        public static extern _LTRNative.LTRERROR LTR_SetServerProcessPriority(ref TLTR ltr, [MarshalAs(UnmanagedType.U4)] ServerPriority Priority);

        //
        // для модуля открытого, как CC_CONTROL
        //
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_GetCrates(ref TLTR ltr, byte[,] csn); //Список крейтов подключенных к серверу
        //BYTE[CRATE_MAX][ SERIAL_NUMBER_SIZE]
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_GetCrateModules(ref TLTR ltr, UInt16[] mid); //Список модулей крейта
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_GetCrateModules(ref TLTR ltr, MODULETYPE[] mid); //Список модулей крейта
        //WORD[MODULE_MAX],
        ///
        /// Ethernet Crate 
        /// 
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_GetServerVersion(ref TLTR ltr, out UInt32 ver);
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_SetServerProcessPriority(ref TLTR ltr, uint Priority);
        // Чтение описания крейта (модель, тип интерфейса)
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_GetCrateInfo(ref TLTR ltr, out TCRATE_INFO CrateInfo);
        // Получение сырых данных из крейта (если был установлен CC_RAW_DATA_FLAG)
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_GetCrateRawData(ref TLTR ltr, ref uint data, ref uint tmark, uint size, uint timeout);
        // Задание конфигурации цифровых входов и выходов крейта
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_Config(ref TLTR ltr, ref TLTR_CONFIG conf);
        // Включение внутренней генерации секундных меток
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_StartSecondMark(ref TLTR ltr, en_LTR_MarkMode mode);
        // Отключение внутренней генерации секундных меток
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_StopSecondMark(ref TLTR ltr);
        // Подача метки СТАРТ (однократно)
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_MakeStartMark(ref TLTR ltr, en_LTR_MarkMode mode);
        // Чтение приоритета процесса ltrserver.exe
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_GetServerProcessPriority(ref TLTR ltr, ref uint Priority);
        // -- Функции управления сервером (работают по управляющему каналу,
        //    в т.ч. без крейта через особый серийный номер CSN_SERVER_CONTROL
        // Получение списка известных серверу IP-крейтов (подключенных и не подключенных)
        // Соответствует содержимому окна сервера "Управление IP-крейтами".
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_GetListOfIPCrates(ref TLTR ltr,
        UInt32 max_entries, UInt32 ip_net, UInt32 ip_mask,
        out UInt32 entries_found, out UInt32 entries_returned,
        IntPtr info_array);//  TIPCRATE_ENTRY[] info_array);
        // Добавление IP-адреса крейта в таблицу (с сохранением в файле конфигурации).
        // Если адрес уже существует, возвращает ошибку.
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_AddIPCrate(ref TLTR ltr, uint ip_addr, uint flags, bool permanent);
        // Удаление IP-адреса крейта из таблицы (и из файла конфигурации, если он статический).
        // Если адреса нет, либо крейт занят, возвращает ошибку.
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_DeleteIPCrate(ref TLTR ltr, uint ip_addr, bool permanent);
        // Установление соединения с IP-крейтом (адрес должен быть в таблице)
        // Если адреса нет, возвращает ошибку. Если крейт уже подключен или идет попытка подключения,
        // возвращает LTR_OK.
        // Если вернулось LTR_OK, это значит, что ОТПРАВЛЕНА КОМАНДА установить соединение.
        // Результат можно отследить позже по LTR_GetCrates() или LTR_GetListOfIPCrates()
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_ConnectIPCrate(ref TLTR ltr, uint ip_addr);
        // Завершение соединения с IP-крейтом (адрес должен быть в таблице)
        // Если адреса нет, возвращает ошибку. Если крейт уже отключен, возвращает LTR_OK.
        // Если вернулось LTR_OK, это значит, что ОТПРАВЛЕНА КОМАНДА завершить соединение.
        // Результат можно отследить позже по LTR_GetCrates() или LTR_GetListOfIPCrates()
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_DisconnectIPCrate(ref TLTR ltr, uint ip_addr);
        // Установление соединения со всеми IP-крейтами, имеющими флаг "автоподключение"
        // Если вернулось LTR_OK, это значит, что ОТПРАВЛЕНА КОМАНДА установить соединение.
        // Результат можно отследить позже по LTR_GetCrates() или LTR_GetListOfIPCrates()
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_ConnectAllAutoIPCrates(ref TLTR ltr);
        // Завершение соединения со всеми IP-крейтами
        // Если вернулось LTR_OK, это значит, что ОТПРАВЛЕНА КОМАНДА завершить соединение.
        // Результат можно отследить позже по LTR_GetCrates() или LTR_GetListOfIPCrates()
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_DisconnectAllIPCrates(ref TLTR ltr);
        // Изменение флагов для IP-крейта
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_SetIPCrateFlags(ref TLTR ltr, uint ip_addr, uint new_flags, bool permanent);
        // Чтение режима поиска IP-крейтов в локальной сети
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_GetIPCrateDiscoveryMode(ref TLTR ltr, out bool enabled, out bool autoconnect);
        // Установка режима поиска IP-крейтов в локальной сети
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_SetIPCrateDiscoveryMode(ref TLTR ltr, bool enabled, bool autoconnect, bool permanent);
        // Чтение уровня журнализации
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_GetLogLevel(ref TLTR ltr, out int level);
        // Установка уровня журнализации
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_SetLogLevel(ref TLTR ltr, int level, bool permanent);
        // Рестарт программы ltrserver (с разрывом всех соединений).
        // После успешного выполнения команды связь с сервером теряется.
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_ServerRestart(ref TLTR ltr);
        // Рестарт программы ltrserver (с разрывом всех соединений)
        // После успешного выполнения команды связь с сервером теряется.
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_ServerShutdown(ref TLTR ltr);

        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_ResetModule(ref TLTR ltr, int iface, string serial, int slot, uint flags);



        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_PutSettings(ref TLTR ltr, ref TLTR_SETTINGS settings);

        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_GetLastUnixTimeMark(ref TLTR ltr, out UInt64 unixtime);




        // расшифровка ошибки
        [DllImport("ltrapi.dll", EntryPoint = "LTR_GetErrorString")]
        static extern IntPtr tmp_LTR_GetErrorString(int error);

        public static string LTR_GetErrorString(int error)
        {
            return GetErrorString((LTRERROR)error);
        }

        public static string LTR_GetErrorString(LTRERROR error)
        {
            return GetErrorString(error);
        }
            

        public _LTRNative()
        {
               module = NewLTR;
        }

        public _LTRNative(ref TLTR module)
        {
            this.module = module;
        }   
        
        public TLTR module;

        public static TLTR NewLTR
        {
            get
            {
                TLTR tempLTR = new TLTR();

                LTR_Init(ref tempLTR);

                return tempLTR;
            }
        }

        public static byte[,] SearchCrates()
        {
            LTRERROR ERRORCODE = LTRERROR.OK;
            TLTR LTR = NewLTR;

            byte[,] CrateList = new byte[CRATE_MAX, SERIAL_NUMBER_SIZE];
            byte[,] RealCrateList = null;
            UInt16[] Modules = new UInt16[_LTRNative.MODULE_MAX];

            try
            {
                LTR.cc = (ushort)LTRCC.CONTROL;

                ERRORCODE = LTR_Open(ref LTR);
                if (ERRORCODE != LTRERROR.OK) throw new Exception("Ошибка открытия соединения с сервером " + GetErrorString(ERRORCODE));

                ERRORCODE = LTR_GetCrates(ref LTR, CrateList);
                if (ERRORCODE != LTRERROR.OK) throw new Exception("Ошибка получения списка крейтов "+ GetErrorString(ERRORCODE));

                int RealNumCrates = 0;
                for (int i=0;i<CrateList.GetLength(0);i++)
                {
                    int CrateSum = 0;
                    for (int j = 0; j < CrateList.GetLength(1); j++)
                    {
                        CrateSum += CrateList[i, j];
                    }

                    if (CrateSum > 0) RealNumCrates++;
                }

                RealCrateList = new byte[RealNumCrates, SERIAL_NUMBER_SIZE];
                int RealCrateCount=0;
                for (int i = 0; i < CrateList.GetLength(0); i++)
                {
                    int CrateSum = 0;
                    for (int j = 0; j < CrateList.GetLength(1); j++)
                    {
                        CrateSum += CrateList[i, j];
                    }

                    if (CrateSum > 0)
                    {
                        for (int j = 0; j < CrateList.GetLength(1); j++)
                        {
                            RealCrateList[RealCrateCount, j] += CrateList[i, j];
                        }
                        RealCrateCount++;
                    }
                }
            }
            finally
            {
                LTR_Close(ref LTR);
            }

            return RealCrateList;
        }

        public static string[] SearchCratesStr()
        {            
            byte[,] CrateList = SearchCrates();
            if (CrateList == null) return null;
            string[] ReturnValue = new string[CrateList.GetLength(0)];

            for (int i = 0; i < CrateList.GetLength(0); i++)
            {
                string CrateName = "";
                for (int j = 0; j < CrateList.GetLength(1); j++)
                {
                    if (CrateList[i, j] != 0)
                    {
                        CrateName += (char)CrateList[i, j];
                    }
                }
                ReturnValue[i] = CrateName;
            }

            return ReturnValue;
        }

        public static ushort[] SearchModules(byte[] CrateSerialNumber)
        {
            LTRERROR ERRORCODE = LTRERROR.OK;
            TLTR LTR = NewLTR;
            ushort[] Modules = new ushort[_LTRNative.MODULE_MAX];
            try
            {
                LTR.cc = (ushort)LTRCC.CONTROL;
                for (int i = 0; i < CrateSerialNumber.Length; i++)
                {
                    LTR.csn[i] = CrateSerialNumber[i];
                }

                ERRORCODE = LTR_Open(ref LTR);
                if (ERRORCODE != LTRERROR.OK) throw new Exception("Не открылся LTR "+ GetErrorString(ERRORCODE));

                ERRORCODE = LTR_GetCrateModules(ref LTR, Modules);
                if (ERRORCODE != LTRERROR.OK) throw new Exception("Не считываются модули " + GetErrorString(ERRORCODE));

            }
            finally
            {
                LTR_Close(ref LTR);
            }

            return Modules;
        }

        public ushort[] SearchModules()
        {
            LTRERROR ERRORCODE = LTRERROR.OK;
            ushort[] Modules = new ushort[_LTRNative.MODULE_MAX];

            ERRORCODE = LTR_GetCrateModules(ref module, Modules);
            if (ERRORCODE != LTRERROR.OK) throw new Exception("Не считываются модули " + GetErrorString(ERRORCODE));

            return Modules;
        }

        public LTRERROR Open(byte [] CrateSerial, ushort ModuleSlot)
        {
            if (CrateSerial!=null)
                CrateSerial.CopyTo(module.csn,0);

            module.cc = ModuleSlot;
            return LTR_Open(ref module);        
        }

        
        public LTRERROR Open(uint saddr, ushort sport, string CrateSerial, ushort ModuleSlot)
        {
            char[] arr = CrateSerial.ToCharArray();
            int i;
            for (i = 0; (i < arr.Length) && (i < 15); i++)
                module.csn[i] = (byte)arr[i];
            module.csn[i] = 0;
            module.cc = ModuleSlot;
            module.saddr = saddr;
            module.sport = sport;
            return LTR_Open(ref module);    
        }

        public LTRERROR Open(string CrateSerial, ushort ModuleSlot)
        {
            return Open(_LTRNative.SADDR_DEFAULT, _LTRNative.SPORT_DEFAULT, CrateSerial, ModuleSlot);    
        }

        public LTRERROR Open(string CrateSerial)
        {
            return Open(_LTRNative.SADDR_DEFAULT, _LTRNative.SPORT_DEFAULT, CrateSerial, 0);
        }

        public virtual LTRERROR Close()
        {
            return LTR_Close(ref module);
        }

        // Подготовка данных к передаче в модуль
        public uint PrepareWriteData(bool Command, byte CommandData, ushort Data)
        {
            uint DDDDDDDD = (uint)Data;
            uint NNNNNNNN = (uint)CommandData;
            uint CXXX = Convert.ToUInt32(Command);

            return DDDDDDDD | (CXXX << 24) | (NNNNNNNN << 16);
        }
        
        public int Send(uint []Buff, uint Size, uint Timeout)
        {
            return LTR_Send(ref module, Buff, Size, Timeout);
        }

        public int Recv(uint []Buff, uint Size, uint Timeout)
        {
            return LTR_Recv(ref module, Buff, null, Size, Timeout);
        }

        public int RecvEx(uint[] data, uint[] tmark, uint size, uint timeout, UInt64[] unixtime)
        {
            return LTR_RecvEx(ref module, data, tmark, size, timeout, unixtime);
        }

        public int Recv(uint []Buff, uint [] tmark , uint Size, uint Timeout)
        {
            return LTR_Recv(ref module, Buff, tmark, Size, Timeout);
        }

        public LTRERROR PutSettings(ref TLTR_SETTINGS settings)
        {
            return LTR_PutSettings(ref module, ref settings);
        }

        public LTRERROR GetLastUnixTimeMark(out UInt64 unixtime)
        {
            return LTR_GetLastUnixTimeMark(ref module, out unixtime);
        }

        public static string GetErrorString(_LTRNative.LTRERROR err)
        {
            IntPtr ptr = tmp_LTR_GetErrorString((int)err);
            string str = Marshal.PtrToStringAnsi(ptr);
            Encoding srcEncodingFormat = Encoding.GetEncoding("windows-1251");
            Encoding dstEncodingFormat = Encoding.UTF8;
            return dstEncodingFormat.GetString(Encoding.Convert(srcEncodingFormat, dstEncodingFormat, srcEncodingFormat.GetBytes(str)));
        }



        // преобразование данных из формата сервера в формат модуля
        public uint[] ConvertReadData(uint ReadData)
        {
            uint DDDDDDDD = (ReadData & 0xffff0000) >> 16;
            uint NNNNNNNN = (ReadData & 0xff);
            uint C = (ReadData & 0x8000) >> 15;
            uint M = (ReadData & 0xf00) >> 8;

            //return DDDDDDDD|(CXXX<<24)|(NNNNNNNN<<16);
            return new uint[4] { C, M, NNNNNNNN, DDDDDDDD };
        }

        public uint[] GetReadData(uint[] ReadDataNonFormatted)
        {
            for (int i = 0; i < ReadDataNonFormatted.Length; i++)
            {
                ReadDataNonFormatted[i] = ConvertReadData(ReadDataNonFormatted[i])[3];
            }
            return ReadDataNonFormatted;
        }       

        public bool ResetModule(uint Timeout)
        {
            uint [] buf = new uint[3]{0x8000, 0x8080, 0x8000};
            if (LTR_Send(ref module, buf, (uint)buf.Length, Timeout)!=buf.Length) return false;
            if (LTR_Recv(ref module, buf, null, 1, Timeout)!=1) return false;
            if (((buf[0]>>24)&0xFF)!=((buf[0]>>16)&0xFF)) return false;
            return true;
        }        

        public bool StopModule()
        {
            uint [] buf = new uint[3]{0x8000, 0x8000, 0x8000};
            if (LTR_Send(ref module, buf, (uint)buf.Length, 2000)!=buf.Length) return false;
            return true;
        }   
        
        public bool FlushBuffers(uint MaxTimeout_ms, uint MaxDuration_ms)
        {
            uint [] buf = new uint[65535];
            int res=1;
            DateTime Begin = DateTime.Now;
            while (res > 0)
            {
                res = LTR_Recv(ref module, buf, null, (uint)buf.Length, MaxTimeout_ms);

                if (((TimeSpan)(DateTime.Now - Begin)).TotalMilliseconds > MaxDuration_ms)
                {
                    return false;
                }
            }
            if (res!=0) return false;
            return true;
        }
    }

}
