using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIIP.Module.BusinessObjects;
using CIIP.Module.BusinessObjects.Analysis;
using CIIP.Module.BusinessObjects.Product;
using CIIP.Module.BusinessObjects.Security;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Security.Strategy;
using SMS;


namespace CIIP.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ReportDataGenerateViewController : ViewController
    {
        public ReportDataGenerateViewController()
        {
            InitializeComponent();
            //this.SaleDistrictReportGenerate.TargetObjectType = typeof (销售分析);
            //this.SaleDistrictReportGenerate.TargetViewType = ViewType.DetailView;
            this.TargetObjectType = typeof (行政区域);
            this.UpdateRegionLevel.TargetObjectType = typeof(行政区域);

        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        void CreateSaleArea(string 省,string 市,string 区)
        {
            var sheng = ObjectSpace.FindObject<省份>(new BinaryOperator("名称", 省));
            if (sheng == null)
                sheng = ObjectSpace.CreateObject<省份>();

            sheng.名称 = 省;

            var shi = ObjectSpace.CreateObject<城市>();
            shi.名称 = 市;
            shi.省份 = sheng;

            var q = 区.Split(',');
            foreach (var item in q)
            {
                var oq = ObjectSpace.CreateObject<销售区域>();
                oq.名称 = item;
                oq.城市 = shi;
            }
        }

        private void SaleDistrictReportGenerate_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var report = this.View.CurrentObject as 销售分析;

            var session = (ObjectSpace as XPObjectSpace).Session;
            
            var rnd = new Random();
            var units = session.Query<往来单位>().ToList();
            
            var products = ObjectSpace.GetObjects<产品>();
            var customers = units.Where(x => x.客户).ToList();
            //var providers = units.Where(x => x.供应商).ToList();
            var currentUnitOid = (SecuritySystem.CurrentUser as 系统用户).员工.往来单位.Oid;

            var employes = session.Query<员工>().Where(x => x.往来单位.Oid == currentUnitOid).ToList();

            //var dates = (ObjectSpace as XPObjectSpace).Session.Query<日期维度>().Where(x => x.TheDate.Year >= 2012 && x.TheDate.Date < DateTime.Now).ToArray();
            //从2009年到当前时间，生成销售数据
            for (var f = new DateTime(2012, 1, 1); f <= DateTime.Now; f = f.AddDays(1))
            {
                var saleOrder = ObjectSpace.CreateObject<销售订单>();
                saleOrder.客户 = customers[rnd.Next(customers.Count - 1)];
                saleOrder.业务员 = employes[rnd.Next(employes.Count - 1)];
                saleOrder.下单日期 = f;

                var ddtm = rnd.Next(10);
                if (ddtm <= 5)
                {
                    ddtm = 1;
                }else if (ddtm > 5 && ddtm <= 8)
                {
                    ddtm = 2;
                }
                else if (ddtm > 9)
                {
                    ddtm = 3;
                }
                for (int i = 0; i < ddtm; i++)
                {
                    var sale = ObjectSpace.CreateObject<销售订单明细>();

                    sale.产品 = products[rnd.Next(products.Count - 1)];
                    sale.数量 = rnd.Next(1, ddtm);
                    sale.单价 = sale.产品.零售价;
                    sale.单据 = saleOrder;
                }
            }
            ObjectSpace.CommitChanges();
        }

        private void UpdateRegionLevel_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var objs = (ObjectSpace as XPObjectSpace).Session.Query<行政区域>().ToList();
            UpdateLevel(objs.ToArray());
            ObjectSpace.CommitChanges();
        }

        void UpdateLevel(行政区域[] objs)
        {
            var level1 = objs.Where(x => x.上级 == null).ToArray();
            foreach (var item in level1)
            {
                item.级别 = 1;
            }
            var level2 = objs.Where(x => level1.Contains(x.上级)).ToArray();
            foreach (var item in level2)
            {
                item.级别 = 2;
            }

            var level3 = objs.Where(x => level2.Contains(x.上级)).ToArray();
            foreach (var item in level3)
            {
                item.级别 = 3;
            }

            var level4 = objs.Where(x => level3.Contains(x.上级)).ToArray();
            foreach (var item in level4)
            {
                item.级别 = 4;
            }

            var level5= objs.Where(x => level4.Contains(x.上级)).ToArray();
            foreach (var item in level5)
            {
                item.级别 = 6;
            }

        }
    }
}
