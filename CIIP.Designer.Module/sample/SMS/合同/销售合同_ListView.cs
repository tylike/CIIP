using CIIP.Module.BusinessObjects;
using CIIP.Module.ViewObjects;

namespace SMS
{
    public class 销售合同_ListView : 订单布局<销售合同, 销售合同明细>
    {
        protected override void LayoutListViewCore()
        {
            LayoutColumns(x => x.编号, x => x.客户, x => x.预计送达时间, x => x.运输方式, x => x.订单总金额, x => x.客户联系人, x => x.结算方式, x => x.税金, x => x.税率, x => x.状态, x => x.收货地址, x => x.折扣总金额, x => x.折后总金额, x => x.含税总金额, x => x.创建时间, x => x.创建者, x => x.修改时间, x => x.修改者);
            base.LayoutListViewCore();
        }

        protected override void LayoutDetailViewCore()
        {
            HGroup(10, t => t.编号, t => t.客户, t => t.客户联系人, t => t.收货地址);
            HGroup(20, t => t.订单总金额, t => t.含税总金额, t => t.折扣总金额, t => t.折后总金额);
            HGroup(30, t => t.税率, t => t.税金, t => t.预计送达时间, t => t.状态);

            AddPropertyToDetailView(x => x.客户联系人.手机);
            AddPropertyToDetailView(x => x.客户联系人.部门);


            HGroup(40, t => t.运输方式, t => t.结算方式, t => t.客户联系人.手机, t => t.客户联系人.部门);


            HGroup(90, t => t.备注);

            var tg = TabbedGroup(100, this.ItemsPropertyName, "单据流程");

            SetItemsPropertyEditor(tg[0]);

            HGroup(1000, x => x.创建者, x => x.创建时间, x => x.修改者, x => x.修改时间);

            var statusListView = GetChildListView(p => p.单据流程);

            LayoutListViewColumns<单据流程状态记录>(statusListView, x => x.创建者, x => x.创建时间, x => x.修改者, x => x.修改时间);
        }
    }
}