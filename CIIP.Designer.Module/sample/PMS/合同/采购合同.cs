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
    public class 采购合同 : 订单<采购合同明细>
    {
        public 采购合同(Session s)
            : base(s)
        {

        }
    }
}