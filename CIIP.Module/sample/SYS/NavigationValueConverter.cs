using System;
using System.Linq;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.Xpo.Metadata;

namespace CIIP.Module.BusinessObjects.SYS
{
    public class NavigationValueConverter : ValueConverter
    {
        public override object ConvertFromStorageType(object stringObjectType)
        {
            var item =
                (CaptionHelper.ApplicationModel as IModelApplicationNavigationItems).NavigationItems.AllItems
                    .FirstOrDefault(x => object.Equals((x as ModelNode).Path, stringObjectType));
            return new ModelNavigation(item);
        }

        public override object ConvertToStorageType(object objectType)
        {
            if (objectType == null)
            {
                return null;
            }
            return ((ModelNavigation) objectType).Path;
        }

        public override Type StorageType
        {
            get { return typeof (string); }
        }
    }
}