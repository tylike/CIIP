using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("基础信息")]
    public class 城市 : NameObject
    {
        public 城市(Session s)
            : base(s)
        {

        }

        private 省份 _省份;
        [Association]
        public 省份 省份
        {
            get { return _省份; }
            set { SetPropertyValue("省份", ref _省份, value); }
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


        [Association,DevExpress.Xpo.Aggregated]
        public XPCollection<销售区域> 销售区域
        {
            get
            {
                return GetCollection<销售区域>("销售区域");
            }
        }
    }
}