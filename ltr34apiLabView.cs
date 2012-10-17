using System;
using System.Text;

namespace ltrModulesNet
{
    public class ltr34apiLabView:_ltr34api
    {
        public uint[] LChTbl;                  // ������� ���������� �������
        //**** ��������� ������           
        public byte FrequencyDivisor;            // �������� ������� ������������� 0..60 (31.25..500 ���)
        public byte ChannelQnt;             // ����� ������� 0, 1, 2, 3 ������������ (1, 2, 4, 8)        
        public bool UseClb;        
        public bool AcknowledgeType;             // ��� ������������� true - �������� ������������� ������� �����, false- �������� ��������� ������� ������ 100 ��        
        public bool ExternalStart;               // ������� ����� true - ������� �����, false - ����������        
        public bool RingMode;                    // ����� ������  true - ����� ������, false - ��������� �����        
        public bool BufferFull;					// ������ - ������ ���������� - ������        
        public bool BufferEmpty;					// ������ - ������ ���� - ������        
        public bool DACRunning;					// ������ - �������� �� ���������

        public float FrequencyDAC;				// ������ - ������� - �� ������� �������� ��� � ������� ������������

        public ltr34apiLabView()
        {
            GetConfigFromModule();
        }

        void GetConfigFromModule()
        {
            LChTbl = module.LChTbl;					
            FrequencyDivisor = module.FrequencyDivisor;
            ChannelQnt = module.ChannelQnt;			
            UseClb = module.UseClb;				
            AcknowledgeType = module.AcknowledgeType;

            ExternalStart = module.ExternalStart;	            
            RingMode = module.RingMode;
            BufferFull = module.BufferFull;
            BufferEmpty = module.BufferEmpty;
            DACRunning = module.DACRunning;
        }

        void SetConfigToModule()
        {
            module.LChTbl  =LChTbl;
            module.FrequencyDivisor = FrequencyDivisor;
            module.ChannelQnt = ChannelQnt;
            module.UseClb = UseClb;
            module.AcknowledgeType = AcknowledgeType;

            module.ExternalStart = ExternalStart;            
            module.RingMode = RingMode;
            module.BufferFull = BufferFull;
            module.BufferEmpty = BufferEmpty;
            module.DACRunning = DACRunning;
        }

        public virtual _LTRNative.LTRERROR Recv(uint[] data, uint size, uint timeout)
        {
            return LTR34_Recv(ref module, data, null, size, timeout);
        }

        public override _LTRNative.LTRERROR Config()
        {
            SetConfigToModule();
            return base.Config();
        }
    }
}
