using System;
using System.ComponentModel;
using System.Drawing;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Xpo;
using CIIP.Persistent.BaseImpl;
using DevExpress.ExpressApp.Utils;

namespace CIIP.Module.BusinessObjects.SYS
{
    [XafDisplayName("业务分类")]
    public class Namespace : NameObject, ITreeNode,ITreeNodeImageProvider
    {
        [Association, DevExpress.Xpo.Aggregated]
        public XPCollection<Namespace> Children
        {
            get { return GetCollection<Namespace>("Children"); }
        }

        public override string Name
        {
            get
            {
                return base.Name;
            }

            set
            {
                base.Name = value;
                if (!IsLoading)
                {
                    _fullName = null;
                }
            }
        }

        private Namespace _Parent;
        [Association, DevExpress.Xpo.Aggregated]
        [VisibleInListView(false)]
        public Namespace Parent
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
            get { return Name; }
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
                        _fullName = Parent.FullName + "." + this.Name;
                    }
                    else
                    {
                        _fullName = this.Name;
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

        public Namespace(Session s) : base(s)
        {

        }

        Image ITreeNodeImageProvider.GetImage(out string imageName)
        {
            imageName = "BO_Folder";
            return ImageLoader.Instance.GetImageInfo(imageName).Image;
        }
    }
}