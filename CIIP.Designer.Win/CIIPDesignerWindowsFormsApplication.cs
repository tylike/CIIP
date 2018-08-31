using System;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win;
using System.Collections.Generic;
using System.Diagnostics;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using System.Linq;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Win.Templates.Ribbon;
using DevExpress.ExpressApp.Win.SystemModule;
using DevExpress.Xpo;
using DevExpress.XtraBars.Docking;
using CIIP;
using CIIP.Module.BusinessObjects;
using DevExpress.ExpressApp.Actions;
using CIIP.Module.BusinessObjects.SYS;

namespace CIIP.Win {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/DevExpressExpressAppWinWinApplicationMembersTopicAll.aspx
    public partial class CIIPDesignerWindowsFormsApplication : WinApplication,IRestartApplication {
        public CIIPDesignerWindowsFormsApplication() {
            InitializeComponent();
        }

        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
            args.ObjectSpaceProvider =  new XPObjectSpaceProvider(args.ConnectionString, args.Connection, false);
            args.ObjectSpaceProviders.Add(new NonPersistentObjectSpaceProvider(TypesInfo, null));
        }
        
        protected override IFrameTemplate CreateDefaultTemplate(TemplateContext context)
        {
            var ribbon = base.CreateDefaultTemplate(context);
            if (ribbon is IDockManagerHolder)
            {
                var r = ribbon as IDockManagerHolder;
                var rp = r.DockManager.AddPanel(DockingStyle.Right);
                rp.Visibility = DockVisibility.Hidden;
                rp.Text = "搜索";
                rp.Width = 200;
            }
            return ribbon;
        }
        
        private void ERPWindowsFormsApplication_CustomizeLanguagesList(object sender, CustomizeLanguagesListEventArgs e) {
            string userLanguageName = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
            if(userLanguageName != "en-US" && e.Languages.IndexOf(userLanguageName) == -1) {
                e.Languages.Add(userLanguageName);
            }
        }

        public bool IsRestartApplication { get; set; }

        private void ERPWindowsFormsApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e) {
#if EASYTEST
            e.Updater.Update();
            e.Handled = true;
#else
            if (true)
            {
                //System.Diagnostics.Debugger.IsAttached) {
                e.Updater.Update();
                e.Handled = true;
            }
            else
            {
                throw new InvalidOperationException(
                    "The application cannot connect to the specified database, because the latter doesn't exist or its version is older than that of the application.\r\n" +
                    "This error occurred  because the automatic database update was disabled when the application was started without debugging.\r\n" +
                    "目标仓库 avoid this error, you should either start the application under Visual Studio in debug mode, or modify the " +
                    "source code of the 'DatabaseVersionMismatch' event handler to enable automatic database update, " +
                    "or manually create a database using the 'DBUpdater' tool.\r\n" +
                    "Anyway, refer to the 'Update Application and Database Versions' help topic at http://help.devexpress.com/#Xaf/CustomDocument2795 " +
                    "for more detailed information. If this doesn't help, please contact our Support Team at http://www.devexpress.com/Support/Center/");
            }
#endif
        }



        public void RestartApplication()
        {
            IsRestartApplication = true;
            this.Exit();

            //var path = System.Windows.Forms.Application.ExecutablePath;
            //Process.Start(path);

            //this.Exit();

        }
    }
}
