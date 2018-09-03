using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
//using Microsoft.CodeAnalysis;
using CIIP.Module.Win.Editors;
using System.Diagnostics;
using CIIP.Module.BusinessObjects.SYS;
using CIIP.Module.BusinessObjects.SYS.Logic;
using DevExpress.XtraEditors;

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
        protected override void Compile(SingleChoiceActionExecuteEventArgs e,bool showCode)
        {
            var os = Application.CreateObjectSpace();
            var workspace = SmartIDEWorkspace.GetIDE(os);
            var rst = workspace.Compile();
            if (rst != null)
            {

                if (!rst.Success || showCode)
                {
                    var solution = new Solution();
                    solution.Code = new CsharpCode("", null);
                    solution.Code.ShowSolutionFiles = true;
                    solution.Code.Workspace = workspace;
                    solution.Code.Diagnostics = rst.Diagnostics.ToList();
                    var view = Application.CreateDetailView(os, solution);
                    e.ShowViewParameters.CreatedView = view;
                    e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                }
                else
                {
                    if (e.SelectedChoiceActionItem != null && Equals(e.SelectedChoiceActionItem.Data, true))
                    {
                        Process.Start(CurrentProject.WinStartupFile);
                    }
                    //Application.ShowViewStrategy.ShowMessage("编译成功!" + AdmiralEnvironment.UserDefineBusinessTempFile.FullName);
                }
            }
        }
    }
}
