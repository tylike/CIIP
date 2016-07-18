using System;

namespace CIIP.Module.BusinessObjects.Flow
{
    public abstract class ExecuteFlowAction<TMaster,FMaster,TItem,FItem>:IExecuteFlowAction
    {


        public abstract void ExecuteMaster(FMaster fMaster,TMaster tMaster);
        public abstract void ExecuteChildren(FItem fitem,TItem tItem);

        void IExecuteFlowAction.ExecuteMasterCore(object fMaster, object tMaster)
        {
            this.ExecuteMaster((FMaster)fMaster, (TMaster)tMaster);
        }

        void IExecuteFlowAction.ExecuteChildrenCore(object fitem, object tItem)
        {
            this.ExecuteChildren((FItem)fitem, (TItem)tItem);
        }
    }
}