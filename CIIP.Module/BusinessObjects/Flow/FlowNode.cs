using System;
using System.Drawing;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Utils;
using CIIP;

namespace CIIP.Module.BusinessObjects.Flow
{
    [XafDefaultProperty("Caption")]
    public class FlowNode : BaseObject, IFlowNode
    {
        public FlowNode(Session s)
            : base(s)
        {

        }
        private Flow _Flow;
        [Association]
        public Flow Flow
        {
            get { return _Flow; }
            set { SetPropertyValue("Flow", ref _Flow, value); }
        }

        private string _Caption;
        [XafDisplayName("标题")]
        public virtual string Caption
        {
            get { return _Caption; }
            set { SetPropertyValue("Caption", ref _Caption, value); }
        }

        [XafDisplayName("业务对象")]

        private string _Form;
        public string Form
        {
            get { return _Form; }
            set { SetPropertyValue("Form", ref _Form, value); }
        }

        private int _X;

        public int X
        {
            get { return _X; }
            set { SetPropertyValue("X", ref _X, value); }

        }

        private int _Y;

        public int Y
        {
            get { return _Y; }
            set { SetPropertyValue("Y", ref _Y, value); }
        }


        private int _Height;

        public int Height
        {
            get { return _Height; }
            set { SetPropertyValue("Height", ref _Height, value); }
        }


        private int _Width;

        public int Width
        {
            get { return _Width; }
            set { SetPropertyValue("Width", ref _Width, value); }
        }



        Image IFlowNode.GetImage()
        {

            var cls = CaptionHelper.ApplicationModel.BOModel.GetClass(ReflectionHelper.FindType(Form));
            if (cls != null)
            {
                if (!string.IsNullOrEmpty(cls.ImageName))
                {
                    return ImageLoader.Instance.GetLargeImageInfo(cls.ImageName).Image;

                }


            }
            return null;
        }


    }
}