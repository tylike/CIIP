using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Utils;
using System.Collections.Generic;

namespace CIIP.Module.BusinessObjects.Flow
{
    [XafDisplayName("属性映射")]
    public abstract class PropertyMapping : BaseObject
    {
        public PropertyMapping(Session s) : base(s)
        {

        }

        private string _FromBill;
        [XafDisplayName("来源单据")]
        [RuleRequiredField]
        public string FromBill
        {
            get { return _FromBill; }
            set { SetPropertyValue("FromBill", ref _FromBill, value); }
        }

        private string _ToBill;
        [XafDisplayName("目标单据")]
        [RuleRequiredField]
        public string ToBill
        {
            get { return _ToBill; }
            set { SetPropertyValue("ToBill", ref _ToBill, value); }
        }

        protected abstract List<StringObject> 来源属性数据源
        {
            get;
        }

        protected abstract List<StringObject> 目标属性数据源
        {
            get;
        }


        private StringObject _FromProperty;
        [XafDisplayName("来源属性")]
        [RuleRequiredField]
        //[EditorAlias(EditorAliases.PopupExpressionPropertyEditor)]
        //[ElementTypeProperty("TargetObjectType")]
        [ValueConverter(typeof(StringObjectToStringConverter))]
        [DataSourceProperty("来源属性数据源")]
        public StringObject FromProperty
        {
            get { return _FromProperty; }
            set { SetPropertyValue("FromProperty", ref _FromProperty, value); }
        }
        
        private StringObject _ToProperty;
        [XafDisplayName("目标属性")]
        [ValueConverter(typeof(StringObjectToStringConverter))]
        [DataSourceProperty("目标属性数据源")]
        [RuleRequiredField]
        public StringObject ToProperty
        {
            get { return _ToProperty; }
            set { SetPropertyValue("ToProperty", ref _ToProperty, value); }
        }

#warning todo:需要制做一个表达式编辑器，实现自动提示 目标字段=目标字段+来源字段

        private string _Expression;
        [XafDisplayName("表达式")]
        [ToolTip("当填写了表达式时，则不使用来源属性的设置，而先计算表达式的值将结果附与目标属性.语法：[字段名] 为来源单据/明细的字段名，@To.[字段名],为目标字段名 @To.数量 + 数量 意义是：目标属性=目标.数量+来源.数量。")]
        //[EditorAlias(EditorAliases.PopupExpressionPropertyEditor)]
        //[ElementTypeProperty("ToType")]
        public string Expression
        {
            get
            {
                return _Expression;
            }
            set { SetPropertyValue("Expression", ref _Expression, value); }
        }

        protected abstract Type ToType
        {
            get;
        }
    }
}