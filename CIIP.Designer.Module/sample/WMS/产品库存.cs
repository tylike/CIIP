using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Persistent.Validation;
using System.ComponentModel;
using CIIP.Module.BusinessObjects.Product;

namespace WMS
{
    [NavigationItem("产品库存")]
    public class 产品库存 : XPLiteObject
    {
        protected 产品库存(Session s):base(s)
        {

        }

        [Key, Persistent, Browsable(false)]
        public InvertoryKey Key { get; set; }

        decimal _数量;
        public decimal 数量
        {
            get
            {
                return _数量;
            }
            set
            {
                SetPropertyValue("数量", ref _数量, value);
            }
        }

        产品 _产品;
        [RuleRequiredField]
        public 产品 产品
        {
            get
            {
                return _产品;
            }
            set
            {
                SetPropertyValue("产品", ref _产品, value);
            }
        }

        库位 _库位;
        [RuleRequiredField]
        public 库位 库位
        {
            get
            {
                return _库位;
            }
            set
            {
                SetPropertyValue("库位", ref _库位, value);
            }
        }
    }


}