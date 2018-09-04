using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
//using Microsoft.CodeAnalysis;
using CIIP.Module.Win.Editors;
using System.Diagnostics;
using CIIP.Module.BusinessObjects.SYS;
using CIIP.Module.BusinessObjects.SYS.Logic;
using DevExpress.XtraEditors;
using System.IO;

namespace CIIP.ProjectManager
{
    public class SwitchProjectController : SwitchProjectControllerBase
    {
        protected override void ShowMessage(string text)
        {
            var msg = new XtraMessageBoxArgs();
            msg.MessageBeepSound = MessageBeepSound.Error;
            msg.Text = text;
            XtraMessageBox.Show(msg);
        }
        protected override void Compile(SingleChoiceActionExecuteEventArgs e)
        {
            var ca = (CompileAction)e.SelectedChoiceActionItem.Data;
            var compile = ca == CompileAction.开始暂停 || ca == CompileAction.编译 || ca == CompileAction.编译运行;
            var run = ca != CompileAction.编译;
            var compileSuccess = true;
            if (compile)
            {
                var outputFullPath = Path.Combine(CurrentProject.WinProjectPath, CurrentProject.Name + ".dll");
                var os = Application.CreateObjectSpace();
                var workspace = SmartIDEWorkspace.GetIDE(os,CurrentProject);
                var rst = workspace.Compile(outputFullPath);
                if (rst != null)
                {

                    if (!rst.Success)
                    {
                        var solution = new Solution();
                        solution.Code = new CsharpCode("", null);
                        solution.Code.ShowSolutionFiles = true;
                        solution.Code.Workspace = workspace;
                        solution.Code.Diagnostics = rst.Diagnostics.ToList();
                        var view = Application.CreateDetailView(os, solution);
                        e.ShowViewParameters.CreatedView = view;
                        e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        compileSuccess = false;
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage("编译通过!");
                    }
                }
            }

            if (run && compileSuccess)
            {
                var msg = new MessageOptions();
                msg.Message = "准备启动客户端:" + CurrentProject.WinStartupFile;
                msg.OkDelegate = () =>
                {
                    var fi = new FileInfo(CurrentProject.WinStartupFile);
                    Process.Start(fi.DirectoryName);
                };
                Application.ShowViewStrategy.ShowMessage(msg);
                var para = "";
                if (ca == CompileAction.开始暂停)
                {
                    para = " -debug";
                }
                var pc = Process.Start(CurrentProject.WinStartupFile, para);
                pc.Exited += (s, evt) =>
                {
                    Application.ShowViewStrategy.ShowMessage("退出代码:" + pc.ExitCode);
                };
            }
        }

        private void Pc_Exited(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
