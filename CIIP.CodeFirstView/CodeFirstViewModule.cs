using System;
using System.Text;
using System.Linq;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Xpo;

namespace CIIP.CodeFirstView {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppModuleBasetopic.aspx.
    public sealed partial class CodeFirstViewModule : ModuleBase {
        public CodeFirstViewModule() {
            InitializeComponent();
			BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }

        public override void AddGeneratorUpdaters(ModelNodesGeneratorUpdaters updaters)
        {
            //updaters.Add(new DetailViewLayoutGeneratorUpdater());
            base.AddGeneratorUpdaters(updaters);
            //updaters.Add(new ViewsGeneratorUpdater());

        }

        public override void AddModelNodeUpdaters(IModelNodeUpdaterRegistrator updaterRegistrator)
        {
            base.AddModelNodeUpdaters(updaterRegistrator);
            //updaterRegistrator.AddUpdater(new ViewsGeneratorUpdaterX1());

        }

        public override void Setup(XafApplication application) {
            application.SetupComplete += Application_SetupComplete;
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.
        }

        private void Application_SetupComplete(object sender, EventArgs e)
        {
            //ViewsManager.Initialize(sender as XafApplication);
        }

        public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
			base.CustomizeTypesInfo(typesInfo);
			CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
		}
    }
}
