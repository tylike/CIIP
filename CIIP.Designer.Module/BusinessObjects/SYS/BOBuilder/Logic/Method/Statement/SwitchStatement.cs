using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.Persistent.Base.General;
using System;

namespace IMatrix.ERP.Module.BusinessObjects.SYS.Logic
{
    [XafDisplayName("条件分支")]
    [ChildrenType(typeof(SwitchCaseStatement))]
    public class SwitchStatement : MethodCode
    {
        public SwitchStatement(Session s) : base(s)
        {

        }
    }

    public class SwitchStatement_ListView : MethodCodeListView<SwitchStatement>
    {
        public override void LayoutDetailView()
        {
            base.LayoutDetailView();
        }
    }
}