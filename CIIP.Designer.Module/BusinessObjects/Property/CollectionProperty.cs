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
    [Appearance("ManyToManyHiddenAggregated", AppearanceItemType = "LayoutItem", Criteria = "AssocicationInfo.ManyToMany", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, TargetItems = "Aggregated")]
    [Appearance("LVisible", AppearanceItemType = "LayoutItem" , Criteria = "Oid == AssocicationInfo.LeftProperty.Oid", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, TargetItems = "LT,LP")]
    [Appearance("RVisible", AppearanceItemType = "LayoutItem", Criteria = "Oid != AssocicationInfo.LeftProperty.Oid", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, TargetItems = "RT,RP")]
    public class CollectionProperty : PropertyBase
    {
        public CollectionProperty(Session s) : base(s)
        {
        }

        public override BusinessObjectBase PropertyType
        {
            get
            {
                if (AssocicationInfo?.LeftProperty?.Oid != this.Oid)
                {
                    return AssocicationInfo?.LeftTable;
                }
                return AssocicationInfo?.RightTable;
            }
            set => base.PropertyType = value;
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            AssocicationInfo = new AssocicationInfo(Session);
            AssocicationInfo.RightProperty = this;
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            CreateRelationProperty();
        }
        protected void CreateRelationProperty()
        {
            //if (ManyToMany)
            //{
            //    //当前是xpcollection<学生> 学生s {get;} 属性
            //    //自动创建的属性是 xpcollection<教师> 教师s {get;} 属性
            //    var property = new CollectionProperty(Session);
            //    property.BusinessObject = this.PropertyType;
            //    property.PropertyType = this.BusinessObject;
            //    property.ManyToMany = true;
            //    property.Name = BusinessObject.Name;
            //    property.Caption = BusinessObject.Caption;
            //    property.RelationProperty = this;
            //    RelationProperty = property;
            //}
            //else
            //{
            //    //当前是xpcollection<order> orders {get;} 属性
            //    //自动创建的属性是 customer customer {get;} 属性
            //    var property = new Property(Session);
            //    property.BusinessObject = this.PropertyType;
            //    property.PropertyType = this.BusinessObject;
            //    property.Name = BusinessObject.Name;
            //    property.Caption = BusinessObject.Caption;
            //    property.RelationProperty = this;
            //    RelationProperty = property;
            //}
        }

        private bool _Aggregated;
        [XafDisplayName("聚合")]
        public bool Aggregated
        {
            get { return _Aggregated; }
            set { SetPropertyValue("Aggregated", ref _Aggregated, value); }
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
            if (propertyName == nameof(this.BusinessObject))
            {
                AssocicationInfo.RightTable = this.BusinessObject;
            }
        }


    }

}