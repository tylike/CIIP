using System.Drawing;
using DevExpress.Diagram.Core;
using DevExpress.XtraDiagram;

namespace CIIP.Module.Win.Editors
{
    public class DiagramShapeEx : DiagramShape
    {
        public DiagramShapeEx(ShapeDescription shape, int x, int y, int width, int height) : base(shape, x, y, width, height) { }


        Image image;
        public Image Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                this.OnPropertiesChanged();
            }
        }
    }
}