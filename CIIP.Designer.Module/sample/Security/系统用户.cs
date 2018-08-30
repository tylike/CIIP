using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIIP.Module.BusinessObjects.Security
{
    [NavigationItem("安全设置")]
    public class 系统用户 : SecuritySystemUser
    {
        public 系统用户(Session s) : base(s)
        {

        }
        
        private 员工 _员工;
        [RuleRequiredField]
        public 员工 员工
        {
            get { return _员工; }
            set { SetPropertyValue("员工", ref _员工, value); }
        }
    }
}
