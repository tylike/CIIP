using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Xpo;

namespace CIIP.FormCode
{
    [DomainComponent]
    public class 单据编号自动编号规则 : 单据编号规则
    {
        public 单据编号自动编号规则(Session s) : base(s)
        {

        }

        private int _步长;

        public int 步长
        {
            get { return _步长; }
            set { SetPropertyValue("步长", ref _步长, value); }
        }

        //
        //string 格式化字符串 { get; set; }
        private string _格式化字符串;

        [ModelDefault("PredefinedValues", "0000;000000;00000000")]
        public string 格式化字符串
        {
            get { return _格式化字符串; }
            set { SetPropertyValue("格式化字符串", ref _格式化字符串, value); }
        }
        [ModelDefault("RowCount", "5")]
        public string 格式化字符串使用说明
        {
            get { return "0000 4位数字"; }
        }

        public override string GetRuleResult(XPBaseObject instance)
        {
            return SequenceGenerator.GenerateNextSequence(this.所属方案.应用单据.FullName).ToString(格式化字符串);
        }
    }
}