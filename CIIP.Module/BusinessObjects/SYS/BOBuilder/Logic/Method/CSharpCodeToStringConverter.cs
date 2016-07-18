using System;
using DevExpress.Xpo.Metadata;

namespace CIIP.Module.BusinessObjects.SYS.Logic
{
    public class CSharpCodeToStringConverter : ValueConverter
    {
        public override Type StorageType
        {
            get { return typeof (string); }
        }

        public override object ConvertFromStorageType(object value)
        {
            return new CsharpCode(value + "",null);
        }

        public override object ConvertToStorageType(object value)
        {
            var v = (CsharpCode) value;
            return v?.Code;
        }
    }
}