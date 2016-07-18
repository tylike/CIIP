using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using System.Linq;
using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.Editors;

namespace IMatrix.ERP.Module.BusinessObjects.SYS.Logic
{
    [XafDisplayName("附值")]
    [CreatableItem]
    public class AssignValue : MethodCode
        , IVariantListProvider
    {
        public AssignValue(Session s) : base(s)
        {

        }

        private string _CodeVariant;

        [EditorAlias("AutoProperties")]
        [XafDisplayName("变量名称")]
        [RuleRequiredField]
        public string CodeVariant
        {
            get { return _CodeVariant; }
            set { SetPropertyValue("CodeVariant", ref _CodeVariant, value); }
        }

        private string _Value;

        [XafDisplayName("值")]
        [EditorAlias(EditorAliases.PopupExpressionPropertyEditor)]
        [RuleRequiredField(TargetCriteria = "!Null")]
        public string Value
        {
            get { return _Value; }
            set { SetPropertyValue("Value", ref _Value, value); }
        }

        private bool _Null;

        [XafDisplayName("置空")]
        public bool Null
        {
            get { return _Null; }
            set { SetPropertyValue("Null", ref _Null, value); }
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            this.名称 = Name;
        }

        public override string Name
        {
            get { return this.CodeVariant + "=" + this.Value; }
        }

        IEnumerable<string> IVariantListProvider.GetNames(string propertyName, string inputed)
        {
            if (propertyName == "CodeVariant")
            {
                var rst = GetReadableVariants();
                if (rst != null)
                {
                    if (!string.IsNullOrEmpty(inputed))
                    {
                        if (inputed.EndsWith("."))
                        {
                            var propertyPath = inputed.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
                            var current = rst.SingleOrDefault(x => x.CodeVariant == propertyPath.First());
                            if (current != null)
                            {
                                var currentType = ReflectionHelper.FindType(current.Type.FullName);
                                foreach (var p in propertyPath.Skip(1))
                                {
                                    currentType = currentType.GetProperty(p).PropertyType;
                                }
                                return currentType.GetProperties(System.Reflection.BindingFlags.Public |
                                                                 System.Reflection.BindingFlags.Instance)
                                    .Select(x => inputed + x.Name);
                            }
                        }
                        else
                        {
                            return rst.Select(x => x.CodeVariant).Where(x => !string.IsNullOrEmpty(x) && x.StartsWith(inputed));
                        }
                    }
                    return rst.Where(x => !string.IsNullOrEmpty(x.CodeVariant)).Select(x => x.CodeVariant);
                }
            }
            return null;
        }
    }

    public class AssignValue_ListView : MethodCodeListView<AssignValue>
    {
        public override void LayoutDetailView()
        {
            base.LayoutDetailView();
            HGroup(10, x => x.CodeVariant, x => x.Value, x => x.Null);
            HGroup(20, x => x.Index);
        }
    }
}