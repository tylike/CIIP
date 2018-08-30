using System.Diagnostics;
using System.IO;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System.Linq;
using System.Reflection;
using DevExpress.Persistent.Validation;

namespace CIIP.Module.BusinessObjects.SYS
{
    [XafDisplayName("业务模块")]
    public class BusinessModule : BaseObject
    {
        public BusinessModule(Session s) : base(s)
        {

        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            this.MobilePlat = true;
            this.WindowsPlat = true;
            this.WebPlat = true;
            this.PadPlat = true;
        }

        private string _ModuleName;
        [XafDisplayName("模块名称")]
        public string ModuleName
        {
            get { return _ModuleName; }
            set { SetPropertyValue("ModuleName", ref _ModuleName, value); }
        }

        private FileData _File;
        [XafDisplayName("模块文件")]
        [RuleRequiredField]
        public FileData File
        {
            get { return _File; }
            set
            {
                SetPropertyValue("File", ref _File, value);
            }
        }

        private string _Description;
        [Size(-1)]
        [XafDisplayName("说明")]
        public string Description
        {
            get { return _Description; }
            set { SetPropertyValue("Description", ref _Description, value); }
        }

        private string _Version;
        [ModelDefault("AllowEdit", "False")]
        [XafDisplayName("版本")]
        public string Version
        {
            get { return _Version; }
            set { SetPropertyValue("Version", ref _Version, value); }
        }

        //private string _FileVersion;
        //[XafDisplayName("文件版本")]
        //public string FileVersion
        //{
        //    get { return _FileVersion; }
        //    set { SetPropertyValue("FileVersion", ref _FileVersion, value); }
        //}



        private bool _WindowsPlat;
        [XafDisplayName("Windows平台")]
        [ToolTip("Windows桌面应用程序")]
        public bool WindowsPlat
        {
            get { return _WindowsPlat; }
            set { SetPropertyValue("WindowsPlat", ref _WindowsPlat, value); }
        }

        private bool _WebPlat;
        [XafDisplayName("Web平台")]
        [ToolTip("适用于B/S应用，基于浏览器的Web应用，可以在各种PC操作系统中使用。")]
        public bool WebPlat
        {
            get { return _WebPlat; }
            set { SetPropertyValue("WebPlat", ref _WebPlat, value); }
        }

        private bool _PadPlat;
        [XafDisplayName("Pad平台")]
        [ToolTip("新版本的B/S应用，基于浏览器的Web应用，可以在各种PC操作系统和Pad、手机中使用。")]
        public bool PadPlat
        {
            get { return _PadPlat; }
            set { SetPropertyValue("PadPlat", ref _PadPlat, value); }
        }

        private bool _MobilePlat;
        [XafDisplayName("移动平台")]
        [ToolTip("手机App移动平台")]
        public bool MobilePlat
        {
            get { return _MobilePlat; }
            set { SetPropertyValue("MobilePlat", ref _MobilePlat, value); }
        }

        private bool _Disable;
        [XafDisplayName("禁用")]
        public bool Disable
        {
            get { return _Disable; }
            set { SetPropertyValue("Disable", ref _Disable, value); }
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (!IsDeleted && this.File != null && !this.File.IsEmpty)
            {
                var memoryStream = new MemoryStream();
                File.SaveToStream(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                var asm = Mono.Cecil.AssemblyDefinition.ReadAssembly(memoryStream);

                this.Version = asm.Name.Version.ToString();
                this.ModuleName = File.FileName;
                this.Description =
                    asm.CustomAttributes.FirstOrDefault(
                        x => x.AttributeType.FullName == typeof(AssemblyDescriptionAttribute).FullName)?
                        .ConstructorArguments.FirstOrDefault()
                        .Value.ToString();
                //asm.MainModule.Types..Name;
            }
        }
    }
}