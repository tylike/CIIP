using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace CIIP.FormCode
{
    public static class SequenceGeneratorInitializer
    {
        // Fields
        private static XafApplication application;

        // Methods
        private static void application_LoggedOn(object sender, LogonEventArgs e)
        {
            Initialize();
        }

        public static void Initialize()
        {
            Guard.ArgumentNotNull(Application, "Application");
            var provider = Application.ObjectSpaceProvider as XPObjectSpaceProvider;
            Guard.ArgumentNotNull(provider, "provider");
            if (provider.DataLayer == null)
            {
                provider.CreateObjectSpace();
            }

            if (provider.DataLayer is ThreadSafeDataLayer)
            {
                //var connectionStringApplication = Application as IConnectionStringApplication;
                //if (connectionStringApplication != null)
                //{
                //    SequenceGenerator.DefaultDataLayer =
                //        XpoDefault.GetDataLayer(connectionStringApplication.GetConnectionString(),
                //            DevExpress.Xpo.DB.AutoCreateOption.SchemaAlreadyExists);
                //}
                //else
                //{
                //    throw new Exception(Application.GetType().FullName + "没有实现IConnectionStringApplication接口!");
                //}
            }
            else
            {
                SequenceGenerator.DefaultDataLayer = provider.DataLayer;
            }
        }

        public static void InitializeDefaultSolution(IObjectSpace os)
        {
            var enumerable = ReflectionHelper.FindTypeDescendants(ReflectionHelper.FindTypeInfoByName(typeof(I单据编号).FullName));
            var dictionary = os.GetObjects<单据编号方案>(null, true).ToDictionary(p => p.应用单据);
            foreach (ITypeInfo info2 in enumerable)
            {
                if (!info2.IsAbstract && info2.IsPersistent)
                {
                    var key = info2.Type;
                    单据编号方案 i单据编号方案 = null;
                    if (dictionary.ContainsKey(key))
                    {
                        i单据编号方案 = dictionary[key];
                    }
                    else
                    {
                        i单据编号方案 = os.CreateObject<单据编号方案>();
                        i单据编号方案.名称 = key.FullName;
                        i单据编号方案.应用单据 = info2.Type;
                        var item = os.CreateObject<单据编号自动编号规则>();
                        item.格式化字符串 = "00000";
                        i单据编号方案.编号规则.Add(item);
                        dictionary.Add(key, i单据编号方案);
                    }
                }
            }
            os.CommitChanges();
        }

        public static void Register(XafApplication app)
        {
            application = app;
            application.LoggedOn += application_LoggedOn;
        }

        // Properties
        private static XafApplication Application
        {
            get
            {
                return application;
            }
        }
    }
}