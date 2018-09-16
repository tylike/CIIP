using System;
using System.Linq;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Xpo;
using DevExpress.XtraEditors.Controls;

namespace CIIP.Module.Win.Editors
{
    [PropertyEditor(typeof(object), "OPE", false)]
    public class ObjectPropertyEditorEx : ObjectPropertyEditor
    {
        public ObjectPropertyEditorEx(Type objectType, IModelMemberViewItem model) : base(objectType, model)
        {
        }

        protected override object CreateControlCore()
        {
            var c = base.CreateControlCore() as ObjectEdit;
            c.Properties.NullText = "<<自动设置>>";
            c.Properties.NullValuePrompt = "自动设置";
            var manual = c.Properties.Buttons.FirstOrDefault(x => x.Kind == ButtonPredefines.Ellipsis);
            if (manual != null)
            {
                manual.ToolTip = "手动设置";                
            }
            var clear = new EditorButton(ButtonPredefines.Delete);
            clear.Caption = "清除";
            c.ButtonClick += C_ButtonClick;
            c.Properties.Buttons.Add(clear);
            return c;
        }

        private void C_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.Delete)
            {
                var t = this.PropertyValue as XPBaseObject;
                t?.Delete();
                this.PropertyValue = null;
            }

        }
    }
}