using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects
{
    [XafDefaultProperty("Title")]
    [NavigationItem("往来单位")]
    public class 地址 : NameObject
    {
        public 地址(Session s) : base(s)
        {

        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public string Title
        {
            get { return 名称 + "-" + 详细地址; }
        }

        private 往来单位 _往来单位;
        [Association]
        public 往来单位 往来单位
        {
            get { return _往来单位; }
            set { SetPropertyValue("往来单位", ref _往来单位, value); }
        }

        private string _详细地址;
        public string 详细地址
        {
            get { return _详细地址; }
            set { SetPropertyValue("详细地址", ref _详细地址, value); }
        }

        protected override void OnSaving()
        {
            if(往来单位.默认地址 == null)
            {
                this.往来单位.默认地址 = this;
            }
            base.OnSaving();
        }
    }
}