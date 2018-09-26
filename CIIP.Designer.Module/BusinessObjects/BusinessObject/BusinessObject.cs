using System.ComponentModel;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.Linq;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using CIIP.Persistent.BaseImpl;
using System.Collections.Generic;
using CIIP.Module.BusinessObjects.SYS.Logic;
using CIIP;

namespace CIIP.Designer
{

    [XafDefaultProperty(nameof(Caption))]
    [XafDisplayName("用户业务")]
    [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
    [DefaultClassOptions]
    public partial class BusinessObject : BusinessObjectBase
    {
        #region is persistent
        [XafDisplayName("可持久化")]
        [ToolTip("即是否在数据库中创建表，可以进行读写，如果不是持久化的，则只做为组合类型时使用.")]
        [VisibleInListView(false)]
        public bool IsPersistent
        {
            get { return GetPropertyValue<bool>(nameof(IsPersistent)); }
            set { SetPropertyValue(nameof(IsPersistent), value); }
        }
        #endregion

        #region can custom logic
        [XafDisplayName("可自定义逻辑")]
        [ModelDefault("AllowEdit", "False")]
        public bool CanCustomLogic
        {
            get { return GetPropertyValue<bool>(nameof(CanCustomLogic)); }
            set { SetPropertyValue(nameof(CanCustomLogic), value); }
        }
        #endregion

        #region 属性



        #region logic method
        //[Association, DevExpress.Xpo.Aggregated]
        //[XafDisplayName("业务逻辑")]
        //public XPCollection<BusinessObjectEvent> Methods
        //{
        //    get
        //    {
        //        return GetCollection<BusinessObjectEvent>("Methods");
        //    }
        //}
        #endregion

        #endregion

        #region option


        [XafDisplayName("允许复制")]
        [VisibleInListView(false)]
        public bool IsCloneable
        {
            get { return GetPropertyValue<bool>(nameof(IsCloneable)); }
            set { SetPropertyValue(nameof(IsCloneable), value); }
        }


        [XafDisplayName("可做报表")]
        [VisibleInListView(false)]
        public bool IsVisibileInReports
        {
            get { return GetPropertyValue<bool>(nameof(IsVisibileInReports)); }
            set { SetPropertyValue(nameof(IsVisibileInReports), value); }
        }


        [XafDisplayName("快速创建")]
        [VisibleInListView(false)]
        public bool IsCreatableItem
        {
            get { return GetPropertyValue<bool>(nameof(IsCreatableItem)); }
            set { SetPropertyValue(nameof(IsCreatableItem), value); }
        }



        [Browsable(false)]
        public int CreateIndex { get; set; }

        public override bool CanCreateAssocication => IsRuntimeDefine;

        #endregion

        #region ctor
        public BusinessObject(Session s) : base(s)
        {

        }



        public override void AfterConstruction()
        {
            IsRuntimeDefine = true;
            IsPersistent = true;
            base.AfterConstruction();
        }
        #endregion

        [DataSourceCriteria("DomainObjectModifier != 'Sealed'")]
        [XafDisplayName("继承")]
        public BusinessObject Base
        {
            get { return GetPropertyValue<BusinessObject>(nameof(Base)); }
            set { SetPropertyValue(nameof(Base), value); }
        }

        public override SystemCategory SystemCategory => SystemCategory.BusinessObject;
    }

    //

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