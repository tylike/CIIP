using System;
using CIIP.CodeFirstView;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects.Product
{
    [NavigationItem("产品管理")]
    [DefaultClassOptions]
    public class 产品 : NameObject
    {
        public 产品(Session s)
            : base(s)
        {

        }

        private 品牌 _品牌;

        public 品牌 品牌
        {
            get { return _品牌; }
            set { SetPropertyValue("品牌", ref _品牌, value); }
        }

        private 型号 _型号;

        public 型号 型号
        {
            get { return _型号; }
            set { SetPropertyValue("型号", ref _型号, value); }
        }

        private string _说明;
        [Size(-1)]
        public string 说明
        {
            get { return _说明; }
            set { SetPropertyValue("说明", ref _说明, value); }
        }


        private decimal _零售价;

        public decimal 零售价
        {
            get { return _零售价; }
            set { SetPropertyValue("零售价", ref _零售价, value); }
        }


        private decimal _进货价格;

        public decimal 进货价格
        {
            get { return _进货价格; }
            set { SetPropertyValue("进货价格", ref _进货价格, value); }
        }

        private 计量单位 _默认单位;
        public 计量单位 默认单位
        {
            get { return _默认单位; }
            set { SetPropertyValue("默认单位", ref _默认单位, value); }
        }
    }

    //功能：用于拼写sql查询各方来源，得到结果，然后附值给真正要使用的地方。

    //public class 产品价格_LookupListView : ListViewObject<产品价格>
    //{
    //    public override void LayoutListView()
    //    {
            
    //        LayoutColumns(x => x.产品, x => x.价格, x => x.单位, x => x.来源);
    //    }
    //}
    //1.以往的采购价格
    //2.价格目录
    //3.产品价格


}