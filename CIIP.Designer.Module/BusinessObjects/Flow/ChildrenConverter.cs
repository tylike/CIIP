using System;
using CIIP.Module.BusinessObjects.SYS.Logic;
using DevExpress.Xpo;

namespace CIIP.Module.BusinessObjects.Flow
{
    public class ChildrenConverter : MethodDefine
    {

        private FlowAction _FlowAction;
        public FlowAction FlowAction
        {
            get { return _FlowAction; }
            set { SetPropertyValue("FlowAction", ref _FlowAction, value); }
        }
        public override void SetCode(string code)
        {
            base.SetCode(code);

        }
        public ChildrenConverter(Session s) : base(s)
        {
        }

        public override Guid GetDocumentGuid()
        {
            return FlowAction.GetDocumentGuid();
        }

        public override string GetFileName()
        {
            return FlowAction.GetFileName();
        }
    }
}