using System;
using CIIP.Module.BusinessObjects.SYS;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;

namespace CIIP.Designer
{
    public static class IObjectSpaceExtend
    {
        public static BusinessObject FindBusinessObject(this IObjectSpace os,Type type)
        {
            return os.FindObject<BusinessObject>(new BinaryOperator("FullName", type.FullName));
        }
        public static BusinessObject FindBusinessObject<T>(this IObjectSpace os)
        {
            return os.FindObject<BusinessObject>(new BinaryOperator("FullName", typeof (T).FullName));
        }
    }
}