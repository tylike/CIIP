using DevExpress.Xpo;

namespace WMS
{
    public class 销售退货明细 : 库存单据明细<销售退货>
    {
        public 销售退货明细(Session s) : base(s)
        {

        }
    }
}