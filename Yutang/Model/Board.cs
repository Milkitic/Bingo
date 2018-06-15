using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mathe = SharpDX.Mathematics.Interop;

namespace Yutang.Model
{
    class Board
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Board(int x, int y)
        {
            X = x;
            Y = y;
            VisibleBoard = new int[x, y];
            //Color = new Mathe.RawColor4[x, y];
            Color = new Mathe.RawColor4[x, y];
        }

        public void SetColor(int x, int y, Mathe.RawColor4 color) => Color[x, y] = color;

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

        private Mathe.RawColor4[,] _color;
        private int[,] _visibleBoard;
    }
}
