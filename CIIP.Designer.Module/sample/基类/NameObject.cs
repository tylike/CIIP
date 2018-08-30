using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace IMatrix.ERP.Module.BusinessObjects
{
    [NonPersistent]
    [XafDefaultProperty("名称")]
    public class NameObject : SimpleObject
    {
        public NameObject(Session s):base(s)
        {

        }
        private string _名称;
        public string 名称
        {
            get { return _名称; }
            set { SetPropertyValue("名称", ref _名称, value); }
        }
        
    }
}