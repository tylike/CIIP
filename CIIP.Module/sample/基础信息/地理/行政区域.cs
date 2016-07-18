using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("往来单位")]

    public class 行政区域 : NameObject, ITreeNode
    {
        public 行政区域(Session s) : base(s)
        {

        }

        IBindingList ITreeNode.Children
        {
            get
            {
                return 子级;
            }
        }

        string ITreeNode.Name
        {
            get
            {
                return this.名称;
            }
        }


        //private int _RegionID;

        //public int RegionID
        //{
        //    get { return _RegionID; }
        //    set { SetPropertyValue("RegionID", ref _RegionID, value); }
        //}


        //private int _ParentRegionID;

        //public int ParentRegionID
        //{
        //    get { return _ParentRegionID; }
        //    set { SetPropertyValue("ParentRegionID", ref _ParentRegionID, value); }
        //}
        
        [Association, DevExpress.Xpo.Aggregated]
        public XPCollection<行政区域> 子级
        {
            get
            {
                return GetCollection<行政区域>("子级");
            }
        }


        private int _级别;

        public int 级别
        {
            get { return _级别; }
            set { SetPropertyValue("级别", ref _级别, value); }
        }
        
        private 行政区域 _上级;
        [Association]
        public 行政区域 上级
        {
            get { return _上级; }
            set { SetPropertyValue("上级", ref _上级, value); }
        }



        ITreeNode ITreeNode.Parent
        {
            get
            {
                return 上级;
            }
        }
    }
}