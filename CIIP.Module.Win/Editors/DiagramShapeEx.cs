using System;
using System.Collections.Generic;
using System.Drawing;
using DevExpress.Diagram.Core;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.XtraDiagram;
using DevExpress.XtraDiagram.Base;
using DevExpress.XtraDiagram.ViewInfo;

namespace CIIP.Module.Win.Editors
{
    public class DiagramShapeEx : DiagramShape
    {
        public DiagramShapeEx(ShapeDescription shape, int x, int y, int width, int height) : base(shape, x, y, width, height) { }
        protected override IDiagramItemView CreateView(DiagramControlViewInfo viewInfo, DiagramAppearanceObject appearance) {
            return new DiagramShapeViewEx(DevExpress.XtraDiagram.Native.ShapeParser.Parse(Controller.Shape, appearance), () => viewInfo.OwnerControl.OptionsView.AllowShapeShadows, appearance, Content, RoundToRectangle(ConvertHelper.ToWinRect(Controller.EditorBounds)), this);
        }

        public Rectangle RoundToRectangle(RectangleF rect) {
            var x = (int)Math.Round(rect.X);
            var y = (int)Math.Round(rect.Y);
            return new Rectangle(x, y, (int)Math.Round(rect.Right) - x, (int)Math.Round(rect.Bottom) - y);
        }

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
    public sealed class DiagramShapeViewEx : DiagramShapeView, IDiagramItemView {
        RectangleF TextBounds;
        DiagramShapeEx Shape;

        public DiagramShapeViewEx(IEnumerable<DiagramGraphicsPath> paths, Func<bool> allowDrawShadows, AppearanceObject textAppearance, string text, RectangleF textBounds, DiagramShapeEx shape)
            : base(paths, allowDrawShadows, textAppearance, text, textBounds) {
            Shape = shape;
            TextBounds = textBounds;
        }
        void IDiagramItemView.Draw(GraphicsCache cache, DiagramItemDrawArgs args) {
            base.Draw(cache, args);
            cache.Graphics.DrawImage(Shape.Image, TextBounds);
        }
    }
}