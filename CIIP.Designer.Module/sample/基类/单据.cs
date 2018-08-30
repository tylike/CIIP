using System.ComponentModel;
using CIIP.StateMachine;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Model;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using System.Collections.Generic;
using CIIP.Module.BusinessObjects.Flow;
using CIIP.FormCode;
using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using CIIP;
using CIIP.Module.BusinessObjects;

namespace 常用基类
{
    [NonPersistent]
    public abstract class 单据<[ItemType(typeof(明细<>))]TItem> : 单据,I审核状态
       where TItem : XPBaseObject

    {
        public 单据(Session s) : base(s)
        {

        }

        private 审核类型 _审核状态;

        [ModelDefault("AllowEdit", "False")]
        public 审核类型 审核状态
        {
            get { return _审核状态; }
            set { SetPropertyValue("审核状态", ref _审核状态, value); }
        }


        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading)
            {
                if(propertyName == "审核状态")
                {
                    if(this.审核状态 == 审核类型.已审核)
                    {
                        this.OnChecked();
                    }
                    else
                    {
                        this.OnUnChecked();
                    }
                }
            }
        }
        
        //[Browsable(false)]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<TItem> 明细项目
        {
            get
            {
                return GetCollection<TItem>("Items");
            }
        }

        public virtual void OnChecked()
        {

        }

        public virtual void OnUnChecked()
        {

        }
    }
    
    //[NonPersistent]
    [XafDefaultProperty("Title")]
    public class 单据 : SimpleObject,I单据编号,I单据
    {
        public 单据(Session s) : base(s)
        {

        }

        public string Title
        {
            get
            {
                return this.GetType().Name + ":" + 创建时间.ToString("yyyy-MM-dd hh:mm:ss");
            }
        }

        private 业务项目 _业务项目;
        [Association]
        public 业务项目 业务项目
        {
            get { return _业务项目; }
            set { SetPropertyValue("业务项目", ref _业务项目, value); }
        }
        
        private CIIPXpoState _状态;
        [IgnoreFormConvert]
        public CIIPXpoState 状态
        {
            get { return _状态; }
            set
            {
                SetPropertyValue("状态", ref _状态, value);
            }
        }

        private string _编号;
        [ModelDefault("AllowEdit", "False")]
        public string 编号
        {
            get { return _编号; }
            set { SetPropertyValue("编号", ref _编号, value); }
        }

        private string _备注;

        [Size(-1)]
        [ModelDefault("RowCount", "3")]
        public string 备注
        {
            get { return _备注; }
            set { SetPropertyValue("备注", ref _备注, value); }
        }

        XPCollection<单据流程状态记录> _单据流程;
        [ModelDefault("AllowEdit", "False")]
        public XPCollection<单据流程状态记录> 单据流程
        {
            get
            {
                if (_单据流程 == null)
                {
                    _单据流程 = new XPCollection<单据流程状态记录>(Session,
                        CriteriaOperator.Or(new BinaryOperator("来源单据", this.Oid), new BinaryOperator("目标单据", this.Oid)));
                }
                return _单据流程;
            }
        }

        [Association,Agg]
        public XPCollection<状态变更记录> 状态记录
        {
            get
            {
                return GetCollection<状态变更记录>("状态记录");
            }
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading)
            {
                if (propertyName == "状态")
                {
                    var rec = new 状态变更记录(Session);
                    rec.来源状态 = (CIIPXpoState)oldValue;
                    rec.目标状态 = (CIIPXpoState)newValue;
                    rec.操作人 = SecuritySystem.CurrentUserName;
                    rec.单据 = this;
                    rec.发生日期 = DateTime.Now;
                    rec.Save();
                }
            }
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (业务项目 == null)
            {
                业务项目 = new 业务项目(Session);
                业务项目.名称 = "未命名" + DateTime.Now.ToString("yyyyMMddhhmmssfff");
            }
        }
    }
}