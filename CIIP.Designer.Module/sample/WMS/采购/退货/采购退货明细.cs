using DevExpress.Xpo;

namespace WMS
{
    public class 采购退货明细 : 库存单据明细<采购退货>
    {
        public 采购退货明细(Session s) : base(s)
        {

        }
    }
}