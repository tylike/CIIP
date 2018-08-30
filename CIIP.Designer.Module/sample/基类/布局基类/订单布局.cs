using System;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Model.Core;
using System.Diagnostics;
using System.Linq;
using CIIP.Module.BusinessObjects;
using 常用基类;

namespace CIIP.Module.ViewObjects
{
    public abstract class 订单布局<TMaster, TItem> : 单据布局<TMaster, TItem>
        where TMaster : 订单<TItem>
        where TItem : 订单明细<TMaster>
    {
        protected virtual void LayoutListViewCore()
        {
            LayoutColumns(x => x.编号, x => x.供应商, x => x.预计送达时间, x => x.运输方式, x => x.订单总金额, x => x.供应商联系人, x => x.结算方式,
                x => x.税金, x => x.税率, x => x.状态, x => x.收货地址, x => x.折扣总金额, x => x.折后总金额, x => x.含税总金额, x => x.创建时间,
                x => x.创建者, x => x.修改时间, x => x.修改者);
        }

        public override void LayoutListView()
        {
            //默认列表
            LayoutListViewCore();
            base.LayoutListView();
        }
        
        public override void LayoutDetailView()
        {
            DetailViewLayout.ClearNodes();
            LayoutDetailViewCore();

            base.LayoutDetailView();
        }

        protected virtual void LayoutDetailViewCore()
        {
            HGroup(10, t => t.编号, t => t.供应商, t => t.供应商联系人, t => t.发货地址);

            HGroup(20, t => t.客户, t => t.部门, t => t.客户联系人, t => t.收货地址);

            HGroup(30, t => t.订单总金额, t => t.含税总金额, t => t.折扣总金额, t => t.折后总金额);

            HGroup(40, t => t.税率, t => t.税金, t => t.业务员, t => t.状态);

            HGroup(50, t => t.运输方式, t => t.结算方式, t => t.预计送达时间, t => t.期望到货时间);

            var g = HGroup(90, t => t.备注);
            //g.First()


            var tg = TabbedGroup(100, ItemsPropertyName, "单据流程", "状态记录");

            SetItemsPropertyEditor(tg[0]);

            HGroup(1000, x => x.创建者, x => x.创建时间, x => x.修改者, x => x.修改时间);

            var statusListView = GetChildListView(p => p.单据流程);

            LayoutListViewColumns<单据流程状态记录>(statusListView,  x => x.创建者, x => x.创建时间, x => x.修改者, x => x.修改时间);

        }

        public override void LayoutItemsListView()
        {
            //明细列表
            LayoutItemsListViewCore();
            base.LayoutItemsListView();
        }

        protected virtual void LayoutItemsListViewCore()
        {
            ItemsListView.Columns["产品价格"].View = CaptionHelper.ApplicationModel.Views["产品价格_ListView"];
            LayoutItemsColumns(x => x.产品价格, x => x.产品, x => x.单价, x => x.数量, x => x.折扣率, x => x.单位, x => x.总价, x => x.税率, x => x.含税单价, x => x.含税总价, x => x.折扣单价, x => x.折后总价);
        }

        public override void LayoutItemsDetailView()
        {
            Debug.WriteLine(((ModelNode)ItemsViewLayout).Path);
            ItemsViewLayout.ClearNodes();
            //ItemsViewLayout.ToArray();
            
            ItemsHGroup(10, x => x.产品, x => x.单价, x => x.数量, x => x.总价);
            ItemsHGroup(20, x => x.单位, x => x.含税单价, x => x.含税总价, x => x.税率);
            ItemsHGroup(30, x => x.折扣单价, x => x.折扣率, x => x.折后总价, x => x.折扣金额);
            ItemsHGroup(40, x => x.创建者, x => x.创建时间, x => x.修改者, x => x.修改时间);
            base.LayoutItemsDetailView();
        }
    }
}