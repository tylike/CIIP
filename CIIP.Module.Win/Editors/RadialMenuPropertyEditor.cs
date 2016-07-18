using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CIIP.Module.BusinessObjects.SYS;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Win.Controls;
using DevExpress.ExpressApp.Win.SystemModule;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using CIIP.Module.BusinessObjects.SYS;

namespace CIIP.Module.Win.Editors
{[PropertyEditor(typeof(IRadialMenu), true)]
    public class RadialMenuPropertyEditor : PropertyEditor, IComplexControl, IComplexViewItem
    {
        private RadialMenu menu;
        private Panel _panel;
        protected override object CreateControlCore()
        {

            var panel = new Panel();
            _panel = panel;
            var items = app.Model as IModelApplicationNavigationItems;
            var ctrl = new RadialMenu();

            menu = ctrl;

            var dock = app.MainWindow.Template as IBarManagerHolder;
            ctrl.Manager = dock.BarManager;
            BuildMenu(items.NavigationItems.Items, ctrl, dock);
            panel.MouseClick += Panel_MouseClick;
            menu.MenuRadius = (Screen.PrimaryScreen.Bounds.Height - 100)/4;
            return panel;
        }

        private void Panel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                menu.ShowPopup(Cursor.Position);
            }
        }

        private static void BuildMenu(IModelNavigationItems items, BarLinksHolder ctrl, IBarManagerHolder dock)
        {
            foreach (var x in items)
            {
                if (x.Items.Count > 0)
                {
                    var sub = new BarSubItem(dock.BarManager, x.Caption);
                    sub.LargeGlyph = ImageLoader.Instance.GetImageInfo(x.ImageName).Image;
                    ctrl.AddItem(sub);
                    BuildMenu(x.Items, sub, dock);
                }
                else
                {
                    var item = new BarButtonItem(dock.BarManager, x.Caption);
                    item.LargeGlyph = ImageLoader.Instance.GetImageInfo(x.ImageName).Image;
                    ctrl.AddItem(item);
                }
            }
        }

        protected override object GetControlValueCore()
        {
            return null;
        }

        protected override void ReadValueCore()
        {
            
        }

        private IObjectSpace os;
        private XafApplication app;
        public void Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this.os = objectSpace;
            this.app = application;
        }

        public RadialMenuPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model)
        {

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _panel.MouseClick -= this.Panel_MouseClick;
            }
            base.Dispose(disposing);
        }
    }
}