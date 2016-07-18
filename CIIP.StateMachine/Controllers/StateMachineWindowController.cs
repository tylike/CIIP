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
using DevExpress.ExpressApp.StateMachine;

namespace CIIP.StateMachine.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class StateMachineWindowController : StateMachineController
    {
        public StateMachineWindowController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            if (this.ChangeStateAction.Enabled && this.ChangeStateAction.Active)
            {
                var obj = this.View.CurrentObject;
                if (ObjectSpace.IsNewObject(obj))
                {
                    var trs = this.ChangeStateAction.Items.FirstActiveItem.Data as ITransition;
                    var sm = trs.TargetState.StateMachine;

                    var member = XafTypesInfo.Instance.FindTypeInfo(this.View.ObjectTypeInfo.Type).FindMember(sm.StatePropertyName);
                    if (member.GetValue(obj) == null)
                    {
                        member.SetValue(obj, sm.FindCurrentState(obj));
                    }
                    //var obj = this.View.CurrentObject as 
                }
            }
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
    }
}
