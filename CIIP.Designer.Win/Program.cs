using System;
using System.Windows.Forms;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using System.Diagnostics;
using DevExpress.Xpo;
using System.Threading;
using CIIP.ProjectManager;
using DevExpress.ExpressApp.Win.Utils;
using DevExpress.LookAndFeel;

namespace CIIP.Win
{
    static class Program {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Project.ApplicationStartupPath = Application.StartupPath;
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("zh-CN");
#if EASYTEST
            DevExpress.ExpressApp.Win.EasyTest.EasyTestRemotingRegistration.Register();
#endif
            CIIP.CIIPDebugger.ShowView = (t) => {
                var t1 = t as XPBaseObject;
                var frm = new Form();
                var pg = new PropertyGrid();
                pg.SelectedObject = t1.GetMemberValue("Master");

                frm.Controls.Add(pg);
                pg.Dock = DockStyle.Fill;
                frm.ShowDialog();
            };

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            EditModelPermission.AlwaysGranted = Debugger.IsAttached;
            AdmiralEnvironment.IsWindows = true;
            var conn = "Provider=Microsoft.Jet.OLEDB.4.0;Password=;User ID=Admin;Data Source=app.cfg;Mode=Share Deny None;";
            conn = @"Integrated Security=SSPI;Pooling=false;Data Source=.\sql2016;Initial Catalog=CIIP2018.1.0904";

            UserLookAndFeel.Default.SetSkinStyle("Office 2016 Colorful");


            CIIPDesignerWindowsFormsApplication winApplication = new CIIPDesignerWindowsFormsApplication();
            winApplication.SplashScreen = new DXSplashScreen();

            winApplication.ConnectionString = conn;
#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
            }
#endif
            winApplication.DatabaseUpdateMode = DatabaseUpdateMode.UpdateOldDatabase;
            winApplication.CheckCompatibilityType = CheckCompatibilityType.ModuleInfo;
            try
            {
                winApplication.Setup();
                winApplication.Start();
            }
            catch (Exception e)
            {
                winApplication.HandleException(e);
            }
            
        }
    }
}
