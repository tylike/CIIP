using CIIP.ProjectManager;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace CIIP
{

    //用于加载模型信息
    //后续应该将此信息保存到服务端
    //客户端只管加载
    [NavigationItem(@"系统设置")]
    [XafDisplayName("加载模块")]
    [JsonObject(MemberSerialization.OptIn)]
    //[DomainComponent]
    public class ReferenceModule : BaseObject
    {
        public static ReferenceModule[] GetModules(string filePath)
        {
            if (File.Exists(filePath))
            {
                var rst = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<ReferenceModule[]>(rst);
            }
            return null;
        }

        [Association]
        public Project ReferencedProject
        {
            get { return GetPropertyValue<Project>(nameof(ReferencedProject)); }
            set { SetPropertyValue(nameof(ReferencedProject), value); }
        }


        public ReferenceModule(Session session) : base(session)
        {

        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Enable = true;
        }

        /// <summary>
        /// 模块所在路径,将从此处加载
        /// </summary>
        [JsonProperty]
        [XafDisplayName("模块文件")]
        [RuleRequiredField]
        [FileTypeFilter("*.dll", "*.dll", "*.dll")]
        [RuleUniqueValue]
        [Size(2000)]
        [ModelDefault("RowCount", "0")]
        [DevExpress.ExpressApp.Data.Key]
        public string FullName
        {
            get{ return GetPropertyValue<string>(nameof(FullName)); }
            set{ SetPropertyValue(nameof(FullName), value); }
        }

        [JsonProperty]
        [XafDisplayName("模块名称")]
        public string Name
        {
            get { return GetPropertyValue<string>(nameof(Name)); }
            set { SetPropertyValue(nameof(Name), value); }
        }

        [RuleFromBoolProperty(CustomMessageTemplate = "CIIP模块文件并不存在!", UsedProperties = nameof(FullName))]
        [XafDisplayName("模块文件验证")]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public bool IsValidateModuleFile
        {
            get
            {
                return File.Exists(FullName);
            }
        }

        [JsonProperty]
        [XafDisplayName("启用")]
        public bool Enable
        {
            get{ return GetPropertyValue<bool>(nameof(Enable)); }
            set { SetPropertyValue(nameof(Enable), value); }
        }
    }


}