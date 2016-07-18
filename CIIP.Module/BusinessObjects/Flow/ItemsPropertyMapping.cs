using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Utils;
using System.Collections.Generic;

namespace CIIP.Module.BusinessObjects.Flow
{
    public class ItemsPropertyMapping : PropertyMapping
    {
        public ItemsPropertyMapping(Session s) : base(s)
        {
        }

        private FlowAction _Action;

        [Association]
        public FlowAction Action
        {
            get { return _Action; }
            set
            {
                SetPropertyValue("Action", ref _Action, value);
                if (!IsLoading && value != null)
                {
                    this.FromBill = value.ItemsType.FullName;

                    this.ToBill = value.ToItemsType.FullName;
                }
            }
        }
        
        protected override List<StringObject> 来源属性数据源
        {
            get
            {
                return Action.子表来源属性数据源;
            }
        }

        protected override List<StringObject> 目标属性数据源
        {
            get
            {
                return Action.子表目标属性数据源;
            }
        }

        protected override Type ToType
        {
            get
            {
                return Action.ItemsType;
                //return typeof(ExpressionParameter<,>).MakeGenericType(Action.ItemsType, Action.ToItemsType);
            }
        }
    }
}