using System.Collections.Generic;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using 常用基类;
using DevExpress.ExpressApp.Model;

namespace CIIP.Module.BusinessObjects.SYS
{
    [XafDefaultProperty("DisplayName")]    
    public abstract class PropertyBase : NameObject
    {
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [XafDisplayName("显示名称")]
        public string DisplayName
        {
            get
            {
                if (OwnerBusinessObject != null)
                    return this.OwnerBusinessObject.FullName + "." + this.名称;
                return this.名称;
            }
        }

        protected virtual bool RelationPropertyNotNull
        {
            get
            {
                return false;
            }
        }

        private PropertyBase _RelationProperty;
        [XafDisplayName("关联属性"), DataSourceProperty("RelationPropertyDataSources")]
        [RuleRequiredField(TargetCriteria = "RelationPropertyNotNull"), LookupEditorMode(LookupEditorMode.AllItems)]
        public PropertyBase RelationProperty
        {
            get { return _RelationProperty; }
            set
            {
                SetPropertyValue("RelationProperty", ref _RelationProperty, value);
                if (!IsLoading && !IsSaving && value != null)
                {
                    if (value.RelationProperty != this)
                        value.RelationProperty = this;
                }
            }
        }

        protected abstract List<PropertyBase> RelationPropertyDataSources { get; }

        protected abstract BusinessObject OwnerBusinessObject
        {
            get;
        }

        public PropertyBase(Session s) : base(s)
        {
        }

        private string _DataSourceProperty;
        [XafDisplayName("数据来源属性")]
        public string DataSourceProperty
        {
            get { return _DataSourceProperty; }
            set { SetPropertyValue("DataSourceProperty", ref _DataSourceProperty, value); }
        }

        private bool? _VisibleInDetailView;
        [XafDisplayName("详细视图可见")]
        public bool? VisibleInDetailView
        {
            get { return _VisibleInDetailView; }
            set { SetPropertyValue("VisibleInDetailView", ref _VisibleInDetailView, value); }
        }

        private bool? _VisibleInListView;
        [XafDisplayName("列表视图可见")]
        public bool? VisibleInListView
        {
            get { return _VisibleInListView; }
            set { SetPropertyValue("VisibleInListView", ref _VisibleInListView, value); }
        }

        private bool? _VisibleInLookupView;
        [XafDisplayName("搜索视图可见")]
        public bool? VisibleInLookupView
        {
            get { return _VisibleInLookupView; }
            set { SetPropertyValue("VisibleInLookupView", ref _VisibleInLookupView, value); }
        }

        private bool? _Browsable;
        [XafDisplayName("可见")]
        [ToolTip("属性在任何位置是否可见")]
        public bool? Browsable
        {
            get { return _Browsable; }
            set { SetPropertyValue("Browsable", ref _Browsable, value); }
        }

        private bool? _AllowEdit;
        [XafDisplayName("允许编辑")]
        public bool? AllowEdit
        {
            get { return _AllowEdit; }
            set { SetPropertyValue("AllowEdit", ref _AllowEdit, value); }
        }

        private bool? _ImmediatePostData;
        [XafDisplayName("立即回发")]
        public bool? ImmediatePostData
        {
            get { return _ImmediatePostData; }
            set { SetPropertyValue("ImmediatePostData", ref _ImmediatePostData, value); }
        }

        private string _DisplayFormat;
        [XafDisplayName("显示格式")]
        public string DisplayFormat
        {
            get { return _DisplayFormat; }
            set { SetPropertyValue("DisplayFormat", ref _DisplayFormat, value); }
        }

        private string _EditMask;
        [XafDisplayName("编辑格式")]
        public string EditMask
        {
            get { return _EditMask; }
            set { SetPropertyValue("EditMask", ref _EditMask, value); }
        }

        private RuleRange _Range;
        [XafDisplayName("范围")]
        [VisibleInListView(false)]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public RuleRange Range
        {
            get { return _Range; }
            set { SetPropertyValue("Range", ref _Range, value); }
        }

        private bool? _RuleRequiredField;
        [XafDisplayName("必填")]
        public bool? RuleRequiredField
        {
            get { return _RuleRequiredField; }
            set { SetPropertyValue("RuleRequiredField", ref _RuleRequiredField, value); }
        }

        private bool? _UniqueValue;
        [XafDisplayName("唯一")]
        public bool? UniqueValue
        {
            get { return _UniqueValue; }
            set { SetPropertyValue("UniqueValue", ref _UniqueValue, value); }
        }
        
    }
}