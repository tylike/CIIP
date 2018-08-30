using DevExpress.Xpo;
using CIIP.Module.BusinessObjects;
using 常用基类;

namespace WMS
{
    [NonPersistent]
    public abstract class 库存单据明细<TMaster> : 库存流水
        where TMaster : 单据
    {
        public 库存单据明细(Session s) : base(s)
        {

        }

        private TMaster _单据;

        public TMaster 单据
        {
            get { return _单据; }
            set
            {
                SetPropertyValue("Master", ref _单据, value);
                if (!IsLoading)
                {
                    OnSetMaster(value);
                }
            }
        }

        protected virtual void OnSetMaster(TMaster value)
        {
            
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (propertyName == "Master")
            {
                var order = newValue as IWarehouseOrder;
                if (order != null)
                {
                    this.操作类型 = order.操作类型;
                }
            }
        }
    }
}