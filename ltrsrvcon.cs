using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Net;

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


        static uint ipAddrToUint(IPAddress addr)
        {
            byte[] bytes = addr.GetAddressBytes();
            Array.Reverse(bytes); // flip big-endian(network order) to little-endian
            return BitConverter.ToUInt32(bytes, 0);
        }

        static IPAddress uintToIpAddr(uint intAddr)
        {
            byte[] bytes = BitConverter.GetBytes(intAddr);
            Array.Reverse(bytes);
            return new IPAddress(bytes);
        }

        // элемент списка IP-крейтов
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct IpCrateEntry
        {
            public enum IpStatus : byte
            {
                Offline = 0,
                Connecting = 1,
                Online = 2,
                Error = 3
            };

            [Flags]
            public enum IpFlags : uint
            {
                Autoconnect = 0x00000001
            }


            uint _ip_addr;                                          // IP адрес (host-endian)
            IpFlags _flags;                                            // флаги режимов (CRATE_IP_FLAG_...)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            char[] _serial_number;                                 // серийный номер (если крейт подключен)
            byte _is_dynamic;                                        // 0 = задан пользователем, 1 = найден автоматически
            IpStatus _status;                                            // состояние (CRATE_IP_STATUS_...)


            public IPAddress IpAddr { get { return uintToIpAddr(_ip_addr); } }
            public uint IpAddrUint { get { return _ip_addr; } }
            public String CrateSerial { get { return new string(_serial_number).Split('\0')[0]; } }
            public IpStatus Status { get { return _status; } }
            public IpFlags Flags { get { return _flags; } }
            public bool Autoconnect { get { return (_flags & IpFlags.Autoconnect) != 0; } }
        }


        public virtual _LTRNative.LTRERROR GetListOfIPCrates(UInt32 ip_net, UInt32 ip_mask, out IpCrateEntry[] info_array)
        {
            UInt32 fnd, ret;
            // info_array = new TIPCRATE_ENTRY[0];
            //   TIPCRATE_ENTRY info = new TIPCRATE_ENTRY();
            IntPtr ptr = IntPtr.Zero;
            _LTRNative.LTRERROR res = _LTRNative.LTR_GetListOfIPCrates(ref hnd, 0, ip_net, ip_mask, out fnd, out ret, ptr);

            if (res == _LTRNative.LTRERROR.OK)
            {
                if (fnd > 0)
                {
                    int size = Marshal.SizeOf(typeof(IpCrateEntry));
                    ptr = Marshal.AllocHGlobal(size * (int)fnd);
                    res = _LTRNative.LTR_GetListOfIPCrates(ref hnd, fnd, ip_net, ip_mask, out fnd, out ret, ptr);

                    info_array = new IpCrateEntry[fnd];

                    for (int i = 0; i < fnd; i++)
                    {
                        IntPtr infoptr = new IntPtr(ptr.ToInt32() + size * i);
                        info_array[i] = new IpCrateEntry();
                        info_array[i] = (IpCrateEntry)Marshal.PtrToStructure(infoptr, typeof(IpCrateEntry));
                    }

                    Marshal.FreeHGlobal(ptr);
                }
                else
                    info_array = new IpCrateEntry[0];

            }
            else
                info_array = new IpCrateEntry[0];

            return res;
        }

        public virtual _LTRNative.LTRERROR AddIPCrate(uint ip_addr, IpCrateEntry.IpFlags flags, bool permanent)
        {
            return _LTRNative.LTR_AddIPCrate(ref hnd, ip_addr, (uint)flags, permanent);
        }

        public virtual _LTRNative.LTRERROR AddIPCrate(IPAddress ip_addr, IpCrateEntry.IpFlags flags, bool permanent)
        {
            return _LTRNative.LTR_AddIPCrate(ref hnd, ipAddrToUint(ip_addr), (uint)flags, permanent);
        }

        public virtual _LTRNative.LTRERROR DeleteIPCrate(uint ip_addr, bool permanent)
        {
            return _LTRNative.LTR_DeleteIPCrate(ref hnd, ip_addr, permanent);
        }

        public virtual _LTRNative.LTRERROR DeleteIPCrate(IPAddress ip_addr, bool permanent)
        {
            return _LTRNative.LTR_DeleteIPCrate(ref hnd, ipAddrToUint(ip_addr), permanent);
        }

        public virtual _LTRNative.LTRERROR ConnectIPCrate(uint ip_addr)
        {
            return _LTRNative.LTR_ConnectIPCrate(ref hnd, ip_addr);
        }

        public virtual _LTRNative.LTRERROR ConnectIPCrate(IPAddress ip_addr)
        {
            return _LTRNative.LTR_ConnectIPCrate(ref hnd, ipAddrToUint(ip_addr));
        }

        public virtual _LTRNative.LTRERROR DisconnectIPCrate(uint ip_addr)
        {
            return _LTRNative.LTR_DisconnectIPCrate(ref hnd, ip_addr);
        }

        public virtual _LTRNative.LTRERROR DisconnectIPCrate(IPAddress ip_addr)
        {
            return _LTRNative.LTR_DisconnectIPCrate(ref hnd, ipAddrToUint(ip_addr));
        }

        public virtual _LTRNative.LTRERROR ConnectAllAutoIPCrates()
        {
            return _LTRNative.LTR_ConnectAllAutoIPCrates(ref hnd);
        }

        public virtual _LTRNative.LTRERROR DisconnectAllIPCrates()
        {
            return _LTRNative.LTR_DisconnectAllIPCrates(ref hnd);
        }

        public virtual _LTRNative.LTRERROR SetIPCrateFlags(uint ip_addr, IpCrateEntry.IpFlags new_flags, bool permanent)
        {
            return _LTRNative.LTR_SetIPCrateFlags(ref hnd, ip_addr, (uint)new_flags, permanent);
        }

        public virtual _LTRNative.LTRERROR ResetModule(ltrcrate.Interfaces iface, string serial, int slot, uint flags)
        {
            return _LTRNative.LTR_ResetModule(ref hnd, (int)iface, serial, slot, flags);
        }

        public virtual _LTRNative.LTRERROR ResetModule(string serial, int slot)
        {
            return ResetModule(ltrcrate.Interfaces.UNKNOWN, serial, slot, 0);
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
