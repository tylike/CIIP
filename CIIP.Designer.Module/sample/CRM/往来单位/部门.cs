using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects
{
    public class 部门 : NameObject
    {
        public 部门(Session s) : base(s)
        {

        }

        private 往来单位 _往来单位;

        [Association]
        public 往来单位 往来单位
        {
            get { return _往来单位; }
            set { SetPropertyValue("往来单位", ref _往来单位, value); }
        }

        private string _备注;

        public string 备注
        {
            get { return _备注; }
            set { SetPropertyValue("备注", ref _备注, value); }
        }

        private 员工 _负责人;

        public 员工 负责人
        {
            get { return _负责人; }
            set { SetPropertyValue("负责人", ref _负责人, value); }
        }

        [Association, Aggregated]
        public XPCollection<员工> 员工
        {
            get { return GetCollection<员工>("员工"); }
        }
    }
}