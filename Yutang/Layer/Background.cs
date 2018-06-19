using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Yutang.Forms;
using D2D = SharpDX.Direct2D1;
using WIC = SharpDX.WIC;
using Mathe = SharpDX.Mathematics.Interop;
using DXIO = SharpDX.IO;

namespace Yutang.Layer
{
    internal class Background : ILayer
    {
        public List<Mathe.RawRectangleF> Rectangles { get; } = new List<Mathe.RawRectangleF>();

        private D2D.Bitmap _bgBitmap;
        private readonly string _resPath = Path.Combine(Environment.CurrentDirectory, "templet");

        public Background()
        {
            _bgBitmap = DxHelper.LoadFromFile(Path.Combine(_resPath, Program.MainSettings.Background));
        }

        public void Measure()
        {

        }

        public void Draw()
        {
            RenderForm.RenderTarget.DrawBitmap(_bgBitmap,
                new Mathe.RawRectangleF(0, 0, RenderForm.Width, RenderForm.Height), 1,
                D2D.BitmapInterpolationMode.Linear);
        }

        public void OnClicked(MouseEventArgs e, Mathe.RawRectangleF recF)
        {

        }

        public void Dispose()
        {
            _bgBitmap.Dispose();
        }
    }
}
