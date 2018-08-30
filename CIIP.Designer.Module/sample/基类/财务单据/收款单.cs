using DevExpress.Xpo;

namespace CIIP.Module.BusinessObjects
{
    /// <summary>
    /// 收款单
    /// </summary>
    [NonPersistent]
    public class 收款单 : 财务单据
    {
        public 收款单(Session s):base(s)
        {
            
        }
    }
}