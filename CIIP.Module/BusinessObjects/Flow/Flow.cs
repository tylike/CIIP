using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.XtraCharts.Native;
using DevExpress.ExpressApp.DC;
using System;
using System.Collections.Generic;
using DevExpress.Diagram.Core;
using DevExpress.XtraDiagram;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Actions;
using System.ComponentModel;
using CIIP;
using 常用基类;

namespace CIIP.Module.BusinessObjects.Flow
{
    [XafDisplayName("单据转换流程")]
    [NavigationItem("审批流程")]
    public class Flow : NameObject,IFlow
    {
        public Flow(Session s) : base(s)
        {

        }

        [Browsable(false)]
        public bool IsDesignMode
        {
            get { return true; }
        }

        public Flow FlowObject
        {
            get { return this; }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [NonPersistent]
        public FlowNode SelectedNode
        {
            get { return GetPropertyValue<FlowNode>("SelectedNode"); }
            set { SetPropertyValue("SelectedNode", value); }
        }

        [Association, DevExpress.Xpo.Aggregated]
        [XafDisplayName("单据结点")]
        public XPCollection<FlowNode> Nodes { get { return GetCollection<FlowNode>("Nodes"); } }

        [Association, DevExpress.Xpo.Aggregated]
        [XafDisplayName("执行动作")]
        public XPCollection<FlowAction> Actions { get { return GetCollection<FlowAction>("Actions"); } }

        IFlowNode IFlow.SelectedNode
        {
            get
            {
                return SelectedNode;
            }

            set
            {
                SelectedNode = (FlowNode)value;
            }
        }

        IEnumerable<IFlowNode> IFlow.Nodes
        {
            get
            {
                return this.Nodes;
            }
        }

        IEnumerable<IFlowAction> IFlow.Actions
        {
            get
            {
                return this.Actions;
            }
        }

        IFlowAction IFlow.CreateAction(IFlowNode from, IFlowNode to)
        {
            var act =  new FlowAction(Session) {From = (FlowNode) from, To = (FlowNode) to, Caption = "生成" + to.Caption};
            act.Flow = this;
            act.Created();
            //act.GenerateMapping(CaptionHelper.ApplicationModel.BOModel);
            return act;
        }

        IFlowNode IFlow.CreateNode(int x, int y, int width, int height, string form, string caption)
        {
            var node = new FlowNode(Session)
            {
                Caption = caption,
                Form = form,
                Flow = this,
                X = x,
                Y = y,
                Width = width,
                Height = height
            };
            return node;
        }

        void IFlow.RemoveNode(IFlowNode node)
        {
            Nodes.Remove((FlowNode)node);
        }

        void IFlow.RemoveAction(IFlowAction action)
        {
            Actions.Remove((FlowAction)action);
        }

        private NonPersistentCollectionSource CreateCollectionSource(ShowNodesEventParameter para)
        {
            var session = Session;

            var forms = para.Application.Model.BOModel.Where(
                x => x.TypeInfo.IsPersistent && typeof(I单据).IsAssignableFrom(x.TypeInfo.Type))
                .Select(x => new FormInfo(session)
                {
                    FullName = x.TypeInfo.FullName,
                    Caption = x.Caption,
                    BaseTypeCaption = x.BaseClass.Caption,
                    BaseTypeName = x.BaseClass.TypeInfo.FullName
                }).ToArray();

            var xpc = new XPCollection<FormInfo>(session,
                forms.Where(x => !para.SelectedForms.OfType<FlowNode>().Select(t => t.Form).Contains(x.FullName)));

            var cs = new NonPersistentCollectionSource(para.ObjectSpace, typeof(FormInfo), xpc);

            return cs;
        }

        public void ShowNodesView(ShowNodesEventParameter para)
        {
            var listViewId = para.Application.FindListViewId(typeof(FormInfo));

            NonPersistentCollectionSource cs = CreateCollectionSource(para);
            var p = para;
            p.ViewParameter.CreatedView = para.Application.CreateListView(listViewId, cs, false);

            p.ViewParameter.CreatedView.Caption = "选择单据";

            p.ViewParameter.NewWindowTarget = NewWindowTarget.Separate;
            p.ViewParameter.TargetWindow = TargetWindow.NewModalWindow;
            var dc = new DialogController();

            dc.SaveOnAccept = false;

            dc.AcceptAction.SelectionDependencyType = para.SingleSelect
                ? SelectionDependencyType.RequireSingleObject
                : SelectionDependencyType.RequireMultipleObjects;

            dc.Accepting += (s, e1) =>
            {
                var x = 20;
                var y = 20;
                if (e1.AcceptActionArgs.SelectedObjects.Count == 1)
                {
                    x = (int)para.MouseClickPoint.X;
                    y = (int)para.MouseClickPoint.Y;
                }

                var i = 0;
                foreach (var item in e1.AcceptActionArgs.SelectedObjects)
                {
                    var selected = (item as FormInfo);
                    if (selected != null)
                    {
                        //创建一个结点
                        var node = (this as IFlow).CreateNode(x, y, 64, 64, selected.FullName, selected.Caption);
                        object single = null;
                        if (para.Shape == null)
                        {
                            single = para.CreateShape(node);
                            //new DiagramShapeEx(SDLDiagramShapes.Procedure, node.X, node.Y, node.Width,
                            //    node.Height);
                            //single.Image = node.GetImage();
                            //_diagram.Items.Add(single);
                            //_diagram.SelectItem(single);
                        }
                        else
                        {
                            single = para.Shape;
                        }
                        para.UpdateShape(node, single);

                        //single.Content = node.Caption;

                        //single.Tag = node;
                    }
                    x += 200;
                    i++;
                    if (i % 5 == 0)
                    {
                        x = 20;
                        y += 200;
                    }
                }
            };
            para.ViewParameter.Controllers.Add(dc);
        }
    }

  
}
