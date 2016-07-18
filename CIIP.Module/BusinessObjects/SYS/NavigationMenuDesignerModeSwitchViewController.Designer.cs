namespace CIIP.Module.BusinessObjects.SYS
{
    partial class NavigationMenuDesignerModeSwitchViewController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.NavigationMenuDesignModeSwitch = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // NavigationMenuDesignModeSwitch
            // 
            this.NavigationMenuDesignModeSwitch.Caption = "查看";
            this.NavigationMenuDesignModeSwitch.ConfirmationMessage = null;
            this.NavigationMenuDesignModeSwitch.Id = "NavigationMenuDesignModeSwitch";
            this.NavigationMenuDesignModeSwitch.ImageName = "Action_Pivot_Printing_Preview";
            this.NavigationMenuDesignModeSwitch.ToolTip = null;
            this.NavigationMenuDesignModeSwitch.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.NavigationMenuDesignModeSwitch_Execute);
            // 
            // NavigationMenuDesignerModeSwitchViewController
            // 
            this.Actions.Add(this.NavigationMenuDesignModeSwitch);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction NavigationMenuDesignModeSwitch;
    }
}
