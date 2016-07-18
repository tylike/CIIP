using System;
using System.Drawing;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;

namespace CIIP
{
    public class ShowNodesEventParameter
    {
        public ShowViewParameters ViewParameter { get; set; }
        public bool SingleSelect { get; set; }
        public object Shape { get; set; }
        public XafApplication Application { get; set; }

        public PointF MouseClickPoint { get; set; }

        public Func<IFlowNode, object> CreateShape { get; set; }

        public Action<IFlowNode, object> UpdateShape { get; set; }

        public IObjectSpace ObjectSpace { get; set; }

        public IFlowNode[] SelectedForms { get; set; }
        
        /// <summary>
        /// 编辑时
        /// </summary>
        public IFlowNode SelectedNode { get; set; }

        public Action DeletSelectedNode { get; set; }

        public Action<string> DoShowNavigationItem { get; set; }

    }
}