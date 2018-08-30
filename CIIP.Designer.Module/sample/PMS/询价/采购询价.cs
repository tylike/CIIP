using System;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using CIIP.Module.BusinessObjects;
using 常用基类;

namespace PMS
{
    /// <summary>
    /// 采购询价单
    /// </summary>
    [DefaultClassOptions]
    [NavigationItem("采购管理")]
    public class 采购询价 : 订单<采购询价明细>
    {
        public 采购询价(Session s) : base(s)
        {

        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (this.业务员 != null)
            {
                this.客户 = this.业务员.往来单位;
            }
        }
    }
}