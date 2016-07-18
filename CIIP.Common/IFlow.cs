using System.Collections.Generic;

namespace CIIP
{
    public interface IFlow
    {
        IFlowNode SelectedNode { get; set; }

        IEnumerable<IFlowNode> Nodes { get; }

        IEnumerable<IFlowAction> Actions { get; }

        IFlowAction CreateAction(IFlowNode from, IFlowNode to);

        IFlowNode CreateNode(int x, int y, int width, int height, string form, string caption);
        void RemoveNode(IFlowNode node);
        void RemoveAction(IFlowAction action);

        void ShowNodesView(ShowNodesEventParameter p);

        bool IsDesignMode { get; }
    }
}