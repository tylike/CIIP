using DevExpress.Xpo;
using CIIP;
using CIIP.Module.BusinessObjects;
using 常用基类;

namespace CRM
{
    public class 销售线索 : SimpleObject
    {
        public 销售线索(Session s) : base(s)
        {
        }

        private string _标题;
        public string 标题
        {
            get { return _标题; }
            set { SetPropertyValue("标题", ref _标题, value); }
        }

        private string _公司名称;
        public string 公司名称
        {
            get { return _公司名称; }
            set { SetPropertyValue("公司名称", ref _公司名称, value); }
        }

        private 往来单位 _客户;
        public 往来单位 客户
        {
            get { return _客户; }
            set { SetPropertyValue("客户", ref _客户, value); }
        }

        private string _地址;
        public string 地址
        {
            get { return _地址; }
            set { SetPropertyValue("地址", ref _地址, value); }
        }

        private string _联系人姓名;
        public string 联系人姓名
        {
            get { return _联系人姓名; }
            set { SetPropertyValue("联系人姓名", ref _联系人姓名, value); }
        }

        private string _电子邮件;
        public string 电子邮件
        {
            get { return _电子邮件; }
            set { SetPropertyValue("电子邮件", ref _电子邮件, value); }
        }

        private string _手机;
        public string 手机
        {
            get { return _手机; }
            set { SetPropertyValue("手机", ref _手机, value); }
        }

        private 员工 _业务员;
        public 员工 业务员
        {
            get { return _业务员; }
            set { SetPropertyValue("业务员", ref _业务员, value); }
        }

        private 销售团队 _销售团队;
        public 销售团队 销售团队
        {
            get { return _销售团队; }
            set { SetPropertyValue("销售团队", ref _销售团队, value); }
        }

        private int _优先级;
        public int 优先级
        {
            get { return _优先级; }
            set { SetPropertyValue("优先级", ref _优先级, value); }
        }

        private string _标签;
        public string 标签
        {
            get { return _标签; }
            set { SetPropertyValue("标签", ref _标签, value); }
        }

        private 客户来源 _客户来源;
        public 客户来源 客户来源
        {
            get { return _客户来源; }
            set { SetPropertyValue("客户来源", ref _客户来源, value); }
        }

        private 途径 _途径;
        public 途径 途径
        {
            get { return _途径; }
            set { SetPropertyValue("途径", ref _途径, value); }
        }

        private 营销活动 _营销活动;
        public 营销活动 营销活动
        {
            get { return _营销活动; }
            set { SetPropertyValue("营销活动", ref _营销活动, value); }
        }

        private bool _邮件订阅;
        public bool 邮件订阅
        {
            get { return _邮件订阅; }
            set { SetPropertyValue("邮件订阅", ref _邮件订阅, value); }
        }

        private bool _有效;
        public bool 有效
        {
            get { return _有效; }
            set { SetPropertyValue("有效", ref _有效, value); }
        }

        private string _推荐人;
        public string 推荐人
        {
            get { return _推荐人; }
            set { SetPropertyValue("推荐人", ref _推荐人, value); }
        }

        [Association,Aggregated]
        public XPCollection<销售线索沟通记录> 沟通记录
        {
            get
            {
                return GetCollection<销售线索沟通记录>("沟通记录");
            }
        }
    }
}