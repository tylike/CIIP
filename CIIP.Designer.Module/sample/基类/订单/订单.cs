using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.Linq;
using CIIP.Module.BusinessObjects.Security;
using System.Collections.Generic;
using System.ComponentModel;
using CIIP;
using CIIP.Module.BusinessObjects;
using CIIP.Module.BusinessObjects.Product;

namespace 常用基类
{
    [NonPersistent]
    public abstract class 订单<[ItemType(typeof(订单明细<>))]TItem> : 单据<TItem>,ICalc
        where TItem : XPBaseObject
    {
        public 订单(Session s)
            : base(s)
        {

        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            业务员 = Session.GetObjectByKey<系统用户>(SecuritySystem.CurrentUserId).员工;
            下单日期 = DateTime.Now;
        }
        
        DateTime _下单日期;
        [Indexed]
        public DateTime 下单日期
        {
            get { return _下单日期; }
            set { SetPropertyValue("下单日期", ref _下单日期, value); }
        }

        private string _下单日期维度;

        [VisibleInDetailView(false),VisibleInListView(false)]
        public string 下单日期维度
        {
            get { return _下单日期维度; }
            set { SetPropertyValue("下单日期维度", ref _下单日期维度, value); }
        }

        protected override void OnSaving()
        {
            this.下单日期维度 = 下单日期.ToString("yyyyMMdd");
            base.OnSaving();
        }

        员工 _业务员;
        
        public 员工 业务员
        {
            get { return _业务员; }
            set { SetPropertyValue("业务员", ref _业务员, value); }
        }


        private 部门 _部门;
        [DataSourceProperty("业务员.往来单位.部门")]
        public 部门 部门
        {
            get { return _部门; }
            set { SetPropertyValue("部门", ref _部门, value); }
        }
        
        private DateTime _预计送达时间;

        public DateTime 预计送达时间
        {
            get { return _预计送达时间; }
            set { SetPropertyValue("预计送达时间", ref _预计送达时间, value); }
        }


        private DateTime _期望到货时间;

        public DateTime 期望到货时间
        {
            get { return _期望到货时间; }
            set { SetPropertyValue("期望到货时间", ref _期望到货时间, value); }
        }



        private 运输方式 _运输方式;

        public 运输方式 运输方式
        {
            get { return _运输方式; }
            set { SetPropertyValue("运输方式", ref _运输方式, value); }
        }

        private 结算方式 _结算方式;

        public 结算方式 结算方式
        {
            get { return _结算方式; }
            set { SetPropertyValue("结算方式", ref _结算方式, value); }
        }

        private 常用税率 _税率;

        public 常用税率 税率
        {
            get { return _税率; }
            set { SetPropertyValue("税率", ref _税率, value); }
        }

        private 往来单位 _客户;

        [ImmediatePostData]
        [DataSourceCriteria("客户")]
        public 往来单位 客户
        {
            get { return _客户; }
            set { SetPropertyValue("客户", ref _客户, value); }
        }

        private 地址 _收货地址;

        [DataSourceProperty("客户.地址")]
        public 地址 收货地址
        {
            get { return _收货地址; }
            set { SetPropertyValue("收货地址", ref _收货地址, value); }
        }


        private 地址 _发货地址;
        [DataSourceProperty("供应商.地址")]
        public 地址 发货地址
        {
            get { return _发货地址; }
            set { SetPropertyValue("发货地址", ref _发货地址, value); }
        }

        private 员工 _客户联系人;

        [DataSourceProperty("客户.联系人")]
        public 员工 客户联系人
        {
            get { return _客户联系人; }
            set { SetPropertyValue("客户联系人", ref _客户联系人, value); }
        }

        private 往来单位 _供应商;

        [DataSourceCriteria("客户")]
        [ImmediatePostData]
        public 往来单位 供应商
        {
            get { return _供应商; }
            set
            {
                SetPropertyValue("供应商", ref _供应商, value);
            }
        }
        
        private 员工 _供应商联系人;
        [DataSourceProperty("供应商.联系人")]
        public 员工 供应商联系人
        {
            get { return _供应商联系人; }
            set { SetPropertyValue("供应商联系人", ref _供应商联系人, value); }
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading)
            {
                if (propertyName == "供应商")
                {
                    供应商联系人 = 供应商.默认联系人;
                    发货地址 = 供应商.默认地址;
                }

                if (propertyName == "客户")
                {
                    客户联系人 = 客户.默认联系人;
                    收货地址 = 客户.默认地址;
                }

                if (propertyName == "业务员")
                {
                    this.部门 = this.业务员?.部门;
                }

                if(propertyName == "税率" )
                {
                    var v = newValue == null ? 0 : 税率.税率;
                    foreach (XPBaseObject item in 明细项目)
                    {
                        item.SetMemberValue("税率", v);
                    }
                }
            }
        }

        private decimal _订单总金额;

        [ModelDefault("AllowEdit", "False")]
        public decimal 订单总金额
        {
            get { return _订单总金额; }
            set { SetPropertyValue("订单总金额", ref _订单总金额, value); }
        }

        private decimal _折扣总金额;

        [ModelDefault("AllowEdit", "False")]
        public decimal 折扣总金额
        {
            get { return _折扣总金额; }
            set { SetPropertyValue("折扣总金额", ref _折扣总金额, value); }
        }


        private decimal _折后总金额;

        [ModelDefault("AllowEdit", "False")]
        public decimal 折后总金额
        {
            get { return _折后总金额; }
            set { SetPropertyValue("折后总金额", ref _折后总金额, value); }
        }

        private decimal _税金;

        [ModelDefault("AllowEdit", "False")]
        public decimal 税金
        {
            get { return _税金; }
            set { SetPropertyValue("税金", ref _税金, value); }
        }

        private decimal _含税总金额;

        [ModelDefault("AllowEdit", "False")]
        public decimal 含税总金额
        {
            get { return _含税总金额; }
            set { SetPropertyValue("含税总金额", ref _含税总金额, value); }
        }

        protected ICollection<产品价格> _产品价格;
        [Browsable(false)]
        public virtual ICollection<产品价格> 产品价格
        {
            get
            {
                if(_产品价格 == null)
                {
                    _产品价格 = Session.GetObjectsFromQuery<产品价格>("select Oid,进货价格,默认单位,N'产品默认' from [产品]");
                }
                return _产品价格;
            }
        }

        public void Calc()
        {
            this.订单总金额 = Convert.ToDecimal(this.Evaluate("明细项目.Sum(总价)"));
            this.折后总金额 = Convert.ToDecimal(this.Evaluate("明细项目.Sum(折后总价)"));
            this.折扣总金额 = Convert.ToDecimal(this.Evaluate("明细项目.Sum(折扣金额)"));
            this.含税总金额 = Convert.ToDecimal(this.Evaluate("明细项目.Sum(含税总价)"));
            this.税金 = this.含税总金额 - this.订单总金额;
        }

    }
}