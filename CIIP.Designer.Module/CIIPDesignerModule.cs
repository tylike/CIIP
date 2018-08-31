using System;
using System.Text;
using System.Linq;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using CIIP.Module.BusinessObjects.SYS;
using CIIP.Module.Controllers;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Validation;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Validation;
using CIIP.Module.BusinessObjects;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using CIIP;
using CIIP.Persistent.BaseImpl;

namespace CIIP.Module {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppModuleBasetopic.aspx.
    public sealed partial class CIIPDesignerModule : ModuleBase {



        public static bool IsNewVersion { get; set; }
        
        public static bool IsSafeMode { get; set; }

        public CIIPDesignerModule()
        {
            InitializeComponent();

            //if (!DesignMode)
            //{
            //    var configSafeMode = ConfigurationManager.AppSettings["SafeMode"];
            //    //if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null)
            //    if (configSafeMode != "true")
            //    {
            //        var runtimeModule = BusinessBuilder.Instance.Register();

            //        if (runtimeModule != null)
            //            this.RequiredModuleTypes.Add(runtimeModule);

            //    }
            //    else
            //    {
            //        IsSafeMode = true;
            //    }

                

            //    //var types = BusinessBuilder.Instance.RegisteBusinessLogics();
            //    //if (types != null)
            //    //{
            //    //    foreach (var type in types)
            //    //    {
            //    //        if (type != null)
            //    //        {
            //    //            //this.RequiredModuleTypes.Add(type);
            //    //        }
            //    //    }
            //    //}
            //}
            BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
        }

        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            var updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }



        public override void Setup(XafApplication application)
        {
            application.SetupComplete += Application_SetupComplete;
            base.Setup(application);

            Context.Application = application;

            //var r = XafTypesInfo.Instance.FindTypeInfo(typeof (IMatrixBaseObject));
            #region 为了防止由于主键类型的不同而造成的基类数量增多问题，使用泛型传入主键类型
            //使用AutoKeyAttribute进行标记是否为自动生成主键内容
            //var helper = XpoTypesInfoHelper.GetXpoTypeInfoSource().XPDictionary;
            //helper.RegisterValueConverter(......);
            //var sot = helper.GetClassInfo(typeof (SimpleObject<>));
            //var oid = sot.FindMember("Oid");
            //oid.RemoveAttribute(typeof (KeyAttribute));

            //var simpleObject = helper.GetClassInfo(typeof (SimpleObject));
            //var oid = simpleObject.GetMember("Oid");
            //oid.AddAttribute(new KeyAttribute(true));

            //var 业务对象 = helper.GetClassInfo(typeof (业务对象));
            //oid = 业务对象.GetMember("Oid");
            //oid.AddAttribute(new KeyAttribute());

            //var assemblys = AppDomain.CurrentDomain.GetAssemblies().Where(
            //    x => !x.FullName.StartsWith("DevEx") &&
            //         !x.FullName.StartsWith("Microsoft") &&
            //         !x.FullName.StartsWith("System") &&
            //         !x.FullName.StartsWith("mscorlib") &&
            //         !x.FullName.StartsWith("Anonymously") &&
            //         !x.FullName.StartsWith("vshost")
            //    ).SelectMany(x => x.GetTypes().Where(t => typeof (IMatrixBaseObject).IsAssignableFrom(t) && t.GetCustomAttributes(typeof(AutoKeyAttribute),false).Any() ) );

            //var autoKeyTypes = XafTypesInfo.Instance.FindTypeInfo(typeof(IMatrixBaseObject)).Descendants;
            //foreach (var t in autoKeyTypes)
            //{
            //    var auto = (t.FindAttribute<AutoKeyAttribute>() != null);

            //    var member = t.FindMember("Oid");
            //    if (member != null)
            //    {
            //        member.AddAttribute(new KeyAttribute(auto));
            //    }
            //    else
            //    {
            //        throw new UserFriendlyException("没有提供Oid属性做为主键!");
            //    }
            //}

            #endregion

            
            // Manage various aspects of the application UI and behavior at the module level.
        }

        private void Application_SetupComplete(object sender, EventArgs e)
        {

        }

        public override void Setup(ApplicationModulesManager moduleManager)
        {
            base.Setup(moduleManager);
            ValidationRulesRegistrator.RegisterRule(moduleManager, typeof (CustomCodeRule), typeof (IRuleBaseProperties));
        }
        
        public override void ExtendModelInterfaces(ModelInterfaceExtenders extenders)
        {
            base.ExtendModelInterfaces(extenders);
            extenders.Add<IModelListView, IModelListViewSetting>();
            extenders.Add<IModelNavigationItem, IListViewCriteriaNavigationItem>();
        }

        public override void CustomizeTypesInfo(ITypesInfo typesInfo)
        {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);

            

        }
        
    }
}
