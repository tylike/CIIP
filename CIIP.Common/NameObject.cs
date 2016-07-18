using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace 常用基类
{
    [NonPersistent]
    [XafDefaultProperty("名称")]
    public abstract class NameObject : SimpleObject
    {
        public NameObject(Session s):base(s)
        {

        }

        private string _名称;
        [RuleRequiredField(TargetCriteria = "名称必填")]
        public virtual string 名称
        {
            get { return _名称; }
            set { SetPropertyValue("名称", ref _名称, value); }
        }

        protected virtual bool 名称必填
        {
            get
            {
                return true;
            }
        }
        
    }
}