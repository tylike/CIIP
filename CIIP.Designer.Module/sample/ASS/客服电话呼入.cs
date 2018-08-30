using System;
using DevExpress.Xpo;
using CIIP.Module.BusinessObjects;
using 常用基类;

namespace CRM
{
    public class 客服电话呼入 : 单据
    {
        public 客服电话呼入(Session s) : base(s)
        {

        }

        private DateTime _呼入时间;

        public DateTime 呼入时间
        {
            get { return _呼入时间; }
            set { SetPropertyValue("呼入时间", ref _呼入时间, value); }
        }

        //日 期 时 间 房 号 来 电 内 容  记录人 受理部门  处理人 处理情况  回访结果
        private string _通记内容;

        [Size(-1)]
        public string 通记内容
        {
            get { return _通记内容; }
            set { SetPropertyValue("通记内容", ref _通记内容, value); }
        }

        private 员工 _处理人;
        public 员工 处理人
        {
            get { return _处理人; }
            set { SetPropertyValue("处理人", ref _处理人, value); }
        }

        [Association,Aggregated]
        public XPCollection<客服沟通记录> 沟通记录
        {
            get { return GetCollection<客服沟通记录>("沟通记录"); }
        }
    }
}