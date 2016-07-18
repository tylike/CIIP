using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;

namespace CIIP.Win.General.DashBoard
{
    public static class DashboardViewItemExtensions {
        public static IModelDashboardViewItem GetModel(this DashboardViewItem item, DashboardView view) {
            return (IModelDashboardViewItem)view.Model.Items[item.Id];
        }
    }
}