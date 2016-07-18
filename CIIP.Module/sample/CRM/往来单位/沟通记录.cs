using CIIP.Module.BusinessObjects;
using DevExpress.Xpo;

namespace CRM
{
    public class 沟通记录 : 沟通记录Base
    {
        public 沟通记录(Session s) : base(s)
        {
        }

        private 往来单位 _往来单位;
        [Association]
        public 往来单位 往来单位
        {
            get { return _往来单位; }
            set { SetPropertyValue("往来单位", ref _往来单位, value); }
        }
    }
}