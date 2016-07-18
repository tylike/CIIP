using System;
using System.Configuration;
using System.Windows.Forms;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security.Adapters;
using DevExpress.ExpressApp.Security.Xpo.Adapters;
using DevExpress.ExpressApp.Xpo;
using CIIP.Module;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DevExpress.ExpressApp.Win.SystemModule;
using DevExpress.Xpo;
using DevExpress.XtraBars.Ribbon;
using System.Reflection;
using DevExpress.Xpo.DB.Exceptions;
using CIIP.Module.BusinessObjects.SYS.BOBuilder;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using CIIP.Module.BusinessObjects.SYS;
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.UI.Wizard;
using DevExpress.DataAccess.Wizard.Model;
using DevExpress.DataAccess.UI.Sql;
using CIIP.Module.Win.Editors;
using System.Threading;

namespace CIIP.Win {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("zh-CN");
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

            //如果没有连接字符串配置文件，则认为需要配置
            //var connectionFileInfo = new FileInfo(AdmiralEnvironment.ConnectionStringConfig);
            //if (!connectionFileInfo.Exists)
            //{
            //    var sds = new SqlDataSource();
            //    Action<IWizardCustomization<SqlDataSourceModel>> action = x =>
            //    {
            //        x.StartPage = typeof(DevExpress.DataAccess.Wizard.Presenters.ChooseConnectionPage<SqlDataSourceModel>);
            //    };
            //    var cc = new ConfigureConnectionContext();
            //    SqlDataSourceUIHelper.ConfigureConnection(sds, cc, action);
            //    MessageBox.Show(sds.Connection.ConnectionString);
            //}
            AdmiralEnvironment.IsWindows = true;
            var conn = AdmiralEnvironment.ReadConnectionString();// "Provider=Microsoft.Jet.OLEDB.4.0;Password=;User ID=Admin;Data Source=IMatrix.ERP.mdb;Mode=Share Deny None;";

            while (string.IsNullOrEmpty(conn))
            {
                var frm = new FrmEditConnectionString();
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show("没有配置连接字符串，将退出系统，如需继续，请重新运行商信系统！");
                    Application.Exit();
                    return;
                }
                conn = AdmiralEnvironment.ReadConnectionString();
            }
            


            //if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null)
            //{
            //    //winApplication.ConnectionString = 
            //    conn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            //}

            try
            {
                //检查数据库中的模块,与本地的是否版本一致.
                //不一致时,则将库中模块保存到本地
                //等待模块被加载应用
                var datalayer = XpoDefault.GetDataLayer(conn, DevExpress.Xpo.DB.AutoCreateOption.None);
                var session = new Session(datalayer);
                var modules = session.Query<BusinessModule>().ToList();
                BusinessBuilder.Instance.LoadAssemblies(modules);

                //if (this.File != null && !this.File.IsEmpty)
                //{
                //    var file = AdmiralEnvironment.SaveBusinessLogic(this.File);
                //    this.FileVersion = FileVersionInfo.GetVersionInfo(file).FileVersion;
                //    var asm = Mono.Cecil.AssemblyDefinition.ReadAssembly(file);
                //    this.Version = asm.Name.Version.ToString();
                //    this.Description =
                //        asm.CustomAttributes.FirstOrDefault(
                //            x => x.AttributeType.FullName == typeof(AssemblyDescriptionAttribute).FullName)?
                //            .ConstructorArguments.FirstOrDefault()
                //            .Value.ToString();
                //    //asm.MainModule.Types..Name;
                //}
            }
            catch (SchemaCorrectionNeededException ex)
            {

            }
            catch (UnableToOpenDatabaseException)
            {
                
            }
            catch (Exception ex)
            {
                throw ex;
            }



            ERPWindowsFormsApplication winApplication = new ERPWindowsFormsApplication();
            // Refer to the https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112680.aspx help article for more details on how to provide a custom splash form.
            winApplication.SplashScreen = new DevExpress.ExpressApp.Win.Utils.DXSplashScreen(typeof(WinSplashForm));// ("YourSplashImage.png");
            IsGrantedAdapter.Enable(XPOSecurityAdapterHelper.GetXpoCachedRequestSecurityAdapters());
            winApplication.ConnectionString = conn;

            
#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
            }
#endif
            //if(System.Diagnostics.Debugger.IsAttached && winApplication.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
            //    winApplication.DatabaseUpdateMode = DatabaseUpdateMode.UpdateOldDatabase;// DatabaseUpdateMode.UpdateDatabaseAlways;
            //}
            //if (System.Diagnostics.Debugger.IsAttached)
            //{

            //}
            
            //AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            winApplication.DatabaseUpdateMode = DatabaseUpdateMode.UpdateOldDatabase;
            winApplication.CheckCompatibilityType = CheckCompatibilityType.ModuleInfo;

            //winApplication.CreateCustomTemplate += delegate (object sender, CreateCustomTemplateEventArgs e)
            //{
            //    bool isRibbon = ((IModelOptionsWin)e.Application.Model.Options).FormStyle == RibbonFormStyle.Ribbon;
            //    //if (isRibbon && e.Context == TemplateContext.ApplicationWindow)
            //    //{
            //    //    e.Template = new MainRibbonForm();
            //    //}
            //};

            try
            {
                winApplication.Setup();
                winApplication.Start();
                if (winApplication.IsRestartApplication)
                {
                    try
                    {
                        Application.Restart();
                    }
                    catch
                    {

                    }
                }

            }
            catch (Exception e)
            {
                winApplication.HandleException(e);
            }
            
        }

        private static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            if(e.Exception is FileNotFoundException)
            {

            }
            Debug.WriteLine(e.Exception.Message);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
              
        }
    }
}
