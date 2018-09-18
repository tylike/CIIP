using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CIIP.Designer;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Localization;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.Core;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp.Win.SystemModule;
using DevExpress.Xpo;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.ViewInfo;

namespace CIIP.Module.Win.Editors
{
    public class AssignDefaultAssocicationInfoController : ObjectViewController<DetailView, PropertyBase>
    {
        public AssignDefaultAssocicationInfoController()
        {

        }

        protected override void OnActivated()
        {
            base.OnActivated();
            foreach (ObjectPropertyEditor objectPropertyEditor in View.GetItems<ObjectPropertyEditor>())
            {
                ((ISupportViewShowing)objectPropertyEditor).ViewShowingNotification += AssignDefaultAssocicationInfoController_ViewShowingNotification;
            }
        }

        private void AssignDefaultAssocicationInfoController_ViewShowingNotification(object sender, EventArgs e)
        {
            Application.ViewShowing += Application_ViewShowing;

        }

        private void Application_ViewShowing(object sender, ViewShowingEventArgs e)
        {
            Application.ViewShowing -= Application_ViewShowing;
            if (e.View != null && e.View.Id == "AssocicationInfo_DetailView")
            {
                var associcationInfo = (AssocicationInfo)e.View.CurrentObject;
                if (e.View.ObjectSpace.IsNewObject(associcationInfo))
                {
                    if (associcationInfo.Properties.Count < 1)
                    {
                        associcationInfo.Properties.Add(e.View.ObjectSpace.GetObject(ViewCurrentObject));
                    }
                    //shownAddress.Street = ViewCurrentObject.DefaultStreet;
                }
            }
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            foreach (ObjectPropertyEditor objectPropertyEditor in View.GetItems<ObjectPropertyEditor>())
            {
                ((ISupportViewShowing)objectPropertyEditor).ViewShowingNotification -= AssignDefaultAssocicationInfoController_ViewShowingNotification;
            }
        }

    }


    [PropertyEditor(typeof(AssocicationInfo), "OPE", false)]
    public class ObjectPropertyEditorEx : ObjectPropertyEditor,IComplexViewItem
    {
        public ObjectPropertyEditorEx(Type objectType, IModelMemberViewItem model) : base(objectType, model)
        {
        }
        XafApplication app;
        IObjectSpace os;

        void IComplexViewItem.Setup(IObjectSpace os, XafApplication app)
        {
            this.app = app;
            this.os = os;
            base.Setup(os, app);
        }

        protected override object CreateControlCore()
        {
            var c = base.CreateControlCore() as ObjectEdit;
            c.Properties.NullText = "<<自动设置>>";
            //c.ShowPopupWindow += C_ShowPopupWindow;
            c.Properties.NullValuePrompt = "自动设置";
            var manual = c.Properties.Buttons.FirstOrDefault(x => x.Kind == ButtonPredefines.Ellipsis);
            if (manual != null)
            {
                manual.ToolTip = "手动设置";
            }
            var clear = new EditorButton(ButtonPredefines.Delete);
            clear.Caption = "清除";
            c.ButtonClick += C_ButtonClick;
            c.Properties.Buttons.Add(clear);
            return c;
        }

        private void C_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.Delete)
            {
                var t = this.PropertyValue as XPBaseObject;
                t?.Delete();
                this.PropertyValue = null;
            }

        }
    }
}