using System.Diagnostics;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System.ComponentModel;
using System;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Editors;
using System.Collections.Generic;
using System.Text;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.ConditionalAppearance;
using CIIP.StateMachine;
using CIIP;
using CIIP.Module.BusinessObjects.SYS;

namespace CIIP.Module.BusinessObjects.Flow
{
    [XafDisplayName("转换动作")]
    [Appearance("FlowAction.明细生效条件何时可见", AppearanceItemType = "ViewItem", Enabled = false, TargetItems = "明细生效条件", Criteria = "MultiGenerate!='允许多次生判断条件'")]
    [Appearance("FlowAction.触发动作条件何时可见", AppearanceItemType = "ViewItem", Enabled = false, TargetItems = "触发动作条件", Criteria = "动作类型!='单据满足条件时执行'")]
    [Appearance("FlowAction.按钮外观控制可见", AppearanceItemType = "ViewItem", Enabled = false, TargetItems = "序号;分组标记", Criteria = "动作类型!='点击按钮执行'")]
    [XafDefaultProperty("Title")]
    public class FlowAction : BaseObject,IFlowAction,IDocumentProvider
    {
        public FlowAction(Session s)
            : base(s)
        {

        }

        private MasterConverter _MasterConverter;
        public MasterConverter MasterConverter
        {
            get { return _MasterConverter; }
            set { SetPropertyValue("MasterConverter", ref _MasterConverter, value); }
        }

        private ChildrenConverter _ChildrenConverter;
        public ChildrenConverter ChildrenConverter
        {
            get { return _ChildrenConverter; }
            set { SetPropertyValue("ChildrenConverter", ref _ChildrenConverter, value); }
        }


        public void Created()
        {
            var models = CaptionHelper.ApplicationModel.BOModel;
            var f = models.GetClass(ReflectionHelper.FindType(From.Form));// (From as FlowChartFormNode).ModelClass;
            var t = models.GetClass(ReflectionHelper.FindType(To.Form));// (To as FlowChartFormNode).ModelClass;
            var sb = new StringBuilder();

            GenerateForm(f, t, sb, "来源主单", "目标主单");

            MasterConverter = new MasterConverter(Session);
            MasterConverter.FlowAction = this;
            MasterConverter.名称 = f.Caption + "TO" + t.Caption;
            MasterConverter.CodeName = MasterConverter.名称;
            MasterConverter.SetCode(sb.ToString());

            sb.Clear();

            
            f = models.GetClass(ItemsType);
            t = models.GetClass(ToItemsType);
            GenerateForm(f, t, sb, "来源明细", "item");
            ChildrenConverter = new ChildrenConverter(Session);
            ChildrenConverter.FlowAction = this;
            ChildrenConverter.名称 = f.Caption + "TO" + t.Caption;
            ChildrenConverter.CodeName = ChildrenConverter.名称;
            ChildrenConverter.SetCode(sb.ToString());
            //SetCode(code);
        }

        [XafDisplayName("描述")]
        public string Title
        {
            get { return Caption + " " + this.动作类型; }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            this.显示编辑界面 = true;
        }

        private Flow _Flow;

        [Association]
        public Flow Flow
        {
            get { return _Flow; }
            set { SetPropertyValue("Flow", ref _Flow, value); }
        }

        #region template
        const string codeTemplate = @"
namespace Flow
{
    public class {流程名称} : CIIP.Module.BusinessObjects.Flow.ExecuteFlowAction<{目标主单类型},{来源主单类型},{目标明细类型},{来源明细类型}>
    {
        
        public override void ExecuteMaster({来源主单类型} {来源主单参数名称},{目标主单类型} {目标主单参数名称})
        {
            {主单转换占位符}
            {主单转换代码}
        }

        public override void ExecuteChildren({来源明细类型} {来源明细参数名称},{目标明细类型} {目标明细参数名称})
        {
            {明细转换占位符}
            {明细转换代码}
        }

    }
}
";
        #endregion
        /// <summary>
        /// 流程逻辑名称
        /// </summary>
        public string GetFlowLogicClassName()
        {
             return this.Flow.名称 + "_" + this.Caption; 
        }

        public string GetFlowLogicClassFullName()
        {
            return "Flow." + GetFlowLogicClassName();
        }
        
        public string GetCode()
        {
            var code = BusinessObject.CommonUsing() + codeTemplate;

            code = code.Replace("{流程名称}", GetFlowLogicClassName());

            code = code.Replace("{主单转换占位符}", this.MasterConverter.DefaultLocation);
            code = code.Replace("{主单转换代码}", this.MasterConverter.Code.Code);

            code = code.Replace("{明细转换占位符}", this.ChildrenConverter.DefaultLocation);
            code = code.Replace("{明细转换代码}", this.ChildrenConverter.Code.Code);

            code = code.Replace("{来源主单类型}", this.From.Form);
            code = code.Replace("{来源主单参数名称}", "来源主单");

            code = code.Replace("{目标主单类型}", this.To.Form);
            code = code.Replace("{目标主单参数名称}", "目标主单");


            code = code.Replace("{来源明细类型}", this.ItemsType.FullName);
            code = code.Replace("{来源明细参数名称}", "来源明细");

            code = code.Replace("{目标明细类型}", this.ToItemsType.FullName);
            code = code.Replace("{目标明细参数名称}", "item");
            return code;
        }

        private string _Caption;

        [XafDisplayName("标题")]
        [RuleRequiredField]
        public string Caption
        {
            get { return _Caption; }
            set { SetPropertyValue("Caption", ref _Caption, value); }
        }
        private FlowNode _From;

        [XafDisplayName("来源单据")]
        [ModelDefault("AllowEdit", "False")]
        [RuleRequiredField]
        public FlowNode From
        {
            get { return _From; }
            set { SetPropertyValue("From", ref _From, value); }
        }

        private FlowNode _To;

        [XafDisplayName("目标单据")]
        [ModelDefault("AllowEdit", "False")]
        [RuleRequiredField]
        public FlowNode To
        {
            get { return _To; }
            set { SetPropertyValue("To", ref _To, value); }
        }

        #region types
        [Browsable(false)]
        public Type Type
        {
            get { return ReflectionHelper.FindType(From.Form); }
        }

        [Browsable(false)]
        public Type ItemsType
        {
            get
            {
                return CaptionHelper.ApplicationModel.BOModel.GetClass(Type).FindMember("明细项目").MemberInfo.ListElementType;
            }
        }

        [Browsable(false)]
        public Type ToItemsType
        {
            get
            {
                return CaptionHelper.ApplicationModel.BOModel.GetClass(ToType).FindMember("明细项目").MemberInfo.ListElementType;
            }
        }
       

        [Browsable(false)]
        public Type ToType
        {
            get { return ReflectionHelper.FindType(To.Form); }
        }
        #endregion

        private string _生效条件;
        [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor), CriteriaOptions("Type")]
        [ToolTip("满足生效条件，下推单据的按钮才可以用")]
        public string 生效条件
        {
            get { return _生效条件; }
            set { SetPropertyValue("生效条件", ref _生效条件, value); }
        }

        private string _明细生效条件;
        [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor), CriteriaOptions("ItemsType")]
        [ToolTip("当明细条目中符和条件的记录数大于0时，才可以下推")]
        [RuleRequiredField(TargetCriteria = "MultiGenerate='允许多次生判断条件'")]
        public string 明细生效条件
        {
            get { return _明细生效条件; }
            set { SetPropertyValue("明细生效条件", ref _明细生效条件, value); }
        }

        //[XafDisplayName("单据映射")]
        //[Association, DevExpress.Xpo.Aggregated]
        //public XPCollection<MasterPropertyMapping> MasterMapping
        //{
        //    get { return GetCollection<MasterPropertyMapping>("MasterMapping"); }
        //}

        //[XafDisplayName("明细映射")]
        //[Association, DevExpress.Xpo.Aggregated]
        //public XPCollection<ItemsPropertyMapping> ItemsMapping
        //{
        //    get { return GetCollection<ItemsPropertyMapping>("ItemsMapping"); }
        //}

        [Browsable(false)]
        public int BeginItemPointIndex { get; set; }

        [Browsable(false)]
        public int EndItemPointIndex { get; set; }

        

        private void GenerateForm(IModelClass fromClass, IModelClass toClass, StringBuilder cols,string from,string to)
        {
            if (fromClass == null)
                throw new ArgumentNullException(nameof(fromClass));
            if (toClass == null)
                throw new ArgumentNullException(nameof(toClass));
            //var fromClass = From as FlowChartFormNode;
            //var toClass = To as FlowChartFormNode;
            foreach (var toMember in toClass.AllMembers)
            {
                //类上没有设置忽略单据转换属性
                if (toMember.ModelClass.TypeInfo.FindAttribute<IgnoreFormConvertAttribute>() == null)
                {
                    //目标属性上也没有设置
                    if (!toMember.MemberInfo.IsAutoGenerate &&
                        !toMember.MemberInfo.IsKey &&
                        !toMember.MemberInfo.IsService &&
                        !toMember.MemberInfo.IsReadOnly &&
                        !toMember.MemberInfo.IsList
                        && toMember.AllowEdit
                        && toMember.MemberInfo.FindAttribute<IgnoreFormConvertAttribute>() == null
                        )
                    {
                        //名字一样，类型一样，可以导入！
                        var fp = fromClass.FindMember(toMember.Name);
                        if (fp != null)
                        {
                            if (fp.MemberInfo.MemberType == toMember.MemberInfo.MemberType)
                            {
                                cols.AppendFormat("{2}.{0} = {3}.{1};\n", toMember.Name, fp.Name, to, from);
                            }
                        }
                    }
                }
                else
                {
                    Debug.WriteLine(toMember.ModelClass.Name + "忽略单据转换");
                }
            }
        }

        #region 数据源

        //List<StringObject> _主表来源属性数据源;
        //[Browsable(false)]
        //public List<StringObject> 主表来源属性数据源
        //{
        //    get
        //    {
        //        if (_主表来源属性数据源 == null)
        //        {
        //            _主表来源属性数据源 = GetProperties(this.Type);
        //        }
        //        return _主表来源属性数据源;
        //    }
        //}

        //List<StringObject> _主表目标属性数据源;
        //[Browsable(false)]
        //public List<StringObject> 主表目标属性数据源
        //{
        //    get
        //    {
        //        if (_主表目标属性数据源 == null)
        //        {
        //            _主表目标属性数据源 = GetProperties(ToType);
        //        }
        //        return _主表目标属性数据源;
        //    }
        //}

        //List<StringObject> _子表来源属性数据源;
        //[Browsable(false)]
        //public List<StringObject> 子表来源属性数据源
        //{
        //    get
        //    {
        //        if (_子表来源属性数据源 == null)
        //        {
        //            _子表来源属性数据源 = GetProperties(ItemsType);
        //        }
        //        return _子表来源属性数据源;
        //    }
        //}

        //List<StringObject> _子表目标属性数据源;
        //[Browsable(false)]
        //public List<StringObject> 子表目标属性数据源
        //{
        //    get
        //    {
        //        if (_子表目标属性数据源 == null)
        //        {
        //            _子表目标属性数据源 = GetProperties(ToItemsType);
        //        }
        //        return _子表目标属性数据源;
        //    }
        //}

        #endregion

        //private IModelClass GetClass(string type)
        //{
        //    return CaptionHelper.ApplicationModel.BOModel.GetClass(ReflectionHelper.FindType(type));
        //}

        //private List<StringObject> GetProperties(Type type)
        //{
        //    var fromClass = CaptionHelper.ApplicationModel.BOModel.GetClass(type);
        //    var rst = new List<StringObject>();
        //    foreach (var p in fromClass.AllMembers)
        //    {
        //        //类上没有设置忽略单据转换属性
        //        if (p.ModelClass.TypeInfo.FindAttribute<IgnoreFormConvertAttribute>() == null)
        //        {
        //            //目标属性上也没有设置
        //            if (!p.MemberInfo.IsAutoGenerate &&
        //                !p.MemberInfo.IsKey &&
        //                !p.MemberInfo.IsService &&
        //                !p.MemberInfo.IsReadOnly &&
        //                !p.MemberInfo.IsList
        //                && p.AllowEdit
        //                && p.MemberInfo.FindAttribute<IgnoreFormConvertAttribute>() == null
        //                )
        //            {
        //                rst.Add(new StringObject(p.Name));
        //            }
        //        }
        //    }
        //    return rst;
        //}

        private bool _Disable;
        [XafDisplayName("禁用")]
        public bool Disable
        {
            get { return _Disable; }
            set { SetPropertyValue("Disable", ref _Disable, value); }
        }

        private 单据转换多次生成控制 _MultiGenerate;
        [XafDisplayName("多次生成")]
        public 单据转换多次生成控制 MultiGenerate
        {
            get { return _MultiGenerate; }
            set { SetPropertyValue("MultiGenerate", ref _MultiGenerate, value); }
        }

        private 动作类型 _动作类型;

        public 动作类型 动作类型
        {
            get { return _动作类型; }
            set { SetPropertyValue("动作类型", ref _动作类型, value); }
        }

        //private string _触发动作条件;
        //[ToolTip("当满足此条件时，执行本动作的内容，如采购入库单审核完成后，自动将采购订单的已入库数量改写。")]
        //[EditorAlias(EditorAliases.PopupCriteriaPropertyEditor), CriteriaOptions("Type")]
        //public string 触发动作条件
        //{
        //    get { return _触发动作条件; }
        //    set { SetPropertyValue("触发动作条件", ref _触发动作条件, value); }
        //}

        private 目标类型 _目标类型;

        public 目标类型 目标类型
        {
            get { return _目标类型; }
            set { SetPropertyValue("目标类型", ref _目标类型, value); }
        }

        private bool _显示编辑界面;

        public bool 显示编辑界面
        {
            get { return _显示编辑界面; }
            set { SetPropertyValue("显示编辑界面", ref _显示编辑界面, value); }
        }

        private bool _自动保存;

        public bool 自动保存
        {
            get { return _自动保存; }
            set { SetPropertyValue("自动保存", ref _自动保存, value); }
        }

        //private CIIPXpoState _改变状态;
        //[ToolTip("来源单据状态")]
        //public CIIPXpoState 改变状态
        //{
        //    get { return _改变状态; }
        //    set { SetPropertyValue("改变状态", ref _改变状态, value); }
        //}

        //private string _改变状态条件;
        //[EditorAlias(EditorAliases.PopupCriteriaPropertyEditor), CriteriaOptions("Type")]
        //public string 改变状态条件
        //{
        //    get { return _改变状态条件; }
        //    set { SetPropertyValue("改变状态条件", ref _改变状态条件, value); }
        //}

        //private CIIPXpoState _目标单据状态;
        
        //public CIIPXpoState 目标单据状态
        //{
        //    get { return _目标单据状态; }
        //    set { SetPropertyValue("目标单据状态", ref _目标单据状态, value); }
        //}

        //private string _目标单据状态条件;

        //public string 目标单据状态条件
        //{
        //    get { return _目标单据状态条件; }
        //    set { SetPropertyValue("目标单据状态条件", ref _目标单据状态条件, value); }
        //}
        
        private int _序号;

        public int 序号
        {
            get { return _序号; }
            set { SetPropertyValue("序号", ref _序号, value); }
        }

        //private bool _分组标记;

        //public bool 分组标记
        //{
        //    get { return _分组标记; }
        //    set { SetPropertyValue("分组标记", ref _分组标记, value); }
        //}

        IFlowNode IFlowAction.From
        {
            get
            {
                return From;
            }

            set
            {
                From = (FlowNode)value;
            }
        }

        IFlowNode IFlowAction.To
        {
            get
            {
                return To;
            }

            set { To = (FlowNode)value; }
        }

        public Guid GetDocumentGuid()
        {
            return this.Oid;
        }

        public string GetFileName()
        {
            return this.Flow.名称 + "." + this.Caption;
        }
    }
}