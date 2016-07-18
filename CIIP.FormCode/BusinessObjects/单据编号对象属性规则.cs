using System;
using DevExpress.Xpo;

namespace CIIP.FormCode
{
    public class 单据编号对象属性规则 : 单据编号规则
    {
        public 单据编号对象属性规则(Session s) : base(s)
        {

        }

        // Properties
        //string 属性名称 { get; set; }
        private string _属性名称;

        public string 属性名称
        {
            get { return _属性名称; }
            set { SetPropertyValue("属性名称", ref _属性名称, value); }
        }

        public override string GetRuleResult(XPBaseObject instance)
        {
            if (!string.IsNullOrEmpty(属性名称) && (所属方案 != null))
            {
                return Convert.ToString(instance.GetMemberValue(属性名称));
            }
            return "";
        }
    }
}