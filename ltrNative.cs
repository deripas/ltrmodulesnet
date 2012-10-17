using System;
using System.Runtime.InteropServices;

namespace ltrModulesNet
{
    /// <summary>
    /// Summary description for _LTRNative.
    /// </summary>
    public class _LTRNative
    {
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TLTR
        {
            public uint saddr;                      // сетевой адрес сервера				
            public ushort sport;                    // сетевой порт сервера						
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] csn;						// серийный номер крейта				
            public ushort cc;                       // номер канала крейта					
            public uint flags;                      // флаги состояния канала				
            public uint tmark;						// время синхронизации					
            public UIntPtr Internal;                // указатель на канал
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
        
        // описание крейта (для TCRATE_INFO)
        public enum en_CrateType
        {
            CRATE_TYPE_UNKNOWN    =  0,
            CRATE_TYPE_LTR010     =  10,
            CRATE_TYPE_LTR021     =  21,
            CRATE_TYPE_LTR030     =  30,
            CRATE_TYPE_LTR031     =  31,
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
            LTR_DIGOUT_DEFAULT = LTR_DIGOUT_CONST0
        }

        // Значения для управления метками "СТАРТ" и "СЕКУНДА"
        public enum en_LTR_MarkMode
        {
            LTR_MARK_OFF = 0x00, // метка отключена
            LTR_MARK_EXT_DIGIN1_RISE = 0x01, // метка по фронту DIGIN1
            LTR_MARK_EXT_DIGIN1_FALL = 0x02, // метка по спаду DIGIN1
            LTR_MARK_EXT_DIGIN2_RISE = 0x03, // метка по фронту DIGIN2
            LTR_MARK_EXT_DIGIN2_FALL = 0x04, // метка по спаду DIGIN2
            LTR_MARK_INTERNAL = 0x05  // внутренняя генерация метки
        };

        public enum LTRCC
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
            ERROR_NOT_CTRL_CHANNEL =-11,            /* Номер канала для этой операции должен быть CC_CONTROL */

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

            LTRBOOT_ERROR_GET_ARRAY = -300,			/*Ошибка выполнения команды GET_ARRAY.*/
            LTRBOOT_ERROR_PUT_ARRAY = -301,      /*Ошибка выполнения команды PUT_ARRAY.*/
            LTRBOOT_ERROR_CALL_APPL = -302,      /*Ошибка выполнения команды CALL_APPLICATION.*/
            LTRBOOT_ERROR_GET_DESCRIPTION = -303,      /*Ошибка выполнения команды GET_DESCRIPTION.*/
            LTRBOOT_ERROR_PUT_DESCRIPTION = -304,     /*Ошибка выполнения команды PUT_DESCRIPTION.*/

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
            LTR114_ERR_INVALID_LCH         = -10018, /*неверный режим лог. канала*/
            LTR114_ERR_CORRECTION_MODE     = -10019 /*неверный режим коррекции данных*/
        }

        public enum MODULETYPE
        {
            EMPTY = 0,
            LTR11 = 0x0B0B,
            LTR22 = 0x1616,
            LTR27 = 0x1B1B,
            LTR34 = 0x2222,
            LTR41 = 0x2929,
            LTR42 = 0x2A2A,
            LTR43 = 0x2B2B,
            LTR51 = 0x3333,
            LTR212 = 0xD4D4,
            LTR114 = 0x7272
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


        public const uint MODULE_MAX = 16;
        public const uint CRATE_MAX = 16;
        public const uint SERIAL_NUMBER_SIZE = 16;


        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_Init(ref TLTR ltr); //Инициализация полей структуры

        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_Open(ref TLTR ltr); //Открытие соединения с модуле

        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_Close(ref TLTR ltr); //Разрыв соединения с модулем

        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_IsOpened(ref TLTR ltr); //Состояние соединения с модулем

		[DllImport("ltrapi.dll")]
		public static extern int LTR_Recv(ref TLTR ltr, uint[] buf, uint [] tmark, uint size, uint timeout); //Прием данных от модуля		

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




        // расшифровка ошибки
        [DllImport("ltrapi.dll")]
        public static extern string LTR_GetErrorString(int error);

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
                if (ERRORCODE != LTRERROR.OK) throw new Exception("Ошибка открытия соединения с сервером " + LTR_GetErrorString((int)ERRORCODE));

                ERRORCODE = LTR_GetCrates(ref LTR, CrateList);
                if (ERRORCODE != LTRERROR.OK) throw new Exception("Ошибка получения списка крейтов "+ LTR_GetErrorString((int)ERRORCODE));

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
                if (ERRORCODE != LTRERROR.OK) throw new Exception("Не открылся LTR "+ LTR_GetErrorString((int)ERRORCODE));

                ERRORCODE = LTR_GetCrateModules(ref LTR, Modules);
                if (ERRORCODE != LTRERROR.OK) throw new Exception("Не считываются модули " + LTR_GetErrorString((int)ERRORCODE));

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
            if (ERRORCODE != LTRERROR.OK) throw new Exception("Не считываются модули " + LTR_GetErrorString((int)ERRORCODE));			

			return Modules;
		}

        public LTRERROR Open(byte [] CrateSerial, ushort ModuleSlot)
        {
			if (CrateSerial!=null) CrateSerial.CopyTo(module.csn,0);
			module.cc = ModuleSlot;

			return LTR_Open(ref module);        
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

		public int Recv(uint []Buff, uint [] tmark , uint Size, uint Timeout)
		{
			return LTR_Recv(ref module, Buff, tmark, Size, Timeout);
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
