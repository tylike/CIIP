using CIIP.Module.BusinessObjects.Security;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects
{
    [XafDefaultProperty("标题")]
    public class 员工 : NameObject
    {
        public 员工(Session s) : base(s)
        {

        }
        
        private 系统用户 _系统用户;
        
        public 系统用户 系统用户
        {
            get { return _系统用户; }
            set { SetPropertyValue("系统用户", ref _系统用户, value); }
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading && propertyName == "系统用户")
            {
                if (newValue != null)
                {
                    (newValue as 系统用户).员工 = this;
                }
            }
        }

        protected override void OnSaving()
        {
            if (往来单位.默认联系人 == null)
            {
                往来单位.默认联系人 = this;
            }
            base.OnSaving();
        }


        [VisibleInDetailView(false),VisibleInListView(false)]
        public string 标题
        {
            get
            {
                return 部门?.名称 + 名称 + 职位?.名称 + "(" + 手机 + ")";
            }
        }

        private 往来单位 _往来单位;
        [Association]
        public 往来单位 往来单位
        {
            get { return _往来单位; }
            set { SetPropertyValue("往来单位", ref _往来单位, value); }
        }

        private 部门 _部门;

        [DataSourceProperty("往来单位.部门")]
        [Association]
        public 部门 部门
        {
            get { return _部门; }
            set { SetPropertyValue("部门", ref _部门, value); }
        }
        
        private 职位 _职位;

        public 职位 职位
        {
            get { return _职位; }
            set { SetPropertyValue("职位", ref _职位, value); }
        }
        
        private string _手机;

        [RuleRequiredField]
        public string 手机
        {
            get { return _手机; }
            set { SetPropertyValue("手机", ref _手机, value); }
        }

        private string _家庭地址;
        public string 家庭地址
        {
            get { return _家庭地址; }
            set { SetPropertyValue("家庭地址", ref _家庭地址, value); }
        }

        private string _邮编;
        public string 邮编
        {
            get { return _邮编; }
            set { SetPropertyValue("邮编", ref _邮编, value); }
        }

        private string _电子邮件;
        public string 电子邮件
        {
            get { return _电子邮件; }
            set { SetPropertyValue("电子邮件", ref _电子邮件, value); }
        }


        private string _QQ;
        public string QQ
        {
            get { return _QQ; }
            set { SetPropertyValue("QQ", ref _QQ, value); }
        }

        private string _微信;
        public string 微信
        {
            get { return _微信; }
            set { SetPropertyValue("微信", ref _微信, value); }
        }

        private string _其他联系方式;
        public string 其他联系方式
        {
            get { return _其他联系方式; }
            set { SetPropertyValue("其他联系方式", ref _其他联系方式, value); }
        }
    }
}