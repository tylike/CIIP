using System;
using System.ComponentModel;
using System.Drawing;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Xpo;
using DevExpress.ExpressApp;

namespace IMatrix.ERP.Module.BusinessObjects.Flow
{
    [XafDisplayName("业务对象")]
    [CreatableItem(true)]
    public class BusinessCodeObject : CodeUnitTreeNode
    {
        public BusinessCodeObject(Session s) : base(s)
        {

        }


        [Association, DevExpress.Xpo.Aggregated]
        public XPCollection<MethodDefine> Events
        {
            get { return GetCollection<MethodDefine>("Events"); }
        }

        public override Image GetImage(out string imageName)
        {
            imageName = "ModelEditor_Class_Object";
            return ImageLoader.Instance.GetImageInfo(imageName).Image;
        }

        public override IBindingList Children
        {
            get { return Events; }
        }
    }

    /// <summary>
    /// 为了保证树形结构的显示
    /// 所有代码都是继承自本类的
    /// </summary>
    [XafDisplayName("业务逻辑")]
    [DefaultListViewOptions(MasterDetailMode.ListViewAndDetailView)]
    public abstract class CodeUnitTreeNode : NameObject, ITreeNode, ITreeNodeImageProvider
    {
        string ITreeNode.Name
        {
            get { return this.名称; }
        }

        ITreeNode ITreeNode.Parent
        {
            get { return this.Category; }
        }

        [Browsable(false)]
        public virtual IBindingList Children
        {
            get { return this.Items; }
        }

        public CodeUnitTreeNode(Session s) : base(s)
        {

        }

        private CodeUnitTreeNode _category;

        [Association,Browsable(false)]
        public CodeUnitTreeNode Category
        {
            get { return _category; }
            set { SetPropertyValue("Category", ref _category, value); }
        }

        [Association, Browsable(false)]
        public XPCollection<CodeUnitTreeNode> Items
        {
            get { return GetCollection<CodeUnitTreeNode>("Items"); }
        }

        public virtual Image GetImage(out string imageName)
        {
            imageName = "BO_Folder";
            return ImageLoader.Instance.GetImageInfo(imageName).Image;
        }
    }




}