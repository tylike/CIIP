namespace CIIP.Module.Controllers
{
    partial class ShowStateMachineDesignerViewController
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
            this.状态转换设计 = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // 状态转换设计
            // 
            this.状态转换设计.Caption = "状态转换设计";
            this.状态转换设计.ConfirmationMessage = null;
            this.状态转换设计.Id = "状态转换设计";
            this.状态转换设计.ToolTip = null;
            this.状态转换设计.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.状态转换设计_Execute);
            // 
            // ShowStateMachineDesignerViewController
            // 
            this.Actions.Add(this.状态转换设计);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction 状态转换设计;
    }
}
