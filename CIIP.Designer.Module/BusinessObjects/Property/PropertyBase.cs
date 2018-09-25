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
    [Appearance("HiddenAssicationInfo",TargetItems =nameof(AssocicationInfo), Enabled =false,Visibility = ViewItemVisibility.Hide,Criteria = "!IsAssocication")]
    [Appearance("HiddenIsAssocication", TargetItems = nameof(IsAssocication), Enabled = false, Visibility = ViewItemVisibility.Hide, Criteria = "PropertyType is null or !PropertyType.CanCreateAssocication")]
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

        [XafDisplayName("类型"), RuleRequiredField]
        [ImmediatePostData]
        [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
        //[EditorAlias(Editors.PropertyTypeTokenEditor)]
        [DataSourceProperty(nameof(PropertyTypes))]
        public virtual BusinessObjectBase PropertyType
        {
            get { return GetPropertyValue<BusinessObjectBase>(nameof(PropertyType)); }
            set
            {
                SetPropertyValue("PropertyType", value);
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
        
        #region 可见性
        [XafDisplayName("可见")]
        [ToolTip("属性在任何位置是否可见")]
        public bool Browsable
        {
            get { return GetPropertyValue<bool>(nameof(Browsable)); }
            set { SetPropertyValue(nameof(Browsable), value); }
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


        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Browsable = true;
            AllowEdit = true;
        }

        //[ModelDefault("AllowEdit", "False")]
        [XafDisplayName("配置")]
        [EditorAlias("OPE"),Association,VisibleInDetailView(true)]
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
        [XafDisplayName("允许编辑")]
        public bool AllowEdit
        {
            get { return GetPropertyValue<bool>(nameof(AllowEdit)); ; }
            set { SetPropertyValue(nameof(AllowEdit),value); }
        }
        #endregion


        #endregion

        [XafDisplayName("关系"),ImmediatePostData]
        public bool IsAssocication
        {
            get { return GetPropertyValue<bool>(nameof(IsAssocication)); }
            set { SetPropertyValue(nameof(IsAssocication), value); }
        }

        [XafDisplayName("说明")]
        [Size(-1)]
        [ModelDefault("RowCount","2")]
        public string Memo
        {
            get { return GetPropertyValue<string>(nameof(Memo)); }
            set { SetPropertyValue(nameof(Memo), value); }
        }

        public PropertyBase(Session s) : base(s)
        {
        }
    }
}