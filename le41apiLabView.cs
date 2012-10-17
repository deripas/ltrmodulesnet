using System;
using System.Runtime.InteropServices;

namespace ltrModulesNet
{
    public class le41apiLabView : _le41api
    {
        public int LE41_OPEN(int ComPort, int mode, uint net_addr, ushort net_port, char[] crate_sn, int slot_num)
        {
            return _le41api.HC_LE41_OPEN(ComPort, mode, net_addr,net_port,crate_sn,slot_num);
        }

        public int LE41_CLOSE()
        {
            return _le41api.HC_LE41_CLOSE();
        }

        public int LE41_SELECT(int FactoryNumber) // 1...999999d;  Serial= 4L543217 -> FactoryNum= 543217d
        {
            return _le41api.HC_LE41_SELECT(FactoryNumber);
        }

        public int LE41_SET_RETRY(int retry)
        {
            return _le41api.HC_LE41_SET_RETRY(retry);
        }

        public int LE41_GET_RETRY(out int retry)
        {
            return _le41api.HC_LE41_GET_RETRY(out retry);
        }

        public int LE41_SET_RELIABILITY(int reliability)
        {
            return _le41api.HC_LE41_SET_RELIABILITY(reliability);
        }
        public int LE41_GET_RELIABILITY(out int reliability)
        {
            return _le41api.HC_LE41_GET_RELIABILITY(out reliability);
        }
        /*Функции модуля LE-41.*/

        public int LE41_CHECK()
        {
            return _le41api.HC_LE41_CHECK();
        }
        public int LE41_GET_VERSION(out int avr_version)
        {
            return _le41api.HC_LE41_GET_VERSION(out avr_version);
        }
        public int LE41_SET_MODE(int new_mode)
        {
            return _le41api.HC_LE41_SET_MODE(new_mode);
        }
        public int LE41_SET_BAND(int Chan, int NewBand)
        {
            return _le41api.HC_LE41_SET_BAND(Chan,NewBand);
        }
        public int LE41_SET_SENSE(int Chan, int NewSense)
        {
            return _le41api.HC_LE41_SET_SENSE(Chan, NewSense);
        }
        public int LE41_STORE_CONFIG(int IsRestoreOnStartUp)
        {
            return _le41api.HC_LE41_STORE_CONFIG(IsRestoreOnStartUp);
        }
        public int LE41_RESTORE_CONFIG()
        {
            return _le41api.HC_LE41_RESTORE_CONFIG();
        }
        public int LE41_FLASH_READ(int addr, out int data)
        {
            return _le41api.HC_LE41_FLASH_READ(addr,out data);
        }
        public int LE41_FLASH_WRITE(int addr, int data)
        {
            return _le41api.HC_LE41_FLASH_WRITE(addr, data);
        }
        public int LE41_GET_MODE(out int cur_Mode)
        {
            return _le41api.HC_LE41_GET_MODE(out cur_Mode);
        }
        public int LE41_GET_BAND(out int cur_Band)
        {
            return _le41api.HC_LE41_GET_BAND(out cur_Band);
        }
        public int LE41_GET_SENSE(out int cur_Sense)
        {
            return _le41api.HC_LE41_GET_SENSE(out cur_Sense);
        }
        public  int LE41_SERIAL_NUMBER(Byte[] SerNum)
        {
            return _le41api.HC_LE41_SERIAL_NUMBER(SerNum);
        }
        public int LE41_FLASH_WRITE_WORD16(int addr, int data)
        {
            return _le41api.HC_LE41_FLASH_WRITE_WORD16( addr,  data);
        }
        public int LE41_FLASH_READ_WORD16(int addr, out int data)
        {
            return _le41api.HC_LE41_FLASH_READ_WORD16( addr,out data);
        }
        public int lE41_WRITE_CC(int Chan, int Sens, double cc)
        {
            return _le41api.HC_LE41_WRITE_CC( Chan,  Sens,  cc);
        }
        public int LE41_READ_CC(int Chan, int Sens, out double cc)
        {
            return _le41api.HC_LE41_READ_CC( Chan,  Sens,out cc);
        }
        public int LE41_REWRITE_CRC()
        {
            return _le41api.HC_LE41_REWRITE_CRC();
        }
        public int LE41_VERIFY_CRC(out int IsValid)
        {
            return _le41api.HC_LE41_VERIFY_CRC(out IsValid);
        }
        public int LE41_VERIFY_CLBMARK(int Channel, out int IsValid)
        {
            return _le41api.HC_LE41_VERIFY_CLBMARK( Channel,out IsValid);
        }
        public int LE41_REWRITE_CLBMARK(int Channel, int IsValid)
        {
            return _le41api.HC_LE41_REWRITE_CLBMARK( Channel, IsValid);
        }
        public int LE41_CONNECT()
        {
            return _le41api.HC_LE41_CONNECT();
        }
        public int LE41_GET_CONFIG(out int cur_Cfg) 
        {
            return _le41api.HC_LE41_GET_CONFIG(out cur_Cfg);
        }
    }
}
