using System;
using CIIP.Module.BusinessObjects.SYS.Logic;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects.SYS
{
    public abstract class CodeFile : NameObject, IDocumentProvider
    {
        private CsharpCode _Code;

        public CodeFile(Session s) : base(s)
        {
        }

        

        [Size(-1)]
        [ValueConverter(typeof(CSharpCodeToStringConverter))]
        public CsharpCode Code
        {
            get {
                if (_Code != null)
                    _Code.Provider = this;
                return _Code;
            }
            set { SetPropertyValue("Code", ref _Code, value); }
        }

        public Guid GetDocumentGuid()
        {
            return this.Oid;
        }

        public abstract string GetFileName();

        public string GetCode()
        {
            return Code.Code;
        }
    }
}