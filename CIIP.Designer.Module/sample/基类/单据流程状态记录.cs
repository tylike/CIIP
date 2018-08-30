using System.ComponentModel;
using CIIP.Module.BusinessObjects.Flow;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.DC;
using 常用基类;

namespace CIIP.Module.BusinessObjects
{
    [NavigationItem("审批流程")]
    public class 单据流程状态记录 : SimpleObject
    {
        public 单据流程状态记录(Session s) : base(s)
        {

        }
        
        private 单据 _来源单据;
        public 单据 来源单据
        {
            get { return _来源单据; }
            set { SetPropertyValue("来源单据", ref _来源单据, value); }
        }
        
        private 单据 _目标单据;
        public 单据 目标单据
        {
            get { return _目标单据; }
            set { SetPropertyValue("目标单据", ref _目标单据, value); }
        }
        
        private FlowAction _执行动作;
        public FlowAction 执行动作
        {
            get { return _执行动作; }
            set { SetPropertyValue("执行动作", ref _执行动作, value); }
        }
        
        [Association, DevExpress.Xpo.Aggregated]
        public XPCollection<单据流程记录明细> 明细
        {
            get
            {
                return GetCollection<单据流程记录明细>("明细");
            }
        }

        private 业务项目 _业务项目;
        [Association]
        public 业务项目 业务项目
        {
            get { return _业务项目; }
            set { SetPropertyValue("业务项目", ref _业务项目, value); }
        }
    }
}