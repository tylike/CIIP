using DevExpress.Xpo;

namespace CRM
{
    public class 销售线索沟通记录 : 沟通记录Base
    {
        public 销售线索沟通记录(Session s) : base(s)
        {
        }

        private 销售线索 _销售线索;
        [Association]
        public 销售线索 销售线索
        {
            get { return _销售线索; }
            set { SetPropertyValue("销售线索", ref _销售线索, value); }
        }
    }
}