using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CIIP.Module.BusinessObjects
{
    public class 单据流程记录明细 : BaseObject
    {
        public 单据流程记录明细(Session s):base(s)
        {

        }

        private 单据流程状态记录 _Master;
        [Association]
        public 单据流程状态记录 Master
        {
            get { return _Master; }
            set { SetPropertyValue("Master", ref _Master, value); }
        }



        private string _来源;

        public string 来源
        {
            get { return _来源; }
            set { SetPropertyValue("来源", ref _来源, value); }
        }

        private string _目标;

        public string 目标
        {
            get { return _目标; }
            set { SetPropertyValue("目标", ref _目标, value); }
        }
    }
}