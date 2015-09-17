using System;
using System.Runtime.InteropServices;
using System.Text;


namespace ltrModulesNet
{
    public class ltrsrvcon
    {
        public virtual _LTRNative.LTRERROR Open(uint saddr, ushort sport)
        {
            string str = "#SERVER_CONTROL";
            char[] arr = str.ToCharArray();
            int i;
            for (i = 0; (i < arr.Length) && (i < 15); i++)
                hnd.csn[i] = (byte)arr[i];
            hnd.csn[i] = 0;
            hnd.cc = 0;
            hnd.saddr = saddr;
            hnd.sport = sport;

            return _LTRNative.LTR_Open(ref hnd);
        }

        public ltrsrvcon() 
        {
            _LTRNative.LTR_Init(ref hnd);	
        }

        ~ltrsrvcon()
        {
            _LTRNative.LTR_Close(ref hnd);
        }

        public virtual _LTRNative.LTRERROR Open()
        {
            return Open(_LTRNative.SADDR_DEFAULT, _LTRNative.SPORT_DEFAULT);
        }
                
        public virtual _LTRNative.LTRERROR Close()
        {
            return _LTRNative.LTR_Close(ref hnd);
        }

        public virtual _LTRNative.LTRERROR IsOpened()
        {
            return _LTRNative.LTR_IsOpened(ref hnd);
        }

        public virtual _LTRNative.LTRERROR GetCrates(out string[] csn)
        {
            int crate_cnt = 0;
            byte[,] csn_byte = new byte[_LTRNative.CRATE_MAX, _LTRNative.SERIAL_NUMBER_SIZE];
            _LTRNative.LTRERROR res = _LTRNative.LTR_GetCrates(ref hnd, csn_byte);
            if (res == _LTRNative.LTRERROR.OK)
            {
                for (int i = 0; i < _LTRNative.CRATE_MAX; i++)
                {
                    if (csn_byte[i, 0] != 0)
                        crate_cnt++;
                }
                csn = new string[crate_cnt];
                for (int i = 0, k = 0; i < _LTRNative.CRATE_MAX; i++)
                {
                    if (csn_byte[i, 0] != 0)
                    {
                        char[] cs = new char[_LTRNative.SERIAL_NUMBER_SIZE];
                        for (int j = 0; j < _LTRNative.SERIAL_NUMBER_SIZE; j++)
                        {
                            cs[j] = (char)csn_byte[i, j];
                        }
                        csn[k++] = new string(cs);
                    }
                }
            }
            else
            {
                csn = new string[0];
            }

            return res;
        }



        public virtual _LTRNative.LTRERROR GetServerVersion(out string version)
        {
            UInt32 ver;
            StringBuilder sb = new StringBuilder();
            _LTRNative.LTRERROR res = GetServerVersion(out ver);
            if (res == _LTRNative.LTRERROR.OK)
            {
                sb.Append((ver >> 24) & 0xFF).Append(".").
                    Append((ver >> 16) & 0xFF).Append(".").Append((ver >> 8) & 0xFF).Append(".").Append(ver & 0xFF);
            }
            version = sb.ToString();
            return res;
        }

        public virtual _LTRNative.LTRERROR GetServerVersion(out UInt32 ver)
        {
            //version = new String();
            return _LTRNative.LTR_GetServerVersion(ref hnd, out ver);
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

        [DllImport("ltrapi.dll")]
        public static extern IntPtr LTR_GetErrorString(int err);
    }
}
