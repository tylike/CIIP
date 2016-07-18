using System.ComponentModel;
using System.Drawing;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base.General;

namespace CIIP.Module.BusinessObjects.SYS
{
    [DomainComponent]
    public class NavigationItem : ITreeNode,ITreeNodeImageProvider
    {
        public string Key
        {
            get
            {
                if(ModelItem != null)
                {
                    return (ModelItem as ModelNode).Path;
                }
                return "";
            }
        }

        public IModelNavigationItem ModelItem { get; }
        public NavigationItem(IModelNavigationItem modelItem)
        {
            this.ModelItem = modelItem;
        }
        BindingList<NavigationItem> _childrens;
        public IBindingList Children
        {
            get
            {
                if (_childrens == null)
                {
                    _childrens = new BindingList<NavigationItem>();
                    foreach (var item in ModelItem.Items)
                    {
                        _childrens.Add(new NavigationItem(item));
                    }
                }
                return _childrens;
            }
        }

        public string Name
        {
            get
            {
                return ModelItem.Caption;
            }
        }
        NavigationItem _parent;
        public ITreeNode Parent
        {
            get
            {
                if (ModelItem.Parent.Parent != null && _parent == null)
                {
                    if (ModelItem.Parent.Parent is IModelNavigationItem)
                        _parent = new NavigationItem(ModelItem.Parent.Parent as IModelNavigationItem);
                }
                return _parent;
            }
        }

        [XafDisplayName("ÐòºÅ")]
        public int? Index
        {
            get { return ModelItem.Index; }
        }
        
        public Image GetImage(out string imageName)
        {
            imageName = "";
            if(ModelItem!=null)
            {
                imageName = ModelItem.ImageName;
                return ImageLoader.Instance.GetImageInfo(imageName).Image;
            }
            return null;
        }
    }
}