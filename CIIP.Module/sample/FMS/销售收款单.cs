using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace CIIP.Module.BusinessObjects
{
    [NavigationItem("财务管理")]
    public class 销售收款单 : 收款单
    {
        public 销售收款单(Session s) : base(s)
        {

        }
    }
}