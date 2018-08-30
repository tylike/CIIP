using System;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using CIIP.Module.BusinessObjects;
using 常用基类;

namespace PMS
{
    [DefaultClassOptions]
    [NavigationItem("采购管理")]
    public class 采购订单 : 订单<采购订单明细>
    {
        public 采购订单(Session s)
            : base(s)
        {

        }
    }
}

