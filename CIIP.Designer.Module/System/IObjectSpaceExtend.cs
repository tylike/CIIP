using System;
using DevExpress.ExpressApp;

namespace CIIP
{
    public static class IObjectSpaceExtend
    {
        public static object GetObjectByStringKey(this IObjectSpace self, Type type, string key)
        {
            return self.GetObjectByKey(type, self.GetObjectKey(type, key));
        }
    }
}