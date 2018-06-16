using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yutang.Form;

using D2D = SharpDX.Direct2D1;
using WIC = SharpDX.WIC;
using Mathe = SharpDX.Mathematics.Interop;
using DXIO = SharpDX.IO;

namespace Yutang.Layer
{
    internal class Background : ILayer
    {
        private D2D.Bitmap _bgBitmap;
        private readonly string _resPath = Path.Combine(Environment.CurrentDirectory, "templet");

        public Background()
        {
            _bgBitmap = LoadFromFile(Path.Combine(_resPath, Program.MainSettings.Background));
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

        public void Dispose()
        {
            _bgBitmap.Dispose();
        }

        private static D2D.Bitmap LoadFromFile(string filePath)
        {
            WIC.ImagingFactory imagingFactory = new WIC.ImagingFactory();
            DXIO.NativeFileStream fileStream = new DXIO.NativeFileStream(filePath,
                DXIO.NativeFileMode.Open, DXIO.NativeFileAccess.Read);

            WIC.BitmapDecoder bitmapDecoder =
                new WIC.BitmapDecoder(imagingFactory, fileStream, WIC.DecodeOptions.CacheOnDemand);
            WIC.BitmapFrameDecode frame = bitmapDecoder.GetFrame(0);

            WIC.FormatConverter converter = new WIC.FormatConverter(imagingFactory);
            converter.Initialize(frame, WIC.PixelFormat.Format32bppPRGBA);

            return D2D.Bitmap.FromWicBitmap(RenderForm.RenderTarget, converter);
        }
    }
}
