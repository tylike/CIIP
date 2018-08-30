namespace CIIP.Module.BusinessObjects.SYS
{
    partial class ShowFlowInstanceViewController
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
            this.显示流程 = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // 显示流程
            // 
            this.显示流程.Caption = "显示流程";
            this.显示流程.ConfirmationMessage = null;
            this.显示流程.Id = "显示流程";
            this.显示流程.ToolTip = null;
            this.显示流程.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.显示流程_Execute);
            // 
            // ShowFlowInstanceViewController
            // 
            this.Actions.Add(this.显示流程);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction 显示流程;
    }
}
