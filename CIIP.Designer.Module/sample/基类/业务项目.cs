using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects
{
    public class 业务项目 : NameObject
    {
        public 业务项目(Session s) : base(s)
        {

        }

        [Association]
        public XPCollection<单据流程状态记录> 记录
        {
            get
            {
                return GetCollection<单据流程状态记录>("记录");
            }
        }
        
        [Association]
        public XPCollection<单据> 单据
        {
            get
            {
                return GetCollection<单据>("单据");
            }
        }
    }
}