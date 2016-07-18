using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects
{
    /// <summary>
    /// 财务单据基类
    /// </summary>
    [NonPersistent]
    public class 财务单据 : 单据
    {
        public 财务单据(Session s) : base(s)
        {

        }
    }
}