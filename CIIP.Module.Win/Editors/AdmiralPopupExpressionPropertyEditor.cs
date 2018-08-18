using System;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using CIIP.Module.BusinessObjects;
using CIIP.Module.BusinessObjects.Flow;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Win.Controls;
using DevExpress.Persistent.Base.General;
using System.ComponentModel;
using System.Collections;
using DevExpress.ExpressApp.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.TreeListEditors.Win;
using CIIP.CodeFirstView;

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

    [ListEditor(typeof(ICategorizedItem), true)]
    public class CategorizedListEditorEx : GridListEditor
    {
        // Fields
        private CollectionSourceBase categoriesDataSource;
        private ListView categoriesListView;
        private string categoriesListViewId;
        private object categoryKey;
        public string CategoryPropertyName = "Category";
        private string criteriaPropertyName;
        private bool isCriteriaUpdating;
        private LayoutManager layoutManager;
        private CategorizedListEditorTypeDescriptionProvider typeDescriptionProvider;
        private Locker updateCriteriaLocker;
        private const string UpdateCriteriaMethodName = "UpdateCriteria";

        // Methods
        public CategorizedListEditorEx(IModelListView info) : base(info)
        {
            this.updateCriteriaLocker = new Locker();
            this.updateCriteriaLocker.LockedChanged += new EventHandler<LockedChangedEventArgs>(this.updateCriteriaLocker_LockedChanged);
        }

        protected override void AssignDataSourceToControl(object dataSource)
        {
            CategorizedListEditorDataSource source;

            if (dataSource != null)
            {
                source = new CategorizedListEditorDataSource(dataSource, base.ObjectType);
            }
            else
            {
                source = null;
            }
            Type c = this.ItemsDataSource.ObjectTypeInfo.Type;
            if ((typeof(IVariablePropertiesCategorizedItem).IsAssignableFrom(c) && (this.CategoriesListView != null)) && (this.CategoriesListView.CurrentObject != null))
            {
                IPropertyDescriptorContainer currentObject = (IPropertyDescriptorContainer)this.CategoriesListView.CurrentObject;
                this.typeDescriptionProvider.Setup(currentObject.PropertyDescriptors);
            }
            base.AssignDataSourceToControl(source);
        }

        private void SetDataSource(object dataSource)
        {

        }

        int cnt = 0;
        private void categoriesListView_SelectionChanged(object sender, EventArgs e)
        {
            if (cnt > 0)
            {
                this.UpdateCriteria();
                this.UpdateColumns();
                AutoLoad = true;
            }
            else
            {
                cnt++;
            }

        }

        protected override object CreateControlsCore()
        {
            if (this.layoutManager == null)
            {
                this.layoutManager = base.Application.CreateLayoutManager(true);
                this.categoriesListView = base.Application.CreateListView(this.categoriesListViewId, this.CategoriesDataSource, false);
                this.categoriesListView.Caption = "Category";
                this.categoriesListView.SelectionChanged += new EventHandler(this.categoriesListView_SelectionChanged);
                this.categoriesListView.CreateControls();
                if (this.ObjectTreeList != null)
                {
                    this.ObjectTreeList.OptionsSelection.MultiSelect = false;
                }
                ViewItemsCollection detailViewItems = new ViewItemsCollection();
                detailViewItems.Add(new ControlViewItem("1", this.categoriesListView.Control));
                detailViewItems.Add(new ControlViewItem("2", base.CreateControlsCore()));
                this.layoutManager.LayoutControls(base.Model.SplitLayout, detailViewItems);
                this.SubscribeToTreeList();
            }
            return this.layoutManager.Container;
        }

        public override void Dispose()
        {
            try
            {
                this.UnsubscribeFromTreeList();
                if (this.typeDescriptionProvider != null)
                {
                    this.typeDescriptionProvider.RemoveProvider();
                    this.typeDescriptionProvider = null;
                }
                if (this.layoutManager != null)
                {
                    this.layoutManager.Dispose();
                    this.layoutManager = null;
                }
                if (this.categoriesListView != null)
                {
                    this.categoriesListView.SelectionChanged -= new EventHandler(this.categoriesListView_SelectionChanged);
                    this.categoriesListView.Dispose();
                    this.categoriesListView = null;
                }
                if (this.categoriesDataSource != null)
                {
                    this.categoriesDataSource.Dispose();
                    this.categoriesDataSource = null;
                }
                if (this.ItemsDataSource != null)
                {
                    this.ItemsDataSource.ObjectSpace.Reloaded -= new EventHandler(this.ObjectSpace_Reloaded);
                }
                this.updateCriteriaLocker.LockedChanged -= new EventHandler<LockedChangedEventArgs>(this.updateCriteriaLocker_LockedChanged);
            }
            finally
            {
                base.Dispose();
            }
        }

        private void ObjectSpace_Reloaded(object sender, EventArgs e)
        {
            this.UpdateCriteria();
        }

        private void objectTreeList_NodesReloaded(object sender, EventArgs e)
        {
            this.updateCriteriaLocker.Unlock();
        }

        private void objectTreeList_NodesReloading(object sender, EventArgs e)
        {
            this.updateCriteriaLocker.Lock();
        }

        public override void SaveModel()
        {
            base.SaveModel();
            if (this.layoutManager != null)
            {
                this.layoutManager.SaveModel();
            }
        }
        CategorySettingAttribute Setting;
        public override void Setup(CollectionSourceBase collectionSource, XafApplication application)
        {
            base.Setup(collectionSource, application);
            AutoLoad = false;
            IObjectSpace objectSpace = collectionSource.ObjectSpace;
            objectSpace.Reloaded += new EventHandler(this.ObjectSpace_Reloaded);
            ITypeInfo objectTypeInfo = collectionSource.ObjectTypeInfo;
            var customSetting = ObjectTypeInfo.FindAttribute<CategorySettingAttribute>();
            Setting = customSetting;
            Type memberType = null;
            if (customSetting == null)
            {
                IMemberInfo info2 = objectTypeInfo.FindMember("Category");
                if (info2 == null)
                {
                    throw new InvalidOperationException(string.Format("The {0} type does not declare the public {1} property.", objectTypeInfo.FullName, "Category"));
                }
                memberType = info2.MemberType;
            }
            else
            {
                memberType = customSetting.CategoryType;
                CategoryPropertyName = Setting.PropertyName;
            }

            this.categoriesListViewId = application.FindListViewId(memberType);
            if (string.IsNullOrEmpty(this.categoriesListViewId))
            {
                throw new InvalidOperationException(string.Format("Cannot find ListView for the Category property type {0} in the Application Model. Make sure the property is of the business object type registered in the application.", memberType.FullName));
            }
            if ((base.Model != null) && (this.categoriesListViewId == base.Model.Id))
            {
                throw new InvalidOperationException(string.Format("The default category ListView ({0}) is the same as the container ListView. To avoid recursion, provide different EditorType settings for it.", this.categoriesListViewId));
            }
            this.categoriesDataSource = application.CreateCollectionSource(objectSpace, memberType, this.categoriesListViewId);
            this.criteriaPropertyName = "Category." + objectSpace.GetKeyPropertyName(memberType);
            this.typeDescriptionProvider = new CategorizedListEditorTypeDescriptionProvider(objectTypeInfo.Type);
            this.typeDescriptionProvider.AddProvider();
        }

        private void SubscribeToTreeList()
        {
            if (this.ObjectTreeList != null)
            {
                this.ObjectTreeList.NodesReloading += new EventHandler(this.objectTreeList_NodesReloading);
                this.ObjectTreeList.NodesReloaded += new EventHandler(this.objectTreeList_NodesReloaded);
            }
        }

        private void UnsubscribeFromTreeList()
        {
            if (this.ObjectTreeList != null)
            {
                this.ObjectTreeList.NodesReloading -= new EventHandler(this.objectTreeList_NodesReloading);
                this.ObjectTreeList.NodesReloaded -= new EventHandler(this.objectTreeList_NodesReloaded);
            }
        }

        private void UpdateColumns()
        {
            Type c = this.ItemsDataSource.ObjectTypeInfo.Type;
            if (typeof(IVariablePropertiesCategorizedItem).IsAssignableFrom(c) && (this.CategoriesListView.CurrentObject != null))
            {
                IPropertyDescriptorContainer currentObject = (IPropertyDescriptorContainer)this.CategoriesListView.CurrentObject;
                object keyValue = this.CategoriesListView.ObjectSpace.GetKeyValue(currentObject);
                if (this.categoryKey != keyValue)
                {
                    this.SaveModel();
                    this.FocusedObject = null;
                    this.typeDescriptionProvider.Setup(currentObject.PropertyDescriptors);
                    string id = c.Name + "_" + keyValue.ToString() + "_ListView";
                    IModelViews parent = (IModelViews)base.Model.Parent;
                    IModelListView newModel = (IModelListView)parent[id];
                    if (newModel == null)
                    {
                        newModel = (IModelListView)((ModelNode)base.Model).Clone(id);
                    }
                    base.SetModel(newModel);
                    this.ApplyModel();
                    this.ItemsDataSource.DisplayableProperties = string.Join(";", this.RequiredProperties);
                    this.categoryKey = keyValue;
                }
            }
        }

        private void UpdateCriteria()
        {
            this.isCriteriaUpdating = true;
            try
            {
                this.updateCriteriaLocker.Call("UpdateCriteria");
                if (!this.updateCriteriaLocker.Locked && (this.CategoriesListView != null))
                {
                    object currentObject = this.CategoriesListView.CurrentObject;
                    if (currentObject != null)
                    {
                        ArrayList categoryKeys = new ArrayList();
                        ITreeNode currentCategory = (ITreeNode)CategoriesListView.CurrentObject;
                        categoryKeys.Add(GetCategoryKey(currentCategory));
                        AddChildrenKeys(currentCategory, categoryKeys);
                        string categoryKeyPropertyName = String.Format("{0}.{1}", CategoryPropertyName, CategoriesListView.ObjectTypeInfo.KeyMember.Name);
                        this.ItemsDataSource.Criteria[CategoryPropertyName] = new InOperator(categoryKeyPropertyName, categoryKeys);

                        //this.ItemsDataSource.Criteria["Category"] = new BinaryOperator(this.criteriaPropertyName, this.CategoriesListView.ObjectSpace.GetKeyValue(currentObject));
                    }
                    else
                    {
                        this.ItemsDataSource.Criteria.Remove(CategoryPropertyName);// = new NullOperator(CategoryPropertyName);
                    }
                }
            }
            finally
            {
                this.isCriteriaUpdating = false;
            }
        }
        private void AddChildrenKeys(ITreeNode current, IList categoryKeys)
        {
            foreach (ITreeNode child in current.Children)
            {
                categoryKeys.Add(GetCategoryKey(child));
                AddChildrenKeys(child, categoryKeys);
            }
        }
        private object GetCategoryKey(object category)
        {
            return CategoriesListView.ObjectSpace.GetKeyValue(category);
        }

        private void updateCriteriaLocker_LockedChanged(object sender, LockedChangedEventArgs e)
        {
            if (!e.Locked && e.PendingCalls.Contains("UpdateCriteria"))
            {
                this.UpdateCriteria();
            }
        }

        // Properties
        public override BorderStyles BorderStyle
        {
            set
            {
                base.BorderStyle = value;
                if (this.ObjectTreeList != null)
                {
                    this.ObjectTreeList.BorderStyle = value;
                }
            }
        }

        protected CollectionSourceBase CategoriesDataSource
        {
            get
            {
                return this.categoriesDataSource;
            }
        }

        public ListView CategoriesListView
        {
            get
            {
                return this.categoriesListView;
            }
        }

        public override object FocusedObject
        {
            get
            {
                return base.FocusedObject;
            }
            set
            {
                if ((this.ObjectTreeList != null) && (value != null) && value is ICategorizedItem)
                {
                    ITreeNode category = ((ICategorizedItem)value).Category;
                    if (!this.isCriteriaUpdating && (category != null))
                    {
                        this.ObjectTreeList.FocusedObject = category;
                    }
                }
                base.FocusedObject = value;
            }
        }

        protected CollectionSourceBase ItemsDataSource
        {
            get
            {
                return base.CollectionSource;
            }
        }

        public ObjectTreeList ObjectTreeList
        {
            get
            {
                if ((this.CategoriesListView != null) && (this.CategoriesListView.Editor != null))
                {
                    return (this.CategoriesListView.Editor.Control as ObjectTreeList);
                }
                return null;
            }
        }
        public bool AutoLoad
        {
            set
            {
                if (value)
                {
                    ItemsDataSource.Criteria.Remove("DontLoad");
                }
                else
                {
                    ItemsDataSource.Criteria.Add("DontLoad", CriteriaOperator.Parse("1=0"));

                }
            }
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