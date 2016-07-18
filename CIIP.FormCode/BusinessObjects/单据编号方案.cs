using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.FormCode
{
    [NavigationItem("系统设置")]
    public class 单据编号方案 : NameObject
    {
        public 单据编号方案(Session s) : base(s)
        {

        }

        // Methods
        public string 生成编号(XPObject ins)
        {
            return null;
        }

        // Properties
        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<单据编号规则> 编号规则
        {
            get { return GetCollection<单据编号规则>("编号规则"); }
        }

        [
            RuleRequiredField,
            ValueConverter(typeof (TypeToStringConverter)),
            //TypeConverter(typeof (StateMachineTypeConverter)),
            ImmediatePostData,Size(-1)
        ]
        public Type 应用单据
        {
            get { return GetPropertyValue<Type>("应用单据"); }
            set { SetPropertyValue("应用单据", value); }
        }

        public string 生成编号(XPBaseObject ins)
        {
            var enumerable = 编号规则.OrderBy(x => x.序号).ToArray();
            var builder = new StringBuilder();
            foreach (var i单据编号规则 in enumerable)
            {
                builder.Append(i单据编号规则.GetRuleResult(ins));
            }
            return builder.ToString();
        }
    }
}