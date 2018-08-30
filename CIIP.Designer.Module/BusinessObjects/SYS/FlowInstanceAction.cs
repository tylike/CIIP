using System;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using CIIP;

namespace CIIP.Module.BusinessObjects.SYS
{
    [NonPersistent]
    public class FlowInstanceAction : BaseObject, IFlowAction
    {
        public FlowInstanceAction(Session s) : base(s)
        {

        }

        public int BeginItemPointIndex
        {
            get;

            set;
        }

        public string Caption
        {
            get;

            set;
        }

        public int EndItemPointIndex
        {
            get;

            set;
        }

        public IFlowNode From
        {
            get;

            set;
        }

        public IFlowNode To
        {
            get;

            set;
        }
    }
}