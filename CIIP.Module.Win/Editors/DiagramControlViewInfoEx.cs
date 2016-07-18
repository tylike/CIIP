using DevExpress.XtraDiagram;
using DevExpress.XtraDiagram.Paint;
using DevExpress.XtraDiagram.ViewInfo;

namespace CIIP.Module.Win.Editors
{
    public class DiagramControlViewInfoEx : DiagramControlViewInfo
    {
        public DiagramControlViewInfoEx(DiagramControl owner)
            : base(owner)
        {
        }


        protected override DiagramShapePainter CreateShapePainter()
        {
            return new DefaultDiagramShapePainterEx();
        }
    }
}