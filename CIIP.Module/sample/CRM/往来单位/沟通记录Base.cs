using System;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using CIIP;
using 常用基类;

namespace CRM
{
    [NonPersistent]
    public class 沟通记录Base : SimpleObject
    {
        private DateTime _沟通时间;
        private string _沟通内容;

        public 沟通记录Base(Session s) : base(s)
        {

        }

        public DateTime 沟通时间
        {
            get { return _沟通时间; }
            set { SetPropertyValue("沟通时间", ref _沟通时间, value); }
        }

        [Size(-1)]
        [EditorAlias(EditorAliases.HtmlPropertyEditor)]
        public string 沟通内容
        {
            get { return _沟通内容; }
            set { SetPropertyValue("沟通内容", ref _沟通内容, value); }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            this.沟通时间 = DateTime.Now;
        }
    }
}