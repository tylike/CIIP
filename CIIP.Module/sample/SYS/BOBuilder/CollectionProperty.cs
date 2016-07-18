using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.DC;
using DevExpress.Xpo;
using System.Linq;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;

namespace CIIP.Module.BusinessObjects.SYS
{
    public class CollectionProperty : PropertyBase
    {
        public CollectionProperty(Session s) : base(s)
        {
        }

        private BusinessObject _Owner;

        [Association]
        public BusinessObject Owner
        {
            get { return _Owner; }
            set { SetPropertyValue("Owner", ref _Owner, value); }
        }

        private PropertyBase _RelationProperty;
        [XafDisplayName("关联属性"), DataSourceProperty("RelationPropertyDataSources")]
        [RuleRequiredField,LookupEditorMode(LookupEditorMode.AllItems)]
        public PropertyBase RelationProperty
        {
            get { return _RelationProperty; }
            set { SetPropertyValue("RelationProperty", ref _RelationProperty, value); }
        }

        protected List<PropertyBase> RelationPropertyDataSources
        {
            get
            {
                if (PropertyType == null)
                    return null;
                return PropertyType.Properties.Where(x => x.PropertyType == this.Owner).OfType<PropertyBase>().Union(
                    PropertyType.CollectionProperties.Where(x => x.PropertyType == this.Owner).OfType<PropertyBase>()
                    ).ToList();
            }
        }

        private BusinessObject _PropertyType;

        [XafDisplayName("类型"), RuleRequiredField, DataSourceProperty("PropertyTypes")]
        [ImmediatePostData, LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
        public BusinessObject PropertyType
        {
            get { return _PropertyType; }
            set
            {
                SetPropertyValue("PropertyType", ref _PropertyType, value);
                if (!IsLoading)
                {
                    if (PropertyType != null)
                    {
                        if (string.IsNullOrEmpty(名称))
                        {
                            名称 = PropertyType.Caption;
                        }
                        if (RelationProperty == null)
                        {
                            try
                            {
                                RelationProperty = PropertyType.Properties.SingleOrDefault(x => x.PropertyType.Oid == this.Owner.Oid);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                        if (RelationProperty == null)
                        {
                            PropertyType.CollectionProperties.SingleOrDefault(x => x.PropertyType.Oid == this.Owner.Oid);
                        }
                    }
                    else
                    {
                        名称 = "";
                    }

                }
            }
        }
        
        private BusinessObject[] types;

        protected  IEnumerable<BusinessObjectBase> PropertyTypes
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

        private bool _Aggregated;
        [XafDisplayName("聚合")]
        public bool Aggregated
        {
            get { return _Aggregated; }
            set { SetPropertyValue("Aggregated", ref _Aggregated, value); }
        }

        protected override BusinessObject OwnerBusinessObject
        {
            get
            {
                return this.Owner;
            }
        }
    }
}