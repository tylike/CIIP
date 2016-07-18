using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace CIIP.Module.BusinessObjects.SYS
{
    public class GenericParameter : BaseObject
    {
        public GenericParameter(Session s) : base(s)
        {

        }

        private BusinessObject _Owner;

        [Association]
        [XafDisplayName("所属业务对象")]
        public BusinessObject Owner
        {
            get { return _Owner; }
            set { SetPropertyValue("Owner", ref _Owner, value); }
        }

        private string _Name;
        [XafDisplayName("参数名称")]
        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue("Name", ref _Name, value); }
        }

        private int _ParameterIndex;
        [XafDisplayName("参数顺序")]
        public int ParameterIndex
        {
            get { return _ParameterIndex; }
            set { SetPropertyValue("ParameterIndex", ref _ParameterIndex, value); }
        }

        protected bool ParameterValueMutsBeNotNull
        {
            get
            {
                if (Owner == null)
                    return true;
                return !Owner.IsGenericTypeDefine;
            }
        }

        private BusinessObjectBase _ParameterValue;
        [XafDisplayName("参数")]
        [RuleRequiredField(TargetCriteria = "ParameterValueMutsBeNotNull")]
        [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
        public BusinessObjectBase ParameterValue
        {
            get { return _ParameterValue; }
            set { SetPropertyValue("ParameterValue", ref _ParameterValue, value); }
        }

        private BusinessObject _DefaultGenericType;
        [ToolTip("当主表为范型类型时,做为基类使用时,默认派生于哪些子表类型.")]
        [XafDisplayName("默认类型")]
        public BusinessObject DefaultGenericType
        {
            get { return _DefaultGenericType; }
            set { SetPropertyValue("DefaultGenericType", ref _DefaultGenericType, value); }
        }

        private string _Constraint;
        [XafDisplayName("约束")]
        public string Constraint
        {
            get { return _Constraint; }
            set { SetPropertyValue("Constraint", ref _Constraint, value); }
        }
    }
}