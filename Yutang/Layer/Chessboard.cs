using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yutang.Form;
using Yutang.Model;
using D2D = SharpDX.Direct2D1;
using Mathe = SharpDX.Mathematics.Interop;
using WIC = SharpDX.WIC;
using DXIO = SharpDX.IO;

namespace Yutang.Layer
{
    internal class Chessboard : ILayer
    {
        private readonly PointF _centerPointF;

        // Board
        private List<Board> _boards = new List<Board>();
        //private readonly Board _boardRound;
        //private readonly Board _boardCentre;

        // Rectangles
        private readonly List<Mathe.RawRectangleF[,]> _rectangles = new List<Mathe.RawRectangleF[,]>();

        // Brushes
        private readonly D2D.Brush _whiteBrush;
        private readonly D2D.Brush _blueBrush;
        private readonly List<D2D.Brush[,]> _brushes = new List<D2D.Brush[,]>();


        private readonly string _resPath = Path.Combine(Environment.CurrentDirectory, "templet");

        public Chessboard()
        {
            _centerPointF = new PointF(RenderForm.Width / 2f, RenderForm.Height / 2f);

            // Create colors
            var cRed = new Mathe.RawColor4(237 / 255f, 28 / 255f, 36 / 255f, 1);
            var cGreen = new Mathe.RawColor4(34 / 255f, 177 / 255f, 76 / 255f, 1);
            var cYellow = new Mathe.RawColor4(255 / 255f, 243 / 255f, 0 / 255f, 1);
            var cPirple = new Mathe.RawColor4(163 / 255f, 72 / 255f, 165 / 255f, 1);
            var cBlue = new Mathe.RawColor4(0 / 255f, 163 / 255f, 233 / 255f, 1);
            var cGrey = new Mathe.RawColor4(195 / 255f, 195 / 255f, 195 / 255f, 1);
            var cWhite = new Mathe.RawColor4(1, 1, 1, 1);

            // Create brushes
            _whiteBrush = new D2D.SolidColorBrush(RenderForm.RenderTarget, cWhite);
            _blueBrush = new D2D.SolidColorBrush(RenderForm.RenderTarget, cBlue);

            LoadSettings();
   
            // Create rectangles
            for (var index = 0; index < _boards.Count; index++)
            {
                var item = _boards[index];
                _rectangles.Add(new Mathe.RawRectangleF[item.X, item.Y]);
                _brushes.Add(new D2D.Brush[item.X, item.Y]);
                const float recWidth = 51, margin = 5;

                float widthC = recWidth * item.X + margin * (item.X - 1),
                    heightC = recWidth * item.Y + margin * (item.Y - 1),
                    leftC = _centerPointF.X - widthC / 2,
                    topC = _centerPointF.Y - heightC / 2;

                for (var i = 0; i < item.X; i++)
                {
                    for (var j = 0; j < item.Y; j++)
                    {
                        if (item.VisibleBoard[i, j] == 0) continue;
                        float left = leftC + i * (recWidth + margin), top = topC + j * (recWidth + margin);
                        _rectangles[index][i, j] = new Mathe.RawRectangleF(left, top, left + recWidth, top + recWidth);
                        _brushes[index][i, j] = new D2D.SolidColorBrush(RenderForm.RenderTarget, item.Color[i, j]);
                    }
                }
            }

            #region Image settings
            //_boardCentre.SetImage(0, 2, LoadFromFile(Path.Combine(_resPath, "13.png")));

            //_boardCentre.SetImage(1, 1, LoadFromFile(Path.Combine(_resPath, "1A.png")));
            //_boardCentre.SetImage(1, 2, LoadFromFile(Path.Combine(_resPath, "6.png")));
            //_boardCentre.SetImage(1, 3, LoadFromFile(Path.Combine(_resPath, "9.png")));

            //_boardCentre.SetImage(2, 0, LoadFromFile(Path.Combine(_resPath, "12.png")));
            //_boardCentre.SetImage(2, 1, LoadFromFile(Path.Combine(_resPath, "2.png")));
            //_boardCentre.SetImage(2, 2, LoadFromFile(Path.Combine(_resPath, "7C.png")));
            //_boardCentre.SetImage(2, 3, LoadFromFile(Path.Combine(_resPath, "8.png")));
            //_boardCentre.SetImage(2, 4, LoadFromFile(Path.Combine(_resPath, "10.png")));

            //_boardCentre.SetImage(3, 1, LoadFromFile(Path.Combine(_resPath, "3.png")));
            //_boardCentre.SetImage(3, 2, LoadFromFile(Path.Combine(_resPath, "4.png")));
            //_boardCentre.SetImage(3, 3, LoadFromFile(Path.Combine(_resPath, "5.png")));

            //_boardCentre.SetImage(4, 2, LoadFromFile(Path.Combine(_resPath, "11.png")));
            #endregion
        }

        public void Measure()
        {

        }

        public void Draw()
        {
            for (var index = 0; index < _boards.Count; index++)
            {
                var item = _boards[index];
                for (var i = 0; i < item.X; i++)
                {
                    for (var j = 0; j < item.Y; j++)
                    {
                        if (item.VisibleBoard[i, j] == 0) continue;
                        RenderForm.RenderTarget.FillRectangle(_rectangles[index][i, j], _brushes[index][i, j]);
                        if (item.Image[i, j] != null)
                            RenderForm.RenderTarget.DrawBitmap(item.Image[i, j], _rectangles[index][i, j], 1,
                                D2D.BitmapInterpolationMode.Linear);
                        RenderForm.RenderTarget.DrawRectangle(_rectangles[index][i, j], _whiteBrush, 1f);
                    }
                }
            }
        }

        public void Dispose()
        {
            foreach (var item in _brushes)
                foreach (var item2 in item)
                    item2?.Dispose();

            _whiteBrush.Dispose();
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

        private void LoadSettings()
        {
            for (var i = 0; i < Program.MainSettings.BoardInfomation.Count; i++)
            {
                var item = Program.MainSettings.BoardInfomation[i];
                _boards.Add(new Board(item.Width, item.Height));
                _boards[i].VisibleBoard = item.VisibleBoard;
                for (var j = 0; j < item.BoardPointInfomation.Count; j++)
                {
                    var item2 = item.BoardPointInfomation[j];
                    _boards[i].SetColor(item2.X, item2.Y, item2.Color);
                    _boards[i].SetVisible(item2.X, item2.Y, item2.Visible);
                    if (item2.ImagePath != null) _boards[i].SetImage(item2.X, item2.Y, LoadFromFile(Path.Combine(_resPath, item2.ImagePath)));
                }
            }
        }
    }
}