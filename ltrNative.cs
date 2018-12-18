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
            public uint saddr;                      // ������� ����� �������
            public ushort sport;                    // ������� ���� �������
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] csn;                        // �������� ����� ������
            public ushort cc;                       // ����� ������ ������
            public uint flags;                      // ����� ��������� ������
            public uint tmark;                        // ����� �������������
            public UIntPtr Internal;                // ��������� �� �����


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
        // ���������� � ���� ������
        public struct TCRATE_INFO
        {
            public byte CrateType;                                           // ��� ������
            public byte CrateInterface;                                      // ��� ����������� ������
        }
        // ������� ������ IP-�������
        public struct TIPCRATE_ENTRY
        {
            public uint ip_addr;                                          // IP ����� (host-endian)
            public uint flags;                                            // ����� ������� (CRATE_IP_FLAG_...)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] serial_number;                                 // �������� ����� (���� ����� ���������)
            public byte is_dynamic;                                        // 0 = ����� �������������, 1 = ������ �������������
            public byte status;                                            // ��������� (CRATE_IP_STATUS_...)
        }
        public struct TLTR_CONFIG
        {
            // ��������� ����� ����������
            // [0] PF1 � ���. 0, PF1 � ���. 1+
            // [1] PG13
            // [2] PF3, ������ ���. 1+, ������ ����
            // [3] ������
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt16[] userio;          // ���� �� �������� LTR_USERIO_...
            // ��������� ������� DIGOUTx
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public UInt16[] digout;          // ������������ ������� (LTR_DIGOUT_...)
            public UInt16 digout_en;          // ���������� ������� DIGOUT1, DIGOUT2
        }


        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TDESCRIPTION_MODULE                                //
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            char[] CompanyName_;                                  //
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            char[] DeviceName_;                                   // �������� �������
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            char[] SerialNumber_;                                 // �������� ����� �������
            byte Revision_;                                       // ������� �������
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = COMMENT_LENGTH)]
            char[] Comment_;

            public string CompanyName { get { return new string(CompanyName_).TrimEnd('\0'); } }
            public string DeviceName { get { return new string(DeviceName_).TrimEnd('\0'); } }
            public string SerialNumber { get { return new string(SerialNumber_).TrimEnd('\0'); } }
            public string Comment { get { return new string(Comment_).TrimEnd('\0'); } }
            public byte Revision { get { return Revision_; } }
        };
        // �������� ���������� � ����������� �����������
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TDESCRIPTION_CPU                                    //
        {                                                                //
            byte Active_;                                         // ���� ������������� ��������� ����� ���������
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            char[] Name_;                                         // ��������            
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
        // �������� ����
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TDESCRIPTION_FPGA
        {                                                                //
            byte Active_;                                         // ���� ������������� ��������� ����� ���������
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            char[] Name_;                                         // ��������
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
            byte Active_;                                         // ���� ������������� ��������� ����� ���������
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            char[] Name_;                                         // ��������
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = COMMENT_LENGTH)]
            char[] Comment_;

            public string Name { get { return new string(Name_).TrimEnd('\0'); } }
            public string Comment { get { return new string(Comment_).TrimEnd('\0'); } }
            public bool Active { get { return Active_!=0; } }
        };

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TDESCRIPTION_MEZZANINE                            //
        {                                                                //
            byte Active_;                                         // ���� ������������� ��������� ����� ���������
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            char[] Name_;                                         // �������� �������
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            char[] SerialNumber_;                                 // �������� ����� �������            
            byte Revision_;                                       // ������� �������
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            double[] Calibration_;                                       // ���������������� ������������
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
        
        // �������� ������ (��� TCRATE_INFO)
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
        // ��������� ������ (��� TCRATE_INFO)
        public enum en_CrateInterface
        {
            CRATE_IFACE_UNKNOWN = 0,
            CRATE_IFACE_USB     = 1,
            CRATE_IFACE_TCPIP   = 2
        }

        // �������� ��� ���������� ������� ����������, ���������� ��� ����������������� ����������������
        public enum en_LTR_UserIoCfg
        {
            LTR_USERIO_DIGIN1  = 1,    // ����� �������� ������ � ���������� � DIGIN1
            LTR_USERIO_DIGIN2  = 2,    // ����� �������� ������ � ���������� � DIGIN2
            LTR_USERIO_DIGOUT  = 0,    // ����� �������� ������� (����������� ��. en_LTR_DigOutCfg)
            LTR_USERIO_DEFAULT = LTR_USERIO_DIGOUT
        }
        // �������� ��� ���������� �������� DIGOUTx
        public enum en_LTR_DigOutCfg
        {
            LTR_DIGOUT_CONST0  = 0x00, // ���������� ������� ����������� "0"
            LTR_DIGOUT_CONST1  = 0x01, // ���������� ������� ���������� "1"
            LTR_DIGOUT_USERIO0 = 0x02, // ����� ��������� � ����� userio0 (PF1 � ���. 0, PF1 � ���. 1)
            LTR_DIGOUT_USERIO1 = 0x03, // ����� ��������� � ����� userio1 (PG13)
            LTR_DIGOUT_DIGIN1  = 0x04, // ����� ��������� �� ����� DIGIN1
            LTR_DIGOUT_DIGIN2  = 0x05, // ����� ��������� �� ����� DIGIN2
            LTR_DIGOUT_START   = 0x06, // �� ����� �������� ����� "�����"
            LTR_DIGOUT_SECOND  = 0x07, // �� ����� �������� ����� "�������"
            LTR_DIGOUT_IRIG = 0x08, // �������� �������� ������� ������� IRIG (digout1: ����������, digout2: �������)
            LTR_DIGOUT_DEFAULT = LTR_DIGOUT_CONST0
        }

        // �������� ��� ���������� ������� "�����" � "�������"
        public enum en_LTR_MarkMode : int
        {
            LTR_MARK_OFF = 0x00, // ����� ���������
            LTR_MARK_EXT_DIGIN1_RISE = 0x01, // ����� �� ������ DIGIN1
            LTR_MARK_EXT_DIGIN1_FALL = 0x02, // ����� �� ����� DIGIN1
            LTR_MARK_EXT_DIGIN2_RISE = 0x03, // ����� �� ������ DIGIN2
            LTR_MARK_EXT_DIGIN2_FALL = 0x04, // ����� �� ����� DIGIN2
            LTR_MARK_INTERNAL        = 0x05, // ���������� ��������� �����
            // ������ ����� LTR-E-7/15
            LTR_MARK_NAMUR1_LO2HI = 0x08, // �� ������� NAMUR1, ����������� ����
            LTR_MARK_NAMUR1_HI2LO = 0x09, // �� ������� NAMUR1, ���� ����
            LTR_MARK_NAMUR2_LO2HI = 0x0A, // �� ������� NAMUR2, ����������� ����
            LTR_MARK_NAMUR2_HI2LO = 0x0B, // �� ������� NAMUR2, ���� ����

            /* �������� ����� - ������� �������� ������� ������� IRIG-B006
                IRIG ����� �������������� ������ ��� ����� "�������", ��� "�����" ������������ */
            LTR_MARK_SEC_IRIGB_DIGIN1 = 16,   // �� ����� DIGIN1, ������ ������
            LTR_MARK_SEC_IRIGB_nDIGIN1 = 17,   // �� ����� DIGIN1, ��������������� ������
            LTR_MARK_SEC_IRIGB_DIGIN2 = 18,   // �� ����� DIGIN2, ������ ������
            LTR_MARK_SEC_IRIGB_nDIGIN2 = 19,   // �� ����� DIGIN2, ��������������� ������
        };

        public enum LTRCC : ushort
        {
            CONTROL = 0,            // ����� ��� ������ c ������� � ����� 0, �� ��
            MODULE0 = 0,           // ����� ��� ���������� ������� � ��������
            MODULE1,              // ����� ��� ������ c ������� � ����� 1
            MODULE2,              // ����� ��� ������ c ������� � ����� 2
            MODULE3,              // ����� ��� ������ c ������� � ����� 3
            MODULE4,              // ����� ��� ������ c ������� � ����� 4
            MODULE5,              // ����� ��� ������ c ������� � ����� 5
            MODULE6,              // ����� ��� ������ c ������� � ����� 6
            MODULE7,              // ����� ��� ������ c ������� � ����� 7
            MODULE8,              // ����� ��� ������ c ������� � ����� 8
            MODULE9,              // ����� ��� ������ c ������� � ����� 9
            MODULE10,             // ����� ��� ������ c ������� � ����� 10
            MODULE11,             // ����� ��� ������ c ������� � ����� 11
            MODULE12,             // ����� ��� ������ c ������� � ����� 12
            MODULE13,             // ����� ��� ������ c ������� � ����� 13
            MODULE14,             // ����� ��� ������ c ������� � ����� 14
            MODULE15,             // ����� ��� ������ c ������� � ����� 15
            MODULE16,             // ����� ��� ������ c ������� � ����� 16
            DEBUG_FLAG = 0x8000   // ���� ������� - ������ ���������� � ����� �
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
            OK = 0,                                 /*��������� ��� ������.*/
            ERROR_UNKNOWN = -1,                     /*����������� ������.*/
            ERROR_PARAMETRS = -2,                   /*������ ������� ����������.*/
            ERROR_MEMORY_ALLOC = -3,                /*������ ������������� ��������� ������.*/
            ERROR_OPEN_CHANNEL = -4,                /*������ �������� ������ ������ � ��������.*/
            ERROR_OPEN_SOCKET = -5,                 /*������ �������� ������.*/
            ERROR_CHANNEL_CLOSED = -6,              /*������. ����� ������ � �������� �� ������.*/
            ERROR_SEND = -7,                        /*������ ����������� ������.*/
            ERROR_RECV = -8,                        /*������ ������ ������.*/
            ERROR_EXECUTE = -9,                     /*������ ������ � �����-������������.*/
            WARNING_MODULE_IN_USE = -10,            /* ����� ������ � �������� ��� ��� ������ � �� ������*/
            ERROR_NOT_CTRL_CHANNEL = -11,           /* ����� ������ ��� ���� �������� ������ ���� CC_CONTROL */
            ERROR_SRV_INVALID_CMD = -12,            /* ������� �� �������������� �������� */
            ERROR_SRV_INVALID_CMD_PARAMS = -13,     /* ������ �� ������������ ��������� ��������� ������� */
            ERROR_INVALID_CRATE = -14,              /* ��������� ����� �� ������ */
            ERROR_EMPTY_SLOT = -15,                 /* � ��������� ����� ����������� ������ */
            ERROR_UNSUP_CMD_FOR_SRV_CTL = -16,      /* ������� �� �������������� ����������� ������� ������� */
            ERROR_INVALID_IP_ENTRY = -17,           /* �������� ������ �������� ������ ������ */
            ERROR_NOT_IMPLEMENTED = -18,            /* ������ ����������� �� ����������� */
            ERROR_CONNECTION_CLOSED = -19,          /* ���������� ���� ������� �������� */
            ERROR_LTRD_UNKNOWN_RETCODE = -20,       /* ����������� ��� ������ ������ ltrd */
            ERROR_LTRD_CMD_FAILED = -21,            /* ������ ���������� ����������� ������� ltrd */
            ERROR_INVALID_CON_SLOT_NUM = -22,       /* ������ �������� ����� ����� ��� �������� ���������� */

            ERROR_INVALID_MODULE_DESCR   = -40,    /* �������� ��������� ������ */
            ERROR_INVALID_MODULE_SLOT    = -41,    /* ������ �������� ���� ��� ������ */
            ERROR_INVALID_MODULE_ID      = -42,    /* �������� ID-������ � ������ �� ����� */
            ERROR_NO_RESET_RESPONSE      = -43,    /* ��� ������ �� ������� ������ */
            ERROR_SEND_INSUFFICIENT_DATA = -44,    /* �������� ������ ������, ��� ������������� */
            ERROR_RECV_INSUFFICIENT_DATA = -45,    /* ������� ������ ������, ��� ������������� */
            ERROR_NO_CMD_RESPONSE        = -46,    /* ��� ������ �� ������� */
            ERROR_INVALID_CMD_RESPONSE   = -47,    /* ������ �������� ����� �� ������� */
            ERROR_INVALID_RESP_PARITY    = -48,    /* �������� ��� �������� � ��������� ������ */
            ERROR_INVALID_CMD_PARITY     = -49,    /* ������ �������� ���������� ������� */
            ERROR_UNSUP_BY_FIRM_VER      = -50,    /* ����������� �� �������������� ������ ������� �������� */
            ERROR_MODULE_STARTED         = -51,    /* ������ ��� ������� */
            ERROR_MODULE_STOPPED         = -52,    /* ������ ���������� */
            ERROR_RECV_OVERFLOW          = -53,    /* ��������� ������������ ������ */
            ERROR_FIRM_FILE_OPEN         = -54,    /* ������ �������� ����� �������� */
            ERROR_FIRM_FILE_READ         = -55,    /* ������ ������ ����� �������� */
            ERROR_FIRM_FILE_FORMAT       = -56,    /* ������ ������� ����� �������� */
            ERROR_FPGA_LOAD_READY_TOUT   = -57,    /* �������� ������� �������� ���������� ���� � �������� */
            ERROR_FPGA_LOAD_DONE_TOUT    = -58,    /* �������� ������� �������� �������� ���� � ������� ����� */
            ERROR_FPGA_IS_NOT_LOADED     = -59,    /* �������� ���� �� ��������� */
            ERROR_FLASH_INVALID_ADDR     = -60,    /* �������� ����� Flash-������ */
            ERROR_FLASH_WAIT_RDY_TOUT    = -61,    /* �������� ������� �������� ���������� ������/�������� Flash-������ */
            ERROR_FIRSTFRAME_NOTFOUND    = -62,    /* First frame in card data stream not found */
            ERROR_CARDSCONFIG_UNSUPPORTED = -63,
            ERROR_FLASH_OP_FAILED         = -64,    /* ������ ��������� �������� flash-������� */
            ERROR_FLASH_NOT_PRESENT       = -65,    /* Flash-������ �� ���������� */
            ERROR_FLASH_UNSUPPORTED_ID    = -66,    /* ��������� ���������������� ��� flash-������ */
            ERROR_FLASH_UNALIGNED_ADDR    = -67,    /* ������������� ����� flash-������ */
            ERROR_FLASH_VERIFY            = -68,    /* ������ ��� �������� ���������� ������ �� flash-������ */
            ERROR_FLASH_UNSUP_PAGE_SIZE   = -69,    /* ���������� ���������������� ������ �������� flash-������ */
            ERROR_FLASH_INFO_NOT_PRESENT  = -70,    /* ����������� ���������� � ������ �� Flash-������ */
            ERROR_FLASH_INFO_UNSUP_FORMAT = -71,    /* ���������������� ������ ���������� � ������ �� Flash-������ */
            ERROR_FLASH_SET_PROTECTION    = -72,    /* �� ������� ���������� ������ Flash-������ */
            ERROR_FPGA_NO_POWER           = -73,    /* ��� ������� ���������� ���� */
            ERROR_FPGA_INVALID_STATE      = -74,    /* �� �������������� ��������� �������� ���� */
            ERROR_FPGA_ENABLE             = -75,    /* �� ������� ��������� ���� � ����������� ��������� */
            ERROR_FPGA_AUTOLOAD_TOUT      = -76,    /* ������� ����� �������� �������������� �������� ���� */
            ERROR_PROCDATA_UNALIGNED      = -77,    /* �������������� ������ �� ��������� �� ������� ����� */
            ERROR_PROCDATA_CNTR           = -78,    /* ������ �������� � �������������� ������ */
            ERROR_PROCDATA_CHNUM          = -79,    /* �������� ����� ������ � �������������� ������ */
            ERROR_PROCDATA_WORD_SEQ       = -80,    /* �������� ������������������ ���� � �������������� ������ */
            ERROR_FLASH_INFO_CRC          = -81,    /* �������� ����������� ����� � ���������� ���������� � ������ */
            ERROR_PROCDATA_UNEXP_CMD      = -82,  /** ���������� ����������� ������� � ������ ������ */
            ERROR_UNSUP_BY_BOARD_REV      = -83,  /** ����������� �� �������������� ������ �������� ����� */
            ERROR_MODULE_NOT_CONFIGURED   = -84,  /** �� ��������� ������������ ������ */

            LTR010_ERROR_GET_ARRAY        = -100, /*������ ���������� ������� GET_ARRAY.*/
            LTR010_ERROR_PUT_ARRAY        = -101, /*������ ���������� ������� PUT_ARRAY.*/
            LTR010_ERROR_GET_MODULE_NAME  = -102, /*������ ���������� ������� GET_MODULE_NAME.*/
            LTR010_ERROR_GET_MODULE_DESCR = -103, /*������ ���������� ������� GET_MODULE_DESCRIPTOR.*/
            LTR010_ERROR_INIT_FPGA        = -104, /*������ ���������� ������� INIT_FPGA.*/
            LTR010_ERROR_RESET_FPGA       = -105, /*������ ���������� ������� RESET_FPGA.*/
            LTR010_ERROR_INIT_DMAC        = -106, /*������ ���������� ������� INIT_DMAC.*/
            LTR_ERROR_LOAD_FPGA           = -107, /*������ ���������� ������� LOAD_FPGA.*/
            LTR010_ERROR_OPEN_FILE        = -108, /*������ �������� �����.*/
            LTR010_ERROR_GET_INFO_FPGA    = -109, /*������ ���������� ������� GET_INFO_FPGA.*/

            LTR021_ERROR_GET_ARRAY        = -200, /*������ ���������� ������� GET_ARRAY.*/
            LTR021_ERROR_PUT_ARRAY        = -201, /*������ ���������� ������� PUT_ARRAY.*/
            LTR021_ERROR_GET_MODULE_NAME  = -202, /*������ ���������� ������� GET_MODULE_NAME.*/
            LTR021_ERROR_GET_MODULE_GESCR = -203, /*������ ���������� ������� GET_MODULE_DESCRIPTOR.*/
            LTR021_ERROR_CRATE_TYPE       = -204, /*�������� ��� ������.*/
            LTR021_ERROR_TIMEOUT          = -205, /*���������� �������� */

            LTRAVR_ERROR_RECV_PRG_DATA_ECHO = -200,     /*������ ������ ��� ������ ������ ��� ����������������.*/
            LTRAVR_ERROR_SEND_PRG_DATA = -201,     /*������ �������� ������ ������� ��������������� avr.*/
            LTRAVR_ERROR_RECV_PRG_ENABLE_ACK = -202,     /*������ ������ ������������� ������� ����� � ����� ����������������.*/
            LTRAVR_ERROR_SEND_PRG_ENB_CMD = -203,     /*������ �������� ������� ����� � ����� ����������������.*/
            LTRAVR_ERROR_CHIP_ERASE = -204,     /*������ �������� flash ������ avr.*/
            LTRAVR_ERROR_READ_PRG_MEM = -205,     /*������ ���������� flash ������ avr.*/
            LTRAVR_ERROR_WRITE_PRG_MEM = -206,     /*������ ���������������� flash ������ avr.*/
            LTRAVR_ERROR_READ_FUSE_BITS = -207,     /*������ ���������� fuse ����� avr.*/
            LTRAVR_ERROR_WRITE_FUSE_BITS = -208,     /*������ ���������������� fuse ����� avr.*/
            LTRAVR_ERROR_READ_SIGN = -209,     /*������ ���������� ��������� avr.*/

            LTRBOOT_ERROR_GET_ARRAY = -300,            /*������ ���������� ������� GET_ARRAY.*/
            LTRBOOT_ERROR_PUT_ARRAY = -301,      /*������ ���������� ������� PUT_ARRAY.*/
            LTRBOOT_ERROR_CALL_APPL = -302,      /*������ ���������� ������� CALL_APPLICATION.*/
            LTRBOOT_ERROR_GET_DESCRIPTION = -303,      /*������ ���������� ������� GET_DESCRIPTION.*/
            LTRBOOT_ERROR_PUT_DESCRIPTION = -304,     /*������ ���������� ������� PUT_DESCRIPTION.*/


            LTR030_ERR_UNSUPORTED_CRATE_TYPE = -400, /* ������ ��� ������ �� �������������� ����������� */
            LTR030_ERR_FIRM_VERIFY           = -401, /* ������ �������� ������������ ������ �������� */
            LTR030_ERR_FIRM_SIZE             = -402,  /* �������� ������ �������� */

            // LTR11
            /* ���� ������, ������������ ��������� ���������� */
            LTR11_ERR_INVALID_DESCR = -1000, /* ��������� �� ��������� ������ ����� NULL */
            LTR11_ERR_INVALID_ADCMODE = -1001, /* ������������ ����� ������� ��� */
            LTR11_ERR_INVALID_ADCLCHQNT = -1002, /* ������������ ���������� ���������� ������� */
            LTR11_ERR_INVALID_ADCRATE = -1003, /* ������������ �������� ������� ������������� ��� ������*/
            LTR11_ERR_INVALID_ADCSTROBE = -1004, /* ������������ �������� �������� ������� ��� ��� */
            LTR11_ERR_GETFRAME = -1005, /* ������ ��������� ����� ������ � ��� */
            LTR11_ERR_GETCFG = -1006, /* ������ ������ ������������ */
            LTR11_ERR_CFGDATA = -1007, /* ������ ��� ��������� ������������ ������ */
            LTR11_ERR_CFGSIGNATURE = -1008, /* �������� �������� ������� ����� ���������������� ������ ������*/
            LTR11_ERR_CFGCRC = -1009, /* �������� ����������� ����� ���������������� ������*/
            LTR11_ERR_INVALID_ARRPOINTER = -1010, /* ��������� �� ������ ����� NULL */
            LTR11_ERR_ADCDATA_CHNUM = -1011, /* �������� ����� ������ � ������� ������ �� ��� */
            LTR11_ERR_INVALID_CRATESN = -1012, /* ��������� �� ������ � �������� ������� ������ ����� NULL*/
            LTR11_ERR_INVALID_SLOTNUM = -1013, /* ������������ ����� ����� � ������ */
            LTR11_ERR_NOACK = -1014, /* ��� ������������� �� ������ */
            LTR11_ERR_MODULEID = -1015, /* ������� �������� ������, ��������� �� LTR11 */
            LTR11_ERR_INVALIDACK = -1016, /* �������� ������������� �� ������ */
            LTR11_ERR_ADCDATA_SLOTNUM = -1017, /* �������� ����� ����� � ������ �� ��� */
            LTR11_ERR_ADCDATA_CNT = -1018, /* �������� ������� ������� � ������ �� ��� */
            LTR11_ERR_INVALID_STARTADCMODE = -1019, /* �������� ����� ������ ����� ������ */

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



            LTR114_ERR_INVALID_DESCR       = -10000, /* ��������� �� ��������� ������ ����� NULL */
            LTR114_ERR_INVALID_SYNCMODE    = -10001, /* ������������ ����� ������������� ������ ��� */
            LTR114_ERR_INVALID_ADCLCHQNT   = -10002, /* ������������ ���������� ���������� ������� */
            LTR114_ERR_INVALID_ADCRATE     = -10003, /* ������������ �������� ������� ������������� ���
                                                      * ������
                                                      */
            LTR114_ERR_GETFRAME            = -10004, /* ������ ��������� ����� ������ � ��� */
            LTR114_ERR_GETCFG              = -10005, /* ������ ������ ������������ */
            LTR114_ERR_CFGDATA             = -10006, /* ������ ��� ��������� ������������ ������ */
            LTR114_ERR_CFGSIGNATURE        = -10007, /* �������� �������� ������� ����� ����������������
                                                      * ������ ������
                                                      */
            LTR114_ERR_CFGCRC              = -10008, /* �������� ����������� ����� ����������������
                                                      * ������
                                                      */
            LTR114_ERR_INVALID_ARRPOINTER  = -10009, /* ��������� �� ������ ����� NULL */
            LTR114_ERR_ADCDATA_CHNUM       = -10010, /* �������� ����� ������ � ������� ������ �� ��� */
            LTR114_ERR_INVALID_CRATESN     = -10011, /* ��������� �� ������ � �������� ������� ������
                                                      * ����� NULL
                                                      */
            LTR114_ERR_INVALID_SLOTNUM     = -10012, /* ������������ ����� ����� � ������ */
            LTR114_ERR_NOACK               = -10013, /* ��� ������������� �� ������ */
            LTR114_ERR_MODULEID            = -10014, /* ������� �������� ������, ��������� �� LTR114 */
            LTR114_ERR_INVALIDACK          = -10015, /* �������� ������������� �� ������ */
            LTR114_ERR_ADCDATA_SLOTNUM     = -10016, /* �������� ����� ����� � ������ �� ��� */
            LTR114_ERR_ADCDATA_CNT         = -10017, /* �������� ������� ������� � ������ �� ��� */
            LTR114_ERR_INVALID_LCH         = -10018, /* �������� ����� ���. ������*/
            LTR114_ERR_CORRECTION_MODE     = -10019, /* �������� ����� ��������� ������*/


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
            LTRT10_ERR_INVALID_SWITCH_POS = -10400, /**< ����� �������� ��� ��������� �����������*/
            LTRT10_ERR_INVALID_DDS_DIV = -10401, /**< ����� �������� ��� ������������ �������� ��������� �������� ������� DDS */
            LTRT10_ERR_INVALID_DDS_GAIN = -10402, /**< ����� �������� ��� �������� ��� DDS */
            LTRT10_ERR_INVALID_FREQ_VAL = -10403, /**< ������� ����� ��� ������� ������� DDS */
            LTRT10_ERR_INVALID_DDS_AMP = -10404, /**< ����� �������� ��� ��������� ������� DDS */
            LTRT10_ERR_GAIN2_EXCEED_GAIN1 = -10405, /**< ����. �������� ������ ������� ��������� ����. ������ ������� */

            LTR210_ERR_INVALID_SYNC_MODE            = -10500, /**< ����� �������� ��� ������� ����� �����*/
            LTR210_ERR_INVALID_GROUP_MODE           = -10501, /**< ����� �������� ��� ������ ������ ������ � ������� ������ */
            LTR210_ERR_INVALID_ADC_FREQ_DIV         = -10502, /**< ������ �������� �������� �������� ������� ���*/
            LTR210_ERR_INVALID_CH_RANGE             = -10503, /**< ����� �������� ��� ��������� ������ ���*/
            LTR210_ERR_INVALID_CH_MODE              = -10504, /**< ����� �������� ����� ��������� ������*/
            LTR210_ERR_SYNC_LEVEL_EXCEED_RANGE      = -10505, /**< ������������� ������� ���������� �������������
                                                                ������� �� ������� �������������� ���������*/
            LTR210_ERR_NO_ENABLED_CHANNEL           = -10506, /**< �� ���� ����� ��� �� ��� ��������*/
            LTR210_ERR_PLL_NOT_LOCKED               = -10507, /**< ������ ������� PLL*/
            LTR210_ERR_INVALID_RECV_DATA_CNTR       = -10508, /**< �������� �������� �������� � �������� ������*/
            LTR210_ERR_RECV_UNEXPECTED_CMD          = -10509, /**< ����� ���������������� ������� � ������ ������*/
            LTR210_ERR_FLASH_INFO_SIGN              = -10510, /**< �������� ������� ���������� � ������ �� Flash-������*/
            LTR210_ERR_FLASH_INFO_SIZE              = -10511, /**< �������� ������ ����������� �� Flash-������ ���������� � ������*/
            LTR210_ERR_FLASH_INFO_UNSUP_FORMAT      = -10512, /**< ���������������� ������ ���������� � ������ �� Flash-������*/
            LTR210_ERR_FLASH_INFO_CRC               = -10513, /**< ������ �������� CRC ���������� � ������ �� Flash-������*/
            LTR210_ERR_FLASH_INFO_VERIFY            = -10514, /**< ������ �������� ������ ���������� � ������ �� Flash-������*/
            LTR210_ERR_CHANGE_PAR_ON_THE_FLY        = -10515, /**< ����� ���������� ���������� ������ �������� �� ���� */
            LTR210_ERR_INVALID_ADC_DCM_CNT          = -10516, /**< ����� �������� ����������� ������������ ������ ��� */
            LTR210_ERR_MODE_UNSUP_ADC_FREQ          = -10517, /**< ������������� ����� �� ������������ �������� ������� ��� */
            LTR210_ERR_INVALID_FRAME_SIZE           = -10518, /**< ������� ����� ������ ����� */
            LTR210_ERR_INVALID_HIST_SIZE            = -10519, /**< ������� ����� ������ ����������� */
            LTR210_ERR_INVALID_INTF_TRANSF_RATE     = -10520, /**< ������� ������ �������� �������� ������ ������ � ��������� */
            LTR210_ERR_INVALID_DIG_BIT_MODE         = -10521, /**< ������� ����� ����� ������ ��������������� ���� */
            LTR210_ERR_SYNC_LEVEL_LOW_EXCEED_HIGH   = -10522, /**< ������ ����� ���������� ������������� ��������� ������� */
            LTR210_ERR_KEEPALIVE_TOUT_EXCEEDED      = -10523, /**< �� ������ �� ������ ������� �� ������ �� �������� �������� */
            LTR210_ERR_WAIT_FRAME_TIMEOUT           = -10524, /**< �� ������� ��������� ������� ����� �� �������� ����� */
            LTR210_ERR_FRAME_STATUS                 = -10525, /**< ����� ������� � �������� ����� ��������� �� ������ ������ */


            LTR25_ERR_FPGA_FIRM_TEMP_RANGE      = -10600, /**< ��������� �������� ���� ��� ��������� �������������� ��������� */
            LTR25_ERR_I2C_ACK_STATUS            = -10601, /**< ������ ������ ��� ��������� � ��������� ��� �� ���������� I2C */
            LTR25_ERR_I2C_INVALID_RESP          = -10602, /**< �������� ����� �� ������� ��� ��������� � ��������� ��� �� ���������� I2C */
            LTR25_ERR_INVALID_FREQ_CODE         = -10603, /**< ������� ����� ��� ������� ��� */
            LTR25_ERR_INVALID_DATA_FORMAT       = -10604, /**< ������� ����� ������ ������ ��� */
            LTR25_ERR_INVALID_I_SRC_VALUE       = -10605, /**< ������� ������ �������� ��������� ����" */
            LTR25_ERR_CFG_UNSUP_CH_CNT          = -10606, /**< ��� �������� ������� � ������� �� �������������� �������� ���������� ������� ��� */
            LTR25_ERR_NO_ENABLED_CH             = -10607, /**< �� ��� �������� �� ���� ����� ��� */
            LTR25_ERR_ADC_PLL_NOT_LOCKED        = -10608, /**< ������ ������� PLL ��� */
            LTR25_ERR_ADC_REG_CHECK             = -10609, /**< ������ �������� �������� ���������� ��������� ��� */
            LTR25_ERR_LOW_POW_MODE_NOT_CHANGED  = -10610, /**< �� ������� ��������� ��� ��/� ����������������� ��������� */
            LTR25_ERR_LOW_POW_MODE              = -10611, /**< ������ ��������� � ����������������� ������ */
            LTR25_ERR_INVALID_SENSOR_POWER_MODE = -10612, /**< �������� �������� ������ ������� �������� */
            LTR25_ERR_CHANGE_SENSOR_POWER_MODE = -10613, /**< �� ������� �������� ����� ������� �������� */
            LTR25_ERR_INVALID_CHANNEL_NUMBER = -10614, /**< ������ �������� ����� ������ ������ */
            LTR25_ERR_ICP_MODE_REQUIRED = -10615, /**< ������ �� ��������� � ICP-����� ������� ��������, ����������� ��� ������ ��������  */
            LTR25_ERR_TEDS_MODE_REQUIRED = -10616, /**< ������ �� ��������� � TEDS ����� ������� ��������, ����������� ��� ������ �������� */
            LTR25_ERR_TEDS_UNSUP_NODE_FAMILY = -10617, /**< ������ ��������� ��������� TEDS ���� �� �������������� ����������� */
            LTR25_ERR_TEDS_UNSUP_NODE_OP = -10618, /**< ������ �������� �� �������������� ����������� ��� ������������ ���� ���� TEDS */
            LTR25_ERR_TEDS_NODE_URN_CRC = -10624, /**< �������� �������� ����������� ����� � URN ���� TEDS */
            LTR25_ERR_TEDS_DATA_CRC = -10619, /**< �������� �������� ����������� ����� � ����������� ������ TEDS */
            LTR25_ERR_TEDS_1W_NO_PRESENSE_PULSE = -10620, /**< �� ���������� ������� ����������� TEDS ���� �� ������������� ���� */
            LTR25_ERR_TEDS_1W_NOT_IDLE = -10621, /**< ������������� ���� �� ���� � ��������� ��������� �� ������ ������ ������ */
            LTR25_ERR_TEDS_1W_UNKNOWN_ERR = -10622, /**< ����������� ������ ��� ������ �� ������������� ���� � ����� TEDS */
            LTR25_ERR_TEDS_MEM_STATUS = -10623, /**< �������� ��������� ������ TEDS ���� */

            LTR216_ERR_ADC_ID_CHECK             = -10700, /**< �� ������� ���������� ���������� ��� */
            LTR216_ERR_ADC_RECV_SYNC_OVERRATE   = -10701, /**< ������� ������������� ��� ��������� ������� �������������� */
            LTR216_ERR_ADC_RECV_INT_CYCLE_ERROR = -10702, /**< ������ ����������� ����� ������ ������ � ��� */
            LTR216_ERR_ADC_REGS_INTEGRITY       = -10703, /**< �������� ����������� ��������� ��� */
            LTR216_ERR_INVALID_ADC_SWMODE       = -10704, /**< ������ �������� �������� ������ ������ ������� ��� */
            LTR216_ERR_INVALID_FILTER_TYPE      = -10705, /**< ������ �������� �������� ���� ������� ��� */
            LTR216_ERR_INVALID_ADC_ODR_CODE     = -10706, /**< ������ �������� �������� ����, ������������� �������� �������������� ��� */
            LTR216_ERR_INVALID_SYNC_FDIV        = -10707, /**< ������ �������� �������� �������� ������� ������������� ��� */
            LTR216_ERR_INVALID_LCH_CNT          = -10708, /**< ������ �������� ���������� ���������� ������� */
            LTR216_ERR_INVALID_ISRC_CODE        = -10709, /**< ����� �������� ���, ������������ ���� ���� ������� �������� */
            LTR216_ERR_CH_NOT_FOUND_IN_LTABLE   = -10710, /**< ��������� ����� �� ��� ������ � ���������� ������� */
            LTR216_ERR_NO_CH_ENABLED            = -10711, /**< �� ���� ����� �� ��� �������� */
            LTR216_ERR_TARE_CHANNELS            = -10712, /**< �� ������� ����� �������������� �������� ��� ��������� ��������� ������� */
            LTR216_ERR_TOO_MANY_LTABLE_CH       = -10713, /**< ��������� ������������ ����� ������� � �������� ����� ������ ��� */
            LTR216_ERR_TOO_MANY_LTABLE_BG_CH    = -10714, /**< ��������� ������������ ����� ������� � ������� ����� ������ ��� */
            LTR216_ERR_UNSUF_SW_TIME            = -10715, /**< ���������� ����� �� ���������� ������ ��������� ������� */
            LTR216_ERR_BAD_INIT_MEAS_STATUS     = -10716, /**< ���������� �������� ���������� ��������� ��������������� */
            LTR216_ERR_INVALID_CH_RANGE         = -10717, /**< ����� �������� ��� ��������� ������ ���*/
            LTR216_ERR_INVALID_CH_NUM           = -10718, /**< ����� �������� ����� ����������� ������ ��� */
            LTR216_ERR_UREF_MEAS_REQ            = -10719,  /**< ��� ���������� �������� ��������� �������� �������������� �������� ���������� \f$U_{ref}\f$ */

            LPW25_ERROR_FD_NOT_SET = -10800, /** �� ������ ������� ������������� ������� */
            LPW25_ERROR_SENS_NOT_SET = -10801, /** �� ����� ����������� �������� ��������������� */
            LPW25_ERROR_PROC_NOT_STARTED = -10802, /** �� �������� ��������� ������ */
            LPW25_ERROR_TEDS_MANUFACTURER_ID = -10803, /** ����������� ������������� ������������� */
            LPW25_ERROR_TEDS_MODEL_ID = -10804, /** ����������� ������ ��������������� */




            LTEDS_ERROR_INSUF_SIZE = -12000, /**< ������������ ����� � ������ TEDS ��� ���������� �������� */
            LTEDS_ERROR_CHECKSUM = -12001, /**< �������� �������� ����������� ����� � ������ TEDS */
            LTEDS_ERROR_INVALID_BITSIZE = -12002, /**< ������� ����� ������� ������ ������ TEDS */
            LTEDS_ERROR_UNSUPPORTED_FORMAT = -12003, /**< �� �������������� ��������� ������ ������ TEDS */
            LTEDS_ERROR_ENCODE_VALUE = -12004, /**< ������� ������� �������� ��� ����������� � TEDS */
            LTEDS_ERROR_UNKNOWN_SEL_CASE = -12005 /**< ����������� ������� ������ ��������� ������ TEDS */
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
            NO_POWER       = 0x0, /**< ��� ������� ������� ���� */
            NSTATUS_TOUT   = 0x1, /**< ������� ����� �������� ���������� ���� � �������� */
            CONF_DONE_TOUT = 0x2, /**< ������� ����� �������� ���������� �������� ���� (������ ��������,
                                       ��� �� Flash ��� �������������� ��������) */
            LOAD_PROGRESS  = 0x3, /**< ���� �������� ���� */
            LOAD_DONE      = 0x7, /**< �������� ���� ���������, �� ������ ���� ��� �� ��������� */
            WORK           = 0xF  /**< ���������� ������� ��������� ���� */
        }

        public const uint MODULE_MAX = 16;
        public const uint CRATE_MAX = 16;
        public const uint SERIAL_NUMBER_SIZE = 16;


        public const uint SADDR_DEFAULT = 0x7F000001U;
        public const ushort SPORT_DEFAULT = 11111;


        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_Init(ref TLTR ltr); //������������� ����� ���������

        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_Open(ref TLTR ltr); //�������� ���������� � �������, ������� ��� ��������

        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_OpenEx(ref TLTR ltr, uint timeout);

        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_OpenSvcControl(ref TLTR ltr, uint saddr, ushort sport);

        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_OpenCrate(ref TLTR ltr, uint saddr, ushort sport, int iface, string csn); 
        
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_Close(ref TLTR ltr); //������ ���������� � �������

        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_IsOpened(ref TLTR ltr); //��������� ���������� � �������

        [DllImport("ltrapi.dll")]
        public static extern int LTR_Recv(ref TLTR ltr, uint[] buf, uint[] tmark, uint size, uint timeout); //����� ������ �� ������

        // ����� ������ �� ������ � ���������� ����������� ����� �������
        [DllImport("ltrapi.dll")]
        public static extern int LTR_RecvEx(ref TLTR ltr, uint[] data, uint[] tmark, uint size, uint timeout,
                                  UInt64[] unixtime);
        [DllImport("ltrapi.dll")]
        public static extern int LTR_Send(ref TLTR ltr, uint[] buf, uint size, uint timeout); //�������� ������ ������

        [DllImport("ltrapi.dll")]
        public static extern _LTRNative.LTRERROR LTR_SetServerProcessPriority(ref TLTR ltr, [MarshalAs(UnmanagedType.U4)] ServerPriority Priority);

        //
        // ��� ������ ���������, ��� CC_CONTROL
        //
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_GetCrates(ref TLTR ltr, byte[,] csn); //������ ������� ������������ � �������
        //BYTE[CRATE_MAX][ SERIAL_NUMBER_SIZE]
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_GetCrateModules(ref TLTR ltr, UInt16[] mid); //������ ������� ������
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_GetCrateModules(ref TLTR ltr, MODULETYPE[] mid); //������ ������� ������
        //WORD[MODULE_MAX],
        ///
        /// Ethernet Crate 
        /// 
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_GetServerVersion(ref TLTR ltr, out UInt32 ver);
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_SetServerProcessPriority(ref TLTR ltr, uint Priority);
        // ������ �������� ������ (������, ��� ����������)
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_GetCrateInfo(ref TLTR ltr, out TCRATE_INFO CrateInfo);
        // ��������� ����� ������ �� ������ (���� ��� ���������� CC_RAW_DATA_FLAG)
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_GetCrateRawData(ref TLTR ltr, ref uint data, ref uint tmark, uint size, uint timeout);
        // ������� ������������ �������� ������ � ������� ������
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_Config(ref TLTR ltr, ref TLTR_CONFIG conf);
        // ��������� ���������� ��������� ��������� �����
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_StartSecondMark(ref TLTR ltr, en_LTR_MarkMode mode);
        // ���������� ���������� ��������� ��������� �����
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_StopSecondMark(ref TLTR ltr);
        // ������ ����� ����� (����������)
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_MakeStartMark(ref TLTR ltr, en_LTR_MarkMode mode);
        // ������ ���������� �������� ltrserver.exe
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_GetServerProcessPriority(ref TLTR ltr, ref uint Priority);
        // -- ������� ���������� �������� (�������� �� ������������ ������,
        //    � �.�. ��� ������ ����� ������ �������� ����� CSN_SERVER_CONTROL
        // ��������� ������ ��������� ������� IP-������� (������������ � �� ������������)
        // ������������� ����������� ���� ������� "���������� IP-��������".
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_GetListOfIPCrates(ref TLTR ltr,
        UInt32 max_entries, UInt32 ip_net, UInt32 ip_mask,
        out UInt32 entries_found, out UInt32 entries_returned,
        IntPtr info_array);//  TIPCRATE_ENTRY[] info_array);
        // ���������� IP-������ ������ � ������� (� ����������� � ����� ������������).
        // ���� ����� ��� ����������, ���������� ������.
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_AddIPCrate(ref TLTR ltr, uint ip_addr, uint flags, bool permanent);
        // �������� IP-������ ������ �� ������� (� �� ����� ������������, ���� �� �����������).
        // ���� ������ ���, ���� ����� �����, ���������� ������.
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_DeleteIPCrate(ref TLTR ltr, uint ip_addr, bool permanent);
        // ������������ ���������� � IP-������� (����� ������ ���� � �������)
        // ���� ������ ���, ���������� ������. ���� ����� ��� ��������� ��� ���� ������� �����������,
        // ���������� LTR_OK.
        // ���� ��������� LTR_OK, ��� ������, ��� ���������� ������� ���������� ����������.
        // ��������� ����� ��������� ����� �� LTR_GetCrates() ��� LTR_GetListOfIPCrates()
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_ConnectIPCrate(ref TLTR ltr, uint ip_addr);
        // ���������� ���������� � IP-������� (����� ������ ���� � �������)
        // ���� ������ ���, ���������� ������. ���� ����� ��� ��������, ���������� LTR_OK.
        // ���� ��������� LTR_OK, ��� ������, ��� ���������� ������� ��������� ����������.
        // ��������� ����� ��������� ����� �� LTR_GetCrates() ��� LTR_GetListOfIPCrates()
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_DisconnectIPCrate(ref TLTR ltr, uint ip_addr);
        // ������������ ���������� �� ����� IP-��������, �������� ���� "���������������"
        // ���� ��������� LTR_OK, ��� ������, ��� ���������� ������� ���������� ����������.
        // ��������� ����� ��������� ����� �� LTR_GetCrates() ��� LTR_GetListOfIPCrates()
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_ConnectAllAutoIPCrates(ref TLTR ltr);
        // ���������� ���������� �� ����� IP-��������
        // ���� ��������� LTR_OK, ��� ������, ��� ���������� ������� ��������� ����������.
        // ��������� ����� ��������� ����� �� LTR_GetCrates() ��� LTR_GetListOfIPCrates()
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_DisconnectAllIPCrates(ref TLTR ltr);
        // ��������� ������ ��� IP-������
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_SetIPCrateFlags(ref TLTR ltr, uint ip_addr, uint new_flags, bool permanent);
        // ������ ������ ������ IP-������� � ��������� ����
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_GetIPCrateDiscoveryMode(ref TLTR ltr, out bool enabled, out bool autoconnect);
        // ��������� ������ ������ IP-������� � ��������� ����
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_SetIPCrateDiscoveryMode(ref TLTR ltr, bool enabled, bool autoconnect, bool permanent);
        // ������ ������ ������������
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_GetLogLevel(ref TLTR ltr, out int level);
        // ��������� ������ ������������
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_SetLogLevel(ref TLTR ltr, int level, bool permanent);
        // ������� ��������� ltrserver (� �������� ���� ����������).
        // ����� ��������� ���������� ������� ����� � �������� ��������.
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_ServerRestart(ref TLTR ltr);
        // ������� ��������� ltrserver (� �������� ���� ����������)
        // ����� ��������� ���������� ������� ����� � �������� ��������.
        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_ServerShutdown(ref TLTR ltr);

        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_ResetModule(ref TLTR ltr, int iface, string serial, int slot, uint flags);



        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_PutSettings(ref TLTR ltr, ref TLTR_SETTINGS settings);

        [DllImport("ltrapi.dll")]
        public static extern LTRERROR LTR_GetLastUnixTimeMark(ref TLTR ltr, out UInt64 unixtime);




        // ����������� ������
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
                if (ERRORCODE != LTRERROR.OK) throw new Exception("������ �������� ���������� � �������� " + GetErrorString(ERRORCODE));

                ERRORCODE = LTR_GetCrates(ref LTR, CrateList);
                if (ERRORCODE != LTRERROR.OK) throw new Exception("������ ��������� ������ ������� "+ GetErrorString(ERRORCODE));

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
                if (ERRORCODE != LTRERROR.OK) throw new Exception("�� �������� LTR "+ GetErrorString(ERRORCODE));

                ERRORCODE = LTR_GetCrateModules(ref LTR, Modules);
                if (ERRORCODE != LTRERROR.OK) throw new Exception("�� ����������� ������ " + GetErrorString(ERRORCODE));

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
            if (ERRORCODE != LTRERROR.OK) throw new Exception("�� ����������� ������ " + GetErrorString(ERRORCODE));

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

        // ���������� ������ � �������� � ������
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



        // �������������� ������ �� ������� ������� � ������ ������
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
