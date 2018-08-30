using DevExpress.Xpo;
using CIIP;
using 常用基类;

namespace WMS
{
    public class 调拨设置 : NameObject
    {
        public 调拨设置(Session s) : base(s)
        {

        }

        private bool _允许负库存;
        public bool 允许负库存
        {
            get { return _允许负库存; }
            set { SetPropertyValue("允许负库存", ref _允许负库存, value); }
        }

    }
}