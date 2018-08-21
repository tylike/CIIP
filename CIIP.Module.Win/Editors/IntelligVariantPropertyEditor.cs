using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIIP;

namespace CIIP.Module.Win.Editors
{
    [PropertyEditor(typeof(string),"AutoProperties",false)]
    public class IntelligVariantPropertyEditor : DXPropertyEditor
    {
        public IntelligVariantPropertyEditor(Type type, IModelMemberViewItem viewItem) : base(type, viewItem)
        {

        }
        
        protected override object CreateControlCore()
        {
            var values = (this.CurrentObject as IVariantListProvider)?.GetNames(this.propertyName, "");

            if (values == null)
            {
                values = new List<string>();
            }

            var rst = new PredefinedValuesStringEdit(base.Model.MaxLength, values);
            rst.EditValueChanging += (s, e) =>
            {
                var v = (this.CurrentObject as IVariantListProvider)?.GetNames(this.propertyName, e.NewValue + "");
                if (v == null)
                    v = new List<string>();

                rst.CreatePredefinedListItems(v);
                if (v.Any())
                    rst.ShowPopup();
            };
            rst.Properties.AutoComplete = false;
            //rst.Properties.ImmediatePopup = true;
            return rst;
        }

        
    }
}
