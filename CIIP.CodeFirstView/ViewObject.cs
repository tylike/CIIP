using System;
using System.Linq;
using DevExpress.ExpressApp.Model;

namespace CIIP.CodeFirstView
{
    public abstract class ViewObject
    {
        public abstract Type BusinessObjectType { get; }

        public abstract void UpdateNode(IModelListView view);

        public abstract void LayoutListView();

        public virtual void LayoutDetailView()
        {

        }
        protected void SetFooterCount(IModelListView view)
        {
            view.IsFooterVisible = true;
            var firstColumn = view.Columns.Where(x => x.Index > -1).OrderBy(x => x.Index).FirstOrDefault();

            var sum = firstColumn?.Summary.FirstOrDefault(x => x.SummaryType == SummaryType.Count);
            if (firstColumn != null && sum == null)
            {
                sum = firstColumn.Summary.AddNode<IModelColumnSummaryItem>(firstColumn.PropertyName+"Count");
                sum.SummaryType = SummaryType.Count;
            }
        }

    }
}