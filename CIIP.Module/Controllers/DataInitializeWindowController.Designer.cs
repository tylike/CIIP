namespace CIIP.Module.Controllers
{
    partial class DataInitializeWindowController
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
            this.数据初始化 = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ActionCreateWMS = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // 数据初始化
            // 
            this.数据初始化.Caption = "数据初始化";
            this.数据初始化.Category = "Tools";
            this.数据初始化.ConfirmationMessage = "此动作将清除现有数据重新初始化内置的系统类型，这将导致自定义的用户业务不可用，确定要这样做吗？";
            this.数据初始化.Id = "数据初始化";
            this.数据初始化.TargetObjectsCriteria = "";
            this.数据初始化.ToolTip = null;
            this.数据初始化.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.数据初始化_Execute);
            // 
            // ActionCreateWMS
            // 
            this.ActionCreateWMS.Caption = "内置系统";
            this.ActionCreateWMS.Category = "Tools";
            this.ActionCreateWMS.ConfirmationMessage = "此动作将清除现有数据重新初始化内置的系统类型，这将导致自定义的用户业务不可用，确定要这样做吗？";
            this.ActionCreateWMS.Id = "CreateWMS";
            this.ActionCreateWMS.ToolTip = null;
            this.ActionCreateWMS.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleAction1_Execute);
            // 
            // DataInitializeWindowController
            // 
            this.Actions.Add(this.数据初始化);
            this.Actions.Add(this.ActionCreateWMS);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction 数据初始化;
        private DevExpress.ExpressApp.Actions.SimpleAction ActionCreateWMS;
    }
}
