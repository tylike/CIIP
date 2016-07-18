using DevExpress.Xpo;

namespace CIIP.Module.BusinessObjects
{
    /// <summary>
    /// 付款单
    /// </summary>
    [NonPersistent]
    public class 付款单 : 财务单据
    {
        public 付款单(Session s) : base(s)
        {

        }
    }
}