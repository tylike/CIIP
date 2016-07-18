using System;
using System.Drawing;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Utils;
using CIIP;

namespace CIIP.Module.BusinessObjects.SYS
{
    [NonPersistent]
    [XafDefaultProperty("Caption")]
    public class FlowInstanceNode : BaseObject, IFlowNode
    {
        public FlowInstanceNode(Session s) : base(s)
        {

        }

        public object Key { get; set; }
        public object Key2 { get; set; }

        public string Caption
        {
            get;

            set;
        }

        public int Height
        {
            get;

            set;
        }

        public int Width
        {
            get;

            set;
        }

        public int X
        {
            get;

            set;
        }

        public int Y
        {
            get;

            set;
        }

        public string ImageName { get; set; }

        public Image GetImage()
        {
            if (!string.IsNullOrEmpty(ImageName))
            {
                return ImageLoader.Instance.GetLargeImageInfo(ImageName).Image;
            }
            return null;
        }
    }
}