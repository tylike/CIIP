using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using CIIP;
using 常用基类;

namespace WMS
{
    [DefaultClassOptions]
    [NavigationItem("仓库管理")]
    public class 仓库 : NameObject
    {
        public 仓库(Session s):base(s)
        {

        }
        
        string _详细地址;

        public string 详细地址
        {
            get
            {
                return _详细地址;
            }
            set
            {
                SetPropertyValue("详细地址", ref _详细地址, value);
            }
        }

        [Association,Aggregated]
        public XPCollection<库位> 库位
        {
            get
            {
                return GetCollection<库位>("库位");
            }
        }
    }
}