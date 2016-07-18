using DevExpress.ExpressApp.DC;
using DevExpress.Xpo;

namespace IMatrix.ERP.Module.BusinessObjects.SYS.Logic
{
    //[DisplayName("参数类型")]
    public enum ParameterType
    {
        [XafDisplayName("输入")]
        Input,
        [XafDisplayName("输出")]
        Output
    }
}