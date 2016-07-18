using CIIP.Module.BusinessObjects;
using 常用基类;

namespace CIIP.Module.ViewObjects
{
    public class 订单布局模板<TMaster, TItem> : 订单布局<TMaster, TItem>
        where TMaster : 订单<TItem>
        where TItem : 订单明细<TMaster>
    {

    }
}