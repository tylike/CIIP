using DevExpress.Xpo;

namespace CIIP.FormCode
{
    public class 单据编号手工输入规则 : 单据编号规则
    {
        public 单据编号手工输入规则(Session s):base(s)
        {
            
        }

        private string _手工输入;
        public string 手工输入
        {
            get { return _手工输入; }
            set { SetPropertyValue("手工输入", ref _手工输入, value); }
        }


        public override string GetRuleResult(XPBaseObject instance)
        {
            return 手工输入;
        }

    }
}