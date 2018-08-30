using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects.Product
{
    [NavigationItem("产品管理")]
    public class 价格目录 : SimpleObject
    {
        public 价格目录(Session s) : base(s)
        {

        }


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

        private decimal _采购价格;

        public decimal 采购价格
        {
            get { return _采购价格; }
            set { SetPropertyValue("采购价格", ref _采购价格, value); }
        }


        private 往来单位 _供应商;

        public 往来单位 供应商
        {
            get { return _供应商; }
            set { SetPropertyValue("供应商", ref _供应商, value); }
        }


        private 往来单位 _客户;

        public 往来单位 客户
        {
            get { return _客户; }
            set { SetPropertyValue("客户", ref _客户, value); }
        }
    }
}