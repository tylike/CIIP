using System.ComponentModel;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using CIIP.Module.BusinessObjects.Flow;
using System.Linq;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using DevExpress.Persistent.BaseImpl;
using 常用基类;
using DevExpress.ExpressApp;
using System.Collections.Generic;
using CIIP.Module.BusinessObjects.SYS.Logic;
using DevExpress.Data.Filtering;
using CIIP;
using System;
using System.Text;

namespace CIIP.Module.BusinessObjects.SYS
{
    public enum Modifier
    {
        [XafDisplayName("普通")]
        None,
        [XafDisplayName("抽象 - 必须被继承")]
        Abstract,
        [XafDisplayName("密封 - 不可以被继承")]
        Sealed
    }
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
            rst.Append(CommonUsing());

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
            #endregion

            rst.Append($"\tpublic ");
            if (this.Modifier != Modifier.None)
            {
                rst.Append($"{ Modifier.ToString().ToLower()} ");
            }
            rst.Append($" partial class { 名称 } ");

            //" { (Modifier == Modifier.Sealed ? "" : "sealed") + (IsAbstract ? "abstract" : "") }");

            #region 本类范型定义
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

            #region 基类 范型处理
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
#warning where 等待制作

                //如果设置了泛型参数的值,则付入,否则认为本类也是泛型类
                rst.AppendFormat("<{0}>",
                    string.Join(",",
                        GenericParameterInstances.Select(
                            x => x.ParameterValue == null ? x.Name : "global::" + x.ParameterValue.FullName).ToArray()));
                //where xxxx : xxxx
                //var constraints = string.Join("\n", GenericParameterInstances.Where(x => !string.IsNullOrEmpty(x.Constraint)).Select(x => " where " + x.Name + " : " + x.Constraint));
                //rst.AppendLine(constraints);
            }
            else
            {
                rst.Append("global::" + Base.FullName);
            }
            #endregion

            rst.AppendLine();

            //begin class
            rst.AppendLine("\t{");

            #region 构造函数
            rst.AppendLine($"\t\tpublic {名称}(Session s):base(s){{  }}");

            #endregion

            #region 属性模板
            var propertyTemplate =
"\t\tpublic {0} {1}\n" +
"\t\t{\n" +
"\t\t\tget { return _{1}; }\n" +
"\t\t\tset { SetPropertyValue(\"{1}\",ref _{1},value); }\n" +
"\t\t}\n";
            #endregion

            #region 属性生成
            foreach (var item in Properties)
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


                rst.Append(propertyTemplate.Replace("{0}", pt).Replace("{1}", item.名称));
            }
            #endregion

            #region 关联集合
            var att = "\t\t[" + typeof(DevExpress.Xpo.AggregatedAttribute).FullName + "]";
            foreach (var item in CollectionProperties)
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
            if (!string.IsNullOrEmpty(property.DataSourceProperty))
            {
                code.AppendFormat("\t\t[{0}(\"{1}\")]\n", typeof(DataSourcePropertyAttribute).FullName, property.DataSourceProperty);
            }
            
            if (property.VisibleInDetailView.HasValue && !property.VisibleInDetailView.Value)
            {
                code.AppendFormat("\t\t[{0}(false)]\n", typeof(VisibleInDetailViewAttribute).FullName);
            }

            if (property.VisibleInListView.HasValue && !property.VisibleInListView.Value)
            {
                code.AppendFormat("\t\t[{0}(false)]\n", typeof(VisibleInListViewAttribute).FullName);
            }

            if (property.Browsable.HasValue && !property.Browsable.Value)
            {
                code.AppendFormat("\t\t[{0}(false)]\n", typeof(BrowsableAttribute).FullName);
            }

            if (property.AllowEdit.HasValue && !property.AllowEdit.Value)
            {
                code.AppendFormat("\t\t[ModelDefault(\"AllowEdit\",\"false\")]\n");
            }

            if (property.ImmediatePostData.HasValue && property.ImmediatePostData.Value)
            {
                code.AppendFormat("\t\t[ImmediatePostData]\n");
            }

            if(!string.IsNullOrEmpty( property.DisplayFormat ))
            {
                code.AppendFormat("\t\t[ModelDefault(\"DisplayFormat\",\"{0}\")]\n",property.DisplayFormat);
            }

            if (!string.IsNullOrEmpty(property.EditMask))
            {
                code.AppendFormat("\t\t[ModelDefault(\"EditMask\",\"{0}\")]\n", property.EditMask);
            }

            if (property.Range != null)
            {
                code.AppendFormat("\t\t[RuleRange({0},{1})]\n", property.Range.Begin, property.Range.End);
            }

            if (property.RuleRequiredField.HasValue && property.RuleRequiredField.Value)
            {
                code.AppendFormat("\t\t[RuleRequiredField]\n");
            }

            if (property.UniqueValue.HasValue && property.UniqueValue.Value)
            {
                code.AppendFormat("\t\t[RuleUniqueValue]\n");
            }
        }
    }
    
    [XafDefaultProperty("Caption")]
    [XafDisplayName("用户业务")]
    [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
    //[ChildrenType(typeof(MethodDefine))]
    public partial class BusinessObject : BusinessObjectBase
    {
        public static string CommonUsing()
        {
            return @"using System;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Xpo;
using System.Linq;
using System.ComponentModel;
using DevExpress.ExpressApp.Model;
using 常用基类;
using 基础信息;
using DevExpress.Persistent.Validation;
";
        }

        #region can custom logic
        private bool _CanCustomLogic;
        [XafDisplayName("可自定义逻辑")]
        [ModelDefault("AllowEdit", "False")]
        public bool CanCustomLogic
        {
            get { return _CanCustomLogic; }
            set { SetPropertyValue("CanCustomLogic", ref _CanCustomLogic, value); }
        }
        #endregion

        #region is persistent
        private bool _IsPersistent;

        [XafDisplayName("可持久化")]
        [ToolTip("即是否在数据库中创建表，可以进行读写，如果不是持久化的，则只做为组合类型时使用.")]
        [VisibleInListView(false)]
        public bool IsPersistent
        {
            get { return _IsPersistent; }
            set { SetPropertyValue("IsPersistent", ref _IsPersistent, value); }
        }
        #endregion
        
        #region modifier
        [XafDisplayName("继承设置")]
        [ToolTip("可以设置为无,抽象,密封的")]
        public Modifier Modifier
        {
            get
            {
                return GetPropertyValue<Modifier>(nameof(Modifier));
            }
            set
            {
                SetPropertyValue(nameof(Modifier), value);
            }
        } 
        #endregion



#warning 考虑使用IsSystem来处理？
        /// <summary>
        /// 用于在自动生成系统类型时，不要自动生成泛型参数
        /// </summary>
        [Browsable(false)]
        [NonPersistent]
        public bool DisableCreateGenericParameterValues;

        #region 继承
        private BusinessObject _Base;

        [XafDisplayName("继承")]
        [RuleRequiredField]
        [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
        public BusinessObject Base
        {
            get { return _Base; }
            set
            {
                SetPropertyValue("Base", ref _Base, value);
            }
        }
        #endregion

        #region 泛型
        [XafDisplayName("范型参数定义")]
        [ToolTip("如果需要类型参数时,可以在此定义,可以在属性及业务逻辑中使用!")]
        [Association, DevExpress.Xpo.Aggregated]
        public XPCollection<GenericParameterDefine> GenericParameterDefines
        {
            get
            {
                return GetCollection<GenericParameterDefine>(nameof(GenericParameterDefines));
            }
        }

        private bool _IsGenericTypeDefine;
        [XafDisplayName("泛型定义")]
        [ToolTip("本类是否是泛型定义")]
        public bool IsGenericTypeDefine
        {
            get { return _IsGenericTypeDefine; }
            set { SetPropertyValue("IsGenericTypeDefine", ref _IsGenericTypeDefine, value); }
        }

        [Association, DevExpress.Xpo.Aggregated]
        [XafDisplayName("传入基类泛型参数")]
        public XPCollection<GenericParameterInstance> GenericParameterInstances
        {
            get { return GetCollection<GenericParameterInstance>(nameof(GenericParameterInstances)); }
        }

        //[Appearance("基类泛型参数可见", TargetItems = "GenericParameters", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        protected bool GenericParameterIsVisible()
        {
            if (Base != null)
            {
                if (Base.IsRuntimeDefine)
                    return true;
                var bt = ReflectionHelper.FindType(Base.FullName);
                if (bt != null)
                {
                    return !bt.IsGenericType;
                }
            }
            return true;
        }
        #endregion
        #region 属性
        [Association, DevExpress.Xpo.Aggregated]
        [XafDisplayName("属性")]
        public XPCollection<Property> Properties
        {
            get { return GetCollection<Property>("Properties"); }
        }

        [Association, DevExpress.Xpo.Aggregated]
        [XafDisplayName("集合属性")]
        public XPCollection<CollectionProperty> CollectionProperties
        {
            get { return GetCollection<CollectionProperty>("CollectionProperties"); }
        }

        public PropertyBase FindProperty(string name)
        {
            var sp = Properties.SingleOrDefault(x => x.名称 == name);
            if (sp != null)
            {
                return sp;
            }
            return CollectionProperties.SingleOrDefault(x => x.名称 == name);
        }

        #region logic method
        [Association, DevExpress.Xpo.Aggregated]
        [XafDisplayName("业务逻辑")]
        public XPCollection<BusinessObjectEvent> Methods
        {
            get
            {
                return GetCollection<BusinessObjectEvent>("Methods");
            }
        } 
        #endregion

        #endregion

        #region option

        private bool? _IsCloneable;

        [XafDisplayName("允许复制")]
        [VisibleInListView(false)]
        public bool? IsCloneable
        {
            get { return _IsCloneable; }
            set { SetPropertyValue("IsCloneable", ref _IsCloneable, value); }
        }

        private bool? _IsVisibileInReports;

        [XafDisplayName("可做报表")]
        [VisibleInListView(false)]
        public bool? IsVisibileInReports
        {
            get { return _IsVisibileInReports; }
            set { SetPropertyValue("IsVisibileInReports", ref _IsVisibileInReports, value); }
        }

        private bool? _IsCreatableItem;

        [XafDisplayName("快速创建")]
        [VisibleInListView(false)]
        public bool? IsCreatableItem
        {
            get { return _IsCreatableItem; }
            set { SetPropertyValue("IsCreatableItem", ref _IsCreatableItem, value); }
        }

        private bool _IsRuntimeDefine;

        [XafDisplayName("动态定义")]
        [ToolTip("为假时是通过代码方式上传的模块生成的。否则是在界面上定义并生成的。")]
        public bool IsRuntimeDefine
        {
            get { return _IsRuntimeDefine; }
            set { SetPropertyValue("IsRuntimeDefine", ref _IsRuntimeDefine, value); }
        }

        [Browsable(false)]
        public int CreateIndex { get; set; }

        #endregion

        #region ctor
        public BusinessObject(Session s) : base(s)
        {

        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading && (propertyName == "名称" || propertyName == "Category"))
            {
                if (this.Category != null)
                    this.FullName = this.Category.FullName + "." + this.名称;
                if (propertyName == "名称" && string.IsNullOrEmpty(Caption))
                {
                    this.Caption = newValue + "";
                }
            }
            if (propertyName == "Base" && !IsLoading && !DisableCreateGenericParameterValues)
            {
                Session.Delete(GenericParameterInstances);
                if (newValue != null)
                {
                    if (!Base.IsRuntimeDefine)
                    {
                        var bt = ReflectionHelper.FindType(Base.FullName);
                        if (bt.IsGenericType)
                        {
                            foreach (var item in bt.GetGenericArguments())
                            {
                                var gp = new GenericParameterInstance(Session);
                                gp.Owner = this;
                                gp.Name = item.Name;
                            }
                        }
                    }
                    else
                    {
                        foreach (var gp in Base.GenericParameterInstances)
                        {
                            var ngp = new GenericParameterInstance(Session);
                            ngp.Name = gp.Name;
                            ngp.ParameterIndex = gp.ParameterIndex;
                            this.GenericParameterInstances.Add(ngp);
                        }
                    }

                }
            }
        }

        public override void AfterConstruction()
        {
            IsRuntimeDefine = true;
            this.IsPersistent = true;
            base.AfterConstruction();
        } 
        #endregion
                
        #region 导航设置
        List<NavigationItem> NavigationItemDataSources
        {
            get
            {
                return ModelDataSource.NavigationItemDataSources;
            }
        }

        private NavigationItem _NavigationItem;
        [ValueConverter(typeof(ModelNavigationToStringConverter))]
        [DataSourceProperty("NavigationItemDataSources")]
        [XafDisplayName("导航")]
        public NavigationItem NavigationItem
        {
            get { return _NavigationItem; }
            set { SetPropertyValue("NavigationItem", ref _NavigationItem, value); }
        }
        #endregion

        //[Association, DevExpress.Xpo.Aggregated]
        //public XPCollection<MethodDefine> Methods
        //{
        //    get
        //    {
        //        return GetCollection<MethodDefine>("Methods");
        //    }
        //}

        

        //private bool _UserDBView;
        //[XafDisplayName("使用DB视图")]
        //public bool UserDBView
        //{
        //    get { return _UserDBView; }
        //    set { SetPropertyValue("UserDBView", ref _UserDBView, value); }
        //}



        //private string _MapToDBView;
        //[Size(-1)]
        //[XafDisplayName("映射到DB视图")]
        //[ToolTip("系统将使用下面的SQL语句创建视图，不要写Create View!")]
        //public string MapToDBView
        //{
        //    get
        //    {

        //        return _MapToDBView;
        //    }
        //    set
        //    {
        //        SetPropertyValue("MapToDBView", ref _MapToDBView, value);
        //    }
        //}
        
        #region quick add
        public Property AddProperty(string name, BusinessObjectBase type, int? size = null)
        {
            var property = new Property(Session);
            property.PropertyType = type;
            property.名称 = name;
            if (size.HasValue)
                property.Size = size.Value;
            this.Properties.Add(property);
            return property;
        }

        public Property AddProperty<T>(string name, int? size = null)
        {
            return AddProperty(name,
                Session.FindObject<BusinessObjectBase>(new BinaryOperator("FullName", typeof (T).FullName))
                , size
                );
        }

        public CollectionProperty AddAssociation(string name, BusinessObject bo, bool isAggregated,Property relation)
        {
            var cp = new CollectionProperty(Session);
            this.CollectionProperties.Add(cp);
            cp.PropertyType = bo;
            cp.名称 = name;
            cp.Aggregated = isAggregated;
            cp.RelationProperty = relation;
            return cp;
        }

        public ObjectAfterConstruction AddAfterConstruction(string code)
        {
            var after = new ObjectAfterConstruction(Session);
            after.SetCode(code);
            Methods.Add(after);
            return after;
        }

        public ObjectSavingEvent AddSavingEvent(string code)
        {
            var after = new ObjectSavingEvent(Session);
            after.SetCode(code);
            Methods.Add(after);
            return after;
        }

        public ObjectDeletingEvent AddDeletingEvent(string code)
        {
            var after = new ObjectDeletingEvent(Session);
            after.SetCode(code);
            Methods.Add(after);
            return after;
        }

        public ObjectPropertyChangedEvent AddPropertyChangedEvent(string code)
        {
            var after = new ObjectPropertyChangedEvent(Session);
            after.SetCode(code);
            Methods.Add(after);
            return after;
        }
        public ObjectSavedEvent AddObjectSavedEvent(string code)
        {
            var after = new ObjectSavedEvent(Session);
            after.SetCode(code);
            Methods.Add(after);
            return after;
        }

        public BusinessObjectPartialLogic AddPartialLogic(string code,string usingBlock = null)
        {
            var logic = new BusinessObjectPartialLogic(Session);
            logic.BusinessObject = this;
            if (usingBlock == null)
            {
                code = CommonUsing() + code;
            }
            logic.Code = new CsharpCode(code, logic);
            return logic;
        }

        public BusinessObjectLayout AddLayout(string code, string usingBlock = null)
        {
            var logic = new BusinessObjectLayout(Session);
            logic.BusinessObject = this;
            if (usingBlock == null)
            {
                code = CommonUsing() + code;
            }
            logic.Code = new CsharpCode(code, logic);
            return logic;
        }

        #endregion

        

#warning 需要验证属性名称不可以重名的情况.
    }


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