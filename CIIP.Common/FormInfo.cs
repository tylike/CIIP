using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CIIP
{
    [NonPersistent]
    public class FormInfo : BaseObject
    {
        public FormInfo(Session s) : base(s)
        {

        }

        [XafDisplayName("表单名称")]
        public string FullName { get; set; }

        [XafDisplayName("显示名称")]
        public string Caption { get; set; }

        [XafDisplayName("基类名称")]
        public string BaseTypeName { get; set; }

        [XafDisplayName("基类显示名称")]
        public string BaseTypeCaption { get; set; }
    }
}