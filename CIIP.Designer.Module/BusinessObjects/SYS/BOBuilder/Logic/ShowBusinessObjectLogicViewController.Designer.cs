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
            this.ShowPartialLogic = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ShowLayout = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
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
            // ShowPartialLogic
            // 
            this.ShowPartialLogic.Caption = "分部逻辑";
            this.ShowPartialLogic.ConfirmationMessage = null;
            this.ShowPartialLogic.Id = "ShowPartialLogic";
            this.ShowPartialLogic.TargetObjectsCriteria = "IsRuntimeDefine";
            this.ShowPartialLogic.ToolTip = null;
            this.ShowPartialLogic.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ShowPartialLogic_Execute);
            // 
            // ShowLayout
            // 
            this.ShowLayout.Caption = "代码布局";
            this.ShowLayout.ConfirmationMessage = null;
            this.ShowLayout.Id = "ShowLayout";
            this.ShowLayout.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ShowLayout.TargetObjectsCriteria = "";
            this.ShowLayout.ToolTip = null;
            this.ShowLayout.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ShowLayout_Execute);
            // 
            // ShowBusinessObjectLogicViewController
            // 
            this.Actions.Add(this.ShowBusinessObjectLogic);
            this.Actions.Add(this.ShowPartialLogic);
            this.Actions.Add(this.ShowLayout);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ShowBusinessObjectLogic;
        private DevExpress.ExpressApp.Actions.SimpleAction ShowPartialLogic;
        private DevExpress.ExpressApp.Actions.SimpleAction ShowLayout;
    }
}
