using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace CIIP.Module.BusinessObjects.SYS
{
    [XafDisplayName("¼òµ¥ÀàÐÍ")]
    [DefaultClassOptions]
    public class SimpleType : BusinessObjectBase
    {
        public SimpleType(Session s) : base(s)
        {
        }

        public override Modifier DomainObjectModifier { get => SYS.Modifier.Sealed; set { } }
    }
}