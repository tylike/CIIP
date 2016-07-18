namespace CIIP
{
    public interface IFlowAction : IPersistentObject
    {
        IFlowNode From { get; set; }
        IFlowNode To { get; set; }

        int BeginItemPointIndex { get; set; }
        int EndItemPointIndex { get; set; }
    }
}