using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace IMatrix.ERP.Module.BusinessObjects.SYS.Logic
{
    [XafDisplayName("ÏÔÊ¾´°¿Ú")]
    [CreatableItem]
    public class ShowViewWindow : MethodCode
    {
        public ShowViewWindow(Session s) : base(s)
        {

        }

        private string _Object;

        public string Object
        {
            get { return _Object; }
            set { SetPropertyValue("Object", ref _Object, value); }
        }

        private string _View;

        public string View
        {
            get { return _View; }
            set { SetPropertyValue("View", ref _View, value); }
        }

        private string _Target;

        public string Target
        {
            get { return _Target; }
            set { SetPropertyValue("Target", ref _Target, value); }
        }


    }
}