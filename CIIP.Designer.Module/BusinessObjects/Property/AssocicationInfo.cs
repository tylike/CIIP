using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using CIIP.Persistent.BaseImpl;
using DevExpress.ExpressApp.Model;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;

namespace CIIP.Designer
{
    [NavigationItem]    
    public class AssocicationInfo : NameObject
    {
        public AssocicationInfo(Session s) : base(s)
        {
            Properties.CollectionChanged += Properties_CollectionChanged;
        }

        private void Properties_CollectionChanged(object sender, XPCollectionChangedEventArgs e)
        {
            if (!IsLoading && !IsSaving)
            {
                _propertiesDataSource = null;
                OnChanged(nameof(CollectionPropertyCount));
                OnChanged(nameof(ReferencePropertyCount));
                OnChanged(nameof(PropertyCount));
                calcName();
            }
        }

        void calcName()
        {
            var ps = Properties.OrderBy(x => x.Name);
            var first = ps.FirstOrDefault();
            var last = ps.LastOrDefault();
            Name = first?.DisplayName + "-" + last?.DisplayName;
        }

        [RuleValueComparison(ValueComparisonType.GreaterThanOrEqual, 1, CustomMessageTemplate = "至少应有一个集合属性!")]
        [XafDisplayName("集合属性数量")]
        public int CollectionPropertyCount { get { return Properties.OfType<CollectionProperty>().Count(); } }

        [RuleValueComparison(ValueComparisonType.LessThan,2,CustomMessageTemplate ="最多只能有一个引用型属性,即为一对多关系.")]
        [XafDisplayName("引用属性数量")]
        public int ReferencePropertyCount { get { return Properties.OfType<Property>().Where(x => x.PropertyType.CanCreateAssocication).Count(); } }

        [RuleValueComparison(ValueComparisonType.Equals,2,CustomMessageTemplate ="必须有选择两个属性!")]
        [XafDisplayName("属性数量")]
        public int PropertyCount { get { return Properties.Count; } }

        public string 关系类型
        {
            get
            {
                if (PropertyCount == 2 && (ReferencePropertyCount + CollectionPropertyCount) == 2)
                {
                    if (CollectionPropertyCount == 2)
                    {
                        return "对多对";
                    }
                    return "一对多";
                }
                return "";
            }
        }


        [Association,XafDisplayName("属性"),DataSourceProperty(nameof(PropertiesDataSource))]
        public XPCollection<PropertyBase> Properties
        {
            get
            {
                return GetCollection<PropertyBase>(nameof(Properties));
            }
        }

        List<PropertyBase> _propertiesDataSource;
        List<PropertyBase> PropertiesDataSource
        {
            get
            {
                if (_propertiesDataSource == null)
                {
                    var fromProperty = Properties.FirstOrDefault();
                    if (fromProperty != null)
                    {
                        _propertiesDataSource = Session.QueryInTransaction<PropertyBase>().ToArray().Where(x => x.PropertyType.CanCreateAssocication && x.BusinessObject == fromProperty.PropertyType && !Properties.Contains(x)).ToList();
                    }
                    else
                    {
                        throw new UserFriendlyException("错误,请先将来源属性填加!");
                    }
                }
                return _propertiesDataSource;
            }
        }

        //[XafDisplayName("业务")]
        //[LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
        //[DataSourceCriteria("IsPersistent")]
        //[ImmediatePostData]
        //public BusinessObject LeftTable
        //{
        //    get { return GetPropertyValue<BusinessObject>(nameof(LeftTable)); }
        //    set { SetPropertyValue(nameof(LeftTable), value); }
        //}

        //[XafDisplayName("属性")]
        //[DataSourceProperty(nameof(LeftProperties))]
        //[LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
        //public PropertyBase LeftProperty
        //{
        //    get { return GetPropertyValue<PropertyBase>(nameof(LeftProperty)); }
        //    set { SetPropertyValue(nameof(LeftProperty), value); }
        //}

//        List<PropertyBase> leftProperties;
//        List<PropertyBase> LeftProperties
//        {
//            get
//            {
//                if (leftProperties == null)
//                {
//                    if (LeftTable == null)
//                    {
//                        leftProperties = new List<PropertyBase>();

//                    }
//                    else
//                    {
//                        //多
//                        if (ManyToMany)
//                        {
//                            leftProperties = LeftTable.Properties.Where(
//                            x => x is CollectionProperty &&
//                                (x.AssocicationInfo != null && x.AssocicationInfo == this)
//                                ||
//                                (x.AssocicationInfo == null)
//                            ).ToList();
//                        }
//                        //一
//                        else
//                        {
//                            leftProperties = LeftTable.Properties.Where(
//                                x =>
//                                    x.PropertyType is BusinessObject &&
//                                    x is Property &&
//                                    (x.AssocicationInfo == null || x.AssocicationInfo == this)
//                            ).ToList();
//                        }
//                    }
//                }
//                return leftProperties;
//            }
//        }

//        List<CollectionProperty> rightProperties;
//        List<CollectionProperty> RightProperties
//        {
//            get
//            {
//                if (rightProperties == null)
//                {
//                    if (RightTable == null)
//                    {
//                        rightProperties = new List<CollectionProperty>();

//                    }
//                    else
//                    {
//                        //必须是集合属性
//                        rightProperties = RightTable.Properties.Where(
//                            x => x is CollectionProperty &&
//                                (x.AssocicationInfo != null && x.AssocicationInfo == this)
//                                ||
//                                (x.AssocicationInfo == null)
//                            ).OfType<CollectionProperty>().ToList();
//                    }
//                }
//                return rightProperties;
//            }
//        }

//        protected override void OnChanged(string propertyName, object oldValue, object newValue)
//        {
//            base.OnChanged(propertyName, oldValue, newValue);
//            if (!IsLoading)
//            {
//                if (propertyName == nameof(ManyToMany))
//                {
//                    leftProperties = null;
//                }
//                if (propertyName == nameof(LeftTable))
//                {
//                    LeftProperty = LeftProperties.FirstOrDefault();
//                    CalcName();
//                }

//                //左右任一属性变化时,都给属性的assocication赋值
//                if (propertyName == nameof(LeftProperty))
//                {
//                    CalcName();
//                    if (LeftProperty != null)
//                    {
//#warning 不应该存在已有associcationInfo的数据
//                        if (LeftProperty.AssocicationInfo == null || LeftProperty.AssocicationInfo.Oid != this.Oid)
//                        {
//                            LeftProperty.AssocicationInfo = this;
//                        }
//                    }

//                    var ov = oldValue as PropertyBase;
//                    if (ov != null)
//                    {
//                        ov.AssocicationInfo = null;
//                    }
//                    RightProperty.CalcNameCaption();
//                }

//                if (propertyName == nameof(RightProperty))
//                {
//                    CalcName();
//                    if (RightProperty != null && RightProperty.AssocicationInfo.Oid != this.Oid)
//                        RightProperty.AssocicationInfo = this;

//                    var ov = oldValue as PropertyBase;
//                    if (ov != null)
//                    {
//                        ov.AssocicationInfo = null;
//                    }
//                }

//                Debug.WriteLine(propertyName + ":" + (oldValue + "").ToString() + "," + (newValue + "").ToString());

//            }

//        }

//        public void CalcName()
//        {
//            Name = LeftProperty?.DisplayName + "-" + RightProperty?.DisplayName;
//        }

//        [XafDisplayName("关系数量"), CaptionsForBoolValues("多对多", "一对多")]
//        [ImmediatePostData]
//        public bool ManyToMany
//        {
//            get { return GetPropertyValue<bool>(nameof(ManyToMany)); }
//            set { SetPropertyValue(nameof(ManyToMany), value); }
//        }

//        [XafDisplayName("业务")]
//        //[ModelDefault("AllowEdit","False")]
//        public BusinessObject RightTable
//        {
//            get { return GetPropertyValue<BusinessObject>(nameof(RightTable)); }
//            set { SetPropertyValue(nameof(RightTable), value); }
//        }

//        [XafDisplayName("属性")]
//        //[ModelDefault("AllowEdit", "False")]
//        [DataSourceProperty(nameof(RightProperties))]
//        public PropertyBase RightProperty
//        {
//            get { return GetPropertyValue<PropertyBase>(nameof(RightProperty)); }
//            set { SetPropertyValue(nameof(RightProperty), value); }
//        }
    }
    
    public class CreateAssocicationPropertyViewController : ObjectViewController<ListView, PropertyBase>
    {
        public CreateAssocicationPropertyViewController()
        {
            TargetViewNesting = Nesting.Nested;
            TargetViewId = "AssocicationInfo_Properties_ListView";

            var crp = new SimpleAction(this, "CreateAssocicationReferenceProperty", PredefinedCategory.ObjectsCreation);
            crp.Caption = "自动创建引用属性";
            crp.Execute += Crp_Execute;
            var ccp = new SimpleAction(this, "CreateAssocicationCollectionProperty", PredefinedCategory.ObjectsCreation);
            ccp.Caption = "自动创建集合属性";
            ccp.Execute += Ccp_Execute;
            //action.Items.Add()
        }
        
        private void Ccp_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //创建集合属性.
            var cs = this.View.CollectionSource;//.List.FirstOrDefault();
            var ai = (Frame as NestedFrame).ViewItem.CurrentObject as AssocicationInfo;
            if (ai.Properties.Count == 1)
            {
                var cp = ai.Properties[0];
                //当前是xpcollection<学生> 学生s {get;} 属性
                //自动创建的属性是 xpcollection<教师> 教师s {get;} 属性
                var property = ObjectSpace.CreateObject<CollectionProperty>();
                property.BusinessObject = cp.PropertyType;
                property.PropertyType = cp.BusinessObject;
                property.Name = cp.BusinessObject.Name;
                property.Caption = cp.BusinessObject.Caption;
                ai.Properties.Add(property);
            }
            else
            {
                Application.ShowViewStrategy.ShowMessage("当前列表中有一个属性时才可以推导创建出另一个属性!", InformationType.Info, 3000, InformationPosition.Bottom);
            }
        }

        private void Crp_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //创建引用属性
            var cs = this.View.CollectionSource;//.List.FirstOrDefault();
            var ai = (Frame as NestedFrame).ViewItem.CurrentObject as AssocicationInfo;
            if (ai.Properties.Count == 1)
            {
                var cp = ai.Properties[0];                
                //    //当前是xpcollection<order> orders {get;} 属性
                //    //自动创建的属性是 customer customer {get;} 属性
                var property = ObjectSpace.CreateObject<Property>();
                property.BusinessObject = cp.PropertyType;
                property.PropertyType = cp.BusinessObject;
                property.Name = cp.BusinessObject.Name;
                property.Caption = cp.BusinessObject.Caption;
                ai.Properties.Add(property);
            }
            else
            {
                Application.ShowViewStrategy.ShowMessage("当前列表中有一个属性时才可以推导创建出另一个属性!", InformationType.Info, 3000, InformationPosition.Bottom);
            }
        }
    }
}