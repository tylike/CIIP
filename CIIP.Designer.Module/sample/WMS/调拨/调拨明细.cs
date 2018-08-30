using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace WMS
{
    public class 调拨明细 : 库存单据明细<调拨单>
    {
        public 调拨明细(Session s) : base(s)
        {

        }

        private 仓库 _调出仓库;
        [RuleRequiredField]
        public 仓库 调出仓库
        {
            get { return _调出仓库; }
            set { SetPropertyValue("调出仓库", ref _调出仓库, value); }
        }

        private 库位 _调出库位;
        [DataSourceCriteria("仓库='@This.调出仓库'")]
        [RuleRequiredField]

        public 库位 调出库位
        {
            get { return _调出库位; }
            set { SetPropertyValue("调出库位", ref _调出库位, value); }
        }

        private 仓库 _目标仓库;
        [RuleRequiredField]

        public 仓库 目标仓库
        {
            get { return _目标仓库; }
            set { SetPropertyValue("目标仓库", ref _目标仓库, value); }
        }

        protected override string 库位条件
        {
            get
            {
                if (目标仓库 == null)
                    return "";
                return "仓库.Oid='" + this.目标仓库.Oid + "'";
            }
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading)
            {
                if (propertyName == "调出仓库")
                {
                    this.调出库位 = null;
                }
                if (propertyName == "目标仓库")
                {
                    this.库位 = null;
                }
            }
        }

        protected override void OnSetMaster(调拨单 value)
        {
            this.调出仓库 = value.调出仓库;
            this.调出库位 = value.调出库位;

            this.目标仓库 = value.目标仓库;

            this.库位 = value.目标库位;
            base.OnSetMaster(value);
        }
    }
}