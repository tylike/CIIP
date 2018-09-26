using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.Linq;
using System.Text;

namespace CIIP.Designer
{
    [DefaultClassOptions]
    [XafDisplayName("接口定义")]
    public class Interface : BusinessObjectBase
    {
        public Interface(Session s) : base(s)
        {

        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            IsRuntimeDefine = true;
        }

        public override BusinessObjectModifier DomainObjectModifier { get => BusinessObjectModifier.Abstract; set { } }

        public override bool CanCreateAssocication => false;

        [ToolTip("指定一个默认实现类,在制作一个业务类时,选择了此接口,则使用这个默认实现类,避免重复的实现相同的接口.实现了类似于多继承的功能.")]
        [XafDisplayName("默认实现")]
        public BusinessObject DefaultImplement
        {
            get { return GetPropertyValue<BusinessObject>(nameof(DefaultImplement)); }
            set { SetPropertyValue(nameof(DefaultImplement), value); }
        }

        public override SystemCategory SystemCategory => SystemCategory.Interface;

        public override string GetCode()
        {
            var rst = new StringBuilder();
            var True = "True";
            #region using
            rst.Append(BusinessObjectCodeGenerateExtendesion.CommonUsing());

            #endregion

            #region namespace
            if (Category != null)
            {
                rst.AppendLine($@"namespace {Category.FullName}");
                rst.AppendLine("{");
            }


            #endregion

            #region 说明
            rst.AppendLine("\t//Interface:" + Oid);

            #endregion
            
            rst.Append($"\tpublic partial interface { Name } ");
            
            #region 本类泛型定义
            if (GenericParameterDefines.Count > 0)
            {
                //如果设置了泛型参数的值,则付入,否则认为本类也是泛型类
                rst.AppendFormat("<{0}>",
                    string.Join(",",
                        GenericParameterDefines
                            .OrderBy(x => x.ParameterIndex)
                            .Select(x => x.Name)
                            .ToArray()));
            }
            #endregion

            //rst.Append(":");
            
            //where xxxx : xxxx
            //var constraints = string.Join("\n", GenericParameters.Where(x => !string.IsNullOrEmpty(x.Constraint)).Select(x => " where " + x.Name + " : " + x.Constraint));
            //rst.AppendLine(constraints);
            rst.AppendLine();

            //begin class
            rst.AppendLine("\t{");
            
            #region 属性模板
            string propertyTemplate(string type, string name)
            {
                return $@"     {type} {name} {{ get;set;}}";
            }
            #endregion

            #region 属性生成
            var properties = Properties.OfType<Property>();
            foreach (var item in properties)
            {
                var pt = "global::" + item.PropertyType.FullName;
                rst.Append(propertyTemplate(pt, item.Name));
            }
            #endregion

            #region 关联集合
            //var collectionProperties = Properties.OfType<CollectionProperty>();
            //foreach (var item in collectionProperties)
            //{
            //    if (item.Aggregated)
            //    {
            //        rst.Aggregated();
            //    }

            //    ProcessPropertyBase(rst, item);

            //    var pt = "global::" + item.PropertyType.FullName;
            //    rst.AppendLine($"\t\tpublic XPCollection<{pt}> {item.Name}{{ get{{ return GetCollection<{pt}>(\"{item.Name}\"); }} }}");
            //}
            #endregion
            
            #region 结束
            //end class
            rst.AppendLine("\t}");

            if (Category != null)
                rst.AppendLine("}");
            #endregion
            return rst.ToString();
        }

    }
}