using DevExpress.ExpressApp.Model.Core;
using CIIP.Module.ViewObjects;
using System.Diagnostics;
using CIIP.Module.BusinessObjects;

namespace WMS
{
    public abstract class 仓库单据布局<TMaster, TItem> : 单据布局<TMaster, TItem>
        where TMaster: 仓库单据基类<TItem>
        where TItem: 库存单据明细<TMaster>
    {

        protected virtual void LayoutListViewCore()
        {
            LayoutColumns(x => x.编号, x => x.审核状态, x => x.目标仓库, x => x.操作日期, x => x.状态, x => x.操作人, x => x.创建时间,
                x => x.创建者,
                x => x.修改时间, x => x.修改者);
        }

        public override void LayoutListView()
        {
            //默认列表
            LayoutListViewCore();
            base.LayoutListView();
        }

        public override void LayoutDetailView()
        {
            LayoutDetailViewCore();
            base.LayoutDetailView();
        }

        protected virtual void LayoutDetailViewCore()
        {
            DetailViewLayout.ClearNodes();
            //默认详细视图
            LayoutDetailViewBaseInfo();
            
            HGroup(90, t => t.备注);

            var tg = TabbedGroup(100, this.ItemsPropertyName, "单据流程");

            SetItemsPropertyEditor(tg[0]);

            HGroup(1000, x => x.创建者, x => x.创建时间, x => x.修改者, x => x.修改时间);

            var statusListView = GetChildListView(p => p.单据流程);

            LayoutListViewColumns<单据流程状态记录>(statusListView, x => x.创建者, x => x.创建时间, x => x.修改者, x => x.修改时间);
        }

        protected virtual void LayoutDetailViewBaseInfo()
        {
            HGroup(10, t => t.编号, t => t.目标仓库, t => t.审核状态, t => t.操作人);
        }

        public override void LayoutItemsListView()
        {
            //明细列表
            LayoutItemsColumns(x => x.产品, x => x.单价, x => x.数量, x => x.计量单位, x => x.总价, x => x.库位, x => x.审核状态);
            base.LayoutItemsListView();
        }

        public override void LayoutItemsDetailView()
        {
            //Debug.WriteLine(((ModelNode)ItemsViewLayout).Path);
            //ItemsViewLayout.ClearNodes();
            //ItemsViewLayout[0].Remove();
            //ItemsHGroup(10, x => x.产品, x => x.单价, x => x.数量, x => x.总价);
            //ItemsHGroup(20, x => x.计量单位, x => x.总价, x => x.库位, x => x.审核状态);

            base.LayoutItemsDetailView();
        }
    }
}