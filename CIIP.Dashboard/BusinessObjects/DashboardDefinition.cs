using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Xml;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;

namespace CIIP.Win.General.DashBoard.BusinessObjects {
    [ImageName("BO_DashboardDefinition")]
    [DefaultProperty("Name")]
    [DefaultClassOptions]
    [CreatableItem(false)]
    [NavigationItem("Reports")]
    public class DashboardDefinition : BaseObject, IDashboardDefinition {

        bool _active;
        BindingList<ITypeWrapper> _dashboardTypes;
        int _index;

        string _name;
        [Size(SizeAttribute.Unlimited)]
        [Persistent("TargetObjectTypes")]
        string _targetObjectTypes;
        IList<TypeWrapper> _types;

        public DashboardDefinition(Session session)
            : base(session) {
        }

        [Browsable(false)]
        internal IList<TypeWrapper> Types {
            get {
                return _types ?? (_types = XafTypesInfo.Instance.PersistentTypes
                                                       .Where(info => (info.IsVisible && info.IsPersistent) && (info.Type != null))
                                                       .Select(info => new TypeWrapper(Session, info.Type))
                                                       .OrderBy(info => info.Caption)
                                                       .ToList());
            }
        }

        public int Index {
            get { return _index; }
            set { SetPropertyValue("Index", ref _index, value); }
        }

        public string Name {
            get { return _name; }
            set { SetPropertyValue("Name", ref _name, value); }
        }

        public bool Active {
            get { return _active; }
            set { SetPropertyValue("Active", ref _active, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        [Delayed]
        [VisibleInDetailView(false)]
        public string Xml {
            get { return GetDelayedPropertyValue<String>("Xml"); }
            set { SetDelayedPropertyValue("Xml", value); }
        }

        [ModelDefault("DetailViewImageEditorFixedHeight", "32"), ModelDefault("DetailViewImageEditorFixedWidth", "32"),
         ModelDefault("ListViewImageEditorCustomHeight", "32")]
        [ImmediatePostData, ValueConverter(typeof(ImageValueConverter))]
        [Size(SizeAttribute.Unlimited), Delayed(true)]
        public Image Icon {
            get { return GetDelayedPropertyValue<Image>("Icon"); }
            set { SetDelayedPropertyValue("Icon", value); }
        }

        [DataSourceProperty("Types")]
        [ImmediatePostData]
        //[EditorAlias("Xpand.Dashboard.Win.PropertyEditors.ChooseFromListCollectionEditor")]
        public IList<ITypeWrapper> DashboardTypes {
            get { return GetBindingList(); }
        }

        BindingList<ITypeWrapper> GetBindingList() {
            return _dashboardTypes ?? (_dashboardTypes = new BindingList<ITypeWrapper>());
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            SubscribeToListEvents();
        }


        protected override void OnLoaded() {
            base.OnLoaded();
            ReloadDashboardItems();
        }

        void UpdateTargetObjectTypes() {
            _targetObjectTypes = "<Types>\r\n";
            foreach (TypeWrapper resource in DashboardTypes)
                _targetObjectTypes += string.Format("<Value Type=\"{0}\"/>\r\n", resource.Type.FullName);
            _targetObjectTypes += "</Types>";
        }

        void ReloadDashboardItems() {
            UnsubscribeToListEvents();
            try {
                DashboardTypes.Clear();
                if (!String.IsNullOrEmpty(_targetObjectTypes)) {
                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(_targetObjectTypes);
                    if (xmlDocument.DocumentElement != null)
                        foreach (XmlNode xmlNode in xmlDocument.DocumentElement.ChildNodes)
                            DashboardTypes.Add(Types.First(type => xmlNode.Attributes != null && type.Type == XafTypesInfo.Instance.FindTypeInfo(xmlNode.Attributes["Type"].Value).Type));
                }
            } finally {
                SubscribeToListEvents();
            }
        }

        void SubscribeToListEvents() {
            ((IBindingList)DashboardTypes).ListChanged += DashboardDefinition_ListChanged;
        }

        void UnsubscribeToListEvents() {
            ((IBindingList)DashboardTypes).ListChanged -= DashboardDefinition_ListChanged;
        }

        void DashboardDefinition_ListChanged(object sender, ListChangedEventArgs e) {
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted) {
                UpdateTargetObjectTypes();
                OnChanged("TargetObjectTypes");
            }
        }
    }
}
