using System;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using CIIP.Module.BusinessObjects;
using CIIP.Module.BusinessObjects.Product;

namespace SMS
{
    [DefaultClassOptions]
    [NavigationItem("销售管理")]
    public class 月销计划 : BaseObject
    {
        public 月销计划(Session s) : base(s)
        {

        }

        private 销售计划分类 _分类;
        public 销售计划分类 分类
        {
            get { return _分类; }
            set { SetPropertyValue("分类", ref _分类, value); }
        }


        private DateTime _日期;

        [ModelDefault("DisplayFormat", "yyyy-MM")]
        [ModelDefault("EditMask", "yyyy-MM")]
        public DateTime 日期
        {
            get { return _日期; }
            set { SetPropertyValue("日期", ref _日期, value); }
        }

        private 员工 _员工;

        public 员工 员工
        {
            get { return _员工; }
            set { SetPropertyValue("员工", ref _员工, value); }
        }

        private 产品 _产品;

        public 产品 产品
        {
            get { return _产品; }
            set { SetPropertyValue("产品", ref _产品, value); }
        }

        private decimal _销量;

        public decimal 销量
        {
            get { return _销量; }
            set { SetPropertyValue("销量", ref _销量, value); }
        }

        private decimal _销售额;
        [XafDisplayName("销售额")]
        public decimal 销售额
        {
            get { return _销售额; }
            set { SetPropertyValue("销售额", ref _销售额, value); }
        }
    }
}