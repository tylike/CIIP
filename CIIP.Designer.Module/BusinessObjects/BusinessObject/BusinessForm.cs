using System.Linq;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace CIIP.Module.BusinessObjects.SYS
{
    //[DefaultClassOptions]
    [XafDisplayName("表单")]
    public class BusinessForm : BusinessObject
    {
        public BusinessForm(Session s) : base(s)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            //this.DisableCreateGenericParameterValues = true;

            ItemBusinessObject = new BusinessObject(Session);
            //var masterProperty = new Property(Session);
            //masterProperty.PropertyType = this;
            
            //ItemBusinessObject.Properties.Add(masterProperty);
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading)
            {
                if (propertyName == "Category")
                {
                    ItemBusinessObject.Category = Category;
                }

                if (propertyName == "Caption")
                {
                    ItemBusinessObject.Name = Name + "明细";
                }

                if (propertyName == "Base")
                {
                    //ItemBusinessObject.Base = this.Base.GenericParameterInstances.FirstOrDefault()?.DefaultGenericType;
                    //var gp = this.GenericParameterInstances.FirstOrDefault();
                    //if (gp != null)
                    //{
                    //    gp.ParameterValue = ItemBusinessObject;
                    //}
                    //gp = ItemBusinessObject.GenericParameterInstances.FirstOrDefault();
                    //if (gp != null)
                    //    gp.ParameterValue = this;
                }
            }
        }

        private BusinessObject _ItemBusinessObject;

        [XafDisplayName("子表")]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public BusinessObject ItemBusinessObject
        {
            get { return _ItemBusinessObject; }
            set { SetPropertyValue("ItemBusinessObject", ref _ItemBusinessObject, value); }
        }
    }
}