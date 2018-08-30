using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects
{
    [NavigationItem("基础信息")]
    public class 结算方式 : NameObject
    {
        public 结算方式(Session s) : base(s)
        {

        }
    }
}