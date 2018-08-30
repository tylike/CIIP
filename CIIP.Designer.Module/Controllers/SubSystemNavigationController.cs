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
using DevExpress.XtraPrinting.Control.Native;
using CIIP.Controller;

namespace CIIP.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    public partial class SubSystemNavigationController : WindowController
    {
        public SubSystemNavigationController()
        {
            InitializeComponent();
            TargetWindowType = WindowType.Main;
            //TargetViewNesting = Nesting.Root;

            var list = new List<SimpleAction>();
            for (int i = 0; i <= 20; i++)
            {
                var action = new SimpleAction(this.Container);
                action.Id = "SubSystem" + i;
                action.Active["InUse"] = true;
                action.Execute += Action_Execute;
                action.PaintStyle = ActionItemPaintStyle.CaptionAndImage;

                if (AdmiralEnvironment.IsWindows)
                    action.Category = "ViewsNavigation";
                if (AdmiralEnvironment.IsWeb)
                    action.Category = "Security";

                list.Add(action);
                this.Actions.Add(action);
            }
            RegisterActions(list.ToArray());
        }

        private void Action_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            NavigationToSystem(e.Action.Tag as string);
        }

        protected override void OnFrameAssigned()
        {
            var navs = (Application.Model as IModelApplicationNavigationItems).NavigationItems.Items.Where(x=>x.Visible).ToList();
            for (int i = 0; i < navs.Count; i++)
            {
                try
                {
                    var model = navs[i];
                    var act = this.Actions["SubSystem" + i] as SimpleAction;
                    act.Caption = model.Caption;
                    act.ImageName = model.ImageName;
                    act.Tag = model.Id;
                    act.Active["InUse"] = true;
                    act.Model.QuickAccess = true;
                    act.ToolTip = model.ToolTip;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            for(int i = navs.Count; i <= 20; i++)
            {
                var act = this.Actions["SubSystem" + i] as SimpleAction;
                act.Active["InUse"] = false;
            }
            base.OnFrameAssigned();
        }

        protected override void OnActivated()
        {
            

            base.OnActivated();
            // Perform various tasks depending on the target Window.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void NavigationToSystem(SimpleActionExecuteEventArgs arg)
        {
            NavigationToSystem(arg.Action.Tag as string);
        }

        public void NavigationToSystem(string selected)
        {
            SecurityShowNavigationItemController.Selected = selected;
            Frame.GetController<SecurityShowNavigationItemController>().ReCreateNavigationItems();
        }
        
    }
}
