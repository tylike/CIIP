using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using CIIP;

namespace CIIP.Module.BusinessObjects.SYS
{
    [XafDisplayName("连接线段")]
    [XafDefaultProperty("Caption")]

    public class NavigationLine : BaseObject, IFlowAction
    {
        public NavigationLine(Session s) : base(s)
        {

        }

        private NavigationMenu _NavigationMenu;

        [Browsable(false)]
        [Association]
        public NavigationMenu NavigationMenu
        {
            get { return _NavigationMenu; }
            set { SetPropertyValue("NavigationMenu", ref _NavigationMenu, value); }
        }



        private int _BeginItemPointIndex;
        [XafDisplayName("开始点号")]
        public int BeginItemPointIndex
        {
            get { return _BeginItemPointIndex; }
            set { SetPropertyValue("BeginItemPointIndex", ref _BeginItemPointIndex, value); }
        }

        private string _Caption;
        [XafDisplayName("标题")]
        public string Caption
        {
            get { return _Caption; }
            set { SetPropertyValue("Caption", ref _Caption, value); }
        }

        private int _EndItemPointIndex;
        [XafDisplayName("结束点号")]
        public int EndItemPointIndex
        {
            get { return _EndItemPointIndex; }
            set { SetPropertyValue("EndItemPointIndex", ref _EndItemPointIndex, value); }
        }

        private NavigationMenuItem _From;
        [XafDisplayName("来源")]
        public NavigationMenuItem From
        {
            get { return _From; }
            set { SetPropertyValue("From", ref _From, value); }
        }

        private NavigationMenuItem _To;
        [XafDisplayName("目标")]
        public NavigationMenuItem To
        {
            get { return _To; }
            set { SetPropertyValue("To", ref _To, value); }
        }

        IFlowNode IFlowAction.From
        {
            get { return From; }

            set { From = (NavigationMenuItem) value; }
        }

        IFlowNode IFlowAction.To
        {
            get { return To; }

            set { To = (NavigationMenuItem) value; }
        }
    }
}