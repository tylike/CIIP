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
using DevExpress.ExpressApp.Model;
using CIIP.Module.BusinessObjects.Flow;

namespace CIIP.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DisableShowDetailViewViewController : ViewController<ListView>
    {
        public DisableShowDetailViewViewController()
        {
            InitializeComponent();
            this.RuntimeListViewSetup.Category = "Workflow";
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            this.View.CreateCustomCurrentObjectDetailView += View_CreateCustomCurrentObjectDetailView;
            var lvpcoc = Frame.GetController<ListViewProcessCurrentObjectController>();
            lvpcoc.ProcessCurrentObjectAction.Executing += ProcessCurrentObjectAction_Executing;
            
            //if (this.Frame is NestedFrame)
            //{
            //    var newObject = Frame.GetController<NewObjectViewController>();
            //    newObject.CollectDescendantTypes += NewObject_CollectDescendantTypes;
            //    newObject.CollectCreatableItemTypes += NewObject_CollectCreatableItemTypes;
            //}
            // Perform various tasks depending on the target View.
        }

        

        private void View_CreateCustomCurrentObjectDetailView(object sender, CreateCustomCurrentObjectDetailViewEventArgs e)
        {
            //当前有选中的对象
            if (e.ListViewCurrentObject != null)
            {
                //当前详细视图不为空，或 当前详细视图类型与当前对象类型不符
                if(string.IsNullOrEmpty(e.DetailViewId) || e.CurrentDetailView.Model.ModelClass.TypeInfo.Type != e.ListViewCurrentObject.GetType())
                {
                    e.DetailViewId = Application.FindDetailViewId(e.ListViewCurrentObject.GetType());
                }
                
            }
        }
        

        private void ProcessCurrentObjectAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var view = this.View.Model as IModelListViewSetting;
            
            if (view != null && view.DisableShowDetailView)
            {
                e.Cancel = true;
            }
        }

        

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            var lvpcoc = Frame.GetController<ListViewProcessCurrentObjectController>();
            lvpcoc.ProcessCurrentObjectAction.Executing -= ProcessCurrentObjectAction_Executing;
            //if (this.Frame is NestedFrame)
            //{
            //    var newObject = Frame.GetController<NewObjectViewController>();
            //    newObject.CollectDescendantTypes -= NewObject_CollectDescendantTypes;
            //}

            base.OnDeactivated();
        }

        private void RuntimeListViewSetup_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var obj = new ModelListViewSetting();
            var model = this.View.Model;
            var mvs = (model as IModelListViewSetting);
            obj.禁止详细视图 = mvs.DisableShowDetailView;
            obj.显示页角 = this.View.Model.IsFooterVisible;
            obj.编辑 = this.View.Model.AllowEdit;
            obj.新建位置 = this.View.Model.GetNewItemRow();

            var os = Application.CreateObjectSpace();

            e.ShowViewParameters.CreatedView = Application.CreateDetailView(os, obj, false);
            e.ShowViewParameters.NewWindowTarget = NewWindowTarget.Default;
            e.ShowViewParameters.Context = TemplateContext.PopupWindow; ;
            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;

            var dc = new DialogController();
            dc.Accepting += (snd, evt) =>
            {
                mvs.DisableShowDetailView = obj.禁止详细视图;
                model.IsFooterVisible = obj.显示页角;
                model.AllowEdit = obj.编辑;
                model.SetNewItemRow(obj.新建位置);

                this.Application.SaveModelChanges();
            };
            e.ShowViewParameters.Controllers.Add(dc);

        }
    }
}
