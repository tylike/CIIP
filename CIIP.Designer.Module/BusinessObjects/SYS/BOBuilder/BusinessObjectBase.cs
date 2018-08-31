using System;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using CIIP.Persistent.BaseImpl;
using System.Collections.Generic;
using System.Linq;

namespace CIIP.Module.BusinessObjects.SYS
{

    [XafDisplayName("类型")]
    [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
    public class BusinessObjectBase : NameObject, ICategorizedItem
    {
        private bool _IsRuntimeDefine;

        [XafDisplayName("动态定义")]
        [ToolTip("为假时是通过代码方式上传的模块生成的。否则是在界面上定义并生成的。")]
        public bool IsRuntimeDefine
        {
            get { return _IsRuntimeDefine; }
            set { SetPropertyValue("IsRuntimeDefine", ref _IsRuntimeDefine, value); }
        }
        private string _FullName;

        [ModelDefault("AllowEdit", "False")]
        [RuleRequiredField]
        //[Browsable(false)]
        [Size(-1)]
        public string FullName
        {
            get { return _FullName; }
            set { SetPropertyValue("FullName", ref _FullName, value); }
        }

        private Namespace _Category;
        [XafDisplayName("分类")]
        [RuleRequiredField]
        public Namespace Category
        {
            get { return _Category; }
            set { SetPropertyValue("Category", ref _Category, value); }
        }

        private string _Caption;
        [XafDisplayName("标题")]
        public string Caption
        {
            get { return _Caption; }
            set { SetPropertyValue("Caption", ref _Caption, value); }
        }

        private string _Description;
        [XafDisplayName("说明"),Size(-1)]
        public string Description
        {
            get { return _Description; }
            set { SetPropertyValue("Description", ref _Description, value); }
        }

        [XafDisplayName("基类接口")]
        [Association, DevExpress.Xpo.Aggregated]
        public XPCollection<ImplementRelation> Implements
        {
            get
            {
                return GetCollection<ImplementRelation>(nameof(Implements));
            }
        }

        #region 泛型参数定义

        #region 泛型
        [Persistent("IsGenericTypeDefine")]
        bool _isGenericTypeDefine;

        [XafDisplayName("泛型定义")]
        [ToolTip("本类是否是泛型定义")]
        [PersistentAlias("_isGenericTypeDefine")]
        public bool IsGenericTypeDefine
        {
            get
            {
                return _isGenericTypeDefine;
            }
        }

        #endregion

        [XafDisplayName("泛型参数定义")]
        [ToolTip("如果需要类型参数时,可以在此定义,可以在属性及业务逻辑中使用!")]
        [Association, DevExpress.Xpo.Aggregated]
        public XPCollection<GenericParameterDefine> GenericParameterDefines
        {
            get
            {
                return GetCollection<GenericParameterDefine>(nameof(GenericParameterDefines));
            }
        }
        #endregion

        [Association, DevExpress.Xpo.Aggregated,XafDisplayName("属性")]
        public XPCollection<PropertyBase> Properties
        {
            get
            {
                return GetCollection<PropertyBase>(nameof(Properties));
            }
        }

        ITreeNode ICategorizedItem.Category
        {
            get { return Category; }
            set { Category = (Namespace)value; }
        }

        public BusinessObjectBase(Session s) : base(s)
        {
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if(propertyName == nameof(GenericParameterDefines))
            {

            }
        }

        protected override void OnSaving()
        {
            _isGenericTypeDefine = GenericParameterDefines.Count > 0;
            base.OnSaving();
        }


    }
}