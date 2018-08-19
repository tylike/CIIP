using System.ComponentModel;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using CIIP.Module.BusinessObjects.Flow;
using System.Linq;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using 常用基类;
using DevExpress.ExpressApp;
using System.Collections.Generic;
using CIIP.Module.BusinessObjects.SYS.Logic;
using CIIP;

namespace CIIP.Module.BusinessObjects.SYS
{
    public enum Modifier
    {
        [XafDisplayName("普通")]
        None,
        [XafDisplayName("抽象 - 必须被继承")]
        Abstract,
        [XafDisplayName("密封 - 不可以被继承")]
        Sealed
    }

    [XafDefaultProperty("Caption")]
    [XafDisplayName("用户业务")]
    [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
    public partial class BusinessObject : BusinessObjectBase
    {

        #region is persistent
        private bool _IsPersistent;

        [XafDisplayName("可持久化")]
        [ToolTip("即是否在数据库中创建表，可以进行读写，如果不是持久化的，则只做为组合类型时使用.")]
        [VisibleInListView(false)]
        public bool IsPersistent
        {
            get { return _IsPersistent; }
            set { SetPropertyValue("IsPersistent", ref _IsPersistent, value); }
        }
        #endregion

        #region modifier
        [XafDisplayName("继承设置")]
        [ToolTip("可以设置为无,抽象,密封的")]
        public Modifier Modifier
        {
            get
            {
                return GetPropertyValue<Modifier>(nameof(Modifier));
            }
            set
            {
                SetPropertyValue(nameof(Modifier), value);
            }
        }
        #endregion
        
        #region can custom logic
        private bool _CanCustomLogic;
        [XafDisplayName("可自定义逻辑")]
        [ModelDefault("AllowEdit", "False")]
        public bool CanCustomLogic
        {
            get { return _CanCustomLogic; }
            set { SetPropertyValue("CanCustomLogic", ref _CanCustomLogic, value); }
        }
        #endregion

        #region 属性

        public PropertyBase FindProperty(string name)
        {
            return Properties.SingleOrDefault(x => x.名称 == name);
        }

        #region logic method
        [Association, DevExpress.Xpo.Aggregated]
        [XafDisplayName("业务逻辑")]
        public XPCollection<BusinessObjectEvent> Methods
        {
            get
            {
                return GetCollection<BusinessObjectEvent>("Methods");
            }
        }
        #endregion

        #endregion

        #region option

        private bool? _IsCloneable;

        [XafDisplayName("允许复制")]
        [VisibleInListView(false)]
        public bool? IsCloneable
        {
            get { return _IsCloneable; }
            set { SetPropertyValue("IsCloneable", ref _IsCloneable, value); }
        }

        private bool? _IsVisibileInReports;

        [XafDisplayName("可做报表")]
        [VisibleInListView(false)]
        public bool? IsVisibileInReports
        {
            get { return _IsVisibileInReports; }
            set { SetPropertyValue("IsVisibileInReports", ref _IsVisibileInReports, value); }
        }

        private bool? _IsCreatableItem;

        [XafDisplayName("快速创建")]
        [VisibleInListView(false)]
        public bool? IsCreatableItem
        {
            get { return _IsCreatableItem; }
            set { SetPropertyValue("IsCreatableItem", ref _IsCreatableItem, value); }
        }



        [Browsable(false)]
        public int CreateIndex { get; set; }

        #endregion

        #region 导航设置
        List<NavigationItem> NavigationItemDataSources
        {
            get
            {
                return ModelDataSource.NavigationItemDataSources;
            }
        }

        private NavigationItem _NavigationItem;
        [ValueConverter(typeof(ModelNavigationToStringConverter))]
        [DataSourceProperty("NavigationItemDataSources")]
        [XafDisplayName("导航")]
        public NavigationItem NavigationItem
        {
            get { return _NavigationItem; }
            set { SetPropertyValue("NavigationItem", ref _NavigationItem, value); }
        }
        #endregion
        
        #region ctor
        public BusinessObject(Session s) : base(s)
        {

        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading && (propertyName == "名称" || propertyName == "Category"))
            {
                if (this.Category != null)
                    this.FullName = this.Category.FullName + "." + this.名称;
                if (propertyName == "名称" && string.IsNullOrEmpty(Caption))
                {
                    this.Caption = newValue + "";
                }
            }
            //if (propertyName == "Base" && !IsLoading 
            //    //&& !DisableCreateGenericParameterValues
            //    )
            //{
            //    Session.Delete(GenericParameterInstances);
            //    if (newValue != null)
            //    {
            //        if (!Base.IsRuntimeDefine)
            //        {
            //            var bt = ReflectionHelper.FindType(Base.FullName);
            //            if (bt.IsGenericType)
            //            {
            //                foreach (var item in bt.GetGenericArguments())
            //                {
            //                    var gp = new GenericParameterInstance(Session);
            //                    //gp.Owner = this;
            //                    gp.Name = item.Name;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            foreach (var gp in Base.GenericParameterInstances)
            //            {
            //                var ngp = new GenericParameterInstance(Session);
            //                ngp.Name = gp.Name;
            //                ngp.ParameterIndex = gp.ParameterIndex;
            //                this.GenericParameterInstances.Add(ngp);
            //            }
            //        }
            //    }
            //}
        }

        public override void AfterConstruction()
        {
            IsRuntimeDefine = true;
            this.IsPersistent = true;
            base.AfterConstruction();
        } 
        #endregion
        
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