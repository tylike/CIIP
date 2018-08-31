using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace CIIP.Module.BusinessObjects.SYS
{
    [DefaultClassOptions]
    public class Interface : BusinessObjectBase
    {
        public Interface(Session s) : base(s)
        {

        }

        //[Association,XafDisplayName("实现业务"),ToolTip("以下业务实现了本接口")]
        //public XPCollection<BusinessObjectBase> Implements
        //{
        //    get
        //    {
        //        return GetCollection<BusinessObjectBase>(nameof(Implements));
        //    }
        //}

    }
}