using System;
using System.Text;

namespace ltrModulesNet
{
    public class ltr42apiLabView:_ltr42api
    {
        public bool AckEna;
		public int SecondMark_Mode; // Режим меток. 0 - внутр., 1-внутр.+выход, 2-внешн
		public int StartMark_Mode; // 

        public ltr42apiLabView()
        {
            GetConfigFromModule();
        }

		void GetConfigFromModule()
		{
			AckEna = module.AckEna;
			SecondMark_Mode = module.Marks.SecondMark_Mode;
			StartMark_Mode = module.Marks.StartMark_Mode;
		}

		void SetConfigToModule()
		{
			module.AckEna = AckEna;
			module.Marks.SecondMark_Mode=SecondMark_Mode;
			module.Marks.StartMark_Mode=StartMark_Mode;
		}

        public override _LTRNative.LTRERROR Config()
        {
            SetConfigToModule();
            return LTR42_Config(ref module);
        }
    }
}
