using System;
using System.Runtime.InteropServices;

namespace ltrModulesNet
{
    public class _le41api
    {               
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_OPEN(int ComPort, int mode, uint net_addr, ushort net_port, char[] crate_sn, int slot_num);                                                                                             
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_CLOSE();
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_SELECT(int FactoryNumber); // 1...999999d;  Serial= 4L543217 -> FactoryNum= 543217d
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_SET_RETRY(int retry);
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_GET_RETRY(out int retry);
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_SET_RELIABILITY(int reliability);
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_GET_RELIABILITY(out int reliability);

        /*Функции модуля LE-41.*/
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_CHECK();
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_GET_VERSION(out int avr_version);
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_SET_MODE(int new_mode);
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_SET_BAND(int Chan, int NewBand);
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_SET_SENSE(int Chan, int NewSense);
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_STORE_CONFIG(int IsRestoreOnStartUp);
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_RESTORE_CONFIG();
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_FLASH_READ(int addr, out int data);
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_FLASH_WRITE(int addr, int data);
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_GET_MODE(out int cur_Mode);
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_GET_BAND(out int cur_Band);
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_GET_SENSE(out int cur_Sense);
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_SERIAL_NUMBER(Byte[] SerNum);
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_FLASH_WRITE_WORD16(int addr, int data);
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_FLASH_READ_WORD16(int addr, out int data);
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_WRITE_CC(int Chan, int Sens, double cc);
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_READ_CC(int Chan, int Sens, out double cc);
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_REWRITE_CRC();
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_VERIFY_CRC(out int IsValid);
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_VERIFY_CLBMARK(int Channel, out int IsValid);
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_REWRITE_CLBMARK(int Channel, int IsValid);
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_CONNECT();
        [DllImport("ltr43_LE41api.dll")]
        public static extern int HC_LE41_GET_CONFIG(out int cur_Cfg);
        
    }
}
