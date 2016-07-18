using System;
using System.ComponentModel;
using DevExpress.ExpressApp.Model;

namespace CIIP.Win.General.DashBoard.Controllers
{
    public interface IModelOptionsDashboardNavigation : IModelNode {
        [Category("Navigation")]
        [DefaultValue("“«±Ì≈Ã")]
        String DashboardGroupCaption { get; set; }

        [DefaultValue(true)]
        [Category("Navigation")]
        bool DashboardsInGroup { get; set; }
    }
}