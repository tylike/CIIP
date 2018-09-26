using DevExpress.ExpressApp.DC;

namespace CIIP.Designer
{
    public enum BusinessObjectModifier
    {
        [XafDisplayName("普通")]
        None,
        [XafDisplayName("抽象 - 必须被继承")]
        Abstract,
        [XafDisplayName("密封 - 不可以被继承")]
        Sealed
    }
}