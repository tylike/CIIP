using System;
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
    }

    #endregion
}