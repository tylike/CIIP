using DevExpress.ExpressApp.Actions;

namespace CIIP.Win.General.DashBoard.Controllers {
    partial class DashboardDesignerController {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.dashboardEdit = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // dashboardEdit
            // 
            this.dashboardEdit.Caption = "Dashboard Edit";
            this.dashboardEdit.ConfirmationMessage = null;
            this.dashboardEdit.Id = "DashboardEdit";
            this.dashboardEdit.ImageName = "BO_DashboardDefinition";
            this.dashboardEdit.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            this.dashboardEdit.Shortcut = null;
            this.dashboardEdit.Tag = null;
            this.dashboardEdit.TargetObjectsCriteria = null;
            this.dashboardEdit.TargetViewId = null;
            this.dashboardEdit.ToolTip = null;
            this.dashboardEdit.TypeOfView = null;
            this.dashboardEdit.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(dashboardEdit_Execute);

        }

        #endregion

        private SimpleAction dashboardEdit;
    }
}
