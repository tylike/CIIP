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
using DevExpress.Persistent.BaseImpl;

namespace CIIP.Designer
{
    [NavigationItem]
    public class Order : BaseObject
    {
        public Order(Session session) : base(session)
        {
        }
        public string Code
        {
            get { return GetPropertyValue<string>(nameof(Code)); }
            set { SetPropertyValue(nameof(Code), value); }
        }


        public string Test
        {
            get
            {
                return "";
            }
        }

        //[PersistentAlias("[Items]")]
        //public XPCollection<OrderItem> Items2
        //{
        //    get
        //    {
        //        var t = EvaluateAlias(nameof(Items2));
        //        return (XPCollection<OrderItem>)t;
        //    }
        //}


        [Association, DevExpress.Xpo.Aggregated]
        public XPCollection<OrderItem> Items
        {
            get
            {
                return GetCollection<OrderItem>(nameof(Items));
            }
        }
    }

    public class OrderItem : BaseObject
    {
        public OrderItem(Session session) : base(session)
        {
        }
        [Association]
        public Order Order
        {
            get { return GetPropertyValue<Order>(nameof(Order)); }
            set { SetPropertyValue(nameof(Order), value); }
        }

        public string Product
        {
            get { return GetPropertyValue<string>(nameof(Product)); }
            set { SetPropertyValue(nameof(Product), value); }
        }
    }

    [XafDisplayName("类型")]
    [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
    public abstract class BusinessObjectBase : NameObject, ICategorizedItem
    {
        public PropertyBase FindProperty(string name)
        {
            return Properties.SingleOrDefault(x => x.Name == name);
        }

        [XafDisplayName("可建关系")]
        public abstract bool CanCreateAssocication { get; }


        #region modifier
        [XafDisplayName("继承设置")]
        [ToolTip("可以设置为无,抽象,密封的")]
        public virtual BusinessObjectModifier DomainObjectModifier
        {
            get
            {
                return GetPropertyValue<BusinessObjectModifier>(nameof(DomainObjectModifier));
            }
            set
            {
                SetPropertyValue(nameof(DomainObjectModifier), value);
            }
        }
        #endregion

        [XafDisplayName("所属模块"),Association]
        public BusinessModule BusinessModule
        {
            get { return GetPropertyValue<BusinessModule>(nameof(BusinessModule)); }
            set { SetPropertyValue(nameof(BusinessModule), value); }
        }

        [XafDisplayName("动态定义")]
        [ToolTip("为假时是通过代码方式上传的模块生成的。否则是在界面上定义并生成的。")]
        public bool IsRuntimeDefine
        {
            get { return GetPropertyValue<bool>(nameof(IsRuntimeDefine)); }
            set { SetPropertyValue(nameof(IsRuntimeDefine), value); }
        }

        [ModelDefault("AllowEdit", "False")]
        [RuleRequiredField]
        //[Browsable(false)]
        [Size(-1)]
        public string FullName
        {
            get { return GetPropertyValue<string>(nameof(FullName)); }
            set { SetPropertyValue(nameof(FullName), value); }
        }

        [XafDisplayName("分类")]
        //[RuleRequiredField]
        public Namespace Category
        {
            get { return GetPropertyValue<Namespace>(nameof(Category)); }
            set { SetPropertyValue(nameof(Category), value); }
        }

        [XafDisplayName("标题")]
        public string Caption
        {
            get { return GetPropertyValue<string>(nameof(Caption)); }
            set { SetPropertyValue(nameof(Caption), value); }
        }

        [XafDisplayName("说明"),Size(-1)]
        public string Description
        {
            get { return GetPropertyValue<string>(nameof(Description)); }
            set { SetPropertyValue(nameof(Description), value); }
        }

        [XafDisplayName("基类接口")]
        [Association, DevExpress.Xpo.Aggregated,EditorAlias("ImplementPropertyEditor")]
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
        [Association, DevExpress.Xpo.Aggregated,EditorAlias("GenericPropertyEditor")]
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