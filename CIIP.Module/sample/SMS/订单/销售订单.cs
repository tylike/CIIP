using System;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Xpo;
using CIIP.Module.BusinessObjects;
using 常用基类;

namespace SMS
{
    [DefaultClassOptions]
    [NavigationItem("销售管理")]
    public class 销售订单 : 订单<销售订单明细>
        //,ICategorizedItem
    {
        public 销售订单(Session s)
            : base(s)
        {

        }

        //ITreeNode ICategorizedItem.Category
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }

        //    set
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
    }
}