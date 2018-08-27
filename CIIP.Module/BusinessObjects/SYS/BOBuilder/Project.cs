using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects.SYS
{
    [XafDisplayName("项目管理")]
    [NavigationItem]
    public class Project : NameObject
    {
        public Project(Session s) : base(s)
        {
        }

        /// <summary>
        /// 是否是本系统生成的
        /// </summary>
        [XafDisplayName("定制生成")]
        [ToolTip("选中时为定系统生成的,否则为存在的dll导入的.")]
        public bool Generated
        {
            get { return GetPropertyValue<bool>(nameof(Generated)); }
            set { SetPropertyValue(nameof(Generated), value); }
        }


        /// <summary>
        /// 在windows下调试时使用哪个起动文件
        /// </summary>
        [XafDisplayName("起动文件")]
        public string StartupFile
        {
            get { return GetPropertyValue<string>(nameof(StartupFile)); }
            set { SetPropertyValue(nameof(StartupFile), value); }
        }

        /// <summary>
        /// 路径:生成dll时保存到如下路径中去.
        /// </summary>
        [XafDisplayName("项目路径")]
        public string ProjectPath
        {
            get { return GetPropertyValue<string>(nameof(ProjectPath)); }
            set { SetPropertyValue(nameof(ProjectPath), value); }
        }
    }
}