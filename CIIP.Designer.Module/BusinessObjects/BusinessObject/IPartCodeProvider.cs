namespace CIIP.Module.BusinessObjects.SYS
{
    /// <summary>
    /// 是否是局部代码
    /// </summary>
    public interface IPartCodeProvider
    {
        /// <summary>
        /// 替换方法新代码到整个类文档中去
        /// </summary>
        /// <param name="allcode">文档代码</param>
        /// <param name="newCode">方法代码</param>
        /// <returns></returns>
        string ReplaceNewCode(string allcode, string newCode);

        string DefaultLocation { get; }
    }
}