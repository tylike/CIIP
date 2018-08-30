using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using CIIP;
using 常用基类;

namespace WMS
{
    public class 库位 : NameObject
    {
        public 库位(Session s)
            : base(s)
        {

        }
        
        仓库 _仓库;
        [Association]
        public 仓库 仓库
        {
            get
            {
                return _仓库;
            }
            set
            {
                SetPropertyValue("仓库", ref _仓库, value);
            }
        }
    }
}