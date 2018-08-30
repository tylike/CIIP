using System;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base.General;

namespace IMatrix.ERP.Module.BusinessObjects.SYS.Logic
{
    [DomainComponent]
    public class CodeVariant :ITreeNode
    {
        public CodeVariant(string name,string description,Type type,CodeVariant parent)
        {
            this.Name = name;
            this.Description = description;
            this.Type = type;
            this.Parent = parent;
        }
        
        public string Name { get; set; }

        public string Description { get; set; }
        public Type Type { get; set; }

        public CodeVariant Parent { get; set; }
        
        ITreeNode ITreeNode.Parent
        {
            get
            {
                return this.Parent;
            }
        }
        BindingList<CodeVariant> children;

        IBindingList ITreeNode.Children
        {
            get
            {
                if (children == null)
                {
                    children = new BindingList<CodeVariant>();
                }
                return children;
            }
        }
    }
}