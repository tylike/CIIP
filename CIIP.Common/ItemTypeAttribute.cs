using System;

namespace CIIP
{
    [AttributeUsage(AttributeTargets.GenericParameter, AllowMultiple = false, Inherited = false)]
    public class ItemTypeAttribute : Attribute
    {

        public Type ItemType { get; set; }

        public ItemTypeAttribute(Type itemType)
        {
            this.ItemType = itemType;
        }
    }
}