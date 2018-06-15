using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yutang.Form;

using D2D = SharpDX.Direct2D1;
using Mathe = SharpDX.Mathematics.Interop;

namespace Yutang.Layer
{
    internal class Chessboard : ILayer
    {
        private readonly PointF _centerPointF;

        // Board
        private readonly Model.Board _boardRound;
        private readonly Model.Board _boardCentre;

        // Rectangles
        private readonly Mathe.RawRectangleF[,] _rectangle;
        private readonly Mathe.RawRectangleF[,] _rectangle2;

        // Brushes
        private readonly D2D.Brush _whiteBrush;
        private readonly D2D.Brush _blueBrush;
        private readonly D2D.Brush[,] _brushes;
        private readonly D2D.Brush[,] _brushes2;

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

            // Create boards
            _boardRound = new Model.Board(7, 7)
            {
                VisibleBoard = new[,]
                {
                    {0, 0, 1, 1, 1, 0, 0},
                    {0, 1, 1, 0, 1, 1, 0},
                    {1, 1, 0, 0, 0, 1, 1},
                    {1, 0, 0, 0, 0, 0, 1},
                    {1, 1, 0, 0, 0, 1, 1},
                    {0, 1, 1, 0, 1, 1, 0},
                    {0, 0, 1, 1, 1, 0, 0},
                }
            };
            _boardCentre = new Model.Board(5, 5)
            {
                VisibleBoard = new[,]
                {
                    {0, 0, 1, 0, 0},
                    {0, 1, 1, 1, 0},
                    {1, 1, 1, 1, 1},
                    {0, 1, 1, 1, 0},
                    {0, 0, 1, 0, 0},
                }
            };

            // Create rectangles
            _rectangle = new Mathe.RawRectangleF[_boardRound.X, _boardRound.Y];
            _rectangle2 = new Mathe.RawRectangleF[_boardCentre.X, _boardCentre.Y];

            #region Color settings
            _boardRound.SetColor(0, 2, cGreen);
            _boardRound.SetColor(0, 3, cYellow);
            _boardRound.SetColor(0, 4, cRed);

            _boardRound.SetColor(1, 1, cPirple);
            _boardRound.SetColor(1, 2, cBlue);
            _boardRound.SetColor(1, 4, cBlue);
            _boardRound.SetColor(1, 5, cPirple);

            _boardRound.SetColor(2, 0, cGreen);
            _boardRound.SetColor(2, 1, cBlue);
            _boardRound.SetColor(2, 5, cBlue);
            _boardRound.SetColor(2, 6, cGrey);

            _boardRound.SetColor(3, 0, cYellow);
            _boardRound.SetColor(3, 6, cYellow);

            _boardRound.SetColor(4, 0, cRed);
            _boardRound.SetColor(4, 1, cBlue);
            _boardRound.SetColor(4, 5, cBlue);
            _boardRound.SetColor(4, 6, cGreen);

            _boardRound.SetColor(5, 1, cPirple);
            _boardRound.SetColor(5, 2, cBlue);
            _boardRound.SetColor(5, 4, cBlue);
            _boardRound.SetColor(5, 5, cPirple);

            _boardRound.SetColor(6, 2, cGrey);
            _boardRound.SetColor(6, 3, cYellow);
            _boardRound.SetColor(6, 4, cGreen);
            #endregion

            _brushes = new D2D.Brush[_boardRound.X, _boardRound.Y];
            _brushes2 = new D2D.Brush[_boardCentre.X, _boardCentre.Y];

            const float recWidth = 51, margin = 5;
            float widthC = recWidth * _boardRound.X + margin * (_boardRound.X - 1),
                heightC = recWidth * _boardRound.Y + margin * (_boardRound.Y - 1),
                leftC = _centerPointF.X - widthC / 2,
                topC = _centerPointF.Y - heightC / 2;

            for (var i = 0; i < _boardRound.X; i++)
            {
                for (var j = 0; j < _boardRound.Y; j++)
                {
                    if (_boardRound.VisibleBoard[i, j] == 0) continue;
                    float left = leftC + i * (recWidth + margin), top = topC + j * (recWidth + margin);
                    _rectangle[i, j] = new Mathe.RawRectangleF(left, top, left + recWidth, top + recWidth);
                    _brushes[i, j] = new D2D.SolidColorBrush(RenderForm.RenderTarget, _boardRound.Color[i, j]);
                }
            }

            widthC = recWidth * _boardCentre.X + margin * (_boardCentre.X - 1);
            heightC = recWidth * _boardCentre.Y + margin * (_boardCentre.Y - 1);
            leftC = _centerPointF.X - widthC / 2;
            topC = _centerPointF.Y - heightC / 2;

            for (var i = 0; i < _boardCentre.X; i++)
            {
                for (var j = 0; j < _boardCentre.Y; j++)
                {
                    if (_boardCentre.VisibleBoard[i, j] == 0) continue;
                    float left = leftC + i * (recWidth + margin), top = topC + j * (recWidth + margin);
                    _rectangle2[i, j] = new Mathe.RawRectangleF(left, top, left + recWidth, top + recWidth);
                    _brushes2[i, j] = new D2D.SolidColorBrush(RenderForm.RenderTarget, _boardCentre.Color[i, j]);
                }
            }
        }

        public void Measure()
        {

        }

        public void Draw()
        {
            for (var i = 0; i < _boardRound.X; i++)
            {
                for (var j = 0; j < _boardRound.Y; j++)
                {
                    if (_boardRound.VisibleBoard[i, j] == 0) continue;
                    RenderForm.RenderTarget.FillRectangle(_rectangle[i, j], _brushes[i, j]);
                    RenderForm.RenderTarget.DrawRectangle(_rectangle[i, j], _whiteBrush, 1f);
                }
            }

            for (var i = 0; i < _boardCentre.X; i++)
            {
                for (var j = 0; j < _boardCentre.Y; j++)
                {
                    if (_boardCentre.VisibleBoard[i, j] == 0) continue;
                    RenderForm.RenderTarget.FillRectangle(_rectangle2[i, j], _brushes2[i, j]);
                    RenderForm.RenderTarget.DrawRectangle(_rectangle2[i, j], _blueBrush, 3f);
                }
            }
        }

        public void Dispose()
        {
            foreach (var item in _brushes)
                item?.Dispose();

            _whiteBrush.Dispose();
        }
    }
}
