using System;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using CIIP;
using 常用基类;

namespace CRM
{
    [DefaultClassOptions]
    public class 营销活动 : NameObject
    {
        public 营销活动(Session s) : base(s)
        {

        }

        private int _参与人数;

        public int 参与人数
        {
            get { return _参与人数; }
            set { SetPropertyValue("参与人数", ref _参与人数, value); }
        }

        private DateTime _开始时间;

        public DateTime 开始时间
        {
            get { return _开始时间; }
            set { SetPropertyValue("开始时间", ref _开始时间, value); }
        }

        private DateTime _结束时间;
        public DateTime 结束时间
        {
            get { return _结束时间; }
            set { SetPropertyValue("结束时间", ref _结束时间, value); }
        }

        private string _活动地点;
        public string 活动地点
        {
            get { return _活动地点; }
            set { SetPropertyValue("活动地点", ref _活动地点, value); }
        }

        private string _活动描述;

        [Size(-1)]
        [EditorAlias(EditorAliases.HtmlPropertyEditor)]
        public string 活动描述
        {
            get { return _活动描述; }
            set { SetPropertyValue("活动描述", ref _活动描述, value); }
        }

    }
}