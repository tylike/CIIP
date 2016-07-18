using System;
using System.ComponentModel;
using DevExpress.ExpressApp.Utils;
using DevExpress.Xpo;

namespace CIIP.Win.General.DashBoard.BusinessObjects {
    [DefaultProperty("Caption")]
    [NonPersistent]
    public class TypeWrapper : XPBaseObject, ITypeWrapper {
        public static CaptionHelper ClassCaptionProvider = new CaptionHelper();

        public TypeWrapper(Session session, Type type)
            : base(session) {
            Type = type;
        }

        public Type Type { get; private set; }

        public String Caption {
            get { return CaptionHelper.GetClassCaption(Type.FullName); }
        }

        public override string ToString() {
            return Caption;
        }

        protected override void OnSaving() {
            throw new Exception("Cannot save DashboardTargetType");
        }
    }
}
