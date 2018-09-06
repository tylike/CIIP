using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace CIIP.Designer
{
    /// <summary>
    /// 泛型参数实例
    /// 用于派生类中传给基类需要的泛型参数
    /// </summary>
    public class GenericParameterInstance : BaseObject
    {
        public GenericParameterInstance(Session session) : base(session)
        {
        }

        //public class ClassName<TItem,MyT> : Form<TItem,客户类型>
        //where MyT : class|interface|类型|接口|enum|delegate 等.

        private ImplementRelation _Owner;

        [Association]
        [XafDisplayName("所属业务对象")]
        [ToolTip("指这个参数实例是在哪个业务对象中定义的")]
        public ImplementRelation Owner
        {
            get { return _Owner; }
            set { SetPropertyValue("Owner", ref _Owner, value); }
        }

        [XafDisplayName("接收参数对象")]
        [ToolTip("可能是基类,也可以传给继承的接口")]
        public BusinessObject TargetBusinessObject
        {
            get { return GetPropertyValue<BusinessObject>(nameof(TargetBusinessObject)); }
            set { SetPropertyValue(nameof(TargetBusinessObject), value); }
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

        //protected bool ParameterValueMutsBeNotNull
        //{
        //    get
        //    {
        //        if (Owner == null)
        //            return true;
        //        return !Owner.IsGenericTypeDefine;
        //    }
        //}

        private BusinessObjectBase _ParameterValue;
        [XafDisplayName("参数")]
        //[RuleRequiredField(TargetCriteria = "ParameterValueMutsBeNotNull")]
        [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
        public BusinessObjectBase ParameterValue
        {
            get { return _ParameterValue; }
            set { SetPropertyValue("ParameterValue", ref _ParameterValue, value); }
        }

        private BusinessObject _DefaultGenericType;

#warning 可能是不合理的,需要优化
        [ToolTip("当主表为泛型类型时,做为基类使用时,默认派生于哪些子表类型.")]
        [XafDisplayName("默认类型")]
        public BusinessObject DefaultGenericType
        {
            get { return _DefaultGenericType; }
            set { SetPropertyValue("DefaultGenericType", ref _DefaultGenericType, value); }
        }
    }

}