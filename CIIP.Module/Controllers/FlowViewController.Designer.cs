namespace CIIP.Module.Controllers
{
    partial class FlowViewController
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
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem1 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem2 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            this.FlowToNext = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // FlowToNext
            // 
            this.FlowToNext.Caption = "下推";
            this.FlowToNext.ConfirmationMessage = null;
            this.FlowToNext.Id = "FlowToNext";
            choiceActionItem1.Caption = "Entry 1";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem1.ToolTip = null;
            choiceActionItem2.Caption = "Entry 2";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem2.ToolTip = null;
            this.FlowToNext.Items.Add(choiceActionItem1);
            this.FlowToNext.Items.Add(choiceActionItem2);
            this.FlowToNext.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.FlowToNext.ToolTip = null;
            // 
            // FlowViewController
            // 
            this.Actions.Add(this.FlowToNext);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction FlowToNext;
    }
}
