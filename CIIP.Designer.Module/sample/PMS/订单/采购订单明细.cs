using System;
using System.Linq;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using CIIP.Module.BusinessObjects;
using 常用基类;

namespace PMS
{
    public class 采购订单明细 : 订单明细<采购订单>
    {
        public 采购订单明细(Session s) : base(s)
        {

        }
    }
}