using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace CIIP.Designer
{
    [DefaultClassOptions]
    [XafDisplayName("接口定义")]
    public class Interface : BusinessObjectBase
    {
        public Interface(Session s) : base(s)
        {

        }

        public override BusinessObjectModifier DomainObjectModifier { get => BusinessObjectModifier.Abstract; set { } }

        public override bool CanCreateAssocication => false;

        [ToolTip("指定一个默认实现类,在制作一个业务类时,选择了此接口,则使用这个默认实现类,避免重复的实现相同的接口.实现了类似于多继承的功能.")]
        [XafDisplayName("默认实现")]
        public BusinessObject DefaultImplement
        {
            get { return GetPropertyValue<BusinessObject>(nameof(DefaultImplement)); }
            set { SetPropertyValue(nameof(DefaultImplement), value); }
        }
    }
}