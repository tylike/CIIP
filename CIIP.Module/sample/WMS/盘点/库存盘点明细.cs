using DevExpress.Xpo;

namespace WMS
{
    public class 库存盘点明细 : 库存单据明细<库存盘点>
    {
        public 库存盘点明细(Session s) : base(s)
        {

        }
    }
}