using DevExpress.ExpressApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp.Model.Core;
using System.Diagnostics;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Utils;

namespace CIIP.CodeFirstView
{
    //public class DetailViewLayoutGeneratorUpdater : ModelNodesGeneratorUpdater<ModelDetailViewLayoutNodesGenerator>
    //{
    //    public override void UpdateNode(ModelNode node)
    //    {
    //        var dv = node.Parent as IModelDetailView;
    //        var vo = ViewsManager.FindDetailViewObject(dv.ModelClass.TypeInfo.Type);
    //        if (vo != null)
    //            vo.UpdateNode(dv);
    //    }
    //}
    public class ViewsGeneratorUpdaterX1 : IModelNodeUpdater<IModelViews>
    {
        public void UpdateNode(IModelViews node, IModelApplication application)
        {

            foreach (var item in ViewsManager.ViewObjects)
            {
                var view = node.GetNode(item.GetType().Name) as IModelListView;

                if (view == null)
                {
                    throw new UserFriendlyException("没有找到视图：" + item.GetType().Name);
                }
                item.UpdateNode(view);
            }

        }
    }
}
