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
using CIIP.Module.BusinessObjects.SYS.Logic;

namespace CIIP.Module.BusinessObjects.SYS.BOBuilder.Logic
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ShowBusinessObjectLogicViewController : ViewController
    {
        public ShowBusinessObjectLogicViewController()
        {
            InitializeComponent();
            TargetViewType = ViewType.ListView;
            TargetViewNesting = Nesting.Root;
            TargetObjectType = typeof(BusinessObject);
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

        private void ShowBusinessObjectLogic_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View.CurrentObject != null)
            {
                var os = Application.CreateObjectSpace();
                var obj = os.GetObject(View.CurrentObject);

                e.ShowViewParameters.CreatedView = Application.CreateDetailView(os, "BusinessObject_DetailView_BOLogic", true, obj);
                e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                e.ShowViewParameters.NewWindowTarget = NewWindowTarget.Separate;
            }

        }
    }
}
