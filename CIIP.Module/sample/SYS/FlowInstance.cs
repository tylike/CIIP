using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Editors;
using CIIP;
using 常用基类;

namespace CIIP.Module.BusinessObjects.SYS
{
    //用于显示工作流实例，单据、状态的变化情况
    [NonPersistent]
    [XafDisplayName("业务流程")]
    public class FlowInstance : BaseObject, IFlow
        //,IAutoLayout
    {
        单据 form;
        public FlowInstance(Session s, 单据 form) : base(s)
        {
            this.form = form;
            var records = Session.Query<单据流程状态记录>().Where(t => t.业务项目 == form.业务项目);
            //var status = Session.Query<状态变更记录>().Where(t => t.单据.业务项目 == form.业务项目);
            _actions = new List<FlowInstanceAction>();
            _nodes = new List<FlowInstanceNode>();

            var nodes = records.Select(x => x.来源单据).Union(records.Select(t => t.目标单据)).OrderBy(x => x.创建时间).Distinct();
            var y = 100;
            //先绘制单据结点
            foreach (var item in nodes)
            {
                var fin = new FlowInstanceNode(Session);
                fin.Caption = CaptionHelper.GetDisplayText(item);

                fin.Width = 200;
                fin.Height = 30;
                fin.X = 100;
                fin.Y = y;
                fin.Key = item;
                fin.ImageName = CaptionHelper.ApplicationModel.BOModel.GetClass(item.GetType()).ImageName;
                _nodes.Add(fin);
                y += 100;
                var xl = 400;

                var status = item.状态记录.OrderBy(x => x.发生日期).Select(x => x.来源状态).Union(item.状态记录.Select(x => x.目标状态)).Where(x => x != null).Distinct();

                foreach (var state in status)
                {
                    var si = new FlowInstanceNode(Session);
                    si.Caption = state.Caption;
                    si.Width = 100;
                    si.Height = 30;
                    si.X = xl;
                    si.Y = fin.Y;
                    si.Key =state;
                    si.Key2 = item;
                    _nodes.Add(si);
                    xl += 350;
                }

                foreach (var sa in item.状态记录)
                {
                    if (sa.来源状态 != null)
                    {
                        var line = new FlowInstanceAction(Session);
                        line.From = _nodes.Single(x => x.Key == sa.来源状态 && x.Key2 == item );
                        line.To = _nodes.Single(x => x.Key == sa.目标状态 && x.Key2 == item );
                        line.BeginItemPointIndex = 1;
                        line.EndItemPointIndex = 3;
                        line.Caption = sa.操作人 + " " + sa.发生日期.ToString("yyyy-MM-dd HH:mmss");
                        _actions.Add(line);
                    }
                }

                if (status.Any())
                {
                    var f = status.FirstOrDefault();
                    var formToState = new FlowInstanceAction(Session);
                    formToState.From = fin;
                    formToState.To = _nodes.Single(x => x.Key == f && x.Key2 == item);
                    formToState.BeginItemPointIndex = 1;
                    formToState.EndItemPointIndex = 3;
                    _actions.Add(formToState);
                }

            }

            foreach (var item in records)
            {
                var action = new FlowInstanceAction(Session);
                action.From = _nodes.Single(x => x.Key == item.来源单据);
                action.To = _nodes.Single(x => x.Key == item.目标单据);
                action.BeginItemPointIndex = 2;
                action.EndItemPointIndex = 0;
                _actions.Add(action);
            }

            ////取所有来源状态，目标状态，不为空的，取除重复。
            //var stateNodes = status.GroupBy(x => x.单据).OrderBy(x => x.Min(t => t.发生日期));

            ////按单据分组的状态数据
            //var y = 50;
            //foreach (var g in stateNodes)
            //{
            //    var gStatus = g.OrderBy(x=>x.发生日期).Select(x => x.来源状态).Union(g.Select(x => x.目标状态)).Where(x => x != null).Distinct();
            //    xl = 100;
            //    foreach (var item in gStatus)
            //    {
            //        var si = new FlowInstanceNode(Session);
            //        si.Caption = item.Caption;
            //        si.Width = 100;
            //        si.Height = 30;
            //        si.X = xl;
            //        si.Y = y;
            //        si.Key = item;
            //        _nodes.Add(si);
            //        xl += 150;
            //    }

            //    foreach (var item in g)
            //    {
            //        if (item.来源状态 != null)
            //        {
            //            var line = new FlowInstanceAction(Session);
            //            line.From = _nodes.SingleOrDefault(x => x.Key == item.来源状态);
            //            line.To = _nodes.SingleOrDefault(x => x.Key == item.目标状态);
            //            line.BeginItemPointIndex = 1;
            //            line.EndItemPointIndex = 3;
            //            _actions.Add(line);
            //        }
            //    }

            //    y += 100;
            //}
            



            //foreach (var item in status)
            //{
            //    var si = new FlowInstanceNode(Session);
            //    if (item.来源状态 != null)
            //        si.Caption = item.来源状态.Caption;
            //    si.Width = 100;
            //    si.Height = 30;
            //    _nodes.Add(si);
            //    var sit = new FlowInstanceNode(Session);
            //    sit.Width = 100;
            //    sit.Height = 30;

            //    sit.Caption = item.目标状态.Caption;
            //    _nodes.Add(sit);
            //    _actions.Add(new FlowInstanceAction(Session) { From = si, To = sit });
            //}



            //如何构建出结点和连接线？
        }

        public IFlow Flow
        {
            get
            {
                return this;
            }
        }

        [Browsable(false)]
        public bool IsDesignMode
        {
            get { return false; }
        }

        List<FlowInstanceAction> _actions;
        [Browsable(false)]
        public IEnumerable<IFlowAction> Actions
        {
            get
            {
                return _actions;
            }
        }

        List<FlowInstanceNode> _nodes;
        [Browsable(false)]
        public IEnumerable<IFlowNode> Nodes
        {
            get
            {
                return _nodes;
            }
        }

        IFlowNode _selectedNode;
        [Browsable(false)]
        public IFlowNode SelectedNode
        {
            get;
            set;
        }

        public IFlowAction CreateAction(IFlowNode from, IFlowNode to)
        {
            return null;
        }

        public IFlowNode CreateNode(int x, int y, int width, int height, string form, string caption)
        {
            return null;
        }

        public void RemoveAction(IFlowAction action)
        {
        }

        public void RemoveNode(IFlowNode node)
        {
        }

        public void ShowNodesView(ShowNodesEventParameter p)
        {
        }
    }
}
