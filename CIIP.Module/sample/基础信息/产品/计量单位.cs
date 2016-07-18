using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects
{
    [NavigationItem("产品管理")]
    [DefaultClassOptions]
    public class 计量单位 : NameObject
    {
        public 计量单位(Session s):base(s)
        {

        }
    }
}