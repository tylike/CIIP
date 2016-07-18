using System;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using CIIP.Module.BusinessObjects;
using CIIP.Module.BusinessObjects.Flow;

namespace CIIP.Module.Win.Editors
{
    [PropertyEditor(typeof(string), false)]
    public class AdmiralPopupExpressionPropertyEditor : PopupExpressionPropertyEditor
    {
        public AdmiralPopupExpressionPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model)
        {
        }

        protected override object CreateControlCore()
        {
            var rst =base.CreateControlCore()  as PopupExpressionEdit;
            
            return rst;
        }
    }
}