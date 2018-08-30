using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using CIIP.Module.BusinessObjects;

namespace WMS
{
    /// <summary>
    /// 采购入库
    /// </summary>
    [NavigationItem("仓库管理")]
    [DefaultClassOptions]
    public class 采购入库 : 仓库单据基类<采购入库明细>
    {
        public 采购入库(Session s) : base(s)
        {

        }
        
        //public override 库存操作类型 操作类型
        //{
        //    get { return 库存操作类型.入库; }
        //}
    }
}
