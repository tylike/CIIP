using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.DC;
using DevExpress.Xpo;
using System.Linq;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using System.Diagnostics;

namespace CIIP.Designer
{
    [XafDisplayName("子表")]
    //[Appearance("ManyToManyHiddenAggregated", AppearanceItemType = "LayoutItem", Criteria = "AssocicationInfo.ManyToMany", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, TargetItems = "Aggregated")]
    //[Appearance("LVisible", AppearanceItemType = "LayoutItem", Criteria = "SelfAtLeft", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, TargetItems = "LT,LP")]
    //[Appearance("RVisible", AppearanceItemType = "LayoutItem", Criteria = "!SelfAtLeft", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, TargetItems = "RT,RP")]
    //[Appearance("LVisible1",  Criteria = "SelfAtLeft",Enabled=false,TargetItems = "AssocicationInfo.LeftTable,AssocicationInfo.LeftProperty")]
    //[Appearance("RVisible1",  Criteria = "!SelfAtLeft",Enabled=false,TargetItems = "AssocicationInfo.RightTable,AssocicationInfo.RightProperty")]
    public class CollectionProperty : PropertyBase
    {
        //public bool SelfAtLeft
        //{
        //    get { return GetPropertyValue<bool>(nameof(SelfAtLeft)); }
        //    set { SetPropertyValue(nameof(SelfAtLeft), value); }
        //}

        public CollectionProperty(Session s) : base(s)
        {
            //CreateAssociation();
        }

        //public CollectionProperty(Session s, AssocicationInfo associcationInfo) : base(s)
        //{
        //    this.SelfAtLeft = true;
        //    this.AssocicationInfo = associcationInfo;
        //}

        //public override BusinessObjectBase PropertyType
        //{
        //    get
        //    {
        //        if (AssocicationInfo?.LeftProperty?.Oid != this.Oid)
        //        {
        //            return AssocicationInfo?.LeftTable;
        //        }
        //        return AssocicationInfo?.RightTable;
        //    }
        //    set => base.PropertyType = value;
        //}

        //private void CreateAssociation()
        //{
        //    if (!SelfAtLeft && !Session.IsObjectsLoading)
        //    {
        //        AssocicationInfo = new AssocicationInfo(Session);
        //        AssocicationInfo.RightProperty = this;
        //    }
        //}

        [XafDisplayName("聚合")]
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

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (IsLoading || IsSaving) return;

            //**********************************************************
            //应该不能放到基类中去,否则可能导致错误的修改右表,需要验证
            //**********************************************************
            //if (propertyName == nameof(this.BusinessObject))
            //{
            //    if (SelfAtLeft)
            //        AssocicationInfo.LeftTable = this.BusinessObject as BusinessObject;
            //    else
            //        AssocicationInfo.RightTable = this.BusinessObject as BusinessObject;
            //}
        }


    }

}