using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CIIP.Win.General.DashBoard.BusinessObjects;
using CIIP.Win.General.DashBoard.Helpers;
using DevExpress.DashboardWin;
using DevExpress.DashboardWin.Bars;
using DevExpress.DashboardWin.Native;
using DevExpress.ExpressApp;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.DashboardCommon;
using DevExpress.XtraBars.Ribbon;

namespace CIIP.DashBoard.Templates {
    public partial class DashboardDesignerForm : RibbonForm, IXPObjectSpaceAwareControl {
        bool _saveDashboard;
        History _editHistory;
        IObjectSpace _objectSpace;
        IDashboardDefinition _template;
        private DashboardDesigner _dashboardDesigner;
        private BarButtonItem _barButtonItemSave;
        private BarButtonItem _barButtonItemSaveAndClose;

        public DevExpress.DashboardCommon.Dashboard Dashboard { get { return _dashboardDesigner.Dashboard; } }
        public bool SaveDashboard { get { return _saveDashboard; } }

        public DashboardDesignerForm()
        {
            InitializeComponent();
            _dashboardDesigner = new DashboardDesigner();
            Controls.Add(_dashboardDesigner);
            _dashboardDesigner.Dock = DockStyle.Fill;
            _dashboardDesigner.CreateRibbon();
            _dashboardDesigner.ActionOnClose = DashboardActionOnClose.Discard;
            _editHistory = Designer.GetPrivatePropertyValue<History>("History");
            
            _dashboardDesigner.DataSourceWizardSettings.EnableCustomSql = true;
        }

        public DashboardDesigner Designer {
            get { return _dashboardDesigner; }
        }

        public IDashboardDefinition Template {
            get { return _template; }
        }

        public IObjectSpace ObjectSpace {
            get { return _objectSpace; }
        }

        public void UpdateDataSource(IObjectSpace objectSpace) {
            _objectSpace = objectSpace;
        }

        void _EditHistory_Changed(object sender, EventArgs e) {
            UpdateActionState();
        }

        protected override void OnClosed(EventArgs e) {
            _editHistory.Changed -= _EditHistory_Changed;
            _editHistory = null;
            _template = null;
            _objectSpace = null;
            _dashboardDesigner.Dashboard.Dispose();
            _dashboardDesigner = null;
            base.OnClosed(e);
        }

        void Save(object sender, ItemClickEventArgs e) {
            UpdateTemplateXml();
            UpdateActionState();
        }

        void UpdateTemplateXml() {
            using (var ms = new MemoryStream()) {
                Designer.Dashboard.SaveToXml(ms);
                ms.Position = 0;
                using (var sr = new StreamReader(ms)) {
                    string xml = sr.ReadToEnd();
                    Template.Xml = xml;
                    sr.Close();
                }
                ms.Close();
                _editHistory.IsModified = false;
            }
        }

        void UpdateActionState(){
            HideButtons();
            _barButtonItemSave.Enabled = _editHistory.IsModified;
            _barButtonItemSaveAndClose.Enabled = _editHistory.IsModified;
        }

        void SaveAndClose(object sender, ItemClickEventArgs e) {
            Save(null, null);
            DialogResult = DialogResult.OK;
        }

        public void LoadTemplate(IDashboardDefinition dashboardDefinition) {
            _template = dashboardDefinition;
            Designer.Dashboard = _template.CreateDashBoard(ObjectSpace, true);
            _editHistory.Changed += _EditHistory_Changed;
        }

        protected override void OnClosing(CancelEventArgs e) {
            base.OnClosing(e);
            if(_dashboardDesigner.IsDashboardModified) {
                DialogResult result = XtraMessageBox.Show(LookAndFeel, this, "Do you want to save changes ?", "Dashboard Designer", 
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if(result == DialogResult.Cancel)
                    e.Cancel = true;
                else
                    _saveDashboard = result == DialogResult.Yes;
            }
        }

        private TDashboardBarButtonItem GetBarItem<TDashboardBarButtonItem>() where TDashboardBarButtonItem : DashboardBarButtonItem{
            return ((RibbonControl) _dashboardDesigner.MenuManager).Items.OfType<TDashboardBarButtonItem>().First();
        }

        void DashboardDesignerForm_Load(object sender, EventArgs e) {
            HideButtons();
            _barButtonItemSave = AddButton("Save", "MenuBar_Save_32x32.png");
            _barButtonItemSave.ItemClick += Save;
            _barButtonItemSaveAndClose = AddButton("Save & Close", "MenuBar_SaveAndClose_32x32.png");
            _barButtonItemSaveAndClose.ItemClick += SaveAndClose;
        }

        private void HideButtons(){
            GetBarItem<FileNewBarItem>().Visibility = BarItemVisibility.Never;
            var fileSaveBarItem = GetBarItem<FileSaveBarItem>();
            fileSaveBarItem.Visibility = BarItemVisibility.Never;
            ((RibbonControl) _dashboardDesigner.MenuManager).Toolbar.ItemLinks.Remove(fileSaveBarItem);
            GetBarItem<FileSaveAsBarItem>().Visibility = BarItemVisibility.Never;
            GetBarItem<FileOpenBarItem>().Visibility = BarItemVisibility.Never;
        }

        private BarButtonItem AddButton(string button, string glyph) {
            var ribbonControl = ((RibbonControl) _dashboardDesigner.MenuManager);
            var ribbonPage = ribbonControl.Pages.Cast<RibbonPage>().First();
            var barButtonItem = new BarButtonItem(ribbonControl.Manager, button){
                Enabled = false,
                Glyph = GetImage(glyph)
            };
            
            ribbonPage.Groups[0].ItemLinks.Add(barButtonItem);
            return barButtonItem;
        }

        Image GetImage(string name) {
            var stream = GetType().Assembly.GetManifestResourceStream("CIIP.Dashboard.Templates."+ name);
            //Debug.Assert(stream != null, "stream != null");
            return Image.FromStream(stream);
        }
    }
}
