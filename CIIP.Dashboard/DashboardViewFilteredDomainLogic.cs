using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.PivotGrid;

namespace CIIP.Win.General.DashBoard
{
    [DomainLogic(typeof(IModelDashboardViewFilter))]
    public static class DashboardViewFilteredDomainLogic {
        public static IModelList<IModelListView> Get_SummaryDataSourceViews(IModelDashboardViewFilter modelDashboardViewFilter) {
            var calculatedModelNodeList = new CalculatedModelNodeList<IModelListView>();
            var modelView = ((IModelDashboardViewItemEx)modelDashboardViewFilter.Parent).View;
            var dashboardViewItemFiltereds = modelDashboardViewFilter.AllDashBoardViewItems().Where(filtered => filtered.View is IModelListView && modelView != filtered.View);
            var modelListViews = dashboardViewItemFiltereds.Select(filtered => filtered.View).OfType<IModelListView>().Where(view => typeof(PivotGridListEditorBase).IsAssignableFrom(view.EditorType));
            calculatedModelNodeList.AddRange(modelListViews);
            return calculatedModelNodeList;
        }

        public static IModelList<IModelListView> Get_DataSourceViews(IModelDashboardViewFilter modelDashboardViewFilter) {
            if (modelDashboardViewFilter.FilteredColumn != null) {
                var modelClass = modelDashboardViewFilter.Application.BOModel.GetClass(modelDashboardViewFilter.FilteredColumn.ModelMember.Type);
                var dashBoardViewItems = modelDashboardViewFilter.AllDashBoardViewItems();
                var modelListViews = dashBoardViewItems.Where(item => item.View != null && item.View.AsObjectView.ModelClass == modelClass).Select(filtered => filtered.View).OfType<IModelListView>();
                return new CalculatedModelNodeList<IModelListView>(modelListViews);
            }
            if (modelDashboardViewFilter.Parent is IModelDashboardViewItemEx)
                return new CalculatedModelNodeList<IModelListView>(modelDashboardViewFilter.AllDashBoardViewItems().Select(ex => ex.View).OfType<IModelListView>());
            return new CalculatedModelNodeList<IModelListView>();
        }

        public static IEnumerable<IModelDashboardViewItemEx> AllDashBoardViewItems(this IModelDashboardViewFilter modelDashboardViewFilter) {
            return ((IModelDashboardView)modelDashboardViewFilter.Parent.Parent.Parent).Items.OfType<IModelDashboardViewItemEx>();
        }

        public static IModelList<IModelColumn> Get_FilteredColumns(IModelDashboardViewFilter modelDashboardViewFilter) {
            var calculatedModelNodeList = new CalculatedModelNodeList<IModelColumn>();
            var modelListView = ((IModelDashboardViewItemEx)modelDashboardViewFilter.Parent).View as IModelListView;
            if (modelListView != null) {
                calculatedModelNodeList.AddRange(modelListView.Columns.Where(column => column.ModelMember.MemberInfo.MemberTypeInfo.IsDomainComponent));
            }
            return calculatedModelNodeList;
        }

    }
}