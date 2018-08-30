using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using CIIP.Module.BusinessObjects.SYS;
using System.Windows.Forms;
using System.IO;
using DevExpress.ExpressApp.Win.Layout;

namespace CIIP.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ConfigDBViewController : ViewController
    {
        public ConfigDBViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(SystemLogonParameter);
            TargetViewType = ViewType.DetailView;
            ConfigDB.Active["CodeHidden"] = false;
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }

        protected override void OnViewControlsCreated()
        {
            var form = (this.Frame as DevExpress.ExpressApp.Win.WinWindow).Form;
            form.BackgroundImage = ImageLoader.Instance.GetImageInfo("C").Image;
            form.Controls[0].BackColor = System.Drawing.Color.Transparent;
            //form.Width = 938;
            //form.Height = 705;
            form.BackgroundImageLayout = ImageLayout.Stretch;
            var ctrl = this.View.Control as XafLayoutControl;
            ctrl.BackColor = System.Drawing.Color.Transparent;
            base.OnViewControlsCreated();
        }



        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void ConfigDB_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var frm = new Win.Editors.FrmEditConnectionString();
            frm.ShowDialog();
        }
    }
}
