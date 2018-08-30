using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects
{
    [NavigationItem("系统设置")]
    public class 常用税率 : NameObject
    {
        public 常用税率(Session s) : base(s)
        {

        }

        private decimal _税率;
        public decimal 税率
        {
            get { return _税率; }
            set { SetPropertyValue("税率", ref _税率, value); }
        }

    }
}