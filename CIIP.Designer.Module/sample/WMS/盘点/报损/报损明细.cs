using DevExpress.Xpo;

namespace WMS
{
    public class 报损明细 : 库存单据明细<报损单>
    {
        public 报损明细(Session s) : base(s)
        {

        }
    }
}