using System;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace CIIP.Module.BusinessObjects.Analysis
{
    [DefaultClassOptions]
    [XafDisplayName("日期维度")]
    [NavigationItem("系统设置")]
    [XafDefaultProperty("DataKey")]
    public class 日期维度 : XPLiteObject
    {
        public 日期维度(Session s) : base(s)
        {

        }

        private string _DataKey;

        [Key(false)]
        [Size(8)]
        [ModelDefault("AllowEdit", "False")]
        public string DataKey
        {
            get { return _DataKey; }
            set { SetPropertyValue("DataKey", ref _DataKey, value); }
        }

        private DateTime _TheDate;

        [RuleRequiredField]
        [XafDisplayName("日期")]
        [ModelDefault("DisplayFormat","yyyy-MM-dd")]
        [ModelDefault("EditMask", "yyyy-MM-dd")]
        public DateTime TheDate
        {
            get { return _TheDate; }
            set { SetPropertyValue("The_Date", ref _TheDate, value); }
        }

        private int _TheYear;

        [ModelDefault("AllowEdit", "False")]
        public int TheYear
        {
            get { return _TheYear; }
            set { SetPropertyValue("TheYear", ref _TheYear, value); }
        }

        private int _TheMonth;

        [ModelDefault("AllowEdit", "False")]
        public int TheMonth
        {
            get { return _TheMonth; }
            set { SetPropertyValue("TheMonth", ref _TheMonth, value); }
        }

        private int _TheDay;

        [ModelDefault("AllowEdit", "False")]
        public int TheDay
        {
            get { return _TheDay; }
            set { SetPropertyValue("TheDay", ref _TheDay, value); }
        }

        private 季度 _Quarter;

        [ModelDefault("AllowEdit", "False")]
        public 季度 Quarter
        {
            get { return _Quarter; }
            set { SetPropertyValue("Quarter", ref _Quarter, value); }
        }


        private DayOfWeek _WeekDay;

        [ModelDefault("AllowEdit", "False")]
        public DayOfWeek WeekDay
        {
            get { return _WeekDay; }
            set { SetPropertyValue("WeekDay", ref _WeekDay, value); }
        }

        private int _WeekOfYear;

        [ModelDefault("AllowEdit", "False")]
        public int WeekOfYear
        {
            get { return _WeekOfYear; }
            set { SetPropertyValue("WeekOfYear", ref _WeekOfYear, value); }
        }

        private int _DayOfYear;

        [ModelDefault("AllowEdit", "False")]
        public int DayOfYear
        {
            get { return _DayOfYear; }
            set { SetPropertyValue("DayOfYear", ref _DayOfYear, value); }
        }

        private 半年 _SemiYearly;

        [ModelDefault("AllowEdit", "False")]
        public 半年 SemiYearly
        {
            get { return _SemiYearly; }
            set { SetPropertyValue("SemiYearly", ref _SemiYearly, value); }
        }

        private 月旬 _PeriodOfTenDays;

        [ModelDefault("AllowEdit", "False")]
        public 月旬 PeriodOfTenDays
        {
            get { return _PeriodOfTenDays; }
            set { SetPropertyValue("PeriodOfTenDays", ref _PeriodOfTenDays, value); }
        }

        private 节假日 _Holiday;

        public 节假日 Holiday
        {
            get { return _Holiday; }
            set { SetPropertyValue("Holiday", ref _Holiday, value); }
        }

        private static System.Globalization.CultureInfo myCI = new System.Globalization.CultureInfo("zh-CN");

        protected override void OnSaving()
        {
            this.DataKey = TheDate.ToString("yyyyMMdd");
            this.TheYear = TheDate.Year;
            this.TheMonth = TheDate.Month;
            this.TheDay = TheDate.Day;

            this.WeekDay = TheDate.DayOfWeek;
            
            this.Quarter = (季度) ((TheDate.Month-1)/3);
            this.WeekOfYear = myCI.Calendar.GetWeekOfYear(TheDate, System.Globalization.CalendarWeekRule.FirstDay,
                DayOfWeek.Monday);

            this.DayOfYear = myCI.Calendar.GetDayOfYear(TheDate);
            this.SemiYearly = TheMonth > 7
                ? 半年.下半年
                : 半年.上半年;
            var t = this.TheDay;
            if (t > 30)
                t = 30;
            this.PeriodOfTenDays = (月旬) ((t-1)/10);
            base.OnSaving();
        }
    }
}