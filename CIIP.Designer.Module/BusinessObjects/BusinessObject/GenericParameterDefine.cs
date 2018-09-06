using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CIIP.Designer
{
    public class GenericParameterDefine : BaseObject
    {
        public GenericParameterDefine(Session s) : base(s)
        {

        }

        private BusinessObjectBase _Owner;

        [Association]
        [XafDisplayName("所属业务对象")]
        [ToolTip("指这个参数实例是在哪个业务对象中定义的")]
        public BusinessObjectBase Owner
        {
            get { return _Owner; }
            set { SetPropertyValue("Owner", ref _Owner, value); }
        }

        private string _Name;
        [XafDisplayName("参数名称")]
        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue("Name", ref _Name, value); }
        }

        private int _ParameterIndex;
        [XafDisplayName("参数顺序")]
        public int ParameterIndex
        {
            get { return _ParameterIndex; }
            set { SetPropertyValue("ParameterIndex", ref _ParameterIndex, value); }
        }

#warning 约束是需要细化的
        private string _Constraint;
        [XafDisplayName("约束")]
        public string Constraint
        {
            get { return _Constraint; }
            set { SetPropertyValue("Constraint", ref _Constraint, value); }
        }
    }

}