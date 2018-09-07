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
using CIIP.Module.Controllers;
using CIIP.Designer;

namespace CIIP.Module.DatabaseUpdate {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppUpdatingModuleUpdatertopic.aspx
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion)
        {
        }
        
        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();

            CreateSimpleType();
            
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
        
        private void CreateSimpleType()
        {
            var nameSpace = ObjectSpace.FindObject<Namespace>(new BinaryOperator("FullName", "系统类型"));
            if (nameSpace == null)
            {
                nameSpace = ObjectSpace.CreateObject<Namespace>();
                nameSpace.Name = "系统类型";
                var createIndex = ObjectSpace.GetObjectsCount(typeof(BusinessObject), null);

                var strType = AddSimpleObject(typeof(string), "string-字符串", nameSpace, "一串文本内容");

                var dec = AddSimpleObject(typeof(decimal), "decimal-数字", nameSpace, "通常用于记录带小数点的数字,比如表示货币金额时");
                var intt = AddSimpleObject(typeof(int), "int-整数", nameSpace, "没有小数的数字");
                var datetimeType = AddSimpleObject(typeof(DateTime), "datetime-日期时间", nameSpace, "用于表示日期时间");
                var boolType = AddSimpleObject(typeof(bool), "bool-布尔", nameSpace, "用于表示是/否,真/假,等2种可选值的情况");
                var imageType = AddSimpleObject(typeof(Image), "image-图片", nameSpace, "用于表示图像信息");
                var colorType = AddSimpleObject(typeof(Color), "color-颜色", nameSpace, "用于表示颜色信息");
                AddSimpleObject(typeof(sbyte), "sbyte-整数(8位有符号)", nameSpace, "8位有符号整数");
                AddSimpleObject(typeof(long), "long-整数(64位有符号)", nameSpace, "");
                AddSimpleObject(typeof(byte), "byte-整数(8位无符号)", nameSpace, "");
                AddSimpleObject(typeof(ushort), "ushort-整数(16位无符号)", nameSpace, "");
                AddSimpleObject(typeof(uint), "uint-整数(32位无符号)", nameSpace, "");
                AddSimpleObject(typeof(ulong), "ulong-整数(64位无符号)", nameSpace, "");
                AddSimpleObject(typeof(float), "float-数字(单精度浮点)", nameSpace, "");
                AddSimpleObject(typeof(double), "double-数字(双精度浮点)", nameSpace, "");
                AddSimpleObject(typeof(TimeSpan), "timespan-时间段", nameSpace, "");
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
