using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Utils;
using System.Collections.Generic;

namespace CIIP.Module.BusinessObjects.Flow
{
    public class MasterPropertyMapping : PropertyMapping
    {
        public MasterPropertyMapping(Session s) : base(s)
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
                    this.FromBill = value.From.Form;
                    this.ToBill = value.To.Form;
                }
            }
        }



        protected override List<StringObject> 来源属性数据源
        {
            get
            {
                return Action.主表来源属性数据源;
            }
        }

        protected override List<StringObject> 目标属性数据源
        {
            get
            {
                return Action.主表目标属性数据源;
            }
        }

        protected override Type ToType
        {
            get
            {
                return Action.Type;
                //return typeof(ExpressionParameter<,>).MakeGenericType(Action.Type, Action.ToType);
            }
        }
    }
}