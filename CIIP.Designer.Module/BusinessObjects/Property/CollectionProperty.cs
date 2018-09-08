using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.DC;
using DevExpress.Xpo;
using System.Linq;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;

namespace CIIP.Designer
{
    [XafDisplayName("子表")]
    [Appearance("ManyToManyHiddenAggregated",Criteria = "ManyToMany", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, TargetItems = "Aggregated")]
    [Appearance("PropertyBase.AutoCreateRelationIsEnable", TargetItems = "AutoCreateRelationProperty", Criteria = "ManyToMany", Enabled = false)]
    public class CollectionProperty : PropertyBase
    {
        public CollectionProperty(Session s) : base(s)
        {
        }
        
        protected override bool RelationPropertyNotNull
        {
            get
            {
                return true;
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
                        if (string.IsNullOrEmpty(Name))
                        {
                            Name = PropertyType.Caption;
                        }
                        if (RelationProperty == null)
                        {
                            try
                            {
                                RelationProperty = PropertyType.Properties.SingleOrDefault(x => x.PropertyType.Oid == this.BusinessObject.Oid);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                        if (RelationProperty == null)
                        {
                            RelationProperty = PropertyType.Properties.SingleOrDefault(x => x.PropertyType.Oid == this.BusinessObject.Oid);
                        }
                    }
                    else
                    {
                        Name = "";
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

        [CaptionsForBoolValues("对多对", "一对多")]
        [XafDisplayName("关系类型")]
        [ImmediatePostData]
        public bool ManyToMany
        {
            get { return GetPropertyValue<bool>(nameof(ManyToMany)); }
            set { SetPropertyValue(nameof(ManyToMany), value); }
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading)
            {
                if (propertyName == nameof(ManyToMany))
                {
                    AutoCreateRelationProperty = true;
                }
            }
        }



        protected override List<PropertyBase> RelationPropertyDataSources
        {
            get
            {
                if (PropertyType == null)
                    return null;
                return PropertyType.Properties.Where(x => x.PropertyType == this.BusinessObject).OfType<PropertyBase>().ToList();
            }
        }
    }

}