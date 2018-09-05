using CIIP.Persistent.BaseImpl;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace CIIP
{
    [NavigationItem]
    [XafDisplayName("模块定义")]
    public class BusinessModule : NameObject
    {
        public BusinessModule(Session s) : base(s)
        {

        }


        /// <summary>
        /// 路径:生成dll时保存到如下路径中去.
        /// </summary>
        [XafDisplayName("输出路径")]
        public string OutputPath
        {
            get { return GetPropertyValue<string>(nameof(OutputPath)); }
            set { SetPropertyValue(nameof(OutputPath), value); }
        }
    }
}