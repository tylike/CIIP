using CIIP.Win;
using DevExpress.ExpressApp.Model;


namespace CIIP.Win.General.DashBoard {
    public class DashboardViewFilterVisibilityCalculator : IModelIsVisible {
        #region Implementation of IModelIsVisible
        public bool IsVisible(IModelNode node, string propertyName) {
            return !(node.Parent is IModelDashboardReportViewItem);
        }
        #endregion
    }
}
