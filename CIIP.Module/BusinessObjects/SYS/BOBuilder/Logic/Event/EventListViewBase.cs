using System.Linq;
using CIIP.CodeFirstView;
using DevExpress.ExpressApp.Model;

namespace CIIP.Module.BusinessObjects.SYS.Logic
{
    public abstract class EventListViewBase<T> : ListViewObject<T>
        where T : BusinessObjectEvent
    {
        public override void LayoutListView()
        {

        }

        public override void LayoutDetailView()
        {
            DetailViewLayout.ClearNodes();
            //var mst = DetailView.Items["StaticText"];
            //if (mst == null)
            //{
            //    var t = DetailView.Items.AddNode<IModelStaticText>("StaticText");
            //    t.Text = "请在菜单中[填加指令],并选择相应的子级项目.";
            //    mst = t;
            //}
            //var msg = DetailViewLayout.AddNode<IModelLayoutViewItem>("Message");
            //msg.ViewItem = mst;
            var group = HGroup(100, x => x.Code);

            var item = group.First() as IModelLayoutViewItem;
            item.ShowCaption = false;
            
            base.LayoutDetailView();
        }
    }
}