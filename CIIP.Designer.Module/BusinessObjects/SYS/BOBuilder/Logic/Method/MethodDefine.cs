using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Validation;
using 常用基类;
using System;

namespace CIIP.Module.BusinessObjects.SYS.Logic
{
    [XafDisplayName("方法定义")]
    [ImageName("PG_Method")]
    [DefaultListViewOptions(MasterDetailMode.ListViewAndDetailView)]
    public abstract class MethodDefine : NameObject,IPartCodeProvider,IDocumentProvider
    {
        public MethodDefine(Session s) : base(s)
        {

        }
        
        public virtual void SetCode(string code)
        {
            if (this.Code == null)
                this.Code = new CsharpCode(code,this);
            else
                this.Code.Code = code;
        }
        
        private BusinessObjectBase _ReturnValue;
        [XafDisplayName("返回类型")]
        public virtual BusinessObjectBase ReturnValue
        {
            get { return _ReturnValue; }
            set { SetPropertyValue("ReturnValue", ref _ReturnValue, value); }
        }
        
        private CsharpCode _Code;
        [Size(-1)]
        [ValueConverter(typeof(CSharpCodeToStringConverter))]
        public CsharpCode Code
        {
            get {
                if(_Code!=null)
                {
                    _Code.Provider = this;
                }

                return _Code;
            }
            set { SetPropertyValue("Code", ref _Code, value); }
        }

        private AccessorModifier _AccessorModifier;
        [XafDisplayName("访问修饰")]
        public AccessorModifier AccessorModifier
        {
            get { return _AccessorModifier; }
            set { SetPropertyValue("AccessorModifier", ref _AccessorModifier, value); }
        }


        private MethodModifier _methodModifier;
        [XafDisplayName("方法修饰")]
        public MethodModifier MethodModifier
        {
            get { return _methodModifier; }
            set { SetPropertyValue("MethodModifier", ref _methodModifier, value); }
        }

        private string _CodeName;
        [XafDisplayName("方法名称")]
        [RuleRequiredField]
        public string CodeName
        {
            get { return _CodeName; }
            set { SetPropertyValue("CodeName", ref _CodeName, value); }
        }

        private string _功能说明;
        [Size(-1)]
        public string 功能说明
        {
            get { return _功能说明; }
            set { SetPropertyValue("功能说明", ref _功能说明, value); }
        }

        public string DefaultLocation
        {
            get { return "//MethodDefine:"+this.Oid; }
        }

        public string ReplaceNewCode(string allcode, string newCode)
        {
            var b = allcode.IndexOf(DefaultLocation) + DefaultLocation.Length + 1;
            var e = allcode.IndexOf(DefaultLocation + "End") - 1;
            if (e > b)
                allcode = allcode.Remove(b, e - b);
            return allcode.Insert(b, newCode);
        }

        public virtual string GetCode()
        {
            throw new NotImplementedException();
        }

        public virtual string MethodDefineCode
        {
            get
            {
                return string.Format(@"
{0} {1} {6} {2}({3}){{
{4}

{5}

{4}End

}}
",
                    AccessorModifier.ToString().ToLower(),
                    MethodModifier == MethodModifier.None ? "" : MethodModifier.ToString().ToLower(),
                    this.CodeName,
                    string.Join(",", Parameters.OrderBy(x=>x.Index).Select(x => $"{ (x.ParameterType.IndexOf(".") > -1 ? "global::" : "") + x.ParameterType} {x.ParameterName}")),
                    this.DefaultLocation,
                    this.Code?.Code,
                    this.ReturnValue == null ? "void" : "global::" + ReturnValue.FullName
                    );
            }
        }

        [Association, DevExpress.Xpo.Aggregated]
        public XPCollection<ParameterDefine> Parameters
        {
            get { return GetCollection<ParameterDefine>("Parameters"); }
        }

        public abstract Guid GetDocumentGuid();
        
        public abstract string GetFileName();
    }

    //public class MethodDefine_ListView : MethodViewBase<MethodDefine>
    //{

    //}

    //public class ObjectAfterConstruction_ListView : MethodViewBase<ObjectAfterConstruction>
    //{

    //}

    //public class ObjectDeletingEvent_ListView : MethodViewBase<ObjectDeletingEvent>
    //{

    //}

    //public class ObjectSavingEvent_ListView : MethodViewBase<ObjectSavingEvent>
    //{

    //}

    //public class ObjectSavedEvent_ListView : MethodViewBase<ObjectSavedEvent>
    //{

    //}

    //public class ObjectPropertyChangedEvent_ListView : MethodViewBase<ObjectPropertyChangedEvent>
    //{

    //}

}