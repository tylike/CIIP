namespace WMS
{
    public class 仓库单据布局模板<TMaster, TItem> : 仓库单据布局<TMaster, TItem>
        where TMaster : 仓库单据基类<TItem>
        where TItem : 库存单据明细<TMaster>
    {

    }
}