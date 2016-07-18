using System;
using System.Drawing;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace IMatrix.ERP.Module.BusinessObjects.SYS.Logic
{
    [XafDisplayName("创建对象")]
    [CreatableItem]
    public class CreateBusinessObject : MethodCode
    {
        public CreateBusinessObject(Session s) : base(s)
        {

        }

        private string _CreateObjectType;

        public string CreateObjectType
        {
            get { return _CreateObjectType; }
            set { SetPropertyValue("CreateObjectType", ref _CreateObjectType, value); }
        }
    }

    public class CreateBusinessObject_ListView : MethodCodeListView<CreateBusinessObject>
    {
        public override void LayoutDetailView()
        {
            base.LayoutDetailView();
            HGroup(10, x => x.CreateObjectType);
            HGroup(20, x => x.Index);
        }
    }
}