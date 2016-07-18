namespace CIIP.Module.Controllers
{
    partial class DisableShowDetailViewViewController
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
            this.RuntimeListViewSetup = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // RuntimeListViewSetup
            // 
            this.RuntimeListViewSetup.Caption = "设置";
            this.RuntimeListViewSetup.Category = "Tools";
            this.RuntimeListViewSetup.ConfirmationMessage = null;
            this.RuntimeListViewSetup.Id = "RuntimeListViewSetup";
            this.RuntimeListViewSetup.ToolTip = null;
            this.RuntimeListViewSetup.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RuntimeListViewSetup_Execute);
            // 
            // DisableShowDetailViewViewController
            // 
            this.Actions.Add(this.RuntimeListViewSetup);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction RuntimeListViewSetup;
    }
}
