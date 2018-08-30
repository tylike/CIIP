using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace SMS
{
    [XafDisplayName("年度销售计划")]
    [NavigationItem("销售管理")]
    public class 年度销售计划 : BaseObject
    {
        public 年度销售计划(Session s) : base(s)
        {

        }

        private int? _年;
        [RuleRequiredField]
        public int? 年
        {
            get { return _年; }
            set { SetPropertyValue("年", ref _年, value); }
        }

        [Association,DevExpress.Xpo.Aggregated]
        public XPCollection<年度销售计划明细> Items
        {
            get { return GetCollection<年度销售计划明细>("Items"); }
        }
    }
}