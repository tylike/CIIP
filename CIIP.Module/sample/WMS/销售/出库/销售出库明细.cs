using DevExpress.Xpo;

namespace WMS
{
    public class 销售出库明细 : 库存单据明细<销售出库>
    {
        public 销售出库明细(Session s) : base(s)
        {

        }
    }
}