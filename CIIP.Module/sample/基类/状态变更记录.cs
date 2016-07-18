using System;
using CIIP.StateMachine;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects
{
    public class 状态变更记录 : BaseObject
    {
        public 状态变更记录(Session s) : base(s)
        {

        }

        private 单据 _单据;
        [Association]
        public 单据 单据
        {
            get { return _单据; }
            set { SetPropertyValue("单据", ref _单据, value); }
        }



        private DateTime _发生日期;

        public DateTime 发生日期
        {
            get { return _发生日期; }
            set { SetPropertyValue("发生日期", ref _发生日期, value); }
        }



        private CIIPXpoState _来源状态;

        public CIIPXpoState 来源状态
        {
            get { return _来源状态; }
            set { SetPropertyValue("来源状态", ref _来源状态, value); }
        }

        private CIIPXpoState _目标状态;

        public CIIPXpoState 目标状态
        {
            get { return _目标状态; }
            set { SetPropertyValue("目标状态", ref _目标状态, value); }
        }

        private string _操作人;

        public string 操作人
        {
            get { return _操作人; }
            set { SetPropertyValue("操作人", ref _操作人, value); }
        }
    }
}