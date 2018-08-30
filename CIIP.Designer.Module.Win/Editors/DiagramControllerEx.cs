using DevExpress.Diagram.Core;
using DevExpress.XtraDiagram;

namespace CIIP.Module.Win.Editors
{
    public class DiagramControllerEx : DiagramController
    {
        public DiagramControllerEx(DiagramControl diagram)
            : base(diagram)
        {

        }
        protected override void OnItemAdded(IDiagramItem item)
        {
           
            base.OnItemAdded(item);

        }

        protected override void OnItemRemoved(IDiagramItem item)
        {
            base.OnItemRemoved(item);
        }

    }
}