using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace CIIP.Designer
{

    //用于加载模型信息
    //后续应该将此信息保存到服务端
    //客户端只管加载
    [NavigationItem(@"系统设置")]
    [XafDisplayName("引用文件模块")]
    [JsonObject(MemberSerialization.OptIn)]
    //[DomainComponent]
    public class ReferenceFileModule : ReferenceModuleBase
    {
        public ReferenceFileModule(Session session) : base(session)
        {
        }

        [RuleFromBoolProperty(CustomMessageTemplate = "模块文件并不存在!", UsedProperties = nameof(FullName))]
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

    }

    [XafDisplayName("引用模块")]
    [JsonObject(MemberSerialization.OptIn)]
    public class ReferenceModule : ReferenceModuleBase
    {
        public ReferenceModule(Session session) : base(session)
        {
        }
        public override string Name { get => Module.Name; set { } }
        public BusinessModule Module
        {
            get { return GetPropertyValue<BusinessModule>(nameof(Module)); }
            set { SetPropertyValue(nameof(Module), value); }
        }

        public override string FullName
        {
            get
            {
                return Module.OutputPath;
            }
            set { }
        }

    }

    [JsonObject(MemberSerialization.OptIn)]
    public abstract class ReferenceModuleBase:BaseObject
    {
        public static ReferenceFileModule[] GetModules(string filePath)
        {
            if (File.Exists(filePath))
            {
                var rst = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<ReferenceFileModule[]>(rst);
            }
            return null;
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
        public virtual string FullName
        {
            get { return GetPropertyValue<string>(nameof(FullName)); }
            set { SetPropertyValue(nameof(FullName), value); }
        }

        [Association]
        public Project ReferencedProject
        {
            get { return GetPropertyValue<Project>(nameof(ReferencedProject)); }
            set { SetPropertyValue(nameof(ReferencedProject), value); }
        }


        public ReferenceModuleBase(Session session) : base(session)
        {

        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Enable = true;
        }

        

        [JsonProperty]
        [XafDisplayName("模块名称")]
        public virtual string Name
        {
            get { return GetPropertyValue<string>(nameof(Name)); }
            set { SetPropertyValue(nameof(Name), value); }
        }



        [JsonProperty]
        [XafDisplayName("启用")]
        public bool Enable
        {
            get { return GetPropertyValue<bool>(nameof(Enable)); }
            set { SetPropertyValue(nameof(Enable), value); }
        }
    }

    
}