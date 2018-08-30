using System.ComponentModel;
using DevExpress.Persistent.Base.General;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects
{
    public class 客户分类 : NameObject, ITreeNode
    {
        public 客户分类(Session s) : base(s)
        {
        }

        private 客户分类 _上级;

        [Association]
        public 客户分类 上级
        {
            get { return _上级; }
            set { SetPropertyValue("上级", ref _上级, value); }
        }

        [Association, Aggregated]
        public XPCollection<客户分类> 子级
        {
            get { return GetCollection<客户分类>("子级"); }
        }

        IBindingList ITreeNode.Children
        {
            get { return 子级; }
        }

        string ITreeNode.Name
        {
            get { return 名称; }
        }

        ITreeNode ITreeNode.Parent
        {
            get { return 上级; }
        }
    }
}