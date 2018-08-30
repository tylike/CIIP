using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using CIIP.Module.BusinessObjects;

namespace WMS
{
    //选择仓库、库位后，自动生成当前库存中有的产品的条目，盘点人员依次录入，与当前数目比对后，生成对应的报损报益单
    [NavigationItem("仓库管理")]
    public class 库存盘点 : 仓库单据基类<库存盘点明细>
    {
        public 库存盘点(Session s) : base(s)
        {

        }

        //public override 库存操作类型 操作类型
        //{
        //    get { return 库存操作类型.不操作; }
        //}
    }
}