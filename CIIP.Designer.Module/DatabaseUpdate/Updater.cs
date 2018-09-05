using System;
using System.Diagnostics;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using System.Drawing;
using DevExpress.ExpressApp.Utils;
using System.Collections.Generic;
using CIIP.Module.BusinessObjects;
using CIIP.Module.BusinessObjects.SYS;
using CIIP.ProjectManager;
using CIIP.Module.Controllers;

namespace CIIP.Module.DatabaseUpdate {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppUpdatingModuleUpdatertopic.aspx
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion)
        {
        }
        List<SystemModule> systemModules;
        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();

            CreateSimpleType();
            systemModules = ObjectSpace.GetObjectsQuery<SystemModule>().ToList();
            CreateSystemModule("报表模块", true, false, false,
                @"C:\Program Files (x86)\DevExpress 18.2\Components\Bin\Framework\DevExpress.ExpressApp.ReportsV2.v18.2.dll",
                @"C:\Program Files (x86)\DevExpress 18.2\Components\Bin\Framework\DevExpress.ExpressApp.ReportsV2.Win.v18.2.dll"
            );
            ObjectSpace.CommitChanges();
            //var first = ObjectSpace.GetObjectsQuery<Project>().FirstOrDefault();
            //if(first == null)
            //{
            //    first = ObjectSpace.CreateObject<Project>();
            //    first.Name = "DefaultProject";
            //    first.ProjectPath = Project.ApplicationStartupPath + "\\" + first.Name;
            //}

            //ObjectSpace.CommitChanges();
            DataInitializeWindowController.CreateSystemTypes(ObjectSpace, false);

            CIIPDesignerModule.IsNewVersion = true;
        }

        private void CreateSystemModule(string name, bool win, bool web, bool mobile, params string[] file)
        {
            var obj = systemModules.SingleOrDefault(x => x.Name == name);
            if(obj == null)
            {
                obj = ObjectSpace.CreateObject<SystemModule>();
                obj.Name = name;
                obj.Web = web;
                obj.Win = win;
                obj.Mobile = mobile;

                foreach (var item in file)
                {
                    var f = ObjectSpace.CreateObject<SystemModuleFile>();
                    f.File = item;
                    obj.Files.Add(f);
                }
            }
        }

        private void CreateSimpleType()
        {
            var nameSpace = ObjectSpace.FindObject<Namespace>(new BinaryOperator("FullName", "系统类型"));
            if (nameSpace == null)
            {
                nameSpace = ObjectSpace.CreateObject<Namespace>();
                nameSpace.Name = "系统类型";
                var createIndex = ObjectSpace.GetObjectsCount(typeof(BusinessObject), null);

                var strType = AddSimpleObject(typeof(string), "字符串", nameSpace, "一串文本内容");

                var dec = AddSimpleObject(typeof(decimal), "数字(decimal)", nameSpace, "通常用于记录带小数点的数字,比如表示货币金额时");
                var intt = AddSimpleObject(typeof(int), "整数(int)", nameSpace, "没有小数的数字");
                var datetimeType = AddSimpleObject(typeof(DateTime), "日期时间", nameSpace, "用于表示日期时间");
                var boolType = AddSimpleObject(typeof(bool), "布尔", nameSpace, "用于表示是/否,真/假,等2种可选值的情况");
                var imageType = AddSimpleObject(typeof(Image), "图片", nameSpace, "用于表示图像信息");
                var colorType = AddSimpleObject(typeof(Color), "颜色", nameSpace, "用于表示颜色信息");
                AddSimpleObject(typeof(sbyte), "整数(sbyte 8位有符号)", nameSpace, "8位有符号整数");
                AddSimpleObject(typeof(long), "整数(long 64位有符号)", nameSpace, "");
                AddSimpleObject(typeof(byte), "整数(byte 8位无符号)", nameSpace, "");
                AddSimpleObject(typeof(ushort), "整数(ushort 16位无符号)", nameSpace, "");
                AddSimpleObject(typeof(uint), "整数(uint 32位无符号)", nameSpace, "");
                AddSimpleObject(typeof(ulong), "整数(ulong 64位无符号)", nameSpace, "");
                AddSimpleObject(typeof(float), "数字(float 单精度浮点)", nameSpace, "");
                AddSimpleObject(typeof(double), "数字(double 双精度浮点)", nameSpace, "");
                AddSimpleObject(typeof(TimeSpan), "时间段(TimeSpan)", nameSpace, "");
                AddSimpleObject(typeof(Guid), "Guid", nameSpace, "全球唯一标识");
            }
        }

        public SimpleType AddSimpleObject(Type type, string caption, Namespace nameSpace, string description)
        {
            var t = ObjectSpace.FindObject<SimpleType>(new BinaryOperator("FullName", type.FullName));
            if (t == null)
            {
                t = ObjectSpace.CreateObject<SimpleType>();
                t.Name = type.Name;
                t.Caption = caption;
                t.Category = nameSpace;
                t.Description = description;
               
                t.FullName = type.FullName;
            }
            return t;
        }
    }
}
