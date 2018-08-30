using System.Drawing;
using DevExpress.Utils.Drawing;
using DevExpress.XtraDiagram.Paint;

namespace CIIP.Module.Win.Editors
{
    public class DefaultDiagramShapePainterEx : DefaultDiagramShapePainter
    {
        public override void DrawObject(ObjectInfoArgs e)
        {
            //base.DrawObject(e);

            if (e is DiagramShapeObjectInfoArgs)
            {
                DiagramShapeObjectInfoArgs args = (DiagramShapeObjectInfoArgs)e;
                if (args.Item is DiagramShapeEx)
                {
                    DiagramShapeEx imageShape = args.Item as DiagramShapeEx;
                    if (imageShape == null)
                        return;
                    var newImage = imageShape.Image;
                    if (newImage != null)
                    {
                        var w = 64;
                        var left = e.Bounds.Left + (e.Bounds.Width - w)/2;
                        var top = e.Bounds.Top + (e.Bounds.Height - w)/2;

                        e.Graphics.DrawImage(newImage, left, top, w, w);

                        var drawFont = new Font("Arial", 8);
                        var drawBrush = new SolidBrush(Color.Black);
                        var drawFormat = new StringFormat();
                        var ms = e.Graphics.MeasureString(imageShape.Content, drawFont);
                        var x = (e.Bounds.Width - ms.Width) / 2;

                        e.Graphics.DrawString(imageShape.Content, drawFont, drawBrush,e.Bounds.X+x,e.Bounds.Y+e.Bounds.Height,
                            //new RectangleF(e.Bounds.X + x, e.Bounds.Y + e.Bounds.Height, e.Bounds.Width, 50),
                            drawFormat);
                    }
                    else
                    {
                        base.DrawObject(e);
                    }
                    
                    //e.Graphics.DrawString(imageShape.Content, newFont, Brushes.Black, new PointF(10, 10));
                }

            }

        }
    }
}