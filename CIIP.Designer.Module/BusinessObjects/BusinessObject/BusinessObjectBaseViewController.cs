using DevExpress.Persistent.Base;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;

namespace CIIP.Designer
{
    public class BusinessObjectBaseViewController : ObjectViewController<ObjectView,BusinessObjectBase>
    {
        public BusinessObjectBaseViewController()
        {
            var showImplementInterfaces = new SimpleAction(this, "ShowImplementInterfaces", "ShowImplementInterfaces");
            showImplementInterfaces.Execute += ShowImplementInterfaces_Execute;
            showImplementInterfaces.Caption = "查看";
            var showGenericParameters = new SimpleAction(this, "ShowGenericParameters", "ShowGenericParameters");
            showGenericParameters.Execute += ShowGenericParameters_Execute;
            showGenericParameters.Caption = "查看";
        }

        private void ShowGenericParameters_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ShowProxyListView(e, "GenericParameterDefines");
        }

        private void ShowProxyListView(SimpleActionExecuteEventArgs e,  string memberName)
        {
            var masterObject = this.View.CurrentObject;
            var memberInfo = this.View.ObjectTypeInfo.FindMember(memberName);
            var listViewID = ModelNodeIdHelper.GetNestedListViewId(memberInfo.Owner.Type, memberName);
            var collectionSource = Application.CreatePropertyCollectionSource(ObjectSpace, typeof(MasterDetailMode), masterObject, memberInfo, listViewID);
            var view = Application.CreateListView(listViewID, collectionSource, false);
            e.ShowViewParameters.CreatedView = view;
            e.ShowViewParameters.Context = TemplateContext.View;
            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            var dc = new DialogController();
            dc.SaveOnAccept = false;
            //e.ShowViewParameters.Controllers.Add(dc);
        }

        private void ShowImplementInterfaces_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            
            ShowProxyListView(e, "ImplementInterfaces");

        }
    }
}