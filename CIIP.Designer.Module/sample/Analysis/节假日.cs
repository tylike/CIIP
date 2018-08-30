using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects.Analysis
{
    [DefaultClassOptions]
    [NavigationItem("系统设置")]
    public class 节假日 : NameObject
    {
        public 节假日(Session s) : base(s)
        {

        }
    }
}