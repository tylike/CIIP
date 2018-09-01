using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using CIIP.Persistent.BaseImpl;
using System.Linq;

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
    [DefaultClassOptions]
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
        protected override void OnActivated()
        {
            base.OnActivated();
            ObjectSpace.Committed += ObjectSpace_Committed;
        }

        private void ObjectSpace_Committed(object sender, System.EventArgs e)
        {
            Application.MainWindow.GetController<SwitchProjectControllerBase>().CreateProjectItems();

            //Frame.GetController<SwitchProjectController>().CreateProjectItems();
        }

        private void GenerateStartFile_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //copy默认起动文件到项目目录中去

        }
    }

    public abstract class SwitchProjectControllerBase : WindowController
    {
        public static Project CurrentProject { get; set; }
        SingleChoiceAction switchProject;
        public SwitchProjectControllerBase()
        {
            TargetWindowType = WindowType.Main;
            switchProject = new SingleChoiceAction(this, "SwitchProject", "项目");
            switchProject.Caption = "当前项目";
            switchProject.Execute += SwitchProject_Execute;
            switchProject.ItemType = SingleChoiceActionItemType.ItemIsOperation;

            var compileProject = new SingleChoiceAction(this, "CompileProject", "项目");
            compileProject.Caption = "生成";
            compileProject.Items.Add(new ChoiceActionItem("生成项目", false));
            compileProject.Items.Add(new ChoiceActionItem("生成运行", true));
            compileProject.ItemType = SingleChoiceActionItemType.ItemIsOperation;

            compileProject.Execute += CompileProject_Execute;
        }
        protected abstract void Compile(SingleChoiceActionExecuteEventArgs e, bool showCode);
        private void CompileProject_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            Compile(e, false);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            CreateProjectItems();

        }

        public void CreateProjectItems()
        {
            switchProject.Items.Clear();
            var os = Application.CreateObjectSpace();
            var projects = os.GetObjectsQuery<Project>().ToArray();
            foreach (var item in projects)
            {
                switchProject.Items.Add(new ChoiceActionItem(item.Name, item));
            }
        }

        private void SwitchProject_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            CurrentProject = e.SelectedChoiceActionItem?.Data as Project;
            if (CurrentProject != null)
            {
                switchProject.Caption = CurrentProject.Name;
            }
        }
    }
}