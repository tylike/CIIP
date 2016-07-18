using Admiral.CodeFirstView;

namespace IMatrix.ERP.Module.BusinessObjects.SYS.Logic
{
    public abstract class MethodViewBase<T> : ListViewObject<T>
        where T : MethodDefine
    {
        public override void LayoutListView()
        {

        }

        public override void LayoutDetailView()
        {
            DetailViewLayout.ClearNodes();

            HGroup(10, x => x.名称);

            //var tabbed = TabbedGroup(20, x => x.Variants);
            //var parameters = tabbed[0][0];
            //parameters.RelativeSize = 20;
            //parameters.Index = 20;
            //var codes = tabbed[0].AddNode<IModelLayoutViewItem>("Codes");
            //codes.RelativeSize = 80;
            //codes.Index = 10;
            //tabbed[0].Caption = "内容";

            //HGroup(30, x => x.创建时间, x => x.创建者, x => x.修改时间, x => x.修改者);
        }
    }
}