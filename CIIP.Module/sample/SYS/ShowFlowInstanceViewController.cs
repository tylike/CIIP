using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects.SYS
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ShowFlowInstanceViewController : ViewController
    {
        public ShowFlowInstanceViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(单据);
            TargetViewType = ViewType.DetailView;
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void 显示流程_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var os =(ObjectSpace as XPObjectSpace);
            var flowInstance = new FlowInstance(os.Session, this.View.CurrentObject as 单据);

            e.ShowViewParameters.CreatedView = Application.CreateDetailView(os, flowInstance, false);

            e.ShowViewParameters.NewWindowTarget = NewWindowTarget.Separate;
            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            var dc = new DialogController();
            dc.SaveOnAccept = false;
            e.ShowViewParameters.Controllers.Add(dc);

        }
    }
}
