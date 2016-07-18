namespace CIIP.Module.BusinessObjects.SYS.BOBuilder
{
    partial class GenerateBusinessModuleController
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
            this.生成业务模型 = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // 生成业务模型
            // 
            this.生成业务模型.Caption = "生成业务模型";
            this.生成业务模型.Category = "Tools";
            this.生成业务模型.ConfirmationMessage = null;
            this.生成业务模型.Id = "生成业务模型";
            this.生成业务模型.ImageName = "";
            this.生成业务模型.ToolTip = null;
            this.生成业务模型.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.生成业务模型_Execute);
            // 
            // GenerateBusinessModuleController
            // 
            this.Actions.Add(this.生成业务模型);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction 生成业务模型;
    }
}
