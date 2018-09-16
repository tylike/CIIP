using System;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using CIIP.Module.BusinessObjects;
using DevExpress.Persistent.Base.General;
using System.ComponentModel;

namespace CIIP.Module.Win.Editors
{
    internal class CategorizedListEditorPropertyDescriptor : PropertyDescriptor
    {
        // Fields
        private PropertyDescriptor descriptor;
        private Type objectType;

        // Methods
        public CategorizedListEditorPropertyDescriptor(Type objectType, PropertyDescriptor descriptor) : base(descriptor)
        {
            this.objectType = objectType;
            this.descriptor = descriptor;
        }

        public override bool CanResetValue(object component)
        {
            return this.descriptor.CanResetValue(component);
        }

        public override object GetValue(object theObject)
        {
            PropertyDescriptor descriptor;
            IVariablePropertiesCategorizedItem item = (IVariablePropertiesCategorizedItem)theObject;
            if (this.TryGetPropertyDescriptor(item, out descriptor))
            {
                return descriptor.GetValue(item.PropertyValueStore);
            }
            return null;
        }

        public override void ResetValue(object component)
        {
            this.descriptor.ResetValue(component);
        }

        public override void SetValue(object theObject, object theValue)
        {
            PropertyDescriptor descriptor;
            IVariablePropertiesCategorizedItem item = (IVariablePropertiesCategorizedItem)theObject;
            if (this.TryGetPropertyDescriptor(item, out descriptor))
            {
                descriptor.SetValue(item.PropertyValueStore, theValue);
            }
        }

        public override bool ShouldSerializeValue(object component)
        {
            return this.descriptor.ShouldSerializeValue(component);
        }

        private bool TryGetPropertyDescriptor(IVariablePropertiesCategorizedItem obj, out PropertyDescriptor propertyDescriptor)
        {
            try
            {
                propertyDescriptor = obj.GetPropertyDescriptorContainer().PropertyDescriptors[this.descriptor.Name];
            }
            catch
            {
                propertyDescriptor = null;
            }
            return (propertyDescriptor != null);
        }

        // Properties
        public override Type ComponentType
        {
            get
            {
                return this.objectType;
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return this.descriptor.IsReadOnly;
            }
        }

        public override Type PropertyType
        {
            get
            {
                return this.descriptor.PropertyType;
            }
        }
    }

    internal class CategorizedListEditorCustomTypeDescriptor : CustomTypeDescriptor
    {
        // Fields
        private PropertyDescriptorCollection properties = new PropertyDescriptorCollection(null);

        // Methods
        public CategorizedListEditorCustomTypeDescriptor(Type objectType, PropertyDescriptorCollection properties)
        {
            foreach (PropertyDescriptor descriptor in properties)
            {
                this.properties.Add(new CategorizedListEditorPropertyDescriptor(objectType, descriptor));
            }
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            return this.properties;
        }
    }

    internal class CategorizedListEditorTypeDescriptionProvider : TypeDescriptionProvider
    {
        // Fields
        private Type objectType;
        private CategorizedListEditorCustomTypeDescriptor objectTypeDescriptor;

        // Methods
        public CategorizedListEditorTypeDescriptionProvider(Type objectType) : base(TypeDescriptor.GetProvider(objectType))
        {
            this.objectType = objectType;
        }

        public void AddProvider()
        {
            TypeDescriptor.AddProvider(this, this.objectType);
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            if (this.objectType.IsAssignableFrom(objectType) && (this.objectTypeDescriptor != null))
            {
                return this.objectTypeDescriptor;
            }
            return base.GetTypeDescriptor(objectType, instance);
        }

        public void RemoveProvider()
        {
            TypeDescriptor.RemoveProvider(this, this.objectType);
        }

        public void Setup(PropertyDescriptorCollection properties)
        {
            this.objectTypeDescriptor = new CategorizedListEditorCustomTypeDescriptor(this.objectType, properties);
        }
    }

    [PropertyEditor(typeof(string), false)]
    public class AdmiralPopupExpressionPropertyEditor : PopupExpressionPropertyEditor
    {
        public AdmiralPopupExpressionPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model)
        {
        }

        protected override object CreateControlCore()
        {
            var rst =base.CreateControlCore()  as PopupExpressionEdit;
            
            return rst;
        }
    }
}