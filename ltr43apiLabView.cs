using System;
using System.Runtime.InteropServices;

namespace ltrModulesNet
{
    public class ltr43apiLabView : _ltr43api
    {

        public double StreamReadRate;

        public int FrameSize;	  // ���-�� ��� � �����
        public int Baud;		  // �������� ������ � �����
        public int StopBit;	  // ���-�� ����-���
        public int Parity;		  // ��������� ���� ��������
        public int SendTimeoutMultiplier; // ��������� �������� ��������
        public int ReceiveTimeoutMultiplier; // ��������� �������� ������ �������������

        public int SecondMark_Mode; // ����� �����. 0 - �����., 1-�����.+�����, 2-�����
        public int StartMark_Mode; // 

        public int Port1;	   // ����������� ����� �����/������ ������ 1
        public int Port2;	   // ����������� ����� �����/������ ������ 2 
        public int Port3;    // ����������� ����� �����/������ ������ 3 
        public int Port4;	   // ����������� ����� �����/������ ������ 4 

        public ltr43apiLabView()
        {
			GetConfigFromModule();
        }

        void GetConfigFromModule()
        {
            StreamReadRate = module.StreamReadRate;

            FrameSize = module.RS485.FrameSize;	  // ���-�� ��� � �����
            Baud = module.RS485.Baud;		  // �������� ������ � �����
            StopBit = module.RS485.StopBit;	  // ���-�� ����-���
            Parity = module.RS485.Parity;		  // ��������� ���� ��������
            SendTimeoutMultiplier = module.RS485.SendTimeoutMultiplier; // ��������� �������� ��������
            ReceiveTimeoutMultiplier = module.RS485.ReceiveTimeoutMultiplier; // ��������� �������� ������ �������������

            SecondMark_Mode = module.Marks.SecondMark_Mode; // ����� �����. 0 - �����., 1-�����.+�����, 2-�����
            StartMark_Mode = module.Marks.StartMark_Mode; // 

            Port1 = module.IO_Ports.Port1;
            Port2 = module.IO_Ports.Port2;
            Port3 = module.IO_Ports.Port3;
            Port4 = module.IO_Ports.Port4;
        }

        void SetConfigToModule()
        {
            module.StreamReadRate = StreamReadRate;

            module.RS485.FrameSize = FrameSize;	  // ���-�� ��� � �����
            module.RS485.Baud = Baud;		  // �������� ������ � �����
            module.RS485.StopBit = StopBit;	  // ���-�� ����-���
            module.RS485.Parity = Parity;		  // ��������� ���� ��������
            module.RS485.SendTimeoutMultiplier = SendTimeoutMultiplier; // ��������� �������� ��������
            module.RS485.ReceiveTimeoutMultiplier = ReceiveTimeoutMultiplier; // ��������� �������� ������ �������������

            module.Marks.SecondMark_Mode = SecondMark_Mode; // ����� �����. 0 - �����., 1-�����.+�����, 2-�����
            module.Marks.StartMark_Mode = StartMark_Mode; // 

            module.IO_Ports.Port1 = Port1;
            module.IO_Ports.Port2 = Port2;
            module.IO_Ports.Port3 = Port3;
            module.IO_Ports.Port4 = Port4;
        }

        public _LTRNative.LTRERROR Recv(uint[] data, uint size, uint timeout)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR43_Recv(ref module, data, null, size, timeout);
            GetConfigFromModule(); 
            return res;
        }


        public _LTRNative.LTRERROR ProcessData(uint[] src, uint[] dest, int size)
        {
            SetConfigToModule();            
            _LTRNative.LTRERROR res = LTR43_ProcessData(ref module, src, dest, ref size);           
            GetConfigFromModule(); 
            return res;
        }


        public override _LTRNative.LTRERROR Config()
        {
            SetConfigToModule();         
            _LTRNative.LTRERROR res = LTR43_Config(ref module);
            GetConfigFromModule(); 
            return res;
        }

        public override _LTRNative.LTRERROR Init()
        {            
            _LTRNative.LTRERROR res = LTR43_Init(ref module);
            GetConfigFromModule(); 
            return res;
        }


        public override _LTRNative.LTRERROR Open(uint net_addr, ushort net_port,
            char[] crate_sn, int slot_num)
        {
			if (net_addr ==0) net_addr = NewTLTR43.Channel.saddr;
			if (net_port ==0) net_port = NewTLTR43.Channel.sport;
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR43_Open(ref module, net_addr, net_port, crate_sn, slot_num);
            GetConfigFromModule(); 
            return res;
        }

        public override _LTRNative.LTRERROR Close()
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR43_Close(ref module);
            GetConfigFromModule(); 
            return res;
        }


        public override _LTRNative.LTRERROR WritePort(uint OutputData)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR43_WritePort(ref module, OutputData);
            GetConfigFromModule(); 
            return res;
        }


        public _LTRNative.LTRERROR ReadPort(uint[] InputData)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR43_ReadPort(ref module, ref InputData[0]);
            GetConfigFromModule(); 
            return res;
        }


        public override _LTRNative.LTRERROR WriteArray(uint[] OutputArray,
            byte ArraySize)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR43_WriteArray(ref module, OutputArray, ArraySize);
            GetConfigFromModule(); 
            return res;
        }


        public override _LTRNative.LTRERROR StartStreamRead()
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR43_StartStreamRead(ref module);
            GetConfigFromModule(); 
            return res;
        }


        public override _LTRNative.LTRERROR StopStreamRead()
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR43_StopStreamRead(ref module);
            GetConfigFromModule(); 
            return res;
        }

        public override _LTRNative.LTRERROR StartSecondMark()
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR43_StopSecondMark(ref module);
            GetConfigFromModule(); 
            return res;
        }


        public override _LTRNative.LTRERROR StopSecondMark()
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR43_StopSecondMark(ref module);
            GetConfigFromModule(); 
            return res;
        }


        public override _LTRNative.LTRERROR MakeStartMark()
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR43_MakeStartMark(ref module);
            GetConfigFromModule(); 
            return res;
        }


        public override _LTRNative.LTRERROR RS485_Exchange(short[] PackToSend,
            short[] ReceivedPack, int OutPackSize,
            int InPackSize)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR43_RS485_Exchange(ref module, PackToSend, ReceivedPack, OutPackSize, InPackSize);
            GetConfigFromModule();
            return res;
        }

        public override _LTRNative.LTRERROR WriteEEPROM(int Address, byte val)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR43_WriteEEPROM(ref module, Address, val);
            GetConfigFromModule(); 
            return res;
        }


        public override _LTRNative.LTRERROR ReadEEPROM(int Address, byte[] val)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR43_ReadEEPROM(ref module, Address, val);
            GetConfigFromModule(); 
            return res;
        }


        public override _LTRNative.LTRERROR RS485_TestReceiveByte(int OutBytesQnt, int InBytesQnt)
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR43_RS485_TestReceiveByte(ref module, OutBytesQnt, InBytesQnt);
            GetConfigFromModule(); 
            return res;
        }


        public override _LTRNative.LTRERROR RS485_TestStopReceive()
        {
            SetConfigToModule();
            _LTRNative.LTRERROR res = LTR43_RS485_TestStopReceive(ref module);
            GetConfigFromModule(); 
            return res;
        }

        
    }
}
