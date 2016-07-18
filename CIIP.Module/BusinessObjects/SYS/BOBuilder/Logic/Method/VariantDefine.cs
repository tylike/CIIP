using System.Collections.Generic;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.Linq;
namespace IMatrix.ERP.Module.BusinessObjects.SYS.Logic
{
    [XafDisplayName("变量定义")]
    public class VariantDefine : AssignValue
    {
        public VariantDefine(Session s):base(s)
        {

        }

        private BusinessObjectBase _Type;

        [XafDisplayName("类型")]
        [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
        public BusinessObjectBase Type
        {
            get { return _Type; }
            set { SetPropertyValue("Type", ref _Type, value); }
        }

        public override List<VariantDefine> GetReadableVariants()
        {
            return null;
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (this.Type != null)
                this.名称 = this.Type.Caption + " " + this.Name;
        }
        public override bool IsValidate(out string message, IRuleBaseProperties properties, RuleBase rule)
        {
            var existedNames = base.GetReadableVariants();
            if (existedNames != null)
            {
                if (existedNames.Any(x => x.CodeVariant == this.CodeVariant))
                {
                    message = "已经存在此变量名称,请修改!";
                    return false;
                }
            }
            return base.IsValidate(out message, properties, rule);
        }
    }
}