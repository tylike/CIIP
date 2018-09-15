using System.Collections.Generic;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using CIIP.Persistent.BaseImpl;
using DevExpress.ExpressApp.Model;
using System.Linq;
using DevExpress.ExpressApp.ConditionalAppearance;
using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Editors;

namespace CIIP.Designer
{

    /// <summary>
    /// 
    /// </summary>
    [XafDefaultProperty("DisplayName")]
    //[Appearance("PropertyBase.RelationPropertyStateByAutoCreate", TargetItems = "RelationProperty", Criteria = "AutoCreateRelationProperty", Enabled = false)]
    //对多对时,自动创建是必须的.
    //一对多时,可选手动创建,默认是自动创建.
    public abstract class PropertyBase : NameObject
    {
        #region 所属业务
        [Association]
        public BusinessObjectBase BusinessObject
        {
            get
            {
                return GetPropertyValue<BusinessObjectBase>(nameof(BusinessObject));
            }
            set
            {
                SetPropertyValue(nameof(BusinessObject), value);
            }
        }
        #endregion

        #region 基本
        private BusinessObjectBase _PropertyType;

        [XafDisplayName("类型"), RuleRequiredField]
        [ImmediatePostData]
        [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
        //[EditorAlias(Editors.PropertyTypeTokenEditor)]
        [DataSourceProperty(nameof(PropertyTypes))]
        public virtual BusinessObjectBase PropertyType
        {
            get { return _PropertyType; }
            set
            {
                SetPropertyValue("PropertyType", ref _PropertyType, value);
                if (!IsLoading)
                {
                    if (PropertyType != null)
                    {
                        if (string.IsNullOrEmpty(Name))
                        {
                            Name = PropertyType.Caption;
                        }
                    }
                }
            }
        }

        protected BusinessObjectBase[] propertyTypes;

        protected virtual IEnumerable<BusinessObjectBase> PropertyTypes
        {
            get
            {
                if (propertyTypes == null)
                {
                    propertyTypes = Session.Query<BusinessObjectBase>().ToArray();
                }
                return propertyTypes;
            }
        }

        [ToolTip("用于显示在界面上的标题内容.")]
        [XafDisplayName("标题")]
        [ImmediatePostData]
        public string Caption
        {
            get { return GetPropertyValue<string>(nameof(Caption)); }
            set { SetPropertyValue(nameof(Caption), value); }
        }


        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [XafDisplayName("显示名称")]
        public string DisplayName
        {
            get
            {
                if (BusinessObject != null)
                    return this.BusinessObject.FullName + "." + this.Name;
                return this.Name;
            }
        }
        #endregion

        #region 关联属性:一对多或多对多时,两个属性的对应关系.
        //protected virtual bool RelationPropertyNotNull
        //{
        //    get
        //    {
        //        return false;
        //    }
        //}

        //private PropertyBase _RelationProperty;
        //[XafDisplayName("关联属性"), DataSourceProperty("RelationPropertyDataSources")]
        //[RuleRequiredField(TargetCriteria = "RelationPropertyNotNull"), LookupEditorMode(LookupEditorMode.AllItems)]
        //[ToolTip("一对多或多对多时,两个属性的对应关系.")]
        //public PropertyBase RelationProperty
        //{
        //    get { return _RelationProperty; }
        //    set
        //    {
        //        SetPropertyValue("RelationProperty", ref _RelationProperty, value);
        //        if (!IsLoading && !IsSaving && value != null)
        //        {
        //            if (value.RelationProperty != this)
        //                value.RelationProperty = this;
        //        }
        //    }
        //}

        //protected virtual List<PropertyBase> RelationPropertyDataSources
        //{
        //    get
        //    {
        //        return PropertyType?.Properties.Where(x => x.PropertyType == BusinessObject).OfType<PropertyBase>().ToList();
        //    }
        //}
        #endregion

        #region 可见性
        private bool? _Browsable;
        [XafDisplayName("可见")]
        [ToolTip("属性在任何位置是否可见")]
        public bool? Browsable
        {
            get { return _Browsable; }
            set { SetPropertyValue("Browsable", ref _Browsable, value); }
        }
        #endregion

        public void CalcNameCaption()
        {
            if (PropertyType == null) return;
            if (string.IsNullOrEmpty(Name))
            {
                Name = PropertyType.Name;
                Caption = PropertyType.Caption;
            }
        }
        //protected override void OnChanged(string propertyName, object oldValue, object newValue)
        //{
        //    base.OnChanged(propertyName, oldValue, newValue);
        //    if (IsLoading) return;
        //    if(propertyName == nameof(Name))
        //    {
        //        AssocicationInfo?.CalcName();
        //        CalcNameCaption();
        //    }
        //}
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Browsable = true;
        }

        //[ModelDefault("AllowEdit", "False")]
        [XafDisplayName("配置")]
        [EditorAlias(EditorAliases.ObjectPropertyEditor)]
        public AssocicationInfo AssocicationInfo
        {
            get { return GetPropertyValue<AssocicationInfo>(nameof(AssocicationInfo)); }
            set { SetPropertyValue(nameof(AssocicationInfo), value); }
        }

        #region 在客户端进行配置即可
        //private string _DataSourceProperty;
        //[XafDisplayName("数据来源属性")]
        //public string DataSourceProperty
        //{
        //    get { return _DataSourceProperty; }
        //    set { SetPropertyValue("DataSourceProperty", ref _DataSourceProperty, value); }
        //}

        //[XafDisplayName("失效处理")]
        //[ToolTip("当数据来源属性中指定的数据为空时,如何处理")]
        //public DataSourcePropertyIsNullMode DataSourcePropertyIsNullMode
        //{
        //    get { return GetPropertyValue<DataSourcePropertyIsNullMode>(nameof(DataSourcePropertyIsNullMode)); }
        //    set { SetPropertyValue(nameof(DataSourcePropertyIsNullMode), value); }
        //}

        //[XafDisplayName("备用条件")]
        //[ToolTip("当数据来源属性中指定的数据为空时,再次使用这个条件进行查询,前提是:失效处理中选择了CustomCriteria(自定义条件)")]
        //public string DataSourceIsNullCriteria
        //{
        //    get { return GetPropertyValue<string>(nameof(DataSourceIsNullCriteria)); }
        //    set { SetPropertyValue(nameof(DataSourceIsNullCriteria), value); }
        //}

        //private bool? _VisibleInDetailView;
        //[XafDisplayName("详细视图可见")]
        //public bool? VisibleInDetailView
        //{
        //    get { return _VisibleInDetailView; }
        //    set { SetPropertyValue("VisibleInDetailView", ref _VisibleInDetailView, value); }
        //}

        //private bool? _VisibleInListView;
        //[XafDisplayName("列表视图可见")]
        //public bool? VisibleInListView
        //{
        //    get { return _VisibleInListView; }
        //    set { SetPropertyValue("VisibleInListView", ref _VisibleInListView, value); }
        //}

        //private bool? _VisibleInLookupView;
        //[XafDisplayName("搜索视图可见")]
        //public bool? VisibleInLookupView
        //{
        //    get { return _VisibleInLookupView; }
        //    set { SetPropertyValue("VisibleInLookupView", ref _VisibleInLookupView, value); }
        //}

        #region 允许编辑
        private bool? _AllowEdit;
        [XafDisplayName("允许编辑")]
        public bool? AllowEdit
        {
            get { return _AllowEdit; }
            set { SetPropertyValue("AllowEdit", ref _AllowEdit, value); }
        }
        #endregion


        #endregion

        public PropertyBase(Session s) : base(s)
        {
        }


    }

    public class PropertyBaseViewController : ObjectViewController<ObjectView, PropertyBase>
    {
        public PropertyBaseViewController()
        {
            var action = new SimpleAction(this, "CreateRelationProperty", "CreateRelationProperty");
            action.Caption = "创建";
            action.ImageName = "Action_New";
            action.Execute += Action_Execute;
        }

        private void Action_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //var os = this.ObjectSpace.CreateNestedObjectSpace();
            //var obj = CreateRelationProperty(os.GetObject(this.ViewCurrentObject), os);

            //os.Committed += (s, evt) =>
            //{
            //    this.ViewCurrentObject.AssocicationInfo.LeftProperty = this.ObjectSpace.GetObject(obj);

            //};

            //var view = Application.CreateDetailView(os, obj, true);
            //e.ShowViewParameters.CreatedView = view;
            //e.ShowViewParameters.Context = TemplateContext.PopupWindow;
            //e.ShowViewParameters.NewWindowTarget = NewWindowTarget.Separate;
            //e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            //var dc = new DialogController();

            //dc.Accepting += (s, evt) =>
            //{
            //    //os.CommitChanges();
            //};

            //e.ShowViewParameters.Controllers.Add(dc);

        }

        //public PropertyBase CreateRelationProperty(PropertyBase currentProperty, IObjectSpace os)
        //{
        //    PropertyBase property;
        //    if (currentProperty.AssocicationInfo.ManyToMany)
        //    {
        //        //当前是xpcollection<学生> 学生s {get;} 属性
        //        //自动创建的属性是 xpcollection<教师> 教师s {get;} 属性
        //        property = new CollectionProperty((os as XPObjectSpace).Session, currentProperty.AssocicationInfo);// os.CreateObject<CollectionProperty>();
        //    }
        //    else
        //    {
        //        //当前是xpcollection<order> orders {get;} 属性
        //        //自动创建的属性是 customer customer {get;} 属性
        //        property = os.CreateObject<Property>();
        //    }
        //    property.BusinessObject = currentProperty.PropertyType;
        //    property.PropertyType = currentProperty.BusinessObject;
        //    property.Name = currentProperty.BusinessObject.Name;
        //    property.Caption = currentProperty.BusinessObject.Caption;
        //    return property;
        //}
    }
}