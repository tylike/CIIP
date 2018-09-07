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

    [XafDisplayName("0. Ù–‘")]
    [Appearance("SizeIsVisible",TargetItems ="Size",Method = "SizeIsVisible", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    public class Property : PropertyBase
    {

        public static bool SizeIsVisible(Property property)
        {
            return property?.PropertyType?.FullName != typeof(string).FullName;
        }
        
        private int _Size;
        [XafDisplayName("≥§∂»")]
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