using DevExpress.Xpo;

namespace CRM
{
    public class 客服沟通记录 : 沟通记录Base
    {
        public 客服沟通记录(Session s) : base(s)
        {

        }

        private 客服电话呼入 _客服任务;
        [Association]
        public 客服电话呼入 客服任务
        {
            get { return _客服任务; }
            set { SetPropertyValue("客服任务", ref _客服任务, value); }
        }

    }
}