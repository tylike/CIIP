using System.Collections.Generic;
using System.Drawing;

namespace CIIP.Win.General.DashBoard.BusinessObjects {
    public interface IDashboardDefinition {
        int Index { get; set; }
        string Name { get; set; }
        Image Icon { get; set; }
        bool Active { get; set; }
        string Xml { get; set; }
        IList<ITypeWrapper> DashboardTypes { get; }
    }
}
