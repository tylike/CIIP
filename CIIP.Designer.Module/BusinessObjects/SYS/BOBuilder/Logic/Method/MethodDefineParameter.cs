using DevExpress.ExpressApp.DC;
using DevExpress.Xpo;

namespace IMatrix.ERP.Module.BusinessObjects.SYS.Logic
{

    [XafDisplayName("方法参数")]
    public class MethodDefineParameter : VariantDefine
    {
        public MethodDefineParameter(Session s) : base(s)
        {

        }

        private ParameterType _ParameterType;


        public ParameterType ParameterType
        {
            get { return _ParameterType; }
            set { SetPropertyValue("ParameterType", ref _ParameterType, value); }
        }
    }
}