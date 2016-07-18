using System;
using DevExpress.Xpo;
using CIIP;
using CIIP.Module.BusinessObjects;
using 常用基类;

namespace CRM
{
    public class 销售商机 : SimpleObject
    {
        public 销售商机(Session s) : base(s)
        {
        }

        private string _标题;
        public string 标题
        {
            get { return _标题; }
            set { SetPropertyValue("标题", ref _标题, value); }
        }

        private decimal _预期收益;
        public decimal 预期收益
        {
            get { return _预期收益; }
            set { SetPropertyValue("预期收益", ref _预期收益, value); }
        }

        private decimal _预期收益比例;
        public decimal 预期收益比例
        {
            get { return _预期收益比例; }
            set { SetPropertyValue("预期收益比例", ref _预期收益比例, value); }
        }

        private 往来单位 _客户;
        public 往来单位 客户
        {
            get { return _客户; }
            set { SetPropertyValue("客户", ref _客户, value); }
        }

        private 销售线索 _销售线索;
        public 销售线索 销售线索
        {
            get { return _销售线索; }
            set { SetPropertyValue("销售线索", ref _销售线索, value); }
        }

        private DateTime _下次行动日期;
        public DateTime 下次行动日期
        {
            get { return _下次行动日期; }
            set { SetPropertyValue("下次行动日期", ref _下次行动日期, value); }
        }

        private DateTime _下次行动结束日期;
        public DateTime 下次行动结束日期
        {
            get { return _下次行动结束日期; }
            set { SetPropertyValue("下次行动结束日期", ref _下次行动结束日期, value); }
        }

        private string _下次行动内容;
        public string 下次行动内容
        {
            get { return _下次行动内容; }
            set { SetPropertyValue("下次行动内容", ref _下次行动内容, value); }
        }
    }
}