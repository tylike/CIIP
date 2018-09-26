using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace CIIP.Designer
{
    [NonPersistent]
    public class BatchInputBusinessObject : BaseObject
    {
        public BatchInputBusinessObject(Session s) : base(s)
        {

        }

        [Size(-1)]
        [XafDisplayName("快速代码")]
        public string Code
        {
            get { return GetPropertyValue<string>(nameof(Code)); }
            set { SetPropertyValue(nameof(Code), value); }
        }

        //规则1:名称1,名称2,名称3,名称4 为4个业务对象名称.建立出4张表.
        //规则2:名称{ 名称1,名称2,名称3 } 为一个类中包含了3个string属性
        //规则3:名称{ string 名称1,int 名称2,datetime 名称3} 为一个类中指定了三个有类型的属性.
        [RuleFromBoolProperty]
        public bool Error
        {
            get
            {
                return false;
            }
        }


        [XafDisplayName("说明")]
        public string Memo
        {
            get
            {
                return "";
            }
        }
    }
    
}