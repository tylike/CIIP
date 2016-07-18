using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace CIIP.Module.BusinessObjects.SYS
{
    public class Property : PropertyBase
    {
        private BusinessObject _Owner;

        [Association]
        public BusinessObject Owner
        {
            get { return _Owner; }
            set { SetPropertyValue("Owner", ref _Owner, value); }
        }

        private string _Expression;

        [XafDisplayName("计算公式")]
        [ToolTip("填正了公式后，此属性将为只读，使用公式进行计算")]
        public string Expression
        {
            get { return _Expression; }
            set { SetPropertyValue("Expression", ref _Expression, value); }
        }
        
        private BusinessObjectBase _PropertyType;

        [XafDisplayName("类型"), RuleRequiredField]
        [ImmediatePostData]
        [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
        public BusinessObjectBase PropertyType
        {
            get { return _PropertyType; }
            set
            {
                SetPropertyValue("PropertyType", ref _PropertyType, value);
                if (!IsLoading)
                {
                    if (PropertyType != null)
                    {
                        if (string.IsNullOrEmpty(名称))
                        {
                            名称 = PropertyType.Caption;
                        }
                    }
                    else
                    {
                        
                        名称 = "";
                    }
                }
            }
        }

        private int _Size;
        [XafDisplayName("长度")]
        public int Size
        {
            get { return _Size; }
            set { SetPropertyValue("Size", ref _Size, value); }
        }

        protected override BusinessObject OwnerBusinessObject
        {
            get
            {
                return this.Owner;
            }
        }

        protected override List<PropertyBase> RelationPropertyDataSources
        {
            get
            {
                var pt = PropertyType as BusinessObject;
                if (pt == null)
                    return null;
                return pt.Properties.Where(x => x.PropertyType == this.Owner).OfType<PropertyBase>().Union(
                    pt.CollectionProperties.Where(x => x.PropertyType == this.Owner).OfType<PropertyBase>()
                    ).ToList();
            }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Size = 100;
        }

        public Property(Session s) : base(s)
        {

        }
    }
}