using CIIP.Module.BusinessObjects.Product;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects
{
    [NavigationItem("产品管理")]
    public class 计量单位转换 : SimpleObject
    {
        public 计量单位转换(Session s):base(s)
        {

        }


        private 产品 _产品;

        public 产品 产品
        {
            get { return _产品; }
            set { SetPropertyValue("产品", ref _产品, value); }
        }



        private 计量单位 _来源单位;

        public 计量单位 来源单位
        {
            get { return _来源单位; }
            set { SetPropertyValue("来源单位", ref _来源单位, value); }
        }


        private decimal _转换系数;

        public decimal 转换系数
        {
            get { return _转换系数; }
            set { SetPropertyValue("转换系数", ref _转换系数, value); }
        }


        private 计量单位 _目标单位;

        public 计量单位 目标单位
        {
            get { return _目标单位; }
            set { SetPropertyValue("目标单位", ref _目标单位, value); }
        }



    }
}