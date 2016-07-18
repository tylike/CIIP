using DevExpress.Xpo;
using 常用基类;

namespace CIIP.FormCode
{
    public abstract class 单据编号规则 : SimpleObject
    {
        protected 单据编号规则(Session s) : base(s)
        {

        }

        public abstract string GetRuleResult(XPBaseObject instance);

        private string _规则名称;

        public string 规则名称
        {
            get { return _规则名称; }
            set { SetPropertyValue("规则名称", ref _规则名称, value); }
        }

        private int _序号;

        public int 序号
        {
            get { return _序号; }
            set { SetPropertyValue("序号", ref _序号, value); }
        }

        private 单据编号方案 _所属方案;

        [Association]
        public 单据编号方案 所属方案
        {
            get { return _所属方案; }
            set
            {
                SetPropertyValue("所属方案", ref _所属方案, value);
                if (this.Session.IsNewObject(this) && value != null && this.序号 == 0)
                {
                    序号 = 所属方案.编号规则.Count + 1;
                }
            }
        }
    }
}