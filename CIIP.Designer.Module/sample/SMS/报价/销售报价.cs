using System;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using CIIP.Module.BusinessObjects;
using 常用基类;

namespace SMS
{
    [DefaultClassOptions,NavigationItem("销售管理")]
    public class 销售报价 : 订单<销售报价明细>
    {
        public 销售报价(Session s) : base(s)
        {

        }
    }
}