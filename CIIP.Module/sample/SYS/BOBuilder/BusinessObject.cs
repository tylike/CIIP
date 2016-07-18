using System.ComponentModel;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using CIIP.Module.BusinessObjects.Flow;
using System.Linq;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using DevExpress.Persistent.BaseImpl;
using 常用基类;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base.General;
using System;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Xpo.Metadata;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Utils;
using System.Collections.Generic;
using CIIP.Module.BusinessObjects.SYS.Logic;
using DevExpress.Data.Filtering;
using CIIP;

namespace CIIP.Module.BusinessObjects.SYS
{
    [XafDefaultProperty("Caption")]
    [XafDisplayName("用户业务")]
    [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
    //[ChildrenType(typeof(MethodDefine))]
    public class BusinessObject : BusinessObjectBase
    {
        private bool _CanCustomLogic;
        [XafDisplayName("可自定义逻辑")]
        [ModelDefault("AllowEdit","False")]
        public bool CanCustomLogic
        {
            get { return _CanCustomLogic; }
            set { SetPropertyValue("CanCustomLogic", ref _CanCustomLogic, value); }
        }

        private bool _IsPersistent;

        [XafDisplayName("可持久化")]
        [ToolTip("即是否在数据库中创建表，可以进行读写，如果不是持久化的，则只做为组合类型时使用.")]
        [VisibleInListView(false)]
        public bool IsPersistent
        {
            get { return _IsPersistent; }
            set { SetPropertyValue("IsPersistent", ref _IsPersistent, value); }
        }

        private bool _CanInherits;

        [XafDisplayName("可被继承")]
        [VisibleInListView(false)]
        public bool CanInherits
        {
            get { return _CanInherits; }
            set { SetPropertyValue("CanInherits", ref _CanInherits, value); }
        }

        private bool _IsAbstract;

        [XafDisplayName("抽象基类")]
        [ToolTip("指本类型是不可以被创建实例的，仅用于继承时使用.")]
        [VisibleInListView(false)]
        public bool IsAbstract
        {
            get { return _IsAbstract; }
            set
            {
                SetPropertyValue("IsAbstract", ref _IsAbstract, value);

                if (!IsLoading)
                {
                    if (value)
                    {
                        CanInherits = true;
                    }
                }
            }
        }

#warning 考虑使用IsSystem来处理？
        /// <summary>
        /// 用于在自动生成系统类型时，不要自动生成泛型参数
        /// </summary>
        [Browsable(false)]
        [NonPersistent]
        public bool DisableCreateGenericParameterValues;

        private BusinessObject _Base;

        [XafDisplayName("继承")]
        [RuleRequiredField]
        [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
        public BusinessObject Base
        {
            get { return _Base; }
            set
            {
                SetPropertyValue("Base", ref _Base, value);
            }
        }

        private bool _IsGenericTypeDefine;
        public bool IsGenericTypeDefine
        {
            get { return _IsGenericTypeDefine; }
            set { SetPropertyValue("IsGenericTypeDefine", ref _IsGenericTypeDefine, value); }
        }
        
        //[Appearance("基类泛型参数可见", TargetItems = "GenericParameters", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        protected bool GenericParameterIsVisible()
        {
            if (Base != null)
            {
                if (Base.IsRuntimeDefine)
                    return true;
                var bt = ReflectionHelper.FindType(Base.FullName);
                if (bt != null)
                {
                    return !bt.IsGenericType;
                }
            }
            return true;
        }
        
        [Association, DevExpress.Xpo.Aggregated]
        [XafDisplayName("属性")]
        public XPCollection<Property> Properties
        {
            get { return GetCollection<Property>("Properties"); }
        }

        [Association, DevExpress.Xpo.Aggregated]
        [XafDisplayName("集合属性")]
        public XPCollection<CollectionProperty> CollectionProperties
        {
            get { return GetCollection<CollectionProperty>("CollectionProperties"); }
        }

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

        private bool _IsRuntimeDefine;

        [XafDisplayName("动态定义")]
        [ToolTip("为假时是通过代码方式上传的模块生成的。否则是在界面上定义并生成的。")]
        public bool IsRuntimeDefine
        {
            get { return _IsRuntimeDefine; }
            set { SetPropertyValue("IsRuntimeDefine", ref _IsRuntimeDefine, value); }
        }

        [Browsable(false)]
        public int CreateIndex { get; set; }

        public BusinessObject(Session s) : base(s)
        {

        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading && (propertyName == "名称" || propertyName == "Category"))
            {
                this.FullName = this.Category.FullName + "." + this.名称;
                if (propertyName == "名称" && string.IsNullOrEmpty(Caption))
                {
                    this.Caption = newValue + "";
                }
            }
            if (propertyName == "Base" && !IsLoading && !DisableCreateGenericParameterValues)
            {
                Session.Delete(GenericParameters);
                if (newValue != null)
                {
                    var bt = ReflectionHelper.FindType(Base.FullName);
                    if (bt.IsGenericType)
                    {
                        foreach (var item in bt.GetGenericArguments())
                        {
                            var gp = new GenericParameter(Session);
                            gp.Owner = this;
                            gp.Name = item.Name;
                        }
                    }
                }
            }

            if ((propertyName == "Base" || propertyName == "IsAbstract") && !IsLoading)
            {
                SetupConfig();
            }
        }

        public override void AfterConstruction()
        {
            IsRuntimeDefine = true;
            this.IsPersistent = true;
            this.CanInherits = true;
            base.AfterConstruction();
        }

        public PropertyBase FindProperty(string name)
        {
            var sp = Properties.SingleOrDefault(x => x.名称 == name);
            if (sp != null)
            {
                return sp;
            }
            return CollectionProperties.SingleOrDefault(x => x.名称 == name);
        }

        [Association, DevExpress.Xpo.Aggregated]
        [XafDisplayName("传入基类泛型参数")]
        public XPCollection<GenericParameter> GenericParameters
        {
            get { return GetCollection<GenericParameter>("GenericParameters"); }
        }

        private BusinessConfigBase _Config;
        public BusinessConfigBase Config
        {
            get { return _Config; }
            set { SetPropertyValue("Config", ref _Config, value); }
        }

        private void SetupConfig()
        {
            if (IsRuntimeDefine && !IsAbstract && Base != null)
            {
                var bt = ReflectionHelper.FindType(Base.FullName);
                if (bt != null)
                {
                    var config = (BusinessConfigTypeAttribute)bt.GetCustomAttributes(typeof(BusinessConfigTypeAttribute), false).FirstOrDefault();

                    if (config != null)
                    {
                        if (Config != null)
                        {
                            if (config.BusinessConfig.FullName == Config.GetType().FullName)
                                return;
                            Config.Delete();
                        }
                        var info = XafTypesInfo.Instance.FindTypeInfo(config.BusinessConfig);
                        Config = info.CreateInstance(Session) as BusinessConfigBase;
                    }
                }
            }
            if(Base == null)
            {
                if (Config != null)
                    Config.Delete();
                Config = null;
            }
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (Config != null)
                Config.Type = this.FullName;

        }
        List<NavigationItem> _navigationItemDataSources;
        List<NavigationItem> NavigationItemDataSources
        {
            get
            {
                if(_navigationItemDataSources == null)
                {
                    _navigationItemDataSources = new List<NavigationItem>();
                    var model = CaptionHelper.ApplicationModel as IModelApplicationNavigationItems;
                    foreach (var item in model.NavigationItems.Items)
                    {
                        var ni = new NavigationItem(item);
                        _navigationItemDataSources.Add(ni);
                    }
                }
                return _navigationItemDataSources;
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
        
        //[Association, DevExpress.Xpo.Aggregated]
        //public XPCollection<MethodDefine> Methods
        //{
        //    get
        //    {
        //        return GetCollection<MethodDefine>("Methods");
        //    }
        //}

        [Association, DevExpress.Xpo.Aggregated]
        [XafDisplayName("业务逻辑")]
        public XPCollection<MethodDefine> Methods
        {
            get
            {
                return GetCollection<MethodDefine>("Methods");
            }
        }

        public Property AddProperty(string name, BusinessObjectBase type, int? size = null)
        {
            var property = new Property(Session);
            property.PropertyType = type;
            property.名称 = name;
            if (size.HasValue)
                property.Size = size.Value;
            this.Properties.Add(property);
            return property;
        }

        public Property AddProperty<T>(string name, int? size = null)
        {
            return AddProperty(name,
                Session.FindObject<BusinessObjectBase>(new BinaryOperator("FullName", typeof (T).FullName))
                , size
                );
        }

        public CollectionProperty AddAssociation(string name, BusinessObject bo, bool isAggregated,Property relation)
        {
            var cp = new CollectionProperty(Session);
            cp.名称 = name;
            cp.Aggregated = isAggregated;
            cp.PropertyType = bo;
            cp.RelationProperty = cp;
            this.CollectionProperties.Add(cp);
            return cp;
        }

        public ObjectAfterConstruction AddAfterConstruction(string code)
        {
            var after = new ObjectAfterConstruction(Session);
            after.Code = code;
            Methods.Add(after);
            return after;
        }

        public ObjectSavingEvent AddSavingEvent(string code)
        {
            var after = new ObjectSavingEvent(Session);
            after.Code = code;
            Methods.Add(after);
            return after;
        }

        public ObjectDeletingEvent AddDeletingEvent(string code)
        {
            var after = new ObjectDeletingEvent(Session);
            after.Code = code;
            Methods.Add(after);
            return after;
        }

        public ObjectPropertyChangedEvent AddPropertyChangedEvent(string code)
        {
            var after = new ObjectPropertyChangedEvent(Session);
            after.Code = code;
            Methods.Add(after);
            return after;
        }
        public ObjectSavedEvent AddObjectSavedEvent(string code)
        {
            var after = new ObjectSavedEvent(Session);
            after.Code = code;
            Methods.Add(after);
            return after;
        }

        public BusinessObjectPartialLogic AddPartialLogic(string code)
        {
            var logic = new BusinessObjectPartialLogic(Session);
            logic.BusinessObject = this;
            logic.Code = code;
            return logic;
        }


#warning 需要验证属性名称不可以重名的情况.
    }

    [XafDisplayName("分部逻辑")]
    public class BusinessObjectPartialLogic : NameObject
    {
        public BusinessObjectPartialLogic(Session s) : base(s)
        {

        }

        private BusinessObject _BusinessObject;

        public BusinessObject BusinessObject
        {
            get { return _BusinessObject; }
            set { SetPropertyValue("BusinessObject", ref _BusinessObject, value); }
        }

        private string _Code;

        [Size(-1)]
        [EditorAlias("CodeEditor")]
        public string Code
        {
            get { return _Code; }
            set { SetPropertyValue("Code", ref _Code, value); }
        }
    }


    public class ModelNavigationToStringConverter : ValueConverter
    {
        public override Type StorageType
        {
            get
            {
                return typeof(string);
            }
        }

        public override object ConvertFromStorageType(object value)
        {
            if (value == null)
                return null;
            var model = CaptionHelper.ApplicationModel as IModelApplicationNavigationItems;
            return model.NavigationItems.AllItems.SingleOrDefault(x => (x as ModelNode).Path == (string) value);
        }

        public override object ConvertToStorageType(object value)
        {
            var v = (NavigationItem) value;
            return v?.Key;
        }
    }

    [DomainComponent]
    public class NavigationItem : ITreeNode
    {
        public string Key
        {
            get
            {
                if(ModelItem == null)
                {
                    return (ModelItem as ModelNode).Path;
                }
                return "";
            }
        }

        public IModelNavigationItem ModelItem { get; }
        public NavigationItem(IModelNavigationItem modelItem)
        {
            this.ModelItem = modelItem;
        }
        BindingList<NavigationItem> _childrens;
        public IBindingList Children
        {
            get
            {
                if (_childrens == null)
                {
                    _childrens = new BindingList<NavigationItem>();
                    foreach (var item in ModelItem.Items)
                    {
                        _childrens.Add(new NavigationItem(item));
                    }
                }
                return _childrens;
            }
        }

        public string Name
        {
            get
            {
                return ModelItem.Caption;
            }
        }
        NavigationItem _parent;
        public ITreeNode Parent
        {
            get
            {
                if (ModelItem.Parent.Parent != null && _parent == null)
                {
                    if (ModelItem.Parent.Parent is IModelNavigationItem)
                        _parent = new NavigationItem(ModelItem.Parent.Parent as IModelNavigationItem);
                }
                return _parent;
            }
        }
    }

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