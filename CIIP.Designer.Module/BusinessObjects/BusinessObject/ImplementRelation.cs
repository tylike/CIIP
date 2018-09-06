using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;

namespace CIIP.Designer
{

    /// <summary>
    /// 一个类继承基类或实现接口时,信息在这里描述
    /// </summary>
    [Appearance("BOBase.IsGenericParametersVisible", Method = "IsGenericParametersHide", TargetItems = nameof(GenericParameters), Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [XafDisplayName("继承/实现接口")]
    public class ImplementRelation : BaseObject
    {
        public ImplementRelation(Session s) : base(s)
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
        /// 继承或实现了哪个类/接口
        /// </summary>
        [XafDisplayName("实现")]
        public BusinessObjectBase ImplementBusinessObject
        {
            get { return GetPropertyValue<BusinessObjectBase>(nameof(ImplementBusinessObject)); }
            set { SetPropertyValue(nameof(ImplementBusinessObject), value); }
        }

        public static bool IsGenericParametersHide(ImplementRelation ins)
        {
            if (ins == null) return true;
            if (ins.ImplementBusinessObject == null) return true;
            return !(ins.ImplementBusinessObject.GenericParameterDefines.Count > 0);
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



        //验证,一个类只能有一个基类
        //但可以实现多个接口.

        //如果主对象是接口,则只能继承接口
    }

#warning 需要验证属性名称不可以重名的情况.
#warning 此功能可以后续实现,当前可以使用复制功能直接copy已有布局
    // 业务类型上面,使用Attribute指定使用哪个布局模板
    // 系统起动时,检查所有使用了Attribute的类,遍历并进行更新

    //[LayoutTemplate(typeof(布局模板)] 
    //泛型参数类型应该是: 某单据,单据明细 两个类型.
    //这种情况,只支持两种类型,如果基类中有多个类型,就按顺序传入,反射取得,无需处理.
    //public class 某单据 :  ......
    //{
    //}
}