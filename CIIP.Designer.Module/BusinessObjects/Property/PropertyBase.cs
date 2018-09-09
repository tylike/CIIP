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

namespace CIIP.Designer
{
    /// <summary>
    /// 
    /// </summary>
    [XafDefaultProperty("DisplayName")]
    [Appearance("PropertyBase.RelationIsEnable", TargetItems = "RelationProperty", Enabled = false, Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Method = "RelationIsEnable")]
    [Appearance("PropertyBase.RelationPropertyStateByAutoCreate", TargetItems = "RelationProperty", Criteria = "AutoCreateRelationProperty", Enabled = false)]
    //对多对时,自动创建是必须的.
    //一对多时,可选手动创建,默认是自动创建.
    public abstract class PropertyBase : NameObject
    {
        private string _Expression;

        [XafDisplayName("计算公式")]
        [ToolTip("填正了公式后，此属性将为只读，使用公式进行计算")]
        public string Expression
        {
            get { return _Expression; }
            set { SetPropertyValue("Expression", ref _Expression, value); }
        }

        public static bool RelationIsEnable(PropertyBase obj)
        {
            return obj != null && obj.PropertyType is SimpleType;
        }

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
        [EditorAlias(Editors.PropertyTypeTokenEditor),DataSourceProperty(nameof(PropertyTypes))]
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
        
        private BusinessObject[] types;

        protected IEnumerable<BusinessObjectBase> PropertyTypes
        {
            get
            {
                if (types == null)
                {
                    types = Session.Query<BusinessObject>().Where(x => x.IsPersistent).ToArray();
                }
                return types;
            }
        }

        [ToolTip("用于显示在界面上的标题内容.")]
        [XafDisplayName("标题")]
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
        protected virtual bool RelationPropertyNotNull
        {
            get
            {
                return false;
            }
        }

        private PropertyBase _RelationProperty;
        [XafDisplayName("关联属性"), DataSourceProperty("RelationPropertyDataSources")]
        [RuleRequiredField(TargetCriteria = "RelationPropertyNotNull && !AutoCreateRelationProperty"), LookupEditorMode(LookupEditorMode.AllItems)]
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

        [CaptionsForBoolValues("自动", "手动")]
        [XafDisplayName("创建关联属性")]
        [ImmediatePostData]
        public bool AutoCreateRelationProperty
        {
            get { return GetPropertyValue<bool>(nameof(AutoCreateRelationProperty)); }
            set { SetPropertyValue(nameof(AutoCreateRelationProperty), value); }
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (IsLoading || IsSaving)
                return;

            if (propertyName == nameof(AutoCreateRelationProperty))
            {
                if (AutoCreateRelationProperty)
                {
                    RelationProperty = null;
                }
            }
            if (propertyName == nameof(PropertyType) && AutoCreateRelationProperty)
            {
                if (PropertyType != null)
                {
                    if (string.IsNullOrEmpty(Name))
                    {
                        Name = PropertyType.Name;
                        Caption = PropertyType.Caption;
                    }

                    //查找一个属性,修改为自动创建一个.
                    //if (RelationProperty == null)
                    //{
                    //    try
                    //    {
                    //        RelationProperty = PropertyType.Properties.SingleOrDefault(x => x.PropertyType.Oid == this.BusinessObject.Oid);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        throw ex;
                    //    }
                    //}

                    //if (RelationProperty == null)
                    //{
                    //    RelationProperty = PropertyType.Properties.SingleOrDefault(x => x.PropertyType.Oid == this.BusinessObject.Oid);
                    //}
                }
                else
                {
                    Name = "";
                }

            }

        }

        [ModelDefault("AllowEdit","False")]
        public bool IsAutoCreated
        {
            get { return GetPropertyValue<bool>(nameof(IsAutoCreated)); }
            set { SetPropertyValue(nameof(IsAutoCreated), value); }
        }





        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Browsable = true;
            AutoCreateRelationProperty = true;
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

        #region 立即回发
        private bool? _ImmediatePostData;
        [XafDisplayName("立即回发")]
        [ToolTip("当属性值发生变化后,立即通知系统,系统可以即时做计算等相关操作,通常用于公式依赖的属性,web中较为常见.")]
        public bool? ImmediatePostData
        {
            get { return _ImmediatePostData; }
            set { SetPropertyValue("ImmediatePostData", ref _ImmediatePostData, value); }
        }
        #endregion

        #region 显示与编辑格式
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
        #endregion

        #region 值范围
        private RuleRange _Range;
        [XafDisplayName("范围")]
        [VisibleInListView(false)]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public RuleRange Range
        {
            get { return _Range; }
            set { SetPropertyValue("Range", ref _Range, value); }
        }
        #endregion

        #region 验证
        #region 必填

        private bool? _RuleRequiredField;
        [XafDisplayName("必填")]
        public bool? RuleRequiredField
        {
            get { return _RuleRequiredField; }
            set { SetPropertyValue("RuleRequiredField", ref _RuleRequiredField, value); }
        }
        #endregion

        #region 唯一
        private bool? _UniqueValue;
        [XafDisplayName("唯一")]
        public bool? UniqueValue
        {
            get { return _UniqueValue; }
            set { SetPropertyValue("UniqueValue", ref _UniqueValue, value); }
        }
        #endregion
        #endregion 
        #endregion
        public PropertyBase(Session s) : base(s)
        {
        }


    }
}