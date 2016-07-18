using System;

namespace CIIP.Module.BusinessObjects.Flow
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class IgnoreFormConvertAttribute : Attribute
    {

    }
}