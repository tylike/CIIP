using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("产品管理")]
    public class 产品分类 : NameObject
    {
        public 产品分类(Session s) : base(s)
        {

        }
    }
}