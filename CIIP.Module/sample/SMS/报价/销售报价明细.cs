using System;
using DevExpress.Xpo;
using System.Linq;
using System.Collections.Generic;
using CIIP.Module.BusinessObjects;
using 常用基类;

namespace SMS
{
    public class 销售报价明细 : 订单明细<销售报价>
    {
        public 销售报价明细(Session s):base(s)
        {
            
        }
    }
}