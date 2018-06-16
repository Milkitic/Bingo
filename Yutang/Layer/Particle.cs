using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    internal class Particle : ILayer
    {
        private readonly Mathe.RawVector2[] _startPos;

        private readonly Mathe.RawVector2[] _nowPos;

        // Bitmap
        private readonly D2D.Bitmap[] _oriBitmaps;
        private readonly D2D.Bitmap[] _bitmaps;

        // Speeds (n px/ms)
        private readonly float[] _speeds;

        // Timing (n ms)
        private readonly float[] _timings;

        // radius
        private readonly float[] _r;

        // fade
        private readonly float[] _startF;
        private readonly float[] _f;

        // shake
        private readonly float[] _shakeOffset;
        private readonly float[] _shakeCycle;


        private readonly Stopwatch _sw;

        private readonly int _particleCount;
        private readonly Random _rnd = new Random();

        public Particle(int count)
        {
            _particleCount = count;

            // Load bitmap
            FileInfo[] fis = new DirectoryInfo("element").GetFiles("*.png", SearchOption.TopDirectoryOnly);
            _oriBitmaps = new D2D.Bitmap[fis.Length];
            for (int i = 0; i < _oriBitmaps.Length; i++)
            {
                _oriBitmaps[i] = LoadFromFile(fis[i].FullName);
            }

            _startPos = new Mathe.RawVector2[count];
            _nowPos = new Mathe.RawVector2[count];
            _speeds = new float[count];
            _timings = new float[count];
            _bitmaps = new D2D.Bitmap[count];

            _r = new float[count];
            _startF = new float[count];
            _f = new float[count];
            _shakeOffset = new float[count];
            _shakeCycle = new float[count];

            for (int i = 0; i < count; i++)
            {
                _bitmaps[i] = _oriBitmaps[_rnd.Next(0, _oriBitmaps.Length)];
                _r[i] = (float)(_rnd.NextDouble() * 20);
                _startF[i] = (float)_rnd.NextDouble();
                _speeds[i] = _r[i] * _r[i] * 0.0005f;
                _timings[i] = 1 / _speeds[i] * 1000;
                _startPos[i] =
                    new Mathe.RawVector2(_rnd.Next(0, RenderForm.Width), RenderForm.Height + _rnd.Next(40, 50));
                _shakeCycle[i] = 1 / _speeds[i] * 500;
                _shakeOffset[i] = (float)_rnd.NextDouble() * _shakeCycle[i];
            }

            _sw = new Stopwatch();
            _sw.Restart();
        }

        public void Measure()
        {
            for (int i = 0; i < _particleCount; i++)
            {
                float ratio = (_sw.ElapsedMilliseconds % _timings[i]) / _timings[i];

                float ratio2 = ((_sw.ElapsedMilliseconds + _shakeOffset[i]) % _shakeCycle[i]) / _shakeCycle[i];
                _f[i] = _startF[i] - ratio * _startF[i];
                _nowPos[i] = new Mathe.RawVector2(_startPos[i].X + (float)Math.Sin(ratio2 * 2 * Math.PI) * 10,
                    _startPos[i].Y - _sw.ElapsedMilliseconds % _timings[i] * _speeds[i]);
            }
        }

        public void Draw()
        {
            Measure();

            for (int i = 0; i < _particleCount; i++)
            {
                if (_nowPos[i].Y < RenderForm.Height + 20 && _nowPos[i].Y > -20)
                    RenderForm.RenderTarget.DrawBitmap(_bitmaps[i],
                        new Mathe.RawRectangleF(_nowPos[i].X, _nowPos[i].Y, _nowPos[i].X + _r[i], _nowPos[i].Y + _r[i]),
                        _f[i], D2D.BitmapInterpolationMode.NearestNeighbor);
            }

        }

        public void Dispose()
        {

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
