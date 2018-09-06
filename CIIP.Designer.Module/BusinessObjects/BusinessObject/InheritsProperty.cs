using DevExpress.ExpressApp.DC;

namespace CIIP.Designer
{
    [XafDisplayName("继承属性")]
    [DomainComponent]
    public class InheritsProperty
    {
        public InheritsProperty(string name, string typeName, string owner)
        {

        }
        [XafDisplayName("名称")]
        public string Name { get; set; }

        [XafDisplayName("类型")]
        public string TypeName { get; set; }

        [XafDisplayName("所在类型")]
        public string Owner { get; set; }

    }
}