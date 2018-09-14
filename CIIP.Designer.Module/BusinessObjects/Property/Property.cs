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
        private string _Expression;

        [XafDisplayName("计算公式")]
        [ToolTip("填正了公式后，此属性将为只读，使用公式进行计算")]
        public string Expression
        {
            get { return _Expression; }
            set { SetPropertyValue("Expression", ref _Expression, value); }
        }

        public static bool SizeIsVisible(Property property)
        {
            return property?.PropertyType?.FullName != typeof(string).FullName;
        }
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

        private int _Size;
        [XafDisplayName("长度")]
        public int Size
        {
            get { return _Size; }
            set { SetPropertyValue("Size", ref _Size, value); }
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