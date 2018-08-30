using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using CIIP.Controller;
using View = DevExpress.ExpressApp.View;

namespace CIIP.Module.Win.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    public partial class WinShowNavigationItemController : SecurityShowNavigationItemController
    {
        public WinShowNavigationItemController()
        {
            InitializeComponent();
            // Target required Windows (via the TargetXXX properties) and create their Actions.
        }

        private void ShowNavigationItemAction_Executed(object sender, ActionBaseEventArgs e)
        {
            //当导航栏按钮点击完成后,给列表视图附与过滤条件
            var mdi = Application.ShowViewStrategy as MdiShowViewStrategy;
            if(mdi!=null)
            {
                var method = typeof(MdiShowViewStrategy).GetMethod("FindWindowByView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod);
                var ww = method.Invoke(mdi, new object[] { e.ShowViewParameters.CreatedView }) as WinWindow;
                var selectedItem = ((SingleChoiceAction)e.Action).SelectedItem;
                if (ww != null && ww.View != null)
                {
                    if (ww.View.Tag != selectedItem.Data)
                    {
                        var view = ww.View;
                        SetCriteria(selectedItem, view);
                        view.Caption = e.ShowViewParameters.CreatedView.Caption + "-" + selectedItem.Caption;
                    }
                }
                else
                {
                    var view = e.ShowViewParameters.CreatedView;
                    SetCriteria(selectedItem, view);
                    view.Caption = selectedItem.Caption;
                }
            }            
        }

        private static void SetCriteria(ChoiceActionItem selectedItem, View view)
        {
            var shortcut = selectedItem.Data as ViewShortcut;

            if (view is DevExpress.ExpressApp.ListView && shortcut.ContainsKey("Criteria"))
            {

                var ck = "NavigationCriteria";
                var lv = view as DevExpress.ExpressApp.ListView;

                var key = (string) shortcut["Criteria"];
                if (!string.IsNullOrEmpty(key))
                {
                    lv.CollectionSource.SetCriteria(ck, key);
                }
                else
                {
                    lv.CollectionSource.Criteria.Remove(ck);
                    shortcut["Criteria"] = "";
                }
                lv.Tag = shortcut;
                view.RefreshDataSource();
                view.Tag = shortcut;
            }
        }

        protected override void OnActivated()
        {
            this.ShowNavigationItemAction.Executed += ShowNavigationItemAction_Executed;
            base.OnActivated();
            // Perform various tasks depending on the target Window.
        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            this.ShowNavigationItemAction.Executed -= ShowNavigationItemAction_Executed;
            base.OnDeactivated();
        }
    }
}
