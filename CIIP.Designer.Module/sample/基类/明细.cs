using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace 常用基类
{
    [NonPersistent]
    public abstract class 明细 : SimpleObject
    {
        public 明细(Session s) : base(s)
        {

        }
    }

    [NonPersistent]
    public abstract class 明细<TMaster> : 明细
    {
        public 明细(Session s) : base(s)
        {

        }

        [NonPersistent]
        public TMaster 单据
        {
            get { return GetPropertyValue<TMaster>("Master"); }
            set
            {
                SetPropertyValue("Master", value);
            }
        }
    }
}