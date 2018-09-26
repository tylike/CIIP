using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace CIIP.Designer
{
    public class QuickAddBusinessObject : ObjectViewController<ListView, BusinessObject>
    {
        public QuickAddBusinessObject()
        {
            var batchCreate = new PopupWindowShowAction(this, "BatchCreate", PredefinedCategory.Unspecified);
            batchCreate.Execute += BatchCreate_Execute;
            batchCreate.CustomizePopupWindowParams += BatchCreate_CustomizePopupWindowParams;

        }

        private void BatchCreate_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace();
            var para = os.CreateObject<BatchInputBusinessObject>();
            var view = Application.CreateDetailView(os, para);
            //Application.ShowViewStrategy.ShowViewInPopupWindow(view, () => { });

            e.View = view;
        }

        private void BatchCreate_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {

        }
    }
    
}