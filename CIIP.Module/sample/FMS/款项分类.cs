using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects
{
    public class 款项分类 : NameObject
    {
        public 款项分类(Session s) : base(s)
        {

        }

        [Association]
        public XPCollection<业务对象> 业务对象
        {
            get { return GetCollection<业务对象>("业务对象"); }
        }
    }
}