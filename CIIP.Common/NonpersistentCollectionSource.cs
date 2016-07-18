using System;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Xpo;

namespace CIIP
{
    public class NonPersistentCollectionSource : CollectionSourceBase
    {
        public NonPersistentCollectionSource(IObjectSpace objectSpace, Type type, object source)
            : this(objectSpace, type, CollectionSourceMode.Normal, source)
        {

        }

        ITypeInfo typeInfo;
        object source;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectSpace"></param>
        /// <param name="type">数据源的数据类型</param>
        /// <param name="mode"></param>
        /// <param name="source">数据源</param>
        public NonPersistentCollectionSource(IObjectSpace objectSpace, Type type, CollectionSourceMode mode, object source)
            : base(objectSpace, mode)
        {
            typeInfo = XafTypesInfo.Instance.FindTypeInfo(type);
            var s = source as XPBaseCollection;
            if (s != null)
            {
                SourceCriteria = s.Filter;
            }
            this.source = source;
        }

        protected override object CreateCollection()
        {
            return source;
        }
        CriteriaOperator SourceCriteria;
        protected override void ApplyCriteriaCore(CriteriaOperator criteria)
        {
            var s = source as XPBaseCollection;
            if (s != null)
            {
                s.Filter = SourceCriteria;

                if (!object.Equals(criteria, null))
                {
                    if (object.Equals(s.Filter, null))
                    {
                        s.Filter = criteria;
                    }
                    else
                    {
                        s.Filter = CriteriaOperator.And(criteria, s.Filter);
                    }
                }
            }
        }

        public override DevExpress.ExpressApp.DC.ITypeInfo ObjectTypeInfo
        {
            get
            {
                return this.typeInfo;
            }
        }
    }
}