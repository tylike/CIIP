using System.Collections.Generic;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using CIIP.Persistent.BaseImpl;
using DevExpress.ExpressApp.Model;
using System.Linq;

namespace CIIP.Module.BusinessObjects.SYS
{
    [XafDefaultProperty("DisplayName")]    
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
        public BusinessObjectBase PropertyType
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
        [ToolTip("一对多或多对多时,两个属性的对应关系.")]
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

        protected virtual List<PropertyBase> RelationPropertyDataSources
        {
            get
            {
                return PropertyType?.Properties.Where(x => x.PropertyType == BusinessObject).OfType<PropertyBase>().ToList();
            }
        }
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
        //private bool? _AllowEdit;
        //[XafDisplayName("允许编辑")]
        //public bool? AllowEdit
        //{
        //    get { return _AllowEdit; }
        //    set { SetPropertyValue("AllowEdit", ref _AllowEdit, value); }
        //}
        #endregion

        #region 立即回发
        //private bool? _ImmediatePostData;
        //[XafDisplayName("立即回发")]
        //[ToolTip("当属性值发生变化后,立即通知系统,系统可以即时做计算等相关操作,通常用于公式依赖的属性,web中较为常见.")]
        //public bool? ImmediatePostData
        //{
        //    get { return _ImmediatePostData; }
        //    set { SetPropertyValue("ImmediatePostData", ref _ImmediatePostData, value); }
        //}
        #endregion

        #region 显示与编辑格式
        //private string _DisplayFormat;
        //[XafDisplayName("显示格式")]
        //public string DisplayFormat
        //{
        //    get { return _DisplayFormat; }
        //    set { SetPropertyValue("DisplayFormat", ref _DisplayFormat, value); }
        //}

        //private string _EditMask;
        //[XafDisplayName("编辑格式")]
        //public string EditMask
        //{
        //    get { return _EditMask; }
        //    set { SetPropertyValue("EditMask", ref _EditMask, value); }
        //}
        #endregion

        #region 值范围
        //private RuleRange _Range;
        //[XafDisplayName("范围")]
        //[VisibleInListView(false)]
        //[ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        //public RuleRange Range
        //{
        //    get { return _Range; }
        //    set { SetPropertyValue("Range", ref _Range, value); }
        //}
        #endregion

        #region 验证
        #region 必填

        //private bool? _RuleRequiredField;
        //[XafDisplayName("必填")]
        //public bool? RuleRequiredField
        //{
        //    get { return _RuleRequiredField; }
        //    set { SetPropertyValue("RuleRequiredField", ref _RuleRequiredField, value); }
        //}
        //#endregion

        //#region 唯一
        //private bool? _UniqueValue;
        //[XafDisplayName("唯一")]
        //public bool? UniqueValue
        //{
        //    get { return _UniqueValue; }
        //    set { SetPropertyValue("UniqueValue", ref _UniqueValue, value); }
        //}
        #endregion
        #endregion 
        #endregion
        public PropertyBase(Session s) : base(s)
        {
        }


    }
}