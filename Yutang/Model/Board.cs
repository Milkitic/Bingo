using System;

using D2D = SharpDX.Direct2D1;
using Mathe = SharpDX.Mathematics.Interop;

namespace Yutang.Model
{
    internal class Board
    {
        public int X { get; }
        public int Y { get; }
        public Board(int x, int y)
        {
            X = x;
            Y = y;
            VisibleBoard = new int[x, y];
            //Color = new Mathe.RawColor4[x, y];
            Color = new Mathe.RawColor4[x, y];
            Image = new D2D.Bitmap[x, y];
            VisibleBoard.Initialize();
            Color.Initialize();
        }

        public void SetColor(int x, int y, Mathe.RawColor4 color) => Color[x, y] = color;
        public void SetVisible(int x, int y, bool visible) => VisibleBoard[x, y] = visible ? 1 : 0;
        public void SetImage(int x, int y, D2D.Bitmap bitmap) => Image[x, y] = bitmap;

        public Mathe.RawColor4[,] Color
        {
            get => _color;
            set
            {
                if (value.GetLength(0) != X || value.GetLength(1) != Y)
                    throw new IndexOutOfRangeException();
                _color = value;
            }
        }

        public int[,] VisibleBoard
        {
            get => _visibleBoard;
            set
            {
                if (value.GetLength(0) != X || value.GetLength(1) != Y)
                    throw new IndexOutOfRangeException(value.GetLength(0) + "," + value.GetLength(1));
                _visibleBoard = value;
            }
        }
        public D2D.Bitmap[,] Image
        {
            get => _image;
            set
            {
                if (value.GetLength(0) != X || value.GetLength(1) != Y)
                    throw new IndexOutOfRangeException(value.GetLength(0) + "," + value.GetLength(1));
                _image = value;
            }
        }

        private Mathe.RawColor4[,] _color;
        private int[,] _visibleBoard;
        private D2D.Bitmap[,] _image;
    }
}
