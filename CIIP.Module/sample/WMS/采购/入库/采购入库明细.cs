using DevExpress.Xpo;

namespace WMS
{
    public class 采购入库明细 : 库存单据明细<采购入库>
    {
        public 采购入库明细(Session s) : base(s)
        {

        }
    }
}