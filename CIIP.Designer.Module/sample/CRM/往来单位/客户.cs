using DevExpress.Xpo;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace CIIP.Module.BusinessObjects
{
    //[NavigationItem("往来单位")]
    //[DefaultClassOptions]
    public class 客户 : 往来单位信息
    {
        public 客户(Session s)
            : base(s)
        {

        }
    }
}
