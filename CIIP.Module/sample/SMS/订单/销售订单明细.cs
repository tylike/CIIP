using System;
using System.Linq;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using CIIP.Module.BusinessObjects;
using 常用基类;

namespace SMS
{
    [XafDisplayName("销售明细")]
    //[Persistent("SMSOrderItem")]
    public class 销售订单明细 : 订单明细<销售订单>
    {
        public 销售订单明细(Session s)
            : base(s)
        {

        }
        
        private decimal _利润;
        [ModelDefault("AllowEdit","False")]
        public decimal 利润
        {
            get { return _利润; }
            set { SetPropertyValue("利润", ref _利润, value); }
        }
    }
}