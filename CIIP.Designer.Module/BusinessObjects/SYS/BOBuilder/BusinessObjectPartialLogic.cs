using DevExpress.ExpressApp.DC;
using DevExpress.Xpo;

namespace CIIP.Module.BusinessObjects.SYS
{
    [XafDisplayName("·Ö²¿Âß¼­")]
    public class BusinessObjectPartialLogic : BusinessObjectPartialLogicBase
    {
        public BusinessObjectPartialLogic(Session s) : base(s)
        {

        }

        public override string Template
        {
            get
            {
                return
                    $@"namespace {this.BusinessObject.Category.FullName}
{{
    public partial class {this.BusinessObject.Ãû³Æ}
    {{

    }}
}}
";
            }
        }
    }
}