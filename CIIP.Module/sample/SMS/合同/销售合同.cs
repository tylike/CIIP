using System;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using CIIP.Module.BusinessObjects;
using 常用基类;

namespace SMS
{
    [DefaultClassOptions]
    [NavigationItem("销售管理")]
    public class 销售合同 : 订单<销售合同明细>
    {
        public 销售合同(Session s)
            : base(s)
        {

        }
    }
}