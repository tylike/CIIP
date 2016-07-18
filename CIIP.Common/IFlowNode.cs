using System.Drawing;

namespace CIIP
{
    public interface IFlowNode : IPersistentObject
    {
        int X { get; set; }
        int Y { get; set; }
        int Width { get; set; }
        int Height { get; set; }

        Image GetImage();

        //string Form { get; set; }

    }
}