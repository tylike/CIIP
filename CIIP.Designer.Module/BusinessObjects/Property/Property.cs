using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace CIIP
{
    public class Editors
    {
        public const string PropertyTypeTokenEditor = "PropertyTypeTokenEditor";
    }
}

namespace CIIP.Designer
{

    [XafDisplayName("属性")]
    [Appearance("SizeIsVisible",TargetItems ="Size",Method = "SizeIsVisible", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    public class Property : PropertyBase
    {

        [XafDisplayName("计算公式")]
        [ToolTip("填正了公式后，此属性将为只读，使用公式进行计算")]
        public string Expression
        {
            get { return GetPropertyValue<string>(nameof(Expression)); }
            set { SetPropertyValue(nameof(Expression), value); }
        }

        public static bool SizeIsVisible(Property property)
        {
            return property?.PropertyType?.FullName != typeof(string).FullName;
        }
        #region 立即回发
        [XafDisplayName("立即回发")]
        [ToolTip("当属性值发生变化后,立即通知系统,系统可以即时做计算等相关操作,通常用于公式依赖的属性,web中较为常见.")]
        public bool ImmediatePostData
        {
            get { return GetPropertyValue<bool>(nameof(ImmediatePostData)); }
            set { SetPropertyValue(nameof(ImmediatePostData), value); }
        }
        #endregion
        
        #region 显示与编辑格式
        [XafDisplayName("显示格式")]
        public string DisplayFormat
        {
            get { return GetPropertyValue<string>(nameof(DisplayFormat)); }
            set { SetPropertyValue(nameof(DisplayFormat), value); }
        }

        [XafDisplayName("编辑格式")]
        public string EditMask
        {
            get { return GetPropertyValue<string>(nameof(EditMask)); }
            set { SetPropertyValue(nameof(EditMask), value); }
        }
        #endregion

        #region 值范围
        [XafDisplayName("范围")]
        [VisibleInListView(false)]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public RuleRange Range
        {
            get { return GetPropertyValue<RuleRange>(nameof(Range)); }
            set { SetPropertyValue(nameof(Range), value); }
        }
        #endregion

        #region 验证
        #region 必填

        [XafDisplayName("必填")]
        public bool RuleRequiredField
        {
            get { return GetPropertyValue<bool>(nameof(RuleRequiredField)); }
            set { SetPropertyValue(nameof(RuleRequiredField), value); }
        }
        #endregion

        #region 唯一
        [XafDisplayName("唯一")]
        public bool UniqueValue
        {
            get { return GetPropertyValue<bool>(nameof(UniqueValue)); }
            set { SetPropertyValue(nameof(UniqueValue), value); }
        }
        #endregion
        #endregion 

        [XafDisplayName("长度")]
        public int Size
        {
            get { return GetPropertyValue<int>(nameof(Size)); }
            set { SetPropertyValue(nameof(Size), value); }
        }
        
        

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Size = 100;
        }

        public Property(Session s) : base(s)
        {

        }
    }
}