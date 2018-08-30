using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using CIIP;

namespace CIIP.Module.BusinessObjects.SYS
{
    [XafDisplayName("导航菜单")]
    public class NavigationMenu : XPLiteObject, IFlow,IRadialMenu
    {

        private string _Name;
        [Key]
        [XafDisplayName("名称")]
        [RuleRequiredField]
        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue("Name", ref _Name, value); }
        }
        
        public NavigationMenu(Session s) : base(s)
        {

        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            IsDesignMode = true;
        }

        private bool _IsDesignMode;
        [Browsable(false)]
        public bool IsDesignMode
        {
            get { return _IsDesignMode; }
            set { SetPropertyValue("IsDesignMode", ref _IsDesignMode, value); }
        }
        
        [VisibleInListView(false)]
        public IFlow Menu
        {
            get { return this; }
        }

        //[VisibleInListView(false)]
        //public IRadialMenu RadialMenu
        //{
        //    get { return this; }
        //}

        [Association, DevExpress.Xpo.Aggregated]
        [XafDisplayName("菜单项目")]
        public XPCollection<NavigationMenuItem> Nodes
        {
            get { return GetCollection<NavigationMenuItem>("Nodes"); }
        }

        [XafDisplayName("指示线段")]
        [Association, DevExpress.Xpo.Aggregated]
        public XPCollection<NavigationLine> Actions
        {
            get { return GetCollection<NavigationLine>("Actions"); }
        }

        private NavigationMenuItem _SelectedNode;
        [Browsable(false)]
        public NavigationMenuItem SelectedNode
        {
            get { return _SelectedNode; }
            set { SetPropertyValue("SelectedNode", ref _SelectedNode, value); }
        }

        IFlowNode IFlow.SelectedNode
        {
            get { return SelectedNode; }

            set { SelectedNode = (NavigationMenuItem) value; }
        }

        IEnumerable<IFlowNode> IFlow.Nodes
        {
            get { return Nodes; }
        }

        IEnumerable<IFlowAction> IFlow.Actions
        {
            get { return Actions; }
        }

        IFlowAction IFlow.CreateAction(IFlowNode from, IFlowNode to)
        {
            var act = new NavigationLine(Session)
            {
                From = (NavigationMenuItem) from,
                To = (NavigationMenuItem) to,
                //Caption = "生成" + to.Caption
            };
            this.Actions.Add(act);
            return act;
        }

        IFlowNode IFlow.CreateNode(int x, int y, int width, int height, string form, string caption)
        {
            var node = new NavigationMenuItem(Session)
            {
                Caption = caption,
                //Form = form,
                X = x,
                Y = y,
                Width = width,
                Height = height
            };
            this.Nodes.Add(node);
            return node;
        }

        public void RemoveAction(IFlowAction action)
        {
            Actions.Remove((NavigationLine) action);
        }

        public void RemoveNode(IFlowNode node)
        {
            Nodes.Remove((NavigationMenuItem) node);
        }

        public void ShowNodesView(ShowNodesEventParameter p)
        {
            if (!IsDesignMode)
            {
                var mi = (p.SelectedNode as NavigationMenuItem);
                if (mi != null)
                    p.DoShowNavigationItem(mi.NavigationItemPath.Path);
                return;
            }
            NavigationMenuItem obj;
            if (p.Shape == null)
            {
                obj =
                    (NavigationMenuItem)
                        (this as IFlow).CreateNode((int) p.MouseClickPoint.X, (int) p.MouseClickPoint.Y, 64, 64, "", "");
                //没有对象选择，弹出树形列表，选择节点。如果节点是有子级的，则将子级显示出来，如果子级是没有子结点的，直接显示
                //结点必须是有view设定的。
            }
            else
            {
                obj = p.SelectedNode as NavigationMenuItem;
                //进行编辑对象功能
            }

            if (obj == null)
                throw new Exception("没有状态对象！");

            var view = p.Application.CreateDetailView(p.ObjectSpace, obj, false);
            p.ViewParameter.CreatedView = view;
            p.ViewParameter.TargetWindow = TargetWindow.NewModalWindow;


            var dc = new DialogController();
            dc.Accepting += (s, p1) =>
            {
                var sp = p.Shape;
                if (p.Shape == null)
                {
                    sp = p.CreateShape(obj);
                }
                this.Nodes.Add(obj);
                p.UpdateShape(obj, sp);

                obj.Save();
            };

            dc.Cancelling += (s, p1) =>
            {
                if (p.Shape == null)
                {
                    obj.Delete();
                    p.DeletSelectedNode();
                }
                //_diagram.DeleteSelectedItems();
            };
            dc.SaveOnAccept = false;
            p.ViewParameter.Controllers.Add(dc);
        }
    }
}