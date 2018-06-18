using System.Collections.Generic;
using System.Drawing;
using Mathe = SharpDX.Mathematics.Interop;

namespace Yutang.Layer
{
    public interface ILayer
    {
        List<Mathe.RawRectangleF> Rectangles { get; }
        void Measure();
        void Draw();
        void Dispose();
        void OnClicked(Point point, Mathe.RawRectangleF recF);
    }
}
