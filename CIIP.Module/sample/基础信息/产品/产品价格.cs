using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Xpo;

namespace CIIP.Module.BusinessObjects.Product
{
    [NonPersistent]
    [XafDefaultProperty("产品")]
    public class 产品价格 : XPLiteObject
    {
        public 产品价格(Session s) : base(s)
        {

        }

        private 产品 _产品;

        public 产品 产品
        {
            get { return _产品; }
            set { SetPropertyValue("产品", ref _产品, value); }
        }


        private decimal _价格;

        public decimal 价格
        {
            get { return _价格; }
            set { SetPropertyValue("价格", ref _价格, value); }
        }


        private 计量单位 _单位;

        public 计量单位 单位
        {
            get { return _单位; }
            set { SetPropertyValue("单位", ref _单位, value); }
        }



        private string _来源;
        [ModelDefault("AllowEdit","False")]
        public string 来源
        {
            get { return _来源; }
            set { SetPropertyValue("来源", ref _来源, value); }
        }
    }
}