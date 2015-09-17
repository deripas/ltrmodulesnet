using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ltrModulesNet
{
    /* Класс реализует функции ltrapi для работы с крейтом */
    public class ltrcrate 
    {
        public enum Types : byte
        {
            UNKNOWN = 0,
            LTR010 = 10,
            LTR021 = 21,
            LTR030 = 30,
            LTR031 = 31,
            LTR032 = 32,
            LTR_CU_1 = 40,
            LTR_CEU_1 = 41,
            BOOTLOADER = 99
        }

        // интерфейс крейта
        public enum Interfaces : byte
        {
            UNKNOWN = 0,
            USB = 1,
            TCPIP = 2
        }

        // Значения для управления ножками процессора, доступными для пользовательского программирования
        public enum UserIoCfg : ushort
        {
            DIGIN1 = 1,    // ножка является входом и подключена к DIGIN1
            DIGIN2 = 2,    // ножка является входом и подключена к DIGIN2
            DIGOUT = 0,    // ножка является выходом (подключение см. en_LTR_DigOutCfg)
            DEFAULT = DIGOUT
        }
        // Значения для управления выходами DIGOUTx
        public enum DigOutCfg : ushort
        {
            CONST0 = 0x00, // постоянный уровень логического "0"
            CONST1 = 0x01, // постоянный уровень логической "1"
            USERIO0 = 0x02, // выход подключен к ножке userio0 (PF1 в рев. 0, PF1 в рев. 1)
            USERIO1 = 0x03, // выход подключен к ножке userio1 (PG13)
            DIGIN1 = 0x04, // выход подключен ко входу DIGIN1
            DIGIN2 = 0x05, // выход подключен ко входу DIGIN2
            START = 0x06, // на выход подаются метки "СТАРТ"
            SECOND = 0x07, // на выход подаются метки "СЕКУНДА"
            IRIG = 0x08, // контроль сигналов точного времени IRIG (digout1: готовность, digout2: секунда)
            DEFAULT = CONST0
        }


        public struct Config
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UserIoCfg[] userio;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public DigOutCfg[] digout;
            [MarshalAs(UnmanagedType.U2)]
            public bool digout_en; 
        }


        public Types Type { get { return (Types)info.CrateType; } }
        public Interfaces Interface { get { return (Interfaces)info.CrateInterface; } }
        public string Serial
        {
            get
            {
                char[] csn = new char[hnd.csn.Length];
                for (int i = 0; i < csn.Length; i++)
                    csn[i] = (char)hnd.csn[i];
                return new string(csn).Split('\0')[0];
            }
        }


        public ltrcrate() 
        {
            _LTRNative.LTR_Init(ref hnd);	
        }

        ~ltrcrate()
        {
            _LTRNative.LTR_Close(ref hnd);
        }

        public virtual _LTRNative.LTRERROR Open(uint saddr, ushort sport, string csn)
        {
            char[] arr = csn.ToCharArray();
            int i;
            for (i = 0; (i < arr.Length) && (i < 15); i++)
                hnd.csn[i] = (byte)arr[i];
            hnd.csn[i] = 0;
            hnd.cc = 0;
            hnd.saddr = saddr;
            hnd.sport = sport;

            _LTRNative.LTRERROR err = _LTRNative.LTR_Open(ref hnd);
            if (err == _LTRNative.LTRERROR.OK)
                err = _LTRNative.LTR_GetCrateInfo(ref hnd, out info);
            return err;
        }

        public virtual _LTRNative.LTRERROR Open(string csn)
        {
            return Open(_LTRNative.SADDR_DEFAULT, _LTRNative.SPORT_DEFAULT, csn);
        }

        public virtual _LTRNative.LTRERROR Open()
        {
            return Open(_LTRNative.SADDR_DEFAULT, _LTRNative.SPORT_DEFAULT, "");
        }

        public virtual _LTRNative.LTRERROR Close()
        {
            return _LTRNative.LTR_Close(ref hnd);
        }

        public virtual _LTRNative.LTRERROR IsOpened()
        {
            return _LTRNative.LTR_IsOpened(ref hnd);
        }

        /* Включение внутренней генерации секундных меток */
        public virtual _LTRNative.LTRERROR StartSecondMark(_LTRNative.en_LTR_MarkMode mode)
        {
            return _LTRNative.LTR_StartSecondMark(ref hnd, mode);
        }
        /* Отключение внутренней генерации секундных меток */
        public virtual _LTRNative.LTRERROR StopSecondMark() 
        {
            return _LTRNative.LTR_StopSecondMark(ref hnd);
        }
        /* Выбор режима генерации метки СТАРТ (или однократная вставка программной метки) */
        public virtual _LTRNative.LTRERROR MakeStartMark(_LTRNative.en_LTR_MarkMode mode)
        {
            return _LTRNative.LTR_MakeStartMark(ref hnd, mode);
        }

        public virtual _LTRNative.LTRERROR SetConfig(ref Config config) 
        {
            return LTR_Config(ref hnd, ref config);
        }

        public virtual _LTRNative.LTRERROR GetModules(out _LTRNative.MODULETYPE[] mid)
        {
            mid = new _LTRNative.MODULETYPE[_LTRNative.MODULE_MAX];
            return _LTRNative.LTR_GetCrateModules(ref hnd, mid);
        }

        public static string GetErrorString(_LTRNative.LTRERROR err)
        {
            IntPtr ptr = LTR_GetErrorString((int)err);
            string str = Marshal.PtrToStringAnsi(ptr);
            Encoding srcEncodingFormat = Encoding.GetEncoding("windows-1251");
            Encoding dstEncodingFormat = Encoding.UTF8;
            return dstEncodingFormat.GetString(Encoding.Convert(srcEncodingFormat, dstEncodingFormat, srcEncodingFormat.GetBytes(str)));
        }


        _LTRNative.TLTR hnd;
        _LTRNative.TCRATE_INFO info;

        [DllImport("ltrapi.dll")]
        static extern _LTRNative.LTRERROR LTR_Config(ref _LTRNative.TLTR ltr, ref Config conf);

        [DllImport("ltrapi.dll")]
        public static extern IntPtr LTR_GetErrorString(int err);


    }
}
