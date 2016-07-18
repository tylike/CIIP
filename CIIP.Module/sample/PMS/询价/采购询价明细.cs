using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using CIIP.Module.BusinessObjects;
using 常用基类;

namespace PMS
{
    public class 采购询价明细 : 订单明细<采购询价>
    {
        public 采购询价明细(Session s) : base(s)
        {

        }

        private decimal _已生成采购订单数量;
        [VisibleInDetailView(false), VisibleInListView(false)]
        public decimal 已生成采购订单数量
        {
            get { return _已生成采购订单数量; }
            set { SetPropertyValue("已生成采购订单数量", ref _已生成采购订单数量, value); }
        }


    }
}