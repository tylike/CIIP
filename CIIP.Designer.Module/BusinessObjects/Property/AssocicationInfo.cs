using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using CIIP.Persistent.BaseImpl;
using DevExpress.ExpressApp.Model;
using System.Collections.Generic;
using System.Linq;

namespace CIIP.Designer
{
    [NavigationItem]
    public class AssocicationInfo : NameObject
    {
        public AssocicationInfo(Session s) : base(s)
        {
        }

        [XafDisplayName("业务")]
        [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
        [DataSourceCriteria("IsPersistent")]
        [ImmediatePostData]
        public BusinessObject LeftTable
        {
            get { return GetPropertyValue<BusinessObject>(nameof(LeftTable)); }
            set { SetPropertyValue(nameof(LeftTable), value); }
        }

        [XafDisplayName("属性")]
        [DataSourceProperty(nameof(LeftProperties))]
        [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
        public PropertyBase LeftProperty
        {
            get { return GetPropertyValue<PropertyBase>(nameof(LeftProperty)); }
            set { SetPropertyValue(nameof(LeftProperty), value); }
        }

        List<PropertyBase> leftProperties;
        List<PropertyBase> LeftProperties
        {
            get
            {
                if (leftProperties == null)
                {
                    if (LeftTable == null)
                    {
                        leftProperties = new List<PropertyBase>();

                    }
                    else
                    {
                        //多
                        if (ManyToMany)
                        {
                            leftProperties = LeftTable.Properties.Where(x => x is CollectionProperty).ToList();
                        }
                        //一
                        else
                        {
                            leftProperties = LeftTable.Properties.Where(x => x is Property && x.PropertyType is BusinessObject).ToList();
                        }
                    }
                }
                return leftProperties;
            }
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading)
            {
                if (propertyName == nameof(ManyToMany))
                {
                    leftProperties = null;
                }
                if (propertyName == nameof(LeftTable))
                {
                    LeftProperty = LeftProperties.FirstOrDefault();
                    calcName();
                }

                //左右任一属性变化时,都给属性的assocication赋值
                if (propertyName == nameof(LeftProperty))
                {
                    calcName();
                    if (LeftProperty != null)
                    {
                        if (LeftProperty.AssocicationInfo == null || LeftProperty.AssocicationInfo.Oid != this.Oid)
                        {
                            LeftProperty.AssocicationInfo = this;
                        }
                    }

                    var ov = oldValue as PropertyBase;
                    if (ov != null)
                    {
                        ov.AssocicationInfo = null;
                    }
                    RightProperty.CalcNameCaption();
                }

                if (propertyName == nameof(RightProperty))
                {
                    calcName();
                    if (RightProperty != null && RightProperty.AssocicationInfo.Oid != this.Oid)
                        RightProperty.AssocicationInfo = this;

                    var ov = oldValue as PropertyBase;
                    if (ov != null)
                    {
                        ov.AssocicationInfo = null;
                    }
                }
            }

        }

        void calcName()
        {
            Name = LeftProperty?.DisplayName + "-" + RightProperty?.DisplayName;
        }

        [XafDisplayName("关系数量"), CaptionsForBoolValues("多对多", "一对多")]
        [ImmediatePostData]
        public bool ManyToMany
        {
            get { return GetPropertyValue<bool>(nameof(ManyToMany)); }
            set { SetPropertyValue(nameof(ManyToMany), value); }
        }

        [XafDisplayName("业务")]
        [ModelDefault("AllowEdit","False")]
        public BusinessObjectBase RightTable
        {
            get { return GetPropertyValue<BusinessObjectBase>(nameof(RightTable)); }
            set { SetPropertyValue(nameof(RightTable), value); }
        }

        [XafDisplayName("属性")]
        [ModelDefault("AllowEdit", "False")]
        [DataSourceProperty(nameof(RightTable))]

        public PropertyBase RightProperty
        {
            get { return GetPropertyValue<PropertyBase>(nameof(RightProperty)); }
            set { SetPropertyValue(nameof(RightProperty), value); }
        }
    }
}