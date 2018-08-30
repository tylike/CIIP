using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.Linq;
using System;
using System.Text;

namespace CIIP.Module.BusinessObjects.SYS
{
    public partial class BusinessObject : IDocumentProvider
    {
        public Guid GetDocumentGuid()
        {
            return this.Oid;
        }

        public string GetFileName()
        {
            return this.FullName;
        }

        public string GetCode()
        {
            var rst = new StringBuilder();

            #region using
            rst.Append(BusinessObjectCodeGenerateExtendesion.CommonUsing());

            #endregion


            #region namespace
            rst.AppendLine($@"namespace {Category.FullName}
{{");
            #endregion

            #region 说明
            rst.AppendLine("\t//BO:" + Oid);

            #endregion

            #region attributes
            if (!IsPersistent)
            {
                rst.AppendLine("\t[NonPersistent]");
            }

            if (IsCloneable.HasValue && IsCloneable.Value)
            {
                rst.AppendLine("\t[ModelDefault(\"Cloneable\",\"True\")]");
            }

            if (IsCreatableItem.HasValue && IsCreatableItem.Value)
            {
                rst.AppendLine("\t[ModelDefault(\"Createable\",\"True\")]");
            }

            if (IsVisibileInReports.HasValue && IsVisibileInReports.Value)
            {
                rst.AppendLine("\t[VisibleInReport]");

            }
            rst.AppendLine("\t[NavigationItem]");

            #endregion

            rst.Append($"\tpublic ");
            if (this.Modifier != Modifier.None)
            {
                rst.Append($"{ Modifier.ToString().ToLower()} ");
            }
            rst.Append($" partial class { 名称 } ");

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

            var imp = Implements.FirstOrDefault(x => x.ImplementBusinessObject is BusinessObject);
            if (imp != null)
            {
                var impBase = imp.ImplementBusinessObject;
                if (impBase.IsGenericTypeDefine)
                {
                    var n = impBase.FullName;
                    if (impBase.IsRuntimeDefine)
                    {
                        rst.Append("global::" + n);
                    }
                    else
                    {
                        rst.Append("global::" + n.Substring(0, n.Length - 2));
                    }
                    
                    //传入参数
                    rst.AppendFormat("<{0}>",string.Join(",",imp.GenericParameters.Select(x => x.ParameterValue == null ? x.Name : "global::" + x.ParameterValue.FullName).ToArray()));

                }
                else
                {
                    rst.Append("global::" + impBase.FullName);
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
            rst.AppendLine($"\t\tpublic {名称}(Session s):base(s){{  }}");

            #endregion

            #region 属性模板
            string propertyTemplate(string type,string name)
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
                rst.AppendLine($"\t\t{ pt } _{ item.名称 };");

                if (item.Size != 100 && item.Size != 0)
                {
                    rst.AppendLine($"\t\t[Size({item.Size})]");
                }
                ProcessPropertyBase(rst, item);
                if (item.RelationProperty != null)
                {
                    var assName = string.Format("{0}_{1}", item.RelationProperty.名称, item.名称);
                    rst.AppendFormat("\t\t[{0}(\"{1}\")]", typeof(AssociationAttribute).FullName, assName);
                }


                rst.Append(propertyTemplate(pt, item.名称));
            }
            #endregion

            #region 关联集合
            var collectionProperties = Properties.OfType<CollectionProperty>();
            var att = "\t\t[" + typeof(AggregatedAttribute).FullName + "]";
            foreach (var item in collectionProperties)
            {
                if (item.Aggregated)
                {
                    rst.AppendLine(att);
                }
                ProcessPropertyBase(rst, item);

                var assName = string.Format("{0}_{1}", item.名称, item.RelationProperty.名称);
                rst.AppendFormat("\t\t[{0}(\"{1}\")]\n", typeof(AssociationAttribute).FullName, assName);
                var pt = "global::" + item.PropertyType.FullName;
                rst.AppendLine($"\t\tpublic XPCollection<{pt}> {item.名称}{{ get{{ return GetCollection<{pt}>(\"{item.名称}\"); }} }}");
            }
            #endregion

            #region 业务逻辑处理
            foreach (var method in Methods)
            {
                rst.AppendLine(method.MethodDefineCode);
            }
            #endregion

            #region 结束
            //end class
            rst.AppendLine("\t}");
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

            if (property.Browsable.HasValue && !property.Browsable.Value)
            {
                code.AppendFormat("\t\t[{0}(false)]\n", typeof(BrowsableAttribute).FullName);
            }

            //if (property.AllowEdit.HasValue && !property.AllowEdit.Value)
            //{
            //    code.AppendFormat("\t\t[ModelDefault(\"AllowEdit\",\"false\")]\n");
            //}

            //if (property.ImmediatePostData.HasValue && property.ImmediatePostData.Value)
            //{
            //    code.AppendFormat("\t\t[ImmediatePostData]\n");
            //}

            //if (!string.IsNullOrEmpty(property.DisplayFormat))
            //{
            //    code.AppendFormat("\t\t[ModelDefault(\"DisplayFormat\",\"{0}\")]\n", property.DisplayFormat);
            //}

            //if (!string.IsNullOrEmpty(property.EditMask))
            //{
            //    code.AppendFormat("\t\t[ModelDefault(\"EditMask\",\"{0}\")]\n", property.EditMask);
            //}

            //if (property.Range != null)
            //{
            //    code.AppendFormat("\t\t[RuleRange({0},{1})]\n", property.Range.Begin, property.Range.End);
            //}

            //if (property.RuleRequiredField.HasValue && property.RuleRequiredField.Value)
            //{
            //    code.AppendFormat("\t\t[RuleRequiredField]\n");
            //}

            //if (property.UniqueValue.HasValue && property.UniqueValue.Value)
            //{
            //    code.AppendFormat("\t\t[RuleUniqueValue]\n");
            //}
        }
    }

#warning 需要验证属性名称不可以重名的情况.


#warning 此功能可以后续实现,当前可以使用复制功能直接copy已有布局
    // 业务类型上面,使用Attribute指定使用哪个布局模板
    // 系统起动时,检查所有使用了Attribute的类,遍历并进行更新

    //[LayoutTemplate(typeof(布局模板)] 
    //泛型参数类型应该是: 某单据,单据明细 两个类型.
    //这种情况,只支持两种类型,如果基类中有多个类型,就按顺序传入,反射取得,无需处理.
    //public class 某单据 :  ......
    //{
    //}
}