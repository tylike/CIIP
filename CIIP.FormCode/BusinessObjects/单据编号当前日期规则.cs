using System;
using DevExpress.ExpressApp.Model;
using DevExpress.Xpo;

namespace CIIP.FormCode
{
    public class 单据编号当前日期规则 : 单据编号规则
    {
        public 单据编号当前日期规则(Session s) : base(s)
        {

        }

        // Properties
        [Custom("RowCount", "6")]
        public string 格式分字符串使用说明
        {
            get { return "yyyy 年\nMM 月\ndd 日 \nhh 小时\n mm 分钟 \n ss 秒"; }
        }


        //string 格式化字符串 { get; set; }
        private string _格式化字符串;

        [ModelDefault("PredefinedValues", "yyyyMMdd;yyyyMMddhhmmss;yyMMdd;yyMMddhhmmss")]
        public string 格式化字符串
        {
            get { return _格式化字符串; }
            set { SetPropertyValue("格式化字符串", ref _格式化字符串, value); }
        }

        public override string GetRuleResult(XPBaseObject instance)
        {
            return DateTime.Now.ToString(格式化字符串); ;
        }
    }
}