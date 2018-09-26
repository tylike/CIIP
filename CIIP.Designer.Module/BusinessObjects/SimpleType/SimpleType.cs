using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace CIIP.Designer
{
    [XafDisplayName("¼òµ¥ÀàÐÍ")]
    [DefaultClassOptions]
    public class SimpleType : BusinessObjectBase
    {
        public SimpleType(Session s) : base(s)
        {
        }
        public override bool CanCreateAssocication => false;
        public override BusinessObjectModifier DomainObjectModifier { get => Designer.BusinessObjectModifier.Sealed; set { } }

        public override SystemCategory SystemCategory => SystemCategory.SimpleType;

        public override string GetCode()
        {
            return null;
        }
    }
}