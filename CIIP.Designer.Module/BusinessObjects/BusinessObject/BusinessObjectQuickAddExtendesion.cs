using CIIP.Module.BusinessObjects.SYS.Logic;
using DevExpress.Data.Filtering;

namespace CIIP.Designer
{
    public static class BusinessObjectQuickAddExtendesion
    {
        #region quick add
        public static Property AddProperty(this BusinessObject self,string name, BusinessObjectBase type, int? size = null)
        {
            var property = new Property(self.Session);
            property.PropertyType = type;
            property.Name = name;
            if (size.HasValue)
                property.Size = size.Value;
            self.Properties.Add(property);
            return property;
        }

        public static Property AddProperty<T>(this BusinessObject self,string name, int? size = null)
        {
            return self.AddProperty(name,
                self.Session.FindObject<BusinessObjectBase>(new BinaryOperator("FullName", typeof(T).FullName))
                , size
                );
        }

        //public static CollectionProperty AddAssociation(this BusinessObject self, string name, BusinessObject bo, bool isAggregated,Property relation)
        //{
        //    var cp = new CollectionProperty(self.Session);
        //    self.Properties.Add(cp);
        //    cp.PropertyType = bo;
        //    cp.Name = name;
        //    cp.Aggregated = isAggregated;
        //    //cp.RelationProperty = relation;
        //    return cp;
        //}
        

        #endregion
    }
}