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

namespace CIIP.Module.BusinessObjects.SYS
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class NavigationMenuDesignerModeSwitchViewController : ViewController
    {
        public NavigationMenuDesignerModeSwitchViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(NavigationMenu);
            TargetViewType = ViewType.DetailView;

            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            var d = (this.View.CurrentObject as NavigationMenu).IsDesignMode;
            NavigationMenuDesignModeSwitch.Caption = d ? "设计" : "查看";
            NavigationMenuDesignModeSwitch.ImageName = d ? "Action_Show_PivotGrid_Designer" : "Action_Pivot_Printing_Preview";
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

        private void NavigationMenuDesignModeSwitch_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var c = NavigationMenuDesignModeSwitch.Caption;
            var isDesignMode = !(this.View.CurrentObject as NavigationMenu).IsDesignMode;
            NavigationMenuDesignModeSwitch.Caption = isDesignMode ? "查看" : "设计";
            NavigationMenuDesignModeSwitch.ImageName = isDesignMode ? "Action_Pivot_Printing_Preview" : "Action_Show_PivotGrid_Designer";
            (this.View.CurrentObject as NavigationMenu).IsDesignMode = isDesignMode;
        }
    }
}
