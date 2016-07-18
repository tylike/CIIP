using System;
using System.Diagnostics;
using System.Linq.Expressions;
using DevExpress.ExpressApp.Model;

namespace CIIP.CodeFirstView
{
    public abstract class ListViewObject<TBusinessObject, TItem> : ListViewObject<TBusinessObject>
    {
        public IModelDetailView ItemsDetailView { get; private set; }

        public IModelListView ItemsListView { get; private set; }

        public IModelLayoutGroup ItemsViewLayout { get; private set; }

        protected virtual string ItemsPropertyName
        {
            get
            {
                return "明细项目";
            }
        }

        public override void UpdateNode(IModelListView view)
        {
            try
            {
                base.UpdateNode(view);
                this.ItemsListView = (this.DetailView.Items[ItemsPropertyName] as IModelPropertyEditor).View as IModelListView;

                this.ItemsListView.DetailView.Layout.ClearNodes();

                this.ItemsViewLayout = this.ItemsListView.DetailView.Layout.AddNode<IModelLayoutGroup>("R");

                this.ItemsDetailView = this.ItemsListView.DetailView;

                if (ItemsViewLayout == null)
                    throw new Exception("没有找到ItemsViewLayout!");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            LayoutListView();
            LayoutItemsListView();
            LayoutItemsDetailView();

            Debug.WriteLine(this.ItemsListView.Id);
        }
        public void LayoutItemsColumns( params Expression<Func<TItem, object>>[] columns)
        {
            LayoutListViewColumns(this.ItemsListView, columns);
        }

        protected IModelLayoutGroup ItemsHGroup(int index, params Expression<Func<TItem, object>>[] property)
        {
            return HGroup(this.ItemsDetailView, this.ItemsViewLayout, index, property);
        }

        public virtual void LayoutItemsDetailView()
        {

        }

        public virtual void LayoutItemsListView()
        {

        }
    }
}