using System;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using CIIP.Module.BusinessObjects;

namespace WMS
{
    [NavigationItem("仓库管理")]
    public class 报溢单 : 仓库单据基类<报溢明细>
    {
        public 报溢单(Session s) : base(s)
        {

        }

        //public override 库存操作类型 操作类型
        //{
        //    get { return 库存操作类型.入库; }
        //}


    }
}