using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ltrModulesNet
{
    public class ltrapiLabView : _LTRNative
    {
        public ltrapiLabView() { }

        public uint StartMarks { get { return (module.tmark >> 16) & 0xFFFF; } }
        public uint SecondMarks { get { return module.tmark & 0xFFFF; } }

        public uint StartMarksAfterOpen { get { return (module.tmark >> 16) & 0xFF; } }
        public uint SecondMarksAfterOpen { get { return module.tmark & 0xFF; } }

        //UInt16 start_mark_init = 0;
        //UInt16 second_mark_init = 0;

        public virtual _LTRNative.LTRERROR Open(uint saddr, ushort sport, byte[] csn)
        {
            _LTRNative.LTRERROR res;
            module.cc = 0;

            if (saddr != 0)
                module.saddr = saddr;
            if (sport != 0)
                module.sport = sport;

            for (int i = 0; (i < csn.Length) && (i < module.csn.Length); i++)
                module.csn[i] = csn[i];

            res = _LTRNative.LTR_Open(ref module);
            if (res == LTRERROR.OK)
            {
                for (int i = 0; (i < csn.Length) && (i < module.csn.Length); i++)
                    csn[i] = (byte)module.csn[i];
            }
            return res;
        }

        public virtual _LTRNative.LTRERROR OpenServerChannel(uint saddr, ushort sport)
        {
            module.cc = 0;

            if (saddr != 0)
                module.saddr = saddr;
            if (sport != 0)
                module.sport = sport;

            string str = "#SERVER_CONTROL";
            char[] arr = str.ToCharArray();
            int i;
            for (i=0; i < arr.Length; i++)
                module.csn[i] = (byte)arr[i];
            module.csn[i] = 0;         

            return _LTRNative.LTR_Open(ref module);
        }


        public override _LTRNative.LTRERROR Close()
        {
            return base.Close();
        }

        public virtual _LTRNative.LTRERROR Config(TLTR_CONFIG conf)
        {
            return _LTRNative.LTR_Config(ref module, ref conf);
        }

        public virtual _LTRNative.LTRERROR MakeStartMark(en_LTR_MarkMode mode)
        {
            return _LTRNative.LTR_MakeStartMark(ref module, mode);
        }

        public virtual _LTRNative.LTRERROR StartSecondMark(en_LTR_MarkMode mode)
        {
            return _LTRNative.LTR_StartSecondMark(ref module, mode);
        }

        public virtual _LTRNative.LTRERROR StopSecondMark()
        {
            return _LTRNative.LTR_StopSecondMark(ref module);
        }

        public virtual _LTRNative.LTRERROR GetCrateInfo(out TCRATE_INFO ci)
        {
            return _LTRNative.LTR_GetCrateInfo(ref module, out ci);
        }

        public virtual _LTRNative.LTRERROR GetCrates(out string[] csn)
        {
            int crate_cnt=0;
            byte[,] csn_byte = new byte[_LTRNative.CRATE_MAX, _LTRNative.SERIAL_NUMBER_SIZE];
            _LTRNative.LTRERROR res =  _LTRNative.LTR_GetCrates(ref module, csn_byte);
            if (res == LTRERROR.OK)
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
                csn = new string[0];

            return res;
        }


        public virtual string GetErrorString(int error)
        {
            return _LTRNative.LTR_GetErrorString(error);
        }

 
        public virtual _LTRNative.LTRERROR GetServerVersion(out string version)
        {
            //version = new String();
            UInt32 ver;
            StringBuilder sb = new StringBuilder();
            _LTRNative.LTRERROR res = GetServerVersion(out ver);
            if (res == LTRERROR.OK)
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
            return _LTRNative.LTR_GetServerVersion(ref module, out ver);           
        }

        public virtual _LTRNative.LTRERROR GetCrateModules(out UInt16[] mid)
        {
            mid = new UInt16[_LTRNative.MODULE_MAX];
            return _LTRNative.LTR_GetCrateModules(ref module, mid);
        }

        public virtual LTRERROR GetListOfIPCrates(UInt32 ip_net, UInt32 ip_mask, out TIPCRATE_ENTRY[] info_array)
        {
            UInt32 fnd, ret;
           // info_array = new TIPCRATE_ENTRY[0];
         //   TIPCRATE_ENTRY info = new TIPCRATE_ENTRY();
            IntPtr ptr = IntPtr.Zero;
            LTRERROR res = _LTRNative.LTR_GetListOfIPCrates(ref module, 0, ip_net, ip_mask, out fnd, out ret, ptr);
            
            if (res == LTRERROR.OK)
            {
                if (fnd > 0)
                {
                    int size = Marshal.SizeOf(typeof(TIPCRATE_ENTRY));
                    ptr = Marshal.AllocHGlobal(size * (int)fnd);
                    res = _LTRNative.LTR_GetListOfIPCrates(ref module, fnd, ip_net, ip_mask, out fnd, out ret, ptr);

                    info_array = new TIPCRATE_ENTRY[fnd];
                    
                    for (int i = 0; i < fnd; i++)
                    {
                        IntPtr infoptr = new IntPtr(ptr.ToInt32() + size * i);
                        info_array[i] = new TIPCRATE_ENTRY();
                        info_array[i] = (TIPCRATE_ENTRY)Marshal.PtrToStructure(ptr, typeof(TIPCRATE_ENTRY));
                    }

                    Marshal.FreeHGlobal(ptr);
                }
                else
                    info_array = new TIPCRATE_ENTRY[0];

            }
            else
                info_array = new TIPCRATE_ENTRY[0];

            return res;            
        }

        public virtual LTRERROR AddIPCrate(uint ip_addr, uint flags, bool permanent)
        {
            return _LTRNative.LTR_AddIPCrate(ref module, ip_addr, flags, permanent);
        }

        public virtual LTRERROR DeleteIPCrate(uint ip_addr, bool permanent)
        {
            return _LTRNative.LTR_DeleteIPCrate(ref module, ip_addr, permanent);
        }

        public virtual LTRERROR ConnectIPCrate(uint ip_addr)
        {
            return _LTRNative.LTR_ConnectIPCrate(ref module, ip_addr);
        }

        public virtual LTRERROR DisconnectIPCrate(uint ip_addr)
        {
            return _LTRNative.LTR_DisconnectIPCrate(ref module, ip_addr);
        }

        public virtual LTRERROR ConnectAllAutoIPCrates()
        {
            return _LTRNative.LTR_ConnectAllAutoIPCrates(ref module);
        }

        public virtual LTRERROR DisconnectAllIPCrates()
        {
            return _LTRNative.LTR_DisconnectAllIPCrates(ref module);
        }

        public virtual LTRERROR SetIPCrateFlags(uint ip_addr, uint new_flags, bool permanent)
        {
            return _LTRNative.LTR_SetIPCrateFlags(ref module, ip_addr, new_flags, permanent);
        }

        public virtual LTRERROR GetIPCrateDiscoveryMode(out bool enabled, out bool autoconnect)
        {
            return _LTRNative.LTR_GetIPCrateDiscoveryMode(ref module, out enabled, out autoconnect);
        }

        public virtual LTRERROR SetIPCrateDiscoveryMode(bool enabled, bool autoconnect, bool permanent)
        {
            return _LTRNative.LTR_SetIPCrateDiscoveryMode(ref module, enabled, autoconnect, permanent);
        }

        public virtual LTRERROR GetLogLevel(out int level)
        {
            return _LTRNative.LTR_GetLogLevel(ref module, out level);
        }

        public virtual LTRERROR SetLogLevel(int level, bool permanent)
        {
            return _LTRNative.LTR_SetLogLevel(ref module, level, permanent);
        }

        public virtual LTRERROR ServerRestart()
        {
            return _LTRNative.LTR_ServerRestart(ref module);
        }


        public virtual LTRERROR LTR_ServerShutdown()
        {
            return _LTRNative.LTR_ServerShutdown(ref module);
        }

    }
}
