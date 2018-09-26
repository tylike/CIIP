using System.ComponentModel;
using DevExpress.Persistent.Base;
using System.Linq;
using System;
using System.Text;

namespace CIIP.Designer
{
    public partial class BusinessObject 
    {
        public Guid GetDocumentGuid()
        {
            return this.Oid;
        }

        public string GetFileName()
        {
            return this.FullName;
        }

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
            rst.AppendLine("\t//BO:" + Oid);

            #endregion

            #region attributes
            if (!IsPersistent)
            {
                rst.AppendLine("\t[NonPersistent]");
            }

            if (IsCloneable)
            {
                rst.ModelDefault("Cloneable", True);
            }

            if (IsCreatableItem)
            {
                rst.ModelDefault("Createable", True);
            }

            if (IsVisibileInReports)
            {
                rst.AppendLine("\t[VisibleInReport]");
            }

            rst.AppendLine("\t[NavigationItem]");

            if (Caption != Name)
            {
                rst.ModelDefault(nameof(Caption), Caption);
            }
            #endregion

            rst.Append($"\tpublic ");
            if (this.DomainObjectModifier != BusinessObjectModifier.None)
            {
                rst.Append($"{ DomainObjectModifier.ToString().ToLower()} ");
            }
            rst.Append($" partial class { Name } ");

            //" { (Modifier == Modifier.Sealed ? "" : "sealed") + (IsAbstract ? "abstract" : "") }");

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

            rst.Append(":");

            #region 基类 泛型处理

            if (Base != null)
            {
                if (Base.IsGenericTypeDefine)
                {
                    var n = Base.FullName;
                    if (Base.IsRuntimeDefine)
                    {
                        rst.Append("global::" + n);
                    }
                    else
                    {
                        rst.Append("global::" + n.Substring(0, n.Length - 2));
                    }

                    //传入参数
                    #warning 需要实现范型类型的基类实现.
                    //rst.AppendFormat("<{0}>", string.Join(",", Base.GenericParameters.Select(x => x.ParameterValue == null ? x.Name : "global::" + x.ParameterValue.FullName).ToArray()));

                }
                else
                {
                    rst.Append("global::" + Base.FullName);
                }
            }

            #endregion

            //where xxxx : xxxx
            //var constraints = string.Join("\n", GenericParameters.Where(x => !string.IsNullOrEmpty(x.Constraint)).Select(x => " where " + x.Name + " : " + x.Constraint));
            //rst.AppendLine(constraints);
            rst.AppendLine();

            //begin class
            rst.AppendLine("\t{");

            #region 构造函数
            rst.AppendLine($"\t\tpublic {Name}(Session s):base(s){{  }}");

            #endregion

            #region 属性模板
            string propertyTemplate(string type, string name)
            {
                return
$@"     public {type} {name}
{{
    get {{ return GetPropertyValue<{type}>(nameof({name})); }}
    set {{ SetPropertyValue(nameof({name}),value); }}
}}";
            }
            #endregion

            #region 属性生成
            var properties = Properties.OfType<Property>();
            foreach (var item in properties)
            {
                var pt = "global::" + item.PropertyType.FullName;
                rst.AppendLine($"\t\t{ pt } _{ item.Name };");

                if (item.Size != 100 && item.Size != 0)
                {
                    rst.AppendLine($"\t\t[Size({item.Size})]");
                }

                if (item.Caption != item.Name)
                {
                    rst.ModelDefault("Caption", item.Caption);
                }

                ProcessPropertyBase(rst, item);

                if (item is Property property)
                {
                    if (property.ImmediatePostData)
                    {
                        rst.AppendFormat("\t\t[ImmediatePostData]\n");
                    }

                    if (!string.IsNullOrEmpty(property.DisplayFormat))
                    {
                        rst.ModelDefault("DisplayFormat", property.DisplayFormat);
                    }

                    if (!string.IsNullOrEmpty(property.EditMask))
                    {
                        rst.AppendFormat("EditMask", property.EditMask);
                    }

                    if (property.Range != null)
                    {
                        rst.AppendFormat("\t\t[RuleRange({0},{1})]\n", property.Range.Begin, property.Range.End);
                    }

                    if (property.RuleRequiredField)
                    {
                        rst.AppendFormat("\t\t[RuleRequiredField]\n");
                    }

                    if (property.UniqueValue)
                    {
                        rst.AppendFormat("\t\t[RuleUniqueValue]\n");
                    }
                }



                rst.Append(propertyTemplate(pt, item.Name));
            }

            //为接口生成默认实现
            //1.当前类是接口的默认实现,正准备生成代码.接口.默认实现==this

            //2.当前类要实现接口,使用某个默认实现.
            foreach (var item in ImplementInterfaces)
            {
                
            }
            #endregion

            #region 关联集合
            var collectionProperties = Properties.OfType<CollectionProperty>();
            foreach (var item in collectionProperties)
            {
                if (item.Aggregated)
                {
                    rst.Aggregated();
                }

                ProcessPropertyBase(rst, item);

                var pt = "global::" + item.PropertyType.FullName;
                rst.AppendLine($"\t\tpublic XPCollection<{pt}> {item.Name}{{ get{{ return GetCollection<{pt}>(\"{item.Name}\"); }} }}");
            }
            #endregion

            //#region 业务逻辑处理
            //foreach (var method in Methods)
            //{
            //    rst.AppendLine(method.MethodDefineCode);
            //}
            //#endregion

            #region 结束
            //end class
            rst.AppendLine("\t}");

            if (Category != null)
                rst.AppendLine("}");
            #endregion
            return rst.ToString();
        }

        public void ProcessPropertyBase(StringBuilder code, PropertyBase property)
        {
            //if (!string.IsNullOrEmpty(property.DataSourceProperty))
            //{
            //    code.AppendFormat("\t\t[{0}(\"{1}\")]\n", typeof(DataSourcePropertyAttribute).FullName, property.DataSourceProperty);
            //}

            //if (property.VisibleInDetailView.HasValue && !property.VisibleInDetailView.Value)
            //{
            //    code.AppendFormat("\t\t[{0}(false)]\n", typeof(VisibleInDetailViewAttribute).FullName);
            //}

            //if (property.VisibleInListView.HasValue && !property.VisibleInListView.Value)
            //{
            //    code.AppendFormat("\t\t[{0}(false)]\n", typeof(VisibleInListViewAttribute).FullName);
            //}

            if (!property.Browsable)
            {
                code.AppendFormat("\t\t[{0}(false)]\n", typeof(BrowsableAttribute).FullName);
            }

            if (!property.AllowEdit)
            {
                code.ModelDefault(nameof(property.AllowEdit), "False");
            }

            if (property.IsAssocication)
            {
                if (property.AssocicationInfo != null)
                    code.Assocication(property.AssocicationInfo.Name);
                else
                    code.Assocication();
            }

        }
    }
}