using System;
using System.ComponentModel;
using System.Drawing;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Xpo;

namespace IMatrix.ERP.Module.BusinessObjects.Flow
{
    [XafDisplayName("表单")]
    [DefaultClassOptions]
    public class Form : BusinessCodeObject
    {
        public Form(Session s) : base(s)
        {

        }
    }

    //[DefaultClassOptions]
    //[DefaultListViewOptions(MasterDetailMode.ListViewAndDetailView)]
    //[XafDisplayName("分类")]
    //public class Namespace : CodeUnitBase
    //{
    //    public Namespace(Session s):base(s)
    //    {
            
    //    }

    //    public override Image GetImage(out string imageName)
    //    {
    //        imageName = "PG_Namespace";
    //        return ImageLoader.Instance.GetImageInfo(imageName).Image;
    //    }
    //}
}