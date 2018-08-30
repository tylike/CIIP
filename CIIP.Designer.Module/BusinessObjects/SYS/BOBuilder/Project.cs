using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.ProjectManager
{
    //系统起动后,必须选择一个项目才可以继续.
    //启动按钮:
    //1.先运行生成按project,
    //2.运行StartupFile
    //3.startupFile如何知道生成的文件?



    //生成按钮:生成project,内容包含bo文件
    //起动文件中配置了自动读取dll模块的方法



    [XafDisplayName("项目管理")]
    [NavigationItem]
    public class Project : NameObject
    {
        public static string ApplicationStartupPath { get; set; }
        public Project(Session s) : base(s)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            ProjectPath = ApplicationStartupPath;
            StartupFile = ApplicationStartupPath + "\\CIIP.Client.Win.Exe";
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
    
    public class ProjectViewController : ViewController
    {
        public ProjectViewController()
        {
            TargetObjectType = typeof(Project);

            var generateStartFile = new SimpleAction(this, "GenerateStartupFile", PredefinedCategory.Unspecified);
            generateStartFile.Caption = "生成启动文件";
            generateStartFile.Execute += GenerateStartFile_Execute;
        }

        private void GenerateStartFile_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //copy默认起动文件到项目目录中去

        }
    }
}