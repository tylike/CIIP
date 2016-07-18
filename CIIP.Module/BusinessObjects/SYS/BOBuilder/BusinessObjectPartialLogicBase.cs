using CIIP.Module.BusinessObjects.SYS.Logic;
using DevExpress.Xpo;

namespace CIIP.Module.BusinessObjects.SYS
{
    public class BusinessObjectPartialLogicBase : CodeFile
    {
        private BusinessObject _BusinessObject;

        public BusinessObjectPartialLogicBase(Session s) : base(s)
        {
        }

        [System.ComponentModel.Browsable(false)]
        public virtual string Template
        {
            get
            {
                return
                    $@"namespace 
{{
    public partial class 
    {{

    }}
}}
";
            }
        }

        public BusinessObject BusinessObject
        {
            get { return _BusinessObject; }
            set
            {
                SetPropertyValue("BusinessObject", ref _BusinessObject, value);
                if (!IsLoading && !IsSaving && value != null)
                {
                    OnSetBusinessObject(value);
                }
            }
        }

        public override string GetFileName()
        {
            return this.BusinessObject.FullName + "_PartialLogic";
        }

        protected virtual void OnSetBusinessObject(BusinessObject value)
        {
            this.Ãû³Æ = value.Ãû³Æ;
            var temp = BusinessObject.CommonUsing();
            temp += Template;
            this.Code = new CsharpCode(temp, this);
        }
    }

}