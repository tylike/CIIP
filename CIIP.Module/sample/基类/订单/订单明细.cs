using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using CIIP.Module.BusinessObjects.Flow;
using CIIP.Module.BusinessObjects.Product;
using System.Collections.Generic;
using System.Linq;
using CIIP.Module.BusinessObjects;

namespace 常用基类
{
    [NonPersistent]
    [VisibleInReports(true)]
    public abstract class 订单明细<TMaster>: 明细<TMaster>,ICalc
        where TMaster:单据
    {
        public 订单明细(Session s):base(s)
        {

        }

        private 产品价格 _产品价格;
        [DataSourceProperty("单据.产品价格")]
        [NonPersistent]
        [IgnoreFormConvert]
        public 产品价格 产品价格
        {
            get { return _产品价格; }
            set { SetPropertyValue("产品价格", ref _产品价格, value); }
        }
        
        //订单产品可以从目录中提取，或设置为不使用目录时，取产品默认价格

        private 产品 _产品;

        public 产品 产品
        {
            get { return _产品; }
            set { SetPropertyValue("产品", ref _产品, value); }
        }
        
        private 计量单位 _单位;

        public 计量单位 单位
        {
            get { return _单位; }
            set { SetPropertyValue("单位", ref _单位, value); }
        }

        private decimal _单价;
        [ImmediatePostData]
        public decimal 单价
        {
            get { return _单价; }
            set
            {
                SetPropertyValue("单价", ref _单价, value);

            }
        }

        private decimal _折扣率;
        [ImmediatePostData]
        [RuleRange(0,1)]
        [ModelDefault("DisplayFormat","P")]
        [ModelDefault("EditMask", "p")]
        public decimal 折扣率
        {
            get { return _折扣率; }
            set { SetPropertyValue("折扣率", ref _折扣率, value); }
        }

        private decimal _折扣单价;
        [ModelDefault("AllowEdit", "False")]
        public decimal 折扣单价
        {
            get { return _折扣单价; }
            set { SetPropertyValue("折扣单价", ref _折扣单价, value); }
        }

        private decimal _折后总价;
        [ModelDefault("AllowEdit", "False")]
        public decimal 折后总价
        {
            get { return _折后总价; }
            set { SetPropertyValue("折后总价", ref _折后总价, value); }
        }

        private decimal _折扣金额;
        [ModelDefault("AllowEdit", "False")]
        public decimal 折扣金额
        {
            get { return _折扣金额; }
            set { SetPropertyValue("折扣金额", ref _折扣金额, value); }
        }


        private decimal _数量;
        [ImmediatePostData]
        public decimal 数量
        {
            get { return _数量; }
            set { SetPropertyValue("数量", ref _数量, value); }
        }



        private decimal _总价;

        [ModelDefault("AllowEdit", "False")]
        public decimal 总价
        {
            get { return _总价; }
            set { SetPropertyValue("总价", ref _总价, value); }
        }

        private decimal _税率;
        [ModelDefault("AllowEdit", "False")]
        public decimal 税率
        {
            get { return _税率; }
            set { SetPropertyValue("税率", ref _税率, value); }
        }

        private decimal _含税单价;

        [ModelDefault("AllowEdit", "False")]
        public decimal 含税单价
        {
            get { return _含税单价; }
            set { SetPropertyValue("含税单价", ref _含税单价, value); }
        }

        private decimal _含税总价;

        [ModelDefault("AllowEdit", "False")]
        public decimal 含税总价
        {
            get { return _含税总价; }
            set { SetPropertyValue("含税总价", ref _含税总价, value); }
        }

        public void Calc()
        {
            折扣单价 = 单价 * (1-折扣率);
            折后总价 = 折扣单价 * 数量;
            折扣金额 = (单价 - 折扣单价) * 数量;
            含税单价 = 单价 * (1 + 税率);
            含税总价 = 含税单价 * 数量;
            总价 = 单价 * 数量;
            var calc = this.单据 as ICalc;
            if (calc != null)
                calc.Calc();

        }
        
        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading)
            {
                if (propertyName == "产品价格")
                {
                    this.产品 = this.产品价格.产品;
                    this.单价 = this.产品价格.价格;
                    this.单位 = this.产品价格.单位;
                }

                if (propertyName == "税率" || propertyName == "单价" || propertyName == "数量" || propertyName == "折扣率" || propertyName == "折扣单价")
                {
                    Calc();
                }
            }
        }
    }
}