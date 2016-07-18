using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.NodeGenerators;

namespace CIIP.CodeFirstView
{
    public class ViewsGeneratorUpdater : ModelNodesGeneratorUpdater<ModelViewsNodesGenerator>
    {
        public override void UpdateNode(ModelNode node)
        {
            var views = node as IModelViews;
            
            foreach (var item in ViewsManager.ViewObjects)
            {
                var view = views.GetNode(item.GetType().Name) as IModelListView;

                if (view == null)
                {
                    throw new UserFriendlyException("没有找到视图：" + item.GetType().Name);
                }
                item.UpdateNode(view);
            }

            //var dv = node.Parent as IModelDetailView;
            //var vo = ViewsManager.FindDetailViewObject(dv.ModelClass.TypeInfo.Type);
            //if (vo != null)
            //    vo.UpdateNode(dv);
        }
    }
}