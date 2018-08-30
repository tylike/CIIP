using CIIP.Module.BusinessObjects.Product;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CIIP.Module.BusinessObjects.Analysis
{
    //按日期最小粒度的，不能取同期。

    [NonPersistent]
    public class SaleReportItem : BaseObject
    {
        public SaleReportItem(Session s) : base(s)
        {

        }

        //private SaleReportByDistrict _SaleReport;

        //[Association]
        //public SaleReportByDistrict SaleReport
        //{
        //    get { return _SaleReport; }
        //    set { SetPropertyValue("SaleReport", ref _SaleReport, value); }
        //}

        private 产品 _Product;

        [XafDisplayName("产品")]
        public 产品 Product
        {
            get { return _Product; }
            set { SetPropertyValue("Product", ref _Product, value); }
        }

        private 销售区域 _District;

        [XafDisplayName("销售区域")]
        public 销售区域 District
        {
            get { return _District; }
            set { SetPropertyValue("District", ref _District, value); }
        }


        
        
        private decimal _Qty;

        [XafDisplayName("销量")]
        public decimal Qty
        {
            get { return _Qty; }
            set { SetPropertyValue("Qty", ref _Qty, value); }
        }
        
        private decimal _SumPrice;

        [XafDisplayName("售额")]
        public decimal SumPrice
        {
            get { return _SumPrice; }
            set { SetPropertyValue("SumPrice", ref _SumPrice, value); }
        }

        private decimal _UnitPrice;

        [XafDisplayName("单价")]
        public decimal UnitPrice
        {
            get { return _UnitPrice; }
            set { SetPropertyValue("UnitPrice", ref _UnitPrice, value); }
        }

        private decimal _Profit;

        [XafDisplayName("利润")]
        public decimal Profit
        {
            get { return _Profit; }
            set { SetPropertyValue("Profit", ref _Profit, value); }
        }

        #region plan
        private decimal _PlanQty;
        [XafDisplayName("计划销量")]
        public decimal PlanQty
        {
            get { return _PlanQty; }
            set { SetPropertyValue("PlanQty", ref _PlanQty, value); }
        }

        private decimal _PlanPrice;
        [XafDisplayName("计划售额")]
        public decimal PlanPrice
        {
            get { return _PlanPrice; }
            set { SetPropertyValue("PlanPrice", ref _PlanPrice, value); }
        }
        #endregion

    }

    //上年情况
    //上月情况
    //---
    //当按月统计时，可以取得


}