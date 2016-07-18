using System;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.Xpo.Metadata;

namespace CIIP.Module.BusinessObjects.SYS
{
    public class ModelNavigationToStringConverter : ValueConverter
    {
        public override Type StorageType
        {
            get
            {
                return typeof(string);
            }
        }

        public override object ConvertFromStorageType(object value)
        {
            if (value == null || object.Equals(value, string.Empty))
                return null;
            var model = CaptionHelper.ApplicationModel as IModelApplicationNavigationItems;
            //var nav = model.NavigationItems.AllItems.SingleOrDefault(x => (x as ModelNode).Path == (string)value);]
            var path = (string)value;
            path = path.Replace('/', '.');
            var nav = ModelNodePersistentPathHelper.FindValueByPath((ModelNode)model,path) as IModelNavigationItem;

            if (nav != null)
                return new NavigationItem(nav);
            return null;
        }

        public override object ConvertToStorageType(object value)
        {
            var v = (NavigationItem) value;
            return v?.Key;
        }
    }
}