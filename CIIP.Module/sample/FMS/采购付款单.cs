using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.XtraCharts.Native;

namespace CIIP.Module.BusinessObjects
{
    [NavigationItem("财务管理")]
    public class 采购付款单 : 付款单
    {
        public 采购付款单(Session s) : base(s)
        {

        }
    }
}