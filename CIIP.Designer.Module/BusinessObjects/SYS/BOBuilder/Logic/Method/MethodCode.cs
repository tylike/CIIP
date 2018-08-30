using System;
using System.ComponentModel;
using Admiral.CodeFirstView;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Xpo;

namespace IMatrix.ERP.Module.BusinessObjects.SYS.Logic
{
    [XafDisplayName("ДњТы")]
    public abstract class MethodCode : LogicCodeUnit,IMethodCode
    {
        public MethodCode(Session s) : base(s)
        {

        }
    }

    public abstract class MethodCodeListView<T> : ListViewObject<T>
    {
        public override void LayoutListView()
        {
            
        }

        public override void LayoutDetailView()
        {
            base.LayoutDetailView();
            DetailViewLayout.ClearNodes();
        }
    }


}