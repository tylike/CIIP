using System.ComponentModel;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;

namespace CIIP.Win.General.DashBoard
{
    public interface IModelDashboardViewFilter : IModelNode {
        [DataSourceProperty("FilteredColumns")]
        [ModelBrowsable(typeof(DashboardViewFilterVisibilityCalculator))]
        IModelColumn FilteredColumn { get; set; }
        string ReportDataTypeMember { get; set; }
        [Browsable(false)]
        IModelList<IModelColumn> FilteredColumns { get; }
        [DataSourceProperty("DataSourceViews")]
        IModelListView DataSourceView { get; set; }
        [Browsable(false)]
        IModelList<IModelListView> DataSourceViews { get; }

        [DataSourceProperty("SummaryDataSourceViews")]
        [ModelBrowsable(typeof(DashboardViewFilterVisibilityCalculator))]
        IModelListView SummaryDataSourceView { get; set; }
        [Browsable(false)]
        IModelList<IModelListView> SummaryDataSourceViews { get; }
    }
}