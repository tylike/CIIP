using System;
using System.ComponentModel;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace CIIP.Module.BusinessObjects.SYS.Logic
{
    #region event
    [XafDisplayName("ÊÂ¼þ")]
    [ImageName("PG_Event")]
    [Appearance("", TargetItems = "ReturnValue;AccessorModifier;MethodModifier;CodeName", Enabled = false)]
    public abstract class BusinessObjectEvent : MethodDefine
    {

        public BusinessObjectEvent(Session s) : base(s)
        {

        }

        private BusinessObject _BusinessObject;

        [Association]
        [Browsable(false)]
        public BusinessObject BusinessObject
        {
            get { return _BusinessObject; }
            set { SetPropertyValue("BusinessObject", ref _BusinessObject, value); }
        }

        public override Guid GetDocumentGuid()
        {
            return BusinessObject.Oid;
        }

        public override string GetFileName()
        {
            return BusinessObject.GetFileName();

        }
    }

    #endregion
}