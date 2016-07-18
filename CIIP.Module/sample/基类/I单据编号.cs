namespace IMatrix.ERP.Module.BusinessObjects
{
    /// <summary>
    /// 用于具体单据上使用，实现此接口即可以根据规则生成编码
    /// 没有使用此接口的，可以使用单据方案进行配置
    /// </summary>
    public interface I单据编号
    {
        // Properties
        string 编号 { get; set; }
    }
}