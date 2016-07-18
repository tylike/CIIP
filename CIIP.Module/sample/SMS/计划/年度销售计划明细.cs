using System;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using CIIP.Module.BusinessObjects;
using CIIP.Module.BusinessObjects.Product;

namespace SMS
{
    public class 年度销售计划明细 : BaseObject
    {
        public 年度销售计划明细(Session s) : base(s)
        {

        }

        private 年度销售计划 _销售计划;
        [Association]
        public 年度销售计划 销售计划
        {
            get { return _销售计划; }
            set { SetPropertyValue("销售计划", ref _销售计划, value); }
        }


        public override void AfterConstruction()
        {
            this.Month1 = new 月销计划(Session);
            this.Month2 = new 月销计划(Session);
            this.Month3 = new 月销计划(Session);
            this.Month4 = new 月销计划(Session);
            this.Month5 = new 月销计划(Session);
            this.Month6 = new 月销计划(Session);
            this.Month7 = new 月销计划(Session);
            this.Month8 = new 月销计划(Session);
            this.Month9 = new 月销计划(Session);
            this.Month10 = new 月销计划(Session);
            this.Month11 = new 月销计划(Session);
            this.Month12 = new 月销计划(Session);

            base.AfterConstruction();
        }

        void SaveMonth(月销计划 month,int monthN)
        {
            if (!销售计划.年.HasValue)
                throw new Exception("请设置计划年份!");

            month.产品 = this.产品;
            month.员工 = this.员工;
            month.日期 = new DateTime(销售计划.年.Value, monthN, 1);
            month.分类 = 销售计划分类.月计划;
        }

        protected override void OnSaving()
        {
            if (!IsDeleted)
            {
                SaveMonth(Month1, 1);
                SaveMonth(Month2, 2);
                SaveMonth(Month3, 3);
                SaveMonth(Month4, 4);
                SaveMonth(Month5, 5);
                SaveMonth(Month6, 6);
                SaveMonth(Month7, 7);
                SaveMonth(Month8, 8);
                SaveMonth(Month9, 9);
                SaveMonth(Month10, 10);
                SaveMonth(Month11, 11);
                SaveMonth(Month12, 12);
            }

            base.OnSaving();
        }


        private 员工 _员工;

        public 员工 员工
        {
            get { return _员工; }
            set { SetPropertyValue("Employee", ref _员工, value); }
        }

        private 产品 _产品;

        public 产品 产品
        {
            get { return _产品; }
            set { SetPropertyValue("产品", ref _产品, value); }
        }

        private 月销计划 _Month1;
        public 月销计划 Month1
        {
            get { return _Month1; }
            set { SetPropertyValue("Month1", ref _Month1, value); }
        }

        private 月销计划 _Month2;
        public 月销计划 Month2
        {
            get { return _Month2; }
            set { SetPropertyValue("Month2", ref _Month2, value); }
        }

        private 月销计划 _Month3;
        public 月销计划 Month3
        {
            get { return _Month3; }
            set { SetPropertyValue("Month3", ref _Month3, value); }
        }

        private 月销计划 _Month4;
        public 月销计划 Month4
        {
            get { return _Month4; }
            set { SetPropertyValue("Month4", ref _Month4, value); }
        }

        private 月销计划 _Month5;
        public 月销计划 Month5
        {
            get { return _Month5; }
            set { SetPropertyValue("Month5", ref _Month5, value); }
        }

        private 月销计划 _Month6;
        public 月销计划 Month6
        {
            get { return _Month6; }
            set { SetPropertyValue("Month6", ref _Month6, value); }
        }

        private 月销计划 _Month7;
        public 月销计划 Month7
        {
            get { return _Month7; }
            set { SetPropertyValue("Month7", ref _Month7, value); }
        }

        private 月销计划 _Month8;
        public 月销计划 Month8
        {
            get { return _Month8; }
            set { SetPropertyValue("Month8", ref _Month8, value); }
        }

        private 月销计划 _Month9;
        public 月销计划 Month9
        {
            get { return _Month9; }
            set { SetPropertyValue("Month9", ref _Month9, value); }
        }

        private 月销计划 _Month10;
        public 月销计划 Month10
        {
            get { return _Month10; }
            set { SetPropertyValue("Month10", ref _Month10, value); }
        }

        private 月销计划 _Month11;
        public 月销计划 Month11
        {
            get { return _Month11; }
            set { SetPropertyValue("Month11", ref _Month11, value); }
        }

        private 月销计划 _Month12;
        public 月销计划 Month12
        {
            get { return _Month12; }
            set { SetPropertyValue("Month12", ref _Month12, value); }
        }

    }
}