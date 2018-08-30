namespace CIIP.Module.BusinessObjects.Flow
{
    public interface IExecuteFlowAction
    {
        void ExecuteMasterCore(object fMaster, object tMaster);

        void ExecuteChildrenCore(object fitem, object tItem);
    }
}