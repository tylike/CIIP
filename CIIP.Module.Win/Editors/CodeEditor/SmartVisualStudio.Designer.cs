namespace CIIP.Module.Win.Editors
{
    partial class SmartVisualStudio
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.tabSolution = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.solutionTreeView = new System.Windows.Forms.TreeView();
            this.消息 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel2_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.ErrorListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblStatus = new System.Windows.Forms.Label();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.tabSolution.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            this.消息.SuspendLayout();
            this.dockPanel2_Container.SuspendLayout();
            this.SuspendLayout();
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.tabSolution,
            this.消息});
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane"});
            // 
            // tabSolution
            // 
            this.tabSolution.Controls.Add(this.dockPanel1_Container);
            this.tabSolution.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.tabSolution.ID = new System.Guid("632d4e2f-d0f6-4600-a34a-924ed63c794c");
            this.tabSolution.Location = new System.Drawing.Point(781, 0);
            this.tabSolution.Name = "tabSolution";
            this.tabSolution.OriginalSize = new System.Drawing.Size(200, 200);
            this.tabSolution.Size = new System.Drawing.Size(200, 599);
            this.tabSolution.Text = "项目";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.solutionTreeView);
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(192, 572);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // solutionTreeView
            // 
            this.solutionTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.solutionTreeView.Location = new System.Drawing.Point(0, 0);
            this.solutionTreeView.Name = "solutionTreeView";
            this.solutionTreeView.Size = new System.Drawing.Size(192, 572);
            this.solutionTreeView.TabIndex = 0;
            this.solutionTreeView.DoubleClick += new System.EventHandler(this.solutionTreeView_DoubleClick);
            // 
            // 消息
            // 
            this.消息.Controls.Add(this.dockPanel2_Container);
            this.消息.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            this.消息.ID = new System.Guid("bb3e7479-4bfd-4414-a93b-b69ae1c0d50c");
            this.消息.Location = new System.Drawing.Point(0, 399);
            this.消息.Name = "消息";
            this.消息.OriginalSize = new System.Drawing.Size(200, 200);
            this.消息.Size = new System.Drawing.Size(781, 200);
            this.消息.Text = "消息";
            // 
            // dockPanel2_Container
            // 
            this.dockPanel2_Container.Controls.Add(this.ErrorListView);
            this.dockPanel2_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel2_Container.Name = "dockPanel2_Container";
            this.dockPanel2_Container.Size = new System.Drawing.Size(773, 173);
            this.dockPanel2_Container.TabIndex = 0;
            // 
            // ErrorListView
            // 
            this.ErrorListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.ErrorListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorListView.FullRowSelect = true;
            this.ErrorListView.HideSelection = false;
            this.ErrorListView.Location = new System.Drawing.Point(0, 0);
            this.ErrorListView.Name = "ErrorListView";
            this.ErrorListView.Size = new System.Drawing.Size(773, 173);
            this.ErrorListView.TabIndex = 0;
            this.ErrorListView.UseCompatibleStateImageBehavior = false;
            this.ErrorListView.View = System.Windows.Forms.View.Details;
            this.ErrorListView.DoubleClick += new System.EventHandler(this.Listview_DoubleClick);
            this.ErrorListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Listview_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "消息类型";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "消息内容";
            this.columnHeader2.Width = 706;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "位置";
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Location = new System.Drawing.Point(0, 376);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(781, 23);
            this.lblStatus.TabIndex = 3;
            // 
            // elementHost1
            // 
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost1.Location = new System.Drawing.Point(0, 0);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(781, 376);
            this.elementHost1.TabIndex = 6;
            this.elementHost1.Text = "elementHost2";
            this.elementHost1.Child = null;
            // 
            // SmartVisualStudio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.elementHost1);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.tabSolution);
            this.Controls.Add(this.消息);
            this.Name = "SmartVisualStudio";
            this.Size = new System.Drawing.Size(981, 599);
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.tabSolution.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            this.消息.ResumeLayout(false);
            this.dockPanel2_Container.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel 消息;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel2_Container;
        private System.Windows.Forms.ListView ErrorListView;
        private DevExpress.XtraBars.Docking.DockPanel tabSolution;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private System.Windows.Forms.TreeView solutionTreeView;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}
