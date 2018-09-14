using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using CIIP.Persistent.BaseImpl;
using DevExpress.ExpressApp.Model;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using DevExpress.Persistent.BaseImpl;

namespace CIIP.Designer
{
    [NavigationItem]
    public class AssocicationInfo : NameObject
    {
        public AssocicationInfo(Session s) : base(s)
        {
        }

        [Association, DevExpress.Xpo.Aggregated,XafDisplayName("属性")]
        public XPCollection<AssocicationItem> Properties
        {
            get
            {
                return GetCollection<AssocicationItem>(nameof(Properties));
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

    public class AssocicationItem : BaseObject
    {
        public AssocicationItem(Session session) : base(session)
        {
        }

        [Association]
        public AssocicationInfo AssocicationInfo
        {
            get { return GetPropertyValue<AssocicationInfo>(nameof(AssocicationInfo)); }
            set { SetPropertyValue(nameof(AssocicationInfo), value); }
        }


        public PropertyBase Property
        {
            get { return GetPropertyValue<PropertyBase>(nameof(Property)); }
            set { SetPropertyValue(nameof(Property), value); }
        }

    }
}