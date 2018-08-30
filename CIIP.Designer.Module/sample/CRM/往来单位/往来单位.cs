using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.Linq;
using DevExpress.Persistent.Base.General;
using System;
using CRM;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using 常用基类;

namespace CIIP.Module.BusinessObjects
{


    [NavigationItem("往来单位")]
    [DefaultListViewOptions( MasterDetailMode.ListViewAndDetailView)]
    public partial class 往来单位 : NameObject
    {
        public 往来单位(Session s)
            : base(s)
        {

        }

        private bool _客户;

        public bool 客户
        {
            get { return _客户; }
            set { SetPropertyValue("客户", ref _客户, value); }
        }

        private bool _供应商;

        public bool 供应商
        {
            get { return _供应商; }
            set { SetPropertyValue("供应商", ref _供应商, value); }
        }

        private 客户 _客户信息;

        public 客户 客户信息
        {
            get { return _客户信息; }
            set { SetPropertyValue("客户信息", ref _客户信息, value); }
        }

        private 供应商 _供应商信息;

        public 供应商 供应商信息
        {
            get { return _供应商信息; }
            set { SetPropertyValue("供应商信息", ref _供应商信息, value); }
        }

        private 销售区域 _销售区域;

        public 销售区域 销售区域
        {
            get { return _销售区域; }
            set { SetPropertyValue("销售区域", ref _销售区域, value); }
        }

        private 员工 _默认联系人;

        [DataSourceProperty("联系人")]
        public 员工 默认联系人
        {
            get { return _默认联系人; }
            set { SetPropertyValue("默认联系人", ref _默认联系人, value); }
        }

        private 地址 _默认地址;

        [DataSourceProperty("默认地址")]
        public 地址 默认地址
        {
            get { return _默认地址; }
            set { SetPropertyValue("默认地址", ref _默认地址, value); }
        }

        private 渠道 _渠道;

        public 渠道 渠道
        {
            get { return _渠道; }
            set { SetPropertyValue("渠道", ref _渠道, value); }
        }

        [Association, DevExpress.Xpo.Aggregated]
        public XPCollection<员工> 联系人
        {
            get { return GetCollection<员工>("联系人"); }
        }

        [Association, DevExpress.Xpo.Aggregated]
        public XPCollection<部门> 部门
        {
            get { return GetCollection<部门>("部门"); }
        }

        [Association, DevExpress.Xpo.Aggregated]
        public XPCollection<地址> 地址
        {
            get { return GetCollection<地址>("地址"); }
        }

        [Association, DevExpress.Xpo.Aggregated]
        public XPCollection<沟通记录> 沟通记录
        {
            get { return GetCollection<沟通记录>("沟通记录"); }
        }

        private 客户分类 _category;
        [XafDisplayName("客户分类")]
        public 客户分类 Category
        {
            get { return _category; }
            set { SetPropertyValue("Category", ref _category, value); }
        }


    }
}