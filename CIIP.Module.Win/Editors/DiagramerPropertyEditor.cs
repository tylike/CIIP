using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CIIP.Module.Controllers;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.XtraDiagram;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Diagram.Core;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.Xpo;
using DevExpress.Utils;
using DevExpress.Diagram.Core.Layout;

namespace CIIP.Module.Win.Editors
{
    [PropertyEditor(typeof(IFlow), true)]
    public class DiagramerPropertyEditor : PropertyEditor,IComplexControl,IComplexViewItem
    {
        DiagramTool line;
        DiagramTool select;
        IFlow Flow
        {
            get
            {
                return this.PropertyValue as IFlow;
            }
        }

        public DiagramerPropertyEditor(Type type, IModelMemberViewItem viewItem) : base(type, viewItem)
        {
            line = new ConnectorTool();
            select = new PointerTool();
        }

        private DXDiagramControl _diagram;
        protected override object CreateControlCore()
        {
            if (_diagram == null)
            {
                this._os.Committing += _os_Committing;

                _diagram = new DXDiagramControl();
                
                _diagram.OptionsView.PageSize = new SizeF(1024, 800);
                _diagram.OptionsView.GridSize = new SizeF(30, 30);
                _diagram.MouseUp += _diagram_MouseUp;
                _diagram.MouseDoubleClick += _diagram_MouseDoubleClick;
                _diagram.EditConnector += _diagram_EditConnector;
                _diagram.EditShape += _diagram_EditShape;
                _diagram.DeleteConnector += _diagram_DeleteConnector;
                _diagram.DeleteShape += _diagram_DeleteShape;
                _diagram.AddedConnector += _diagram_AddedConnector;
                _diagram.SelectionChanged += _diagram_SelectionChanged;
                _diagram.ClientSizeChanged += _diagram_ClientSizeChanged;
                ReadValueCore();

                UpdateDesignerMode(isDesignMode);
            }
            return _diagram;
        }

        private void _diagram_ClientSizeChanged(object sender, EventArgs e)
        {
            _diagram.OptionsView.PageSize = new SizeF(_diagram.Width, _diagram.Height);
            _diagram.FitToPage();
        }

        private void _diagram_SelectionChanged(object sender, DiagramSelectionChangedEventArgs e)
        {
            if (_diagram.SelectedItems.Count == 1)
            {
                var selected = _diagram.SelectedItems.First();
                var node = selected.Tag as IFlowNode;
                Flow.SelectedNode = node;
                // node.Form
            }
            else
            {
                Flow.SelectedNode = null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _os.Committing -= this._os_Committing;

                var flow = (Flow as XPBaseObject);
                if (flow != null)
                    flow.Changed -= Flow_Changed;

                if (_diagram != null)
                {
                    _diagram.MouseUp -= _diagram_MouseUp;
                    _diagram.MouseDoubleClick -= _diagram_MouseDoubleClick;
                    _diagram.EditConnector -= _diagram_EditConnector;
                    _diagram.EditShape -= _diagram_EditShape;
                    _diagram.DeleteConnector -= _diagram_DeleteConnector;
                    _diagram.DeleteShape -= _diagram_DeleteShape;
                    _diagram.AddedConnector -= _diagram_AddedConnector;
                    _diagram.SizeChanged -= _diagram_ClientSizeChanged;
                    _diagram.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region delete
        private void _diagram_DeleteShape(object sender, DiagramShape e)
        {
            if (!_isRefreshing)
            {
                var t = e.Tag as IFlowNode;
                if (t != null)
                {
                    Flow.RemoveNode(t);
                    t.Delete();
                }
            }
        }

        private void _diagram_DeleteConnector(object sender, DiagramConnector e)
        {
            if (!_isRefreshing)
            {
                var t = e.Tag as IFlowAction;
                if (t != null)
                {
                    Flow.RemoveAction(t);
                    t.Delete();
                }
            }
        }
        #endregion
        
        #region 编辑
        private void _diagram_EditShape(object sender, DiagramShape e)
        {
            var para = CreateShowNodesEventParameter(new PointFloat(0, 0), true, e);
            Application.ShowViewStrategy.ShowView(para.ViewParameter, new ShowViewSource(null,null));
        }

        private void _diagram_EditConnector(object sender, DiagramConnector e)
        {
            if (!this.isDesignMode)
                return;
            var obj = e.Tag as IFlowAction;

            //obj = OS.GetObject(obj);

            var view = Application.CreateDetailView(_os, obj, false);

            //编辑
            var svp = new ShowViewParameters(view);
            var dc = new DialogController();
            dc.SaveOnAccept = false;
            dc.Accepting += (s, e1) =>
            {
                e.Tag = obj;
                e.Text = obj.Caption;
            };

            dc.Cancelling += (s, e1) =>
            {
                //ObjectSpace.Rollback();
            };
            svp.Controllers.Add(dc);
            //svp.NewWindowTarget = NewWindowTarget.Default;
            svp.TargetWindow = TargetWindow.NewModalWindow;
            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));

        }
        #endregion
        
        #region add
        private void _diagram_AddedConnector(object sender, DiagramConnector conn)
        {
            conn.Appearance.BorderSize = 3;
            conn.Appearance.Options.UseBorderSize = true;

            #region 新建

            var obj = Flow.CreateAction((conn.BeginItem as DiagramShape).Tag as IFlowNode, (conn.EndItem as DiagramShape).Tag as IFlowNode);
                //_os.CreateObject<FlowAction>();
            //obj.From = ;
            //obj.To =;
            //obj.Caption = "生成" + obj.To.Caption;
            
            //obj.GenerateMapping(this.Application.Model.BOModel);

            //obj.Flow = (this.PropertyValue as Flow);

            var view = Application.CreateDetailView(_os, obj, false);
            var svp = new ShowViewParameters(view);
            var dc = new DialogController();
            dc.SaveOnAccept = false;
            dc.Accepting += (s, e1) =>
            {
                conn.Tag = obj;
                conn.Text = obj.Caption;
                obj.Save();
                _diagram.ClearSelection();
            };

            dc.Cancelling += (s, e1) =>
            {
                _os.Delete(obj);
                _diagram.DeleteSelectedItems();
            };
            svp.Controllers.Add(dc);
            //svp.NewWindowTarget = NewWindowTarget.Default;
            svp.TargetWindow = TargetWindow.NewModalWindow;
            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));

            #endregion
        }
        private void _diagram_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && this.isDesignMode)
            {
                var curr = _diagram.OptionsBehavior.ActiveTool;
                _diagram.OptionsBehavior.ActiveTool = curr == line ? select : line;
            }
        }

        private void _diagram_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //左键双击，且没有选中任何项目
            if (e.Button == MouseButtons.Left && this._diagram.SelectedItems.Count == 0)
            {
                var pnt = this._diagram.PointToDocument(new DevExpress.Utils.PointFloat(e.X, e.Y));
                ShowNodesEventParameter para = CreateShowNodesEventParameter(pnt,false,null);
                //ShowNodesView(p, false, null);
                Application.ShowViewStrategy.ShowView(para.ViewParameter, new ShowViewSource(null, null));
            }
        }

        ChoiceActionItem FindChoiceActionItemByModelPath(ChoiceActionItemCollection items, string path)
        {
            foreach (var ni in items)
            {
                if ((ni.Model as ModelNode).Path == path)
                {
                    return ni;
                }

                if (ni.Items.Count > 0)
                {
                    var rst = FindChoiceActionItemByModelPath(ni.Items, path);
                    if (rst != null)
                        return rst;
                }
            }
            return null;
        }

        private ShowNodesEventParameter CreateShowNodesEventParameter(PointFloat pnt,bool singleSelect,DiagramShape currentShape)
        {
            var para = new ShowNodesEventParameter();
            para.MouseClickPoint = pnt;
            para.Application = Application;
            para.CreateShape = (x) =>
            {
                var rst = new DiagramShapeEx(SDLDiagramShapes.Procedure, x.X, x.Y, x.Width, x.Height);
                rst.Image = x.GetImage();
                _diagram.Items.Add(rst);
                _diagram.SelectItem(rst);
                return rst;
            };
            para.ObjectSpace = _os;
            para.SelectedForms = _diagram.Items.OfType<DiagramShape>().Select(x => (x.Tag as IFlowNode)).ToArray();
            para.Shape = currentShape;
            para.SingleSelect = singleSelect;
            para.UpdateShape = (node, shape) =>
            {
                var s = shape as DiagramShape;
                s.Content = node.Caption;
                s.Tag = node;
            };
            
            //para.DoShowNavigationItem = (x) =>
            //{
            //    var snc = Application.MainWindow.GetController<SubSystemNavigationController>();
            //    var sname = x.Split('/');
            //    if (sname.Length > 3)
            //        snc.NavigationToSystem(sname[3]);
            //    var ctrl = Application.MainWindow.GetController<ShowNavigationItemController>().ShowNavigationItemAction;
            //    var toItem = FindChoiceActionItemByModelPath(ctrl.Items, x);
            //    ctrl.SelectedItem = toItem;
            //    ctrl.DoExecute(toItem);

            //    //ctrl.FindItemByIdPath()
            //};

            if (currentShape != null)
                para.SelectedNode = currentShape.Tag as IFlowNode;

            para.DeletSelectedNode = () => { _diagram.DeleteSelectedItems(); };
            para.ViewParameter = new ShowViewParameters();
            Flow.ShowNodesView(para);
            return para;
        }

        #endregion

        #region load from db
        protected override void ReadValueCore()
        {
            var curr = Flow;

            isDesignMode = curr.IsDesignMode;

            var flow = (Flow as XPBaseObject);

            if (flow != null)
                flow.Changed += Flow_Changed;

            foreach (var item in curr.Nodes)
            {
                var ds = new DiagramShapeEx(SDLDiagramShapes.Procedure, item.X, item.Y, item.Width,item.Height);
                ds.Content = item.Caption;
                ds.Image = item.GetImage();

                ds.Tag = item;
                _diagram.Items.Add(ds);
            }
            //SelectedForms = curr.Nodes.Select(x => x.Form).ToList();
            foreach (var item in curr.Actions)
            {
                var b = _diagram.Items.Single(x => object.Equals(x.Tag, item.From));
                var e = _diagram.Items.Single(x => object.Equals(x.Tag, item.To));
                var edge = new DiagramConnector(b, e);
                edge.Appearance.BorderSize = 3;
                edge.Appearance.Options.UseBorderSize = true;
                edge.Text = item.Caption;

                //if (item.目标类型 == 目标类型.更新单据)
                //{
                //    edge.Appearance.Options.UseBackColor = true;
                //    edge.Appearance.ForeColor = Color.Red;
                //}
                
                edge.BeginItemPointIndex = item.BeginItemPointIndex;
                edge.EndItemPointIndex = item.EndItemPointIndex;

                edge.Tag = item;
                _diagram.Items.Add(edge);
            }
            if (Flow is IAutoLayout)
            {
                _diagram.ApplyTreeLayout(new TreeLayoutSettings(40.0, 40.0, DevExpress.Diagram.Core.Direction.Down, 20), SplitToConnectedComponentsMode.AllComponents);

            }
        }

        bool isDesignMode;

        private void Flow_Changed(object sender, ObjectChangeEventArgs e)
        {
            if (e.PropertyName == "IsDesignMode")
            {
                UpdateDesignerMode((bool)e.NewValue);
            }
        }

        private void UpdateDesignerMode(bool isDesignMode)
        {
            _diagram.OptionsView.ShowGrid = isDesignMode;
            _diagram.OptionsView.ShowRulers = isDesignMode;
            foreach (IDiagramItem item in _diagram.Items)
            {
                item.CanCopy = isDesignMode;
                item.CanDelete = isDesignMode;
                if (item is DiagramConnector && !isDesignMode)
                {
                    var dc = item as DiagramConnector;
                    dc.CanMove = false;
                }
            }
            
        }

        bool _isRefreshing;
        public override void Refresh()
        {
            _isRefreshing = true;
            _diagram.Items.Clear();
            base.Refresh();
            _isRefreshing = false;
        }
        #endregion

        #region get control value
        protected override object GetControlValueCore()
        {
            return null;
        }
        #endregion

        #region setup
        private IObjectSpace _os;
        private XafApplication Application;
        public void Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this._os = objectSpace;
            this.Application = application;
        }
        #endregion

        #region save to db
        private void _os_Committing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (DiagramItem item in this._diagram.Items)
            {
                if (item.Tag is IFlowNode)
                {
                    var fn = item.Tag as IFlowNode;
                    fn.X = (int)item.Position.X;
                    fn.Y = (int)item.Position.Y;
                    fn.Width = (int)item.Width;
                    fn.Height = (int)item.Height;
                }

                if (item.Tag is IFlowAction)
                {
                    var fa = item.Tag as IFlowAction;
                    var conn = item as DiagramConnector;
                    fa.BeginItemPointIndex = conn.BeginItemPointIndex;
                    fa.EndItemPointIndex = conn.EndItemPointIndex;
                }
            }
            FlowViewController.ActionInfos = null;
        }
        #endregion

    }
}
