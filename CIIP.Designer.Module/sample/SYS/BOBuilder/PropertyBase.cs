using System.Collections.Generic;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects.SYS
{
    [XafDefaultProperty("DisplayName")]    
    public abstract class PropertyBase : NameObject
    {
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [XafDisplayName("显示名称")]
        public string DisplayName
        {
            get
            {
                if (OwnerBusinessObject != null)
                    return this.OwnerBusinessObject.FullName + "." + this.名称;
                return this.名称;
            }
        }
        
        protected abstract BusinessObject OwnerBusinessObject
        {
            get;
        }

        public PropertyBase(Session s) : base(s)
        {
        }
    }
}