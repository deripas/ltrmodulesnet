using System;
using System.Text;

namespace ltrModulesNet
{
    public class ltr34apiLabView:_ltr34api
    {
        public uint[] LChTbl;                  // Таблица логических каналов
        //**** настройки модуля           
        public byte FrequencyDivisor;            // делитель частоты дискретизации 0..60 (31.25..500 кГц)
        public byte ChannelQnt;             // число каналов 0, 1, 2, 3 соотвествнно (1, 2, 4, 8)        
        public bool UseClb;        
        public bool AcknowledgeType;             // тип подтверждения true - высылать подтверждение каждого слова, false- высылать состояние буффера каждые 100 мс        
        public bool ExternalStart;               // внешний старт true - внешний старт, false - внутренний        
        public bool RingMode;                    // режим кольца  true - режим кольца, false - потоковый режим        
        public bool BufferFull;					// статус - буффер переполнен - ошибка        
        public bool BufferEmpty;					// статус - буффер пуст - ошибка        
        public bool DACRunning;					// статус - запущена ли генерация

        public float FrequencyDAC;				// статус - частота - на которую настроен ЦАП в текущей конфигурации

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
