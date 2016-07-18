using System;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using CIIP;
using CIIP.Module.BusinessObjects;
using CIIP.Module.BusinessObjects.Product;
using 常用基类;

namespace WMS
{
    [NavigationItem("产品库存")]
    [DefaultClassOptions]
    public class 库存流水 : SimpleObject
    {
        public 库存流水(Session s)
            : base(s)
        {

        }

        产品 _产品;
        [RuleRequiredField]
        public 产品 产品
        {
            get
            {
                return _产品;
            }
            set
            {
                SetPropertyValue("产品", ref _产品, value);
            }
        }

        protected virtual string 库位条件
        {
            get { return ""; }
        }
        
        库位 _库位;
        [RuleRequiredField]
        [DataSourceCriteria("库位条件")]
        public 库位 库位
        {
            get
            {
                return _库位;
            }
            set
            {
                SetPropertyValue("库位", ref _库位, value);
            }
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading)
            {
                if (propertyName == "单价" || propertyName == "数量")
                {
                    this.总价 = this.单价 * this.数量;
                }
            }
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            this.操作时间 = DateTime.Now;
        }

        decimal _数量;
        [RuleValueComparison(ValueComparisonType.GreaterThan, 0)]
        public decimal 数量
        {
            get
            {
                return _数量;
            }
            set
            {
                SetPropertyValue("数量", ref _数量, value);
            }
        }

        private 计量单位 _计量单位;
        [RuleRequiredField]
        public 计量单位 计量单位
        {
            get { return _计量单位; }
            set { SetPropertyValue("计量单位", ref _计量单位, value); }
        }


        decimal _单价;
        public decimal 单价
        {
            get
            {
                return _单价;
            }
            set
            {
                SetPropertyValue("单价", ref _单价, value);
            }
        }
        decimal _总价;

        [ModelDefault("AllowEdit", "False")]
        public decimal 总价
        {
            get
            {
                return _总价;
            }
            set
            {
                SetPropertyValue("总价", ref _总价, value);
            }
        }

        private 库存操作类型 _操作类型;
        /// <summary>
        /// 操作类型
        /// </summary>        
        [ModelDefault("AllowEdit","False")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public 库存操作类型 操作类型
        {
            get { return _操作类型; }
            set { SetPropertyValue("操作类型", ref _操作类型, value); }
        }

        private DateTime _操作时间;
        [ModelDefault("AllowEdit", "False")]
        public DateTime 操作时间
        {
            get { return _操作时间; }
            set { SetPropertyValue("操作时间", ref _操作时间, value); }
        }

        private 审核类型 _审核状态;
        [ModelDefault("AllowEdit","False")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public 审核类型 审核状态
        {
            get { return _审核状态; }
            set { SetPropertyValue("审核状态", ref _审核状态, value); }
        }
    }
}