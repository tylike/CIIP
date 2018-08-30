using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects
{
    [XafDisplayName("城市区域")]
    [NavigationItem("往来单位")]
    public class 销售区域 : NameObject
    {
        public 销售区域(Session s)
            : base(s)
        {

        }

        private decimal _经度;
        public decimal 经度
        {
            get { return _经度; }
            set { SetPropertyValue("经度", ref _经度, value); }
        }

        private decimal _纬度;
        public decimal 纬度
        {
            get { return _纬度; }
            set { SetPropertyValue("纬度", ref _纬度, value); }
        }


        private 城市 _城市;
        [Association]
        public 城市 城市
        {
            get { return _城市; }
            set { SetPropertyValue("城市", ref _城市, value); }
        }
    }
}