using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CIIP.Module.BusinessObjects.SYS.Logic
{
    [XafDisplayName("参数定义")]
    public class ParameterDefine : BaseObject
    {
        public ParameterDefine(Session s) : base(s)
        {

        }

        private MethodDefine _OwnerMethod;
        [XafDisplayName("所属方法"),Association]
        public MethodDefine OwnerMethod
        {
            get { return _OwnerMethod; }
            set { SetPropertyValue("OwnerMethod", ref _OwnerMethod, value); }
        }


        private string _ParameterType;
        [XafDisplayName("参数类型")]
        public string ParameterType
        {
            get { return _ParameterType; }
            set { SetPropertyValue("ParameterType", ref _ParameterType, value); }
        }

        private string _ParameterName;
        [XafDisplayName("参数名称")]
        public string ParameterName
        {
            get { return _ParameterName; }
            set { SetPropertyValue("ParameterName", ref _ParameterName, value); }
        }

        private int _Index;
        public int Index
        {
            get { return _Index; }
            set { SetPropertyValue("Index", ref _Index, value); }
        }



    }
}