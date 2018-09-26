using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;

namespace CIIP.Designer
{

    /// <summary>
    /// 一个类继承基类或实现接口时,信息在这里描述
    /// </summary>
    [Appearance("BOBase.IsGenericParametersVisible", Method = "IsGenericParametersHide", TargetItems = nameof(GenericParameters), Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [XafDisplayName("实现接口")]
    public class ImplementInterface : BaseObject
    {
        public ImplementInterface(Session s) : base(s)
        {

        }

        /// <summary>
        /// 主业务对象
        /// </summary>
        [Association]
        [XafDisplayName("所属业务")]
        public BusinessObjectBase MasterBusinessObject
        {
            get { return GetPropertyValue<BusinessObjectBase>(nameof(MasterBusinessObject)); }
            set { SetPropertyValue(nameof(MasterBusinessObject), value); }
        }

        /// <summary>
        /// 实现了哪个接口
        /// </summary>
        [XafDisplayName("实现")]
        [RuleRequiredField]
        public Interface ImplementInterfaceInfo
        {
            get { return GetPropertyValue<Interface>(nameof(ImplementInterfaceInfo)); }
            set { SetPropertyValue(nameof(ImplementInterfaceInfo), value); }
        }

        [ToolTip("默认取接口上的默认实现,修改后使用新的设置")]
        [XafDisplayName("默认实现")]
        [RuleRequiredField]
        public BusinessObject DefaultImplement
        {
            get { return GetPropertyValue<BusinessObject>(nameof(DefaultImplement)); }
            set { SetPropertyValue(nameof(DefaultImplement), value); }
        }


        public static bool IsGenericParametersHide(ImplementInterface ins)
        {
            if (ins == null) return true;
            if (ins.ImplementInterfaceInfo == null) return true;
            return !(ins.ImplementInterfaceInfo.GenericParameterDefines.Count > 0);
        }

        /// <summary>
        /// 如果类是泛型定义,则传入泛型的参数
        /// </summary>
        [Association, DevExpress.Xpo.Aggregated]
        [XafDisplayName("输入参数")]
        public XPCollection<GenericParameterInstance> GenericParameters
        {
            get
            {
                return GetCollection<GenericParameterInstance>(nameof(GenericParameters));
            }
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading)
            {
                if(propertyName == nameof(ImplementInterfaceInfo) && newValue != null)
                {
                    DefaultImplement = ImplementInterfaceInfo.DefaultImplement;
                }
            }
        }
    }


}