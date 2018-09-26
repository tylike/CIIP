namespace CIIP.Designer
{
    public static class BusinessObjectCodeGenerateExtendesion
    {
        public static string CommonUsing()
        {
            return @"using System;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Xpo;
using System.Linq;
using System.ComponentModel;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
";
        }
    }
}