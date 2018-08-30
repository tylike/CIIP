using CIIP.CodeFirstView;

namespace CIIP.Module.BusinessObjects.Flow
{
    public class FlowAction_ListView : ListViewObject<FlowAction>
    {
        public override void LayoutListView()
        {

        }

        public override void LayoutDetailView()
        {
            DetailViewLayout.ClearNodes();
            HGroup(10, x => x.Caption);
            HGroup(20, x => x.From, x => x.To);
            HGroup(30, x => x.MultiGenerate, x => x.明细生效条件);
            HGroup(40, x => x.生效条件, x => x.Disable);
            HGroup(50, x => x.动作类型, x => x.触发动作条件);
            HGroup(55, x => x.目标类型);
            HGroup(60, x => x.显示编辑界面, x => x.自动保存);
            HGroup(70, x => x.改变状态条件, x => x.改变状态);
            HGroup(80, x => x.目标单据状态条件, x => x.目标单据状态);
            HGroup(90, x => x.序号, x => x.分组标记);

            TabbedGroup(100, x => x.MasterMapping, x => x.ItemsMapping);

            var masterMappinListView = GetChildListView(x => x.MasterMapping);
            (masterMappinListView as IModelListViewSetting).DisableShowDetailView = true;
            masterMappinListView.AllowEdit = true;
            masterMappinListView.SetNewItemRow(DevExpress.ExpressApp.NewItemRowPosition.Bottom);
            LayoutListViewColumns<MasterPropertyMapping>(masterMappinListView, x => x.ToProperty, x => x.FromProperty, x => x.Expression);

            var detailMappingListView = GetChildListView(x => x.ItemsMapping);
            (detailMappingListView as IModelListViewSetting).DisableShowDetailView = true;
            detailMappingListView.AllowEdit = true;
            detailMappingListView.SetNewItemRow(DevExpress.ExpressApp.NewItemRowPosition.Bottom);
            LayoutListViewColumns<ItemsPropertyMapping>(detailMappingListView, x => x.ToProperty, x => x.FromProperty, x => x.Expression);

            GetChildListView(x => x.MasterMapping).DataAccessMode = DevExpress.ExpressApp.CollectionSourceDataAccessMode.Client;
            GetChildListView(x => x.ItemsMapping).DataAccessMode = DevExpress.ExpressApp.CollectionSourceDataAccessMode.Client;

            base.LayoutDetailView();
        }
    }
}