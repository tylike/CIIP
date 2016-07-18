using System;
using System.ComponentModel;
using System.Drawing;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Xpo;
using 常用基类;
using DevExpress.ExpressApp.Utils;

namespace CIIP.Module.BusinessObjects.SYS
{
    [XafDisplayName("业务分类")]
    public class NameSpace : NameObject, ITreeNode,ITreeNodeImageProvider
    {
        [Association, DevExpress.Xpo.Aggregated]
        public XPCollection<NameSpace> Children
        {
            get { return GetCollection<NameSpace>("Children"); }
        }

        public override string 名称
        {
            get
            {
                return base.名称;
            }

            set
            {
                base.名称 = value;
                if (!IsLoading)
                {
                    _fullName = null;
                }
            }
        }

        private NameSpace _Parent;
        [Association, DevExpress.Xpo.Aggregated]
        [VisibleInListView(false)]
        public NameSpace Parent
        {
            get { return _Parent; }
            set
            {
                SetPropertyValue("Parent", ref _Parent, value);
                if (!IsLoading)
                {
                    _fullName = null;
                }
            }
        }
        
        string ITreeNode.Name
        {
            get { return 名称; }
        }

        ITreeNode ITreeNode.Parent
        {
            get
            {
                return Parent;
            }
        }

        IBindingList ITreeNode.Children
        {
            get { return Children; }
        }

        string _fullName;

        [ModelDefault("AllowEdit","False")]
        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(_fullName))
                {
                    if (Parent != null)
                    {
                        _fullName = Parent.FullName + "." + this.名称;
                    }
                    else
                    {
                        _fullName = this.名称;
                    }
                }
                return _fullName;
            }
            set
            {
                if (IsLoading)
                {
                    SetPropertyValue("FullName", ref _fullName, value);
                }
            }
        }

        public NameSpace(Session s) : base(s)
        {

        }

        Image ITreeNodeImageProvider.GetImage(out string imageName)
        {
            imageName = "BO_Folder";
            return ImageLoader.Instance.GetImageInfo(imageName).Image;
        }
    }
}