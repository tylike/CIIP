using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Model;
using CIIP;
using CIIP.Module.BusinessObjects;
using CIIP.Module.BusinessObjects.SYS;
using 常用基类;

namespace WMS
{
    public class 仓库单据配置 : BusinessConfigBase
    {
        public 仓库单据配置(Session s) : base(s)
        {

        }

        private 库存操作类型 _操作类型;

        public 库存操作类型 操作类型
        {
            get { return _操作类型; }
            set { SetPropertyValue("操作类型", ref _操作类型, value); }
        }
    }

    [NonPersistent]
    [BusinessConfigType(typeof(仓库单据配置))]
    public abstract class 仓库单据基类<[ItemType(typeof(库存单据明细<>))]TItem> : 单据<TItem>,IWarehouseOrder
        where TItem : 库存流水
    {
        private 仓库 _目标仓库;

        public 仓库 目标仓库
        {
            get { return _目标仓库; }
            set { SetPropertyValue("目标仓库", ref _目标仓库, value); }
        }

        [Browsable(false)]
        public 库存操作类型 操作类型
        {
            get
            {
                return GetConfig<仓库单据配置>(this.GetType()).操作类型;
            }
        }

        private DateTime _操作日期;
        public DateTime 操作日期
        {
            get { return _操作日期; }
            set { SetPropertyValue("操作日期", ref _操作日期, value); }
        }
        private string _操作人;
        public string 操作人
        {
            get { return _操作人; }
            set { SetPropertyValue("操作人", ref _操作人, value); }
        }


        //private 审核类型 _审核状态;
        //[ModelDefault("AllowEdit","False")]
        //public 审核类型 审核状态
        //{
        //    get { return _审核状态; }
        //    set { SetPropertyValue("审核状态", ref _审核状态, value); }
        //}

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading)
            {
                if (propertyName == "状态")
                {
                    foreach (var item in this.明细项目)
                    {
                        item.审核状态 = this.状态;
                    }
                }
            }
        }

        public 仓库单据基类(Session s) : base(s)
        {

        }
    }
}