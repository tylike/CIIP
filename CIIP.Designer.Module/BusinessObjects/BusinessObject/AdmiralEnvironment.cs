using System;
using System.IO;
using CIIP.Module.BusinessObjects.SYS.Logic;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CIIP
{

    public class AdmiralEnvironment
    {
        static AdmiralEnvironment()
        {
            var appPath = new FileInfo( new Uri( typeof (AdmiralEnvironment).Assembly.CodeBase).AbsolutePath).Directory.FullName;
            // AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            //UserDefineBusinessFile = new FileInfo(appPath + "\\Runtime\\BusinessObject\\RuntimeBusinessObject.dll");
            //if (!UserDefineBusinessFile.Exists)
            //{
            //    if (!UserDefineBusinessFile.Directory.Exists)
            //    {
            //        UserDefineBusinessFile.Directory.Create();
            //    }
            //    UserDefineBusinessTempFile = UserDefineBusinessFile;
            //}
            //else
            //{
            //    UserDefineBusinessTempFile = new FileInfo(appPath + "\\Runtime\\BusinessObject\\RuntimeBusinessObjectTemp.dll");
            //}

            //Runtime = new DirectoryInfo(ApplicationBase + "\\Runtime");
            //UserDefineBusinessDirectoryInfo = new DirectoryInfo(Runtime.FullName + "\\DCD");

            //if (!UserDefineBusinessDirectoryInfo.Exists)
            //{
            //    UserDefineBusinessDirectoryInfo.Create();
            //}

            //if (DomainComponentDefineConfig.Exists)
            //{
            //    var sr = DomainComponentDefineConfig.OpenText();
            //    var fileName = sr.ReadToEnd();
            //    sr.Close();
            //    UserDefineBusinessFile = new FileInfo(fileName);
            //}
            //else
            //{
            //    //还没有生成过dc定义
            //}
            ApplicationPath = appPath;
            ConnectionStringConfigFileInfo = new FileInfo(appPath + "\\connection.cfg");
        }
        
        public static FileInfo ConnectionStringConfigFileInfo { get; }

        public static string ApplicationPath { get; }
        //static string ConnectionStringConfigPath { get; }

        public static string ReadConnectionString()
        {
            ConnectionStringConfigFileInfo.Refresh();
            if (!ConnectionStringConfigFileInfo.Exists)
                return "";
            return File.ReadAllText(ConnectionStringConfigFileInfo.FullName);
        }

        public static void WriteConnectionString(string connectionString)
        {
            File.WriteAllText(ConnectionStringConfigFileInfo.FullName, connectionString);
        }



        

        ///// <summary>
        ///// 应用程序所在路径
        ///// </summary>
        //public static string ApplicationBase { get; private set; }

        ///// <summary>
        ///// 运行时产生的文件位置,如用户定义的业务对象的dll的文件夹
        ///// </summary>
        //public static DirectoryInfo Runtime { get; private set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public static DirectoryInfo UserDefineBusinessDirectoryInfo { get; private set; }

        /// <summary>
        /// 运行时用户定义的业务dll所在中径
        /// </summary>

        
        ///// <summary>
        ///// dc.dll 的名称,保存到这个配置文件中
        ///// </summary>
        //public static FileInfo DomainComponentDefineConfig { get; set; }

        public static bool IsWeb { get; set; }
        public static bool IsWindows { get; set; }
    }
}