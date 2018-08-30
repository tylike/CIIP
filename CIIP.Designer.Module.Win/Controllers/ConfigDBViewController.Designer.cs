namespace CIIP.Module.Controllers
{
    partial class ConfigDBViewController
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
            this.ConfigDB = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ConfigDB
            // 
            this.ConfigDB.Caption = "配置数据库";
            this.ConfigDB.Category = "PopupActions";
            this.ConfigDB.ConfirmationMessage = null;
            this.ConfigDB.Id = "ConfigDB";
            this.ConfigDB.ToolTip = null;
            this.ConfigDB.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ConfigDB_Execute);
            // 
            // ConfigDBViewController
            // 
            this.Actions.Add(this.ConfigDB);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ConfigDB;
    }
}
