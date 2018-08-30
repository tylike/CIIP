namespace CIIP.Module.BusinessObjects.SYS.BOBuilder.Logic
{
    partial class ShowBusinessObjectLogicViewController
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
            this.ShowBusinessObjectLogic = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ShowBusinessObjectLogic
            // 
            this.ShowBusinessObjectLogic.Caption = "业务逻辑";
            this.ShowBusinessObjectLogic.ConfirmationMessage = null;
            this.ShowBusinessObjectLogic.Id = "ShowBusinessObjectLogic";
            this.ShowBusinessObjectLogic.TargetObjectsCriteria = "IsRuntimeDefine";
            this.ShowBusinessObjectLogic.ToolTip = null;
            this.ShowBusinessObjectLogic.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ShowBusinessObjectLogic_Execute);
            // 
            // ShowBusinessObjectLogicViewController
            // 
            this.Actions.Add(this.ShowBusinessObjectLogic);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ShowBusinessObjectLogic;
    }
}
