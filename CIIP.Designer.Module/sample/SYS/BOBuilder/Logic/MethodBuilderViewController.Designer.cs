namespace IMatrix.ERP.Module.BusinessObjects.SYS.BOBuilder.Logic
{
    partial class MethodBuilderViewController
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
            this.AddStatement = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // AddStatement
            // 
            this.AddStatement.Caption = "填加指令";
            this.AddStatement.ConfirmationMessage = null;
            this.AddStatement.Id = "AddStatement";
            this.AddStatement.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.AddStatement.ToolTip = null;
            this.AddStatement.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.AddStatement_Execute);
            // 
            // MethodBuilderViewController
            // 
            this.Actions.Add(this.AddStatement);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction AddStatement;
    }
}
