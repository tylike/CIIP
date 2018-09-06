using System.Collections.Generic;
using DevExpress.ExpressApp.DC;
using Microsoft.CodeAnalysis;

namespace CIIP.Designer
{
    /// <summary>
    /// ÓÃÓÚc#´úÂë±à¼­
    /// </summary>
    [DomainComponent]
    public class CsharpCode
    {
        public CsharpCode(string value,IDocumentProvider documentProvider)
        {
            this.Code = value;
            this.Provider = documentProvider;
        }

        public string Code { get; set; }

        public IDocumentProvider Provider { get; set; }

        public bool ShowSolutionFiles { get; set; }

        public List<Diagnostic> Diagnostics { get; set; }

        public object Workspace { get; set; }
    }
}