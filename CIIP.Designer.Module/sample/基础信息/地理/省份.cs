using DevExpress.ExpressApp.DC;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects
{
    [XafDisplayName("销售区域")]
    //[ToolTip("通常是省范围")]
    public class 省份 : NameObject
    {
        public 省份(Session s)
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

        [Association,DevExpress.Xpo.Aggregated]
        public XPCollection<城市> 城市
        {
            get
            {
                return GetCollection<城市>("城市");
            }
        }
    }
}