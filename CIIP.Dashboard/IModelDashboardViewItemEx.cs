using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;

namespace CIIP.Win.General.DashBoard
{
    [ModelAbstractClass]
    public interface IModelDashboardViewItemEx : IModelDashboardViewItem {
        IModelDashboardViewFilter Filter { get; }
        [DefaultValue(ViewItemVisibility.Show)]
        ViewItemVisibility Visibility { get; set; }
        MasterDetailMode? MasterDetailMode { get; set; }
    }
}