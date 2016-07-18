using System;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp;
using CIIP.Module.BusinessObjects.SYS.Logic;

namespace CIIP.Module.Win.Editors
{
    [PropertyEditor(typeof(CsharpCode),"CodeEditor",true)]
    public class CodePropertyEditor : WinPropertyEditor,IComplexControl,IComplexViewItem
    {
        SmartVisualStudio editor;
        IObjectSpace os;

        CsharpCode Value
        {
            get { return PropertyValue as CsharpCode; }
        }

        protected override object CreateControlCore()
        {
            var codeEditor = new SmartVisualStudio(os, this.Value);
            editor = codeEditor;
            codeEditor.OnValueChanged += CodeEditor_OnValueChanged;
            ControlBindingProperty = "Code";
            return codeEditor;
        }

        public bool Validated()
        {
            return editor.Validated();
        }

        private void CodeEditor_OnValueChanged(object sender, EventArgs e)
        {
            this.PropertyValue = editor.Code;
            this.OnControlValueChanged();
        }

        public void Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this.os = objectSpace;
        }

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (editor != null)
                {
                    editor.OnValueChanged -= this.CodeEditor_OnValueChanged;
                }
                
            }
            base.Dispose(disposing);
        }

        public CodePropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model)
        {

        }
    }
}