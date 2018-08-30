using System;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.ExpressApp.DC;
using CIIP.Module.BusinessObjects;

namespace WMS
{
    /// <summary>
    /// 销售出库
    /// </summary>
    [NavigationItem("仓库管理")]
    public class 销售出库 : 仓库单据基类<销售出库明细>
    {
        public 销售出库(Session s) : base(s)
        {

        }

        //public override 库存操作类型 操作类型
        //{
        //    get { return 库存操作类型.出库; }
        //}

        
    }
}