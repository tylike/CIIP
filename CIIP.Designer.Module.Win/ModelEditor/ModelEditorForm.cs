using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Win.Controls;
using DevExpress.ExpressApp.Win.Core;
using DevExpress.ExpressApp.Win.Core.ModelEditor;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CIIP.Win.ModelEditor
{
    [ToolboxItem(false)]
    public class ModelEditorForm : DevExpress.ExpressApp.Win.Templates.Bars.DetailFormV2, IModelEditorSettings
    {
        public const string Title = "模型设置";
        private IModelEditorController controller;
        private SettingsStorage settingsStorage;
        private ModelEditorControl modelEditorControl;
        private bool dropModelDifs = false;
        public ModelEditorForm(ModelEditorViewController controller, SettingsStorage settingsStorage)
            : base()
        {
            this.settingsStorage = settingsStorage;
            ((IBarManagerHolder)this).BarManager.MainMenu.Visible = false;
            var t = new CIIPExtendModelInterfaceAdapter(); 
            //***********************************************************
            //here modified
            modelEditorControl = new ModelEditorControlEx(new DevExpress.ExpressApp.Win.Core.ModelEditor.NodesTree.ModelTreeList(t), settingsStorage);

            controller.SetControl(modelEditorControl);
            controller.SetTemplate(this);
            this.controller = controller;
            modelEditorControl.Dock = DockStyle.Fill;
            ((Control)((IViewSiteTemplate)this).ViewSiteControl).Controls.Add(modelEditorControl);
            if (settingsStorage != null)
            {
                new FormStateAndBoundsManager().Load(this, settingsStorage);
            }
            Image modelEditorImage = ImageLoader.Instance.GetImageInfo("EditModel").Image;
            if (modelEditorImage != null)
            {
                this.Icon = Icon.FromHandle(new Bitmap(modelEditorImage).GetHicon());
            }
            Text = Title;
            Disposed += new EventHandler(ModelEditorForm_Disposed);
            this.controller.LoadSettings();
            this.Tag = "testdialog=ModelEditor";
        }
        void ModelEditorForm_Disposed(object sender, EventArgs e)
        {
        }
        public void SetCaption(string text)
        {
            Text = string.IsNullOrEmpty(text) ? Title : string.Format("{0} - {1}", text, Title);
        }
        protected virtual DialogResult CloseModelEditorDialog()
        {
            return ModelEditorViewController.CloseModelEditorDialog;
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            if (controller.IsModified)
            {
                switch (CloseModelEditorDialog())
                {
                    case DialogResult.Yes:
                        {
                            if (controller.Save())
                            {
                                DialogResult = DialogResult.Yes;
                            }
                            else
                            {
                                e.Cancel = true;
                            }
                            break;
                        }
                    case DialogResult.No:
                        {
                            dropModelDifs = true;
                            break;
                        }
                    case DialogResult.Cancel:
                        {
                            e.Cancel = true;
                            break;
                        }
                }
            }
            if (!e.Cancel)
            {
                //((ModelEditorViewController)controller).UnSubscribeEvents();
                ModelEditorSaveSettings();
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            if (dropModelDifs)
            {
                controller.ReloadModel(false, false);
            }
            modelEditorControl.OnClosed();
            controller.Dispose();
            controller = null;
            base.OnClosed(e);
        }
        protected override void Dispose(bool disposing)
        {
            lock (ModelEditorViewController.LockDisposeObject)
            {
                if (controller != null)
                {
                    controller.IsDisposing = true;
                }
                base.Dispose(disposing);
                controller = null;
                settingsStorage = null;
                if (disposing)
                {
                    modelEditorControl.Dispose();
                }
            }
        }
        #region IModelEditorSettingsStorage Members
        public void ModelEditorSaveSettings()
        {
            if (controller != null)
            {
                controller.SaveSettings();
            }
            modelEditorControl.CurrentModelTreeListNode = null;
            new FormStateAndBoundsManager().Save(this, settingsStorage);
        }
        #endregion
    }
}
