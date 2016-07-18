using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;

namespace CIIP.Module.BusinessObjects.SYS
{
    public class ModelDataSource
    {
        static List<NavigationItem> _navigationItemDataSources;
        public static List<NavigationItem> NavigationItemDataSources
        {
            get
            {
                if (_navigationItemDataSources == null)
                {
                    _navigationItemDataSources = new List<NavigationItem>();
                    var model = CaptionHelper.ApplicationModel as IModelApplicationNavigationItems;
                    foreach (var item in model.NavigationItems.Items.Where(x => x.Visible).OrderBy(x => x.Index).ToArray())
                    {
                        var ni = new NavigationItem(item);
                        _navigationItemDataSources.Add(ni);
                    }
                }
                return _navigationItemDataSources;
            }
        }
    }
}