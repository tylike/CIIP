using System;
using DevExpress.ExpressApp.DC;

namespace CIIP.Module.BusinessObjects.SYS
{
    [XafDisplayName("登陆系统")]
    [DomainComponent, Serializable]
    public class SystemLogonParameter:DevExpress.ExpressApp.Security.AuthenticationStandardLogonParameters
    {
        private bool _safeMode;
        [XafDisplayName("安全模式")]
        public bool SafeMode
        {
            get
            {
                return this._safeMode;
            }
            set
            {
                this._safeMode = value;
                this.RaisePropertyChanged("SafeMode");
            }
        }
    }
}