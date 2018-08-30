using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using CIIP.Module.BusinessObjects;

namespace WMS
{
    [NavigationItem("仓库管理")]
    public class 调拨入库 : 仓库单据基类<调拨入库明细>
    {
        public 调拨入库(Session s) : base(s)
        {

        }

        //public override 库存操作类型 操作类型
        //{
        //    get
        //    {
        //        return 库存操作类型.入库;
        //    }
        //}
    }
}