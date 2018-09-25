using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.DC;
using DevExpress.Xpo;
using System.Linq;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using System.Diagnostics;
using DevExpress.ExpressApp.Xpo;

namespace CIIP.Designer
{
    [XafDisplayName("вс╠М")]
    public class CollectionProperty : PropertyBase
    {
        public CollectionProperty(Session s) : base(s)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            IsAssocication = true;
        }

        [XafDisplayName("╬ш╨о")]
        public bool Aggregated
        {
            get { return GetPropertyValue<bool>(nameof(Aggregated)); }
            set { SetPropertyValue(nameof(Aggregated), value); }
        }

        protected override IEnumerable<BusinessObjectBase> PropertyTypes
        {
            get
            {
                if (propertyTypes == null)
                {
                    propertyTypes = Session.Query<BusinessObject>().Where(x => x.IsPersistent).ToArray();
                }
                return propertyTypes;
            }
        }
    }

}