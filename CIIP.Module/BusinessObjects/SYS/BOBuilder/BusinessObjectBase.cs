using System;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using 常用基类;
using System.Collections.Generic;
using System.Linq;

namespace CIIP.Module.BusinessObjects.SYS
{
    [XafDisplayName("类型")]
    [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
    public class BusinessObjectBase : NameObject, ICategorizedItem
    {
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

        private NameSpace _Category;
        [XafDisplayName("分类")]
        [RuleRequiredField]
        public NameSpace Category
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
        [XafDisplayName("说明")]
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

        //[Association]
        //[XafDisplayName("实现接口")]
        //public XPCollection<Interface> Interfaces
        //{
        //    get
        //    {
        //        return GetCollection<Interface>(nameof(Interfaces));
        //    }
        //}

        ITreeNode ICategorizedItem.Category
        {
            get { return Category; }
            set { Category = (NameSpace)value; }
        }

        public BusinessObjectBase(Session s) : base(s)
        {
        }


    }
}