using System;
using DevExpress.Xpo;

namespace CIIP
{
    /// <summary>
    /// 用于逻辑代码模块树，子级逻辑，新建时，可以创建出parentType的派生类
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ChildrenTypeAttribute : Attribute
    {
        public Type ChildrenType { get; set; }
        public ChildrenTypeAttribute(Type childrenType)
        {
            this.ChildrenType = childrenType;
        }
    }
}