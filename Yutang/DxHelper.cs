using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yutang.Forms;

namespace Yutang
{
    internal static class DxHelper
    {
        public static SharpDX.Direct2D1.Bitmap LoadFromFile(string filePath)
        {
            SharpDX.WIC.ImagingFactory imagingFactory = new SharpDX.WIC.ImagingFactory();
            SharpDX.IO.NativeFileStream fileStream = new SharpDX.IO.NativeFileStream(filePath,
                SharpDX.IO.NativeFileMode.Open, SharpDX.IO.NativeFileAccess.Read);

            SharpDX.WIC.BitmapDecoder bitmapDecoder =
                new SharpDX.WIC.BitmapDecoder(imagingFactory, fileStream, SharpDX.WIC.DecodeOptions.CacheOnDemand);
            SharpDX.WIC.BitmapFrameDecode frame = bitmapDecoder.GetFrame(0);

            SharpDX.WIC.FormatConverter converter = new SharpDX.WIC.FormatConverter(imagingFactory);
            converter.Initialize(frame, SharpDX.WIC.PixelFormat.Format32bppPRGBA);

            return SharpDX.Direct2D1.Bitmap.FromWicBitmap(RenderForm.RenderTarget, converter);
        }
    }
}
