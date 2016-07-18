using DevExpress.Persistent.Validation;

namespace CIIP
{
    public interface ICustomValidate
    {
        bool IsValidate(out string message, IRuleBaseProperties properties,RuleBase rule);
    }
}