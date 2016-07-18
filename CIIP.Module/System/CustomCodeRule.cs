using DevExpress.Persistent.Validation;

namespace CIIP
{
    public class CIIPDebugger
    {
        public static System.Action<object> ShowView;
        public static void Break(object obj)
        {
            if (ShowView != null)
            {
                ShowView(obj);
            }
        }
    }

    [CodeRule]
    public class CustomCodeRule : RuleBase<ICustomValidate>
    {
        protected override bool IsValidInternal(
            ICustomValidate target, out string errorMessageTemplate)
        {
            return target.IsValidate(out errorMessageTemplate,Properties,this);
        }

        public CustomCodeRule() : base("", "Save")
        {
        }

        public CustomCodeRule(IRuleBaseProperties properties) : base(properties)
        {
        }
    }
}