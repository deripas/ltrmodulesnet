using System;
using System.Text;

namespace ltrModulesNet
{
    public class ltr41apiLabView:_ltr41api
    {
        public double StreamReadRate;
		public int SecondMark_Mode; // Режим меток. 0 - внутр., 1-внутр.+выход, 2-внешн
		public int StartMark_Mode; // 

        public ltr41apiLabView()
        {
            GetConfigFromModule();
        }

		void GetConfigFromModule()
		{
			StreamReadRate=module.StreamReadRate;
			SecondMark_Mode = module.Marks.SecondMark_Mode;
			StartMark_Mode = module.Marks.StartMark_Mode;
		}

		void SetConfigToModule()
		{
			module.StreamReadRate=StreamReadRate;
			module.Marks.SecondMark_Mode=SecondMark_Mode;
			module.Marks.StartMark_Mode=StartMark_Mode;
		}


        public virtual _LTRNative.LTRERROR Recv(uint[] data, uint size, uint timeout)
        {
            return LTR41_Recv(ref module, data, null, size, timeout);
        }


        public _LTRNative.LTRERROR ReadPort(ushort [] InputData)
        {
            return LTR41_ReadPort(ref module, ref InputData[0]);
        }

        public override _LTRNative.LTRERROR Config()
        {
            SetConfigToModule();
            return base.Config();
        }
    }
}
