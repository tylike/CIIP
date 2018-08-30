using System;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using CIIP.Module.BusinessObjects;

namespace WMS
{
    /// <summary>
    /// 销售退货
    /// </summary>
    [NavigationItem("仓库管理")]
    public class 销售退货 : 仓库单据基类<销售退货明细>
    {
        public 销售退货(Session s):base(s)
        {
            
        }

        //public override 库存操作类型 操作类型
        //{
        //    get { return 库存操作类型.入库; }
        //}
    }
}