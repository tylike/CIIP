using DevExpress.Xpo;
using CIIP.Module.BusinessObjects.Product;

namespace WMS
{
    public struct InvertoryKey
    {
        [Persistent("产品")]
        public 产品 产品 { get; set; }

        [Persistent("库位")]
        public 库位 库位 { get; set; }
    }
}