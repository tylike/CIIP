using System.Linq;
using CIIP.CodeFirstView;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using CIIP;

namespace 基础信息
{
    public abstract class 单据布局<TMaster, TItem> : ListViewObject<TMaster, TItem>
    {
        public override void LayoutDetailView()
        {
            base.LayoutDetailView();
        }

        protected override string ItemsPropertyName
        {
            get
            {
                return "明细项目";
            }
        }
        protected void SetItemsPropertyEditor(IModelLayoutGroup itemsLayoutItem)
        {
            itemsLayoutItem.ImageName = "BO_Product_Group";
        }

        public override void LayoutItemsListView()
        {
            this.ItemsListView.AllowEdit = true;
            var nir = this.ItemsListView as IModelListViewNewItemRow;

            if (nir != null)
                nir.NewItemRowPosition = DevExpress.ExpressApp.NewItemRowPosition.Bottom;
            SetFooterCount(this.ItemsListView);
            base.LayoutItemsListView();
            (ItemsListView as IModelListViewSetting).DisableShowDetailView = true;

        }

        public override void LayoutListView()
        {
            SetFooterCount(this.View);
        }



    }
}