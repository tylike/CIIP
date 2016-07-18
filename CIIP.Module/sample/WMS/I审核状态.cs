namespace CIIP.Module.BusinessObjects
{
    public interface I审核状态
    {
        审核类型 审核状态 { get; set; }

        void OnChecked();
        void OnUnChecked();
    }
}