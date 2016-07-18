using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace IMatrix.ERP.Module.BusinessObjects.SYS.Logic
{
    [XafDisplayName("≤È’“∂‘œÛ")]
    [CreatableItem]
    public class FindBusinessObject : MethodCode
    {
        public FindBusinessObject(Session s) : base(s)
        {

        }

        private string _FindObjectType;

        public string FindObjectType
        {
            get { return _FindObjectType; }
            set { SetPropertyValue("FindObjectType", ref _FindObjectType, value); }
        }

        private bool _ByKey;

        public bool ByKey
        {
            get { return _ByKey; }
            set { SetPropertyValue("ByKey", ref _ByKey, value); }
        }

        private string _Expression;

        public string Expression
        {
            get { return _Expression; }
            set { SetPropertyValue("Expression", ref _Expression, value); }
        }
    }
    public class FindBusinessObject_ListView : MethodCodeListView<FindBusinessObject>
    {
        public override void LayoutDetailView()
        {
            base.LayoutDetailView();
            HGroup(10, x => x.FindObjectType,x=>x.Expression);
            HGroup(20, x => x.Index);
        }
    }
}