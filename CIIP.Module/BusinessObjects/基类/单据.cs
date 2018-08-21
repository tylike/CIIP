using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Model;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using System.Collections.Generic;
using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using CIIP;
using CIIP.Module.BusinessObjects;

namespace 常用基类
{
    [NonPersistent]
    public abstract class 单据<[ItemType(typeof(明细<>))]TItem> : 单据
        where TItem : XPBaseObject

    {
        public 单据(Session s) : base(s)
        {

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
    }
    
    //[NonPersistent]
    [XafDefaultProperty("Title")]
    public class 单据 : SimpleObject,I单据
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
    }
}