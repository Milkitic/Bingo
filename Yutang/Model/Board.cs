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
            ImageNormal = new D2D.Bitmap[x, y];
            ImageActive = new D2D.Bitmap[x, y];
            IsActive = new bool[x, y];
            VisibleBoard.Initialize();
            Color.Initialize();
        }

        public void SetColor(int x, int y, Mathe.RawColor4 color) => Color[x, y] = color;
        public void SetVisible(int x, int y, bool visible) => VisibleBoard[x, y] = visible ? 1 : 0;
        public void SetNormalImage(int x, int y, D2D.Bitmap bitmap) => ImageNormal[x, y] = bitmap;
        public void SetActiveImage(int x, int y, D2D.Bitmap bitmap) => ImageActive[x, y] = bitmap;

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
        public D2D.Bitmap[,] ImageNormal
        {
            get => _imageNormal;
            set
            {
                if (value.GetLength(0) != X || value.GetLength(1) != Y)
                    throw new IndexOutOfRangeException(value.GetLength(0) + "," + value.GetLength(1));
                _imageNormal = value;
            }
        }
        public D2D.Bitmap[,] ImageActive
        {
            get => _imageActive;
            set
            {
                if (value.GetLength(0) != X || value.GetLength(1) != Y)
                    throw new IndexOutOfRangeException(value.GetLength(0) + "," + value.GetLength(1));
                _imageActive = value;
            }
        }
        public bool[,] IsActive
        {
            get => _isActive;
            set
            {
                if (value.GetLength(0) != X || value.GetLength(1) != Y)
                    throw new IndexOutOfRangeException(value.GetLength(0) + "," + value.GetLength(1));
                _isActive = value;
            }
        }
        private Mathe.RawColor4[,] _color;
        private int[,] _visibleBoard;
        private D2D.Bitmap[,] _imageNormal;
        private D2D.Bitmap[,] _imageActive;
        private bool[,] _isActive;
    }
}
