using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("产品管理")]
    public class 品牌 : NameObject
    {
        public 品牌(Session s):base(s)
        {

        }
    }
}