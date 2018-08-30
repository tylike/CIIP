namespace WMS
{
    public class 调拨单_ListView : 仓库单据布局<调拨单, 调拨明细>
    {
        protected override void LayoutListViewCore()
        {
            LayoutColumns(x => x.编号, x=>x.调出库位, x => x.目标仓库, x => x.操作日期, x => x.状态, x => x.审核状态, x => x.操作人, x => x.创建时间,
                x => x.创建者,
                x => x.修改时间, x => x.修改者);
            base.LayoutListViewCore();
        }

        protected override void LayoutDetailViewBaseInfo()
        {
            HGroup(10, t => t.编号, t => t.审核状态, t => t.操作人, null);
            HGroup(20, t => t.调出仓库, t => t.调出库位, t => t.目标仓库, t => t.目标库位);

            //base.LayoutDetailViewBaseInfo();
        }

    }
}