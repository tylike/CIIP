using System;

namespace CIIP
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple =false,Inherited = false)]
    public class BusinessConfigTypeAttribute : Attribute
    {
        public Type BusinessConfig { get; set; }            
        public BusinessConfigTypeAttribute(Type configType)
        {
            this.BusinessConfig = configType;
        }
    }
}