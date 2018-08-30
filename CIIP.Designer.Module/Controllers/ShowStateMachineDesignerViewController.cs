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
using CIIP.Module.BusinessObjects.Flow;

namespace CIIP.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ShowStateMachineDesignerViewController : ViewController<DetailView>
    {
        public ShowStateMachineDesignerViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof (Flow);
            this.状态转换设计.Enabled["SelectedNode"] = false;
            //this.状态转换设计.TargetObjectsCriteria = "Flow.SelectedNode is not null";
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            var flow = this.View.CurrentObject as Flow;
            flow.Changed += Flow_Changed;
            // Perform various tasks depending on the target View.
        }

        private void Flow_Changed(object sender, DevExpress.Xpo.ObjectChangeEventArgs e)
        {
            if (e.PropertyName == "SelectedNode")
            {
                this.状态转换设计.Enabled["SelectedNode"] = (e.NewValue != null);
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            var flow = this.View.CurrentObject as Flow;
            flow.Changed -= Flow_Changed;
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void 状态转换设计_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var curr = this.View.CurrentObject as Flow;
            var targetType = ReflectionHelper.FindType(curr.SelectedNode.Form);
            var obj = ObjectSpace.FindObject<CIIPXpoStateMachine>(new BinaryOperator("TargetObjectType", targetType), true);
            if (obj == null)
            {
                obj = ObjectSpace.CreateObject<CIIPXpoStateMachine>();
                obj.TargetObjectType = targetType;

                var cls = Application.Model.BOModel.GetClass(targetType);

                obj.StatePropertyName =
                    new StringObject(
                        cls.AllMembers.FirstOrDefault(x => x.MemberInfo.MemberType == typeof (CIIPXpoState))?.Name);
                obj.Name = cls.Caption + "状态转换";
                obj.Active = true;
                obj.ExpandActionsInDetailView = true;
                obj.Save();
            }

            var view = Application.CreateDetailView(this.ObjectSpace, obj, false);
            e.ShowViewParameters.CreatedView = view;
            e.ShowViewParameters.NewWindowTarget = NewWindowTarget.Separate;
            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            var dc = new DialogController();

            e.ShowViewParameters.Controllers.Add(dc);
            dc.SaveOnAccept = false;
        }
    }
}
