using System;

namespace CIIP.CodeFirstView
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public class CategorySettingAttribute : Attribute
    {
        readonly string _propertyName;

        // This is a positional argument
        public CategorySettingAttribute(string propertyName, Type categoryType)
        {
            this._propertyName = propertyName;
            this.CategoryType = categoryType;
        }

        public string PropertyName
        {
            get { return _propertyName; }
        }

        public Type CategoryType { get; private set; }
    }
}