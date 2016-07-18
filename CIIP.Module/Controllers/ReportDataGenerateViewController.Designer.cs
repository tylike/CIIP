namespace CIIP.Module.Controllers
{
    partial class ReportDataGenerateViewController
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
            this.SaleDistrictReportGenerate = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.UpdateRegionLevel = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SaleDistrictReportGenerate
            // 
            this.SaleDistrictReportGenerate.Caption = "生成报表";
            this.SaleDistrictReportGenerate.ConfirmationMessage = null;
            this.SaleDistrictReportGenerate.Id = "SaleDistrictReportGenerate";
            this.SaleDistrictReportGenerate.ToolTip = null;
            this.SaleDistrictReportGenerate.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SaleDistrictReportGenerate_Execute);
            // 
            // UpdateRegionLevel
            // 
            this.UpdateRegionLevel.Caption = "更新级别";
            this.UpdateRegionLevel.ConfirmationMessage = null;
            this.UpdateRegionLevel.Id = "UpdateRegionLevel";
            this.UpdateRegionLevel.ToolTip = null;
            this.UpdateRegionLevel.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.UpdateRegionLevel_Execute);
            // 
            // ReportDataGenerateViewController
            // 
            this.Actions.Add(this.SaleDistrictReportGenerate);
            this.Actions.Add(this.UpdateRegionLevel);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction SaleDistrictReportGenerate;
        private DevExpress.ExpressApp.Actions.SimpleAction UpdateRegionLevel;
    }
}
