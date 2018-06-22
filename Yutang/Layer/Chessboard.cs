using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Yutang.Forms;
using Yutang.Model;
using D2D = SharpDX.Direct2D1;
using Mathe = SharpDX.Mathematics.Interop;
using WIC = SharpDX.WIC;
using DXIO = SharpDX.IO;

namespace Yutang.Layer
{
    internal class Chessboard : ILayer
    {
        public List<Mathe.RawRectangleF> Rectangles { get; private set; } = new List<Mathe.RawRectangleF>();
        public bool HasActive { get; set; }

        private readonly PointF _centerPointF;

        // Board
        public List<Board> Boards { get; set; } = new List<Board>();
        //private readonly Board _boardRound;
        //private readonly Board _boardCentre;

        // Rectangles
        private readonly List<Mathe.RawRectangleF[,]> _rectangles = new List<Mathe.RawRectangleF[,]>();
        private Mathe.RawRectangleF _selectedRec;

        // Brushes
        private readonly D2D.Brush _yellowBrush;
        private readonly D2D.Brush _blueBrush;
        private readonly D2D.Brush _redBrush;
        private readonly D2D.Brush _maskBrush;

        // Bitmaps
        private D2D.Bitmap _boxBitmap;
        private Mathe.RawRectangleF _boxRec;

        private readonly List<D2D.Brush[,]> _brushes = new List<D2D.Brush[,]>();

        private readonly Dictionary<Mathe.RawRectangleF, int> _boardMap =
            new Dictionary<Mathe.RawRectangleF, int>();
        private bool _isBoxing = false;
        private List<Mathe.RawRectangleF> _tmpRectangles = new List<Mathe.RawRectangleF>();
        private int _sevenLv = 0;

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
            var cYellow2 = new Mathe.RawColor4(226 / 255f, 234 / 255f, 152 / 255f, 0.8f);
            // Create brushes
            _yellowBrush = new D2D.SolidColorBrush(RenderForm.RenderTarget, cYellow2);
            _blueBrush = new D2D.SolidColorBrush(RenderForm.RenderTarget, cBlue);
            _redBrush = new D2D.SolidColorBrush(RenderForm.RenderTarget, cRed);
            _maskBrush = new D2D.SolidColorBrush(RenderForm.RenderTarget, new Mathe.RawColor4(0, 0, 0, 0.5f));

            LoadSettings();
            int intI = 0;
            // Create rectangles
            for (var index = 0; index < Boards.Count; index++)
            {
                var item = Boards[index];
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
                        Rectangles.Add(_rectangles[index][i, j]);
                        _boardMap.Add(_rectangles[index][i, j], intI);
                        intI++;
                    }
                }
            }

            #region ImageNormal settings
            //_boardCentre.SetNormalImage(0, 2, LoadFromFile(Path.Combine(_resPath, "13.png")));

            //_boardCentre.SetNormalImage(1, 1, LoadFromFile(Path.Combine(_resPath, "1A.png")));
            //_boardCentre.SetNormalImage(1, 2, LoadFromFile(Path.Combine(_resPath, "6.png")));
            //_boardCentre.SetNormalImage(1, 3, LoadFromFile(Path.Combine(_resPath, "9.png")));

            //_boardCentre.SetNormalImage(2, 0, LoadFromFile(Path.Combine(_resPath, "12.png")));
            //_boardCentre.SetNormalImage(2, 1, LoadFromFile(Path.Combine(_resPath, "2.png")));
            //_boardCentre.SetNormalImage(2, 2, LoadFromFile(Path.Combine(_resPath, "7C.png")));
            //_boardCentre.SetNormalImage(2, 3, LoadFromFile(Path.Combine(_resPath, "8.png")));
            //_boardCentre.SetNormalImage(2, 4, LoadFromFile(Path.Combine(_resPath, "10.png")));

            //_boardCentre.SetNormalImage(3, 1, LoadFromFile(Path.Combine(_resPath, "3.png")));
            //_boardCentre.SetNormalImage(3, 2, LoadFromFile(Path.Combine(_resPath, "4.png")));
            //_boardCentre.SetNormalImage(3, 3, LoadFromFile(Path.Combine(_resPath, "5.png")));

            //_boardCentre.SetNormalImage(4, 2, LoadFromFile(Path.Combine(_resPath, "11.png")));
            #endregion
        }

        public void Measure()
        {
            #region 检测触发情况

            if (Boards[1].IsActive[1, 1] && Boards[1].IsActive[2, 1] && Boards[1].IsActive[3, 1])
            {
                Boards[0].IsActive[1, 2] = true;
                Boards[0].IsActive[5, 2] = true;
                HasActive = true;
            }
            if (Boards[1].IsActive[0, 2] && Boards[1].IsActive[1, 2] && Boards[1].IsActive[2, 2] &&
                    Boards[1].IsActive[3, 2] && Boards[1].IsActive[4, 2])
            {
                Boards[0].IsActive[0, 3] = true;
                Boards[0].IsActive[6, 3] = true;
                HasActive = true;
            }
            if (Boards[1].IsActive[2, 0] && Boards[1].IsActive[2, 1] && Boards[1].IsActive[2, 2] &&
                    Boards[1].IsActive[2, 3] && Boards[1].IsActive[2, 4])
            {
                Boards[0].IsActive[3, 0] = true;
                Boards[0].IsActive[3, 6] = true;
                HasActive = true;
            }
            if (Boards[1].IsActive[0, 2] && Boards[1].IsActive[1, 3] && Boards[1].IsActive[2, 4])
            {
                Boards[0].IsActive[0, 2] = true;
                Boards[0].IsActive[4, 6] = true;
                HasActive = true;
            }
            if (Boards[1].IsActive[0, 2] && Boards[1].IsActive[1, 1] && Boards[1].IsActive[2, 0])
            {
                Boards[0].IsActive[0, 4] = true;
                Boards[0].IsActive[4, 0] = true;
                HasActive = true;
            }
            if (Boards[1].IsActive[1, 1] && Boards[1].IsActive[2, 2] && Boards[1].IsActive[3, 3])
            {
                Boards[0].IsActive[1, 1] = true;
                Boards[0].IsActive[5, 5] = true;
                HasActive = true;
            }
            if (Boards[1].IsActive[1, 1] && Boards[1].IsActive[2, 1] && Boards[1].IsActive[3, 1])
            {
                Boards[0].IsActive[1, 2] = true;
                Boards[0].IsActive[5, 2] = true;
                HasActive = true;
            }
            if (Boards[1].IsActive[1, 3] && Boards[1].IsActive[2, 3] && Boards[1].IsActive[3, 3])
            {
                Boards[0].IsActive[1, 4] = true;
                Boards[0].IsActive[5, 4] = true;
                HasActive = true;
            }
            if (Boards[1].IsActive[1, 3] && Boards[1].IsActive[2, 2] && Boards[1].IsActive[3, 1])
            {
                Boards[0].IsActive[1, 5] = true;
                Boards[0].IsActive[5, 1] = true;
                HasActive = true;
            }
            if (Boards[1].IsActive[1, 1] && Boards[1].IsActive[1, 2] && Boards[1].IsActive[1, 3])
            {
                Boards[0].IsActive[2, 1] = true;
                Boards[0].IsActive[2, 5] = true;
                HasActive = true;
            }
            if (Boards[1].IsActive[2, 4] && Boards[1].IsActive[3, 3] && Boards[1].IsActive[4, 2])
            {
                Boards[0].IsActive[2, 6] = true;
                Boards[0].IsActive[6, 2] = true;
                HasActive = true;
            }
            if (Boards[1].IsActive[1, 1] && Boards[1].IsActive[2, 2] && Boards[1].IsActive[3, 3])
            {
                Boards[0].IsActive[1, 1] = true;
                Boards[0].IsActive[5, 5] = true;
                HasActive = true;
            }
            if (Boards[1].IsActive[3, 1] && Boards[1].IsActive[3, 2] && Boards[1].IsActive[3, 3])
            {
                Boards[0].IsActive[4, 1] = true;
                Boards[0].IsActive[4, 5] = true;
                HasActive = true;
            }
            if (Boards[1].IsActive[2, 0] && Boards[1].IsActive[3, 1] && Boards[1].IsActive[4, 2])
            {
                Boards[0].IsActive[2, 0] = true;
                Boards[0].IsActive[6, 4] = true;
                HasActive = true;
            }
            #endregion
            #region bug

            if (Boards[0].IsActive[0, 2] && Boards[0].IsActive[4, 6])
            {
                Boards[1].IsActive[0, 2] = true;
                Boards[1].IsActive[1, 3] = true;
                Boards[1].IsActive[2, 4] = true;
            }
            if (Boards[0].IsActive[0, 3] && Boards[0].IsActive[6, 3])
            {
                Boards[1].IsActive[0, 2] = true;
                Boards[1].IsActive[1, 2] = true;
                Boards[1].IsActive[2, 2] = true;
                Boards[1].IsActive[3, 2] = true;
                Boards[1].IsActive[4, 2] = true;
            }
            if (Boards[0].IsActive[0, 4] && Boards[0].IsActive[4, 0])
            {
                Boards[1].IsActive[0, 2] = true;
                Boards[1].IsActive[1, 1] = true;
                Boards[1].IsActive[2, 0] = true;
                Boards[0].IsActive[0, 4] = true;
                Boards[0].IsActive[4, 0] = true;
            }
            if (Boards[0].IsActive[1, 1] && Boards[0].IsActive[5, 5])
            {
                Boards[1].IsActive[1, 1] = true;
                Boards[1].IsActive[2, 2] = true;
                Boards[1].IsActive[3, 3] = true;
            }
            if (Boards[0].IsActive[1, 2] && Boards[0].IsActive[5, 2])
            {
                Boards[1].IsActive[1, 1] = true;
                Boards[1].IsActive[2, 1] = true;
                Boards[1].IsActive[3, 1] = true;
            }
            if (Boards[0].IsActive[1, 4] && Boards[0].IsActive[5, 4])
            {
                Boards[1].IsActive[1, 3] = true;
                Boards[1].IsActive[2, 3] = true;
                Boards[1].IsActive[3, 3] = true;
            }
            if (Boards[0].IsActive[1, 5] && Boards[0].IsActive[5, 1])
            {
                Boards[1].IsActive[1, 3] = true;
                Boards[1].IsActive[2, 2] = true;
                Boards[1].IsActive[3, 1] = true;
            }
            if (Boards[0].IsActive[2, 1] && Boards[0].IsActive[2, 5])
            {
                Boards[1].IsActive[1, 1] = true;
                Boards[1].IsActive[1, 2] = true;
                Boards[1].IsActive[1, 3] = true;
            }
            if (Boards[0].IsActive[2, 6] && Boards[0].IsActive[6, 2])
            {
                Boards[1].IsActive[2, 4] = true;
                Boards[1].IsActive[3, 3] = true;
                Boards[1].IsActive[4, 2] = true;
            }
            if (Boards[0].IsActive[3, 0] && Boards[0].IsActive[3, 6])
            {
                Boards[1].IsActive[2, 0] = true;
                Boards[1].IsActive[2, 1] = true;
                Boards[1].IsActive[2, 2] = true;
                Boards[1].IsActive[2, 3] = true;
                Boards[1].IsActive[2, 4] = true;
            }
            if (Boards[0].IsActive[4, 1] && Boards[0].IsActive[4, 5])
            {
                Boards[1].IsActive[3, 1] = true;
                Boards[1].IsActive[3, 2] = true;
                Boards[1].IsActive[3, 3] = true;
            }
            if (Boards[0].IsActive[2, 0] && Boards[0].IsActive[6, 4])
            {
                Boards[1].IsActive[2, 0] = true;
                Boards[1].IsActive[3, 1] = true;
                Boards[1].IsActive[4, 2] = true;
            }
            #endregion
        }

        public void Draw()
        {
            if (HasActive)
                RenderForm.RenderTarget.FillRectangle(
                    new Mathe.RawRectangleF(0, 0, RenderForm.Width, RenderForm.Height), _maskBrush);
            for (var index = 0; index < Boards.Count; index++)
            {
                var item = Boards[index];
                for (var i = 0; i < item.X; i++)
                {
                    for (var j = 0; j < item.Y; j++)
                    {
                        if (item.VisibleBoard[i, j] == 0) continue;
                        RenderForm.RenderTarget.FillRectangle(_rectangles[index][i, j], _brushes[index][i, j]);
                        if (!item.IsActive[i, j])
                        {
                            if (item.ImageNormal[i, j] != null)
                                RenderForm.RenderTarget.DrawBitmap(item.ImageNormal[i, j], _rectangles[index][i, j], 1,
                                    D2D.BitmapInterpolationMode.Linear);
                        }
                        else
                        {
                            RenderForm.RenderTarget.DrawBitmap(item.ImageActive[i, j], _rectangles[index][i, j], 1,
                                 D2D.BitmapInterpolationMode.Linear);
                        }
                        RenderForm.RenderTarget.DrawRectangle(_rectangles[index][i, j], _yellowBrush, 0.6f);
                    }
                }
            }

            RenderForm.RenderTarget.DrawRectangle(_selectedRec, _redBrush, 2f);
            if (_boxBitmap != null)
            {
                RenderForm.RenderTarget.FillRectangle(
                   new Mathe.RawRectangleF(0, 0, RenderForm.Width, RenderForm.Height), _maskBrush);
                RenderForm.RenderTarget.DrawBitmap(_boxBitmap, _boxRec, 1, D2D.BitmapInterpolationMode.Linear);
            }
        }

        public void Dispose()
        {
            foreach (var item in _brushes)
                foreach (var item2 in item)
                    item2?.Dispose();

            _yellowBrush.Dispose();
            _blueBrush.Dispose();
        }

        public void OnClicked(MouseEventArgs e, Mathe.RawRectangleF recF)
        {
            if (!_isBoxing)
            {
                _selectedRec = recF;


                if (!_boardMap.Keys.Contains(recF)) return;

                int i = _boardMap[recF];
                Debug.WriteLine(i);
                ShowBox(e, recF, i);
            }
            else
            {
                Rectangles = _tmpRectangles.ToArray().ToList();
                _tmpRectangles.Clear();
                _boxBitmap = null;
                _boxRec = default;
                _isBoxing = false;
            }
        }

        private void ShowBox(MouseEventArgs e, Mathe.RawRectangleF recF, int i)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (true)
                    switch (i)
                    {
                        case 24:
                            Boards[1].IsActive[0, 2] = !Boards[1].IsActive[0, 2];
                            break;
                        case 25:
                            Boards[1].IsActive[1, 1] = !Boards[1].IsActive[1, 1];
                            break;
                        case 26:
                            Boards[1].IsActive[1, 2] = !Boards[1].IsActive[1, 2];
                            break;
                        case 27:
                            Boards[1].IsActive[1, 3] = !Boards[1].IsActive[1, 3];
                            break;
                        case 28:
                            Boards[1].IsActive[2, 0] = !Boards[1].IsActive[2, 0];
                            break;
                        case 29:
                            Boards[1].IsActive[2, 1] = !Boards[1].IsActive[2, 1];
                            break;
                        case 30:
                            if (!Boards[1].IsActive[2, 2])
                            {
                                Boards[1].IsActive[2, 2] = true;
                            }
                            else
                            {
                                switch (_sevenLv)
                                {
                                    case 0:
                                        Boards[1].SetActiveImage(2, 2,
                                            DxHelper.LoadFromFile(Path.Combine(_resPath, "7B.png")));
                                        _sevenLv = 1;
                                        break;
                                    case 1:
                                        Boards[1].SetActiveImage(2, 2,
                                            DxHelper.LoadFromFile(Path.Combine(_resPath, "7C.png")));
                                        _sevenLv = 2;
                                        break;
                                    default:
                                        Boards[1].IsActive[2, 2] = false;
                                        Boards[1].SetActiveImage(2, 2,
                                            DxHelper.LoadFromFile(Path.Combine(_resPath, "7A.png")));
                                        _sevenLv = 0;
                                        break;
                                }
                            }
                            break;
                        case 31:
                            Boards[1].IsActive[2, 3] = !Boards[1].IsActive[2, 3];
                            break;
                        case 32:
                            Boards[1].IsActive[2, 4] = !Boards[1].IsActive[2, 4];
                            break;
                        case 33:
                            Boards[1].IsActive[3, 1] = !Boards[1].IsActive[3, 1];
                            break;
                        case 34:
                            Boards[1].IsActive[3, 2] = !Boards[1].IsActive[3, 2];
                            break;
                        case 35:
                            Boards[1].IsActive[3, 3] = !Boards[1].IsActive[3, 3];
                            break;
                        case 36:
                            Boards[1].IsActive[4, 2] = !Boards[1].IsActive[4, 2];
                            break;
                    }
                switch (i)
                {
                    case 0:
                    case 16:
                        if (Boards[0].IsActive[0, 2] && Boards[0].IsActive[4, 6])
                        {
                            Boards[1].IsActive[0, 2] = false;
                            Boards[1].IsActive[1, 3] = false;
                            Boards[1].IsActive[2, 4] = false;
                            Boards[0].IsActive[0, 2] = false;
                            Boards[0].IsActive[4, 6] = false;
                            HasActive = false;
                        }
                        break;
                    case 1:
                    case 22:
                        if (Boards[0].IsActive[0, 3] && Boards[0].IsActive[6, 3])
                        {
                            Boards[1].IsActive[0, 2] = false;
                            Boards[1].IsActive[1, 2] = false;
                            Boards[1].IsActive[2, 2] = false;
                            Boards[1].IsActive[3, 2] = false;
                            Boards[1].IsActive[4, 2] = false;
                            Boards[0].IsActive[0, 3] = false;
                            Boards[0].IsActive[6, 3] = false;
                            HasActive = false;
                        }
                        break;
                    case 2:
                    case 13:
                        if (Boards[0].IsActive[0, 4] && Boards[0].IsActive[4, 0])
                        {
                            Boards[1].IsActive[0, 2] = false;
                            Boards[1].IsActive[1, 1] = false;
                            Boards[1].IsActive[2, 0] = false;
                            Boards[0].IsActive[0, 4] = false;
                            Boards[0].IsActive[4, 0] = false;
                            HasActive = false;
                        }
                        break;
                    case 3:
                    case 20:
                        if (Boards[0].IsActive[1, 1] && Boards[0].IsActive[5, 5])
                        {
                            Boards[1].IsActive[1, 1] = false;
                            Boards[1].IsActive[2, 2] = false;
                            Boards[1].IsActive[3, 3] = false;
                            Boards[0].IsActive[1, 1] = false;
                            Boards[0].IsActive[5, 5] = false;
                            HasActive = false;
                        }
                        break;
                    case 4:
                    case 18:
                        if (Boards[0].IsActive[1, 2] && Boards[0].IsActive[5, 2])
                        {
                            Boards[1].IsActive[1, 1] = false;
                            Boards[1].IsActive[2, 1] = false;
                            Boards[1].IsActive[3, 1] = false;
                            Boards[0].IsActive[1, 2] = false;
                            Boards[0].IsActive[5, 2] = false;
                            HasActive = false;
                        }
                        break;
                    case 5:
                    case 19:
                        if (Boards[0].IsActive[1, 4] && Boards[0].IsActive[5, 4])
                        {
                            Boards[1].IsActive[1, 3] = false;
                            Boards[1].IsActive[2, 3] = false;
                            Boards[1].IsActive[3, 3] = false;
                            Boards[0].IsActive[1, 4] = false;
                            Boards[0].IsActive[5, 4] = false;
                            HasActive = false;
                        }
                        break;
                    case 6:
                    case 17:
                        if (Boards[0].IsActive[1, 5] && Boards[0].IsActive[5, 1])
                        {
                            Boards[1].IsActive[1, 3] = false;
                            Boards[1].IsActive[2, 2] = false;
                            Boards[1].IsActive[3, 1] = false;
                            Boards[0].IsActive[1, 5] = false;
                            Boards[0].IsActive[5, 1] = false;
                            HasActive = false;
                        }
                        break;
                    case 7:
                    case 23:
                        if (Boards[0].IsActive[2, 0] && Boards[0].IsActive[6, 4])
                        {
                            Boards[1].IsActive[2, 0] = false;
                            Boards[1].IsActive[3, 1] = false;
                            Boards[1].IsActive[4, 2] = false;
                            Boards[0].IsActive[2, 0] = false;
                            Boards[0].IsActive[6, 4] = false;
                            HasActive = false;
                        }
                        break;
                    case 8:
                    case 9:
                        if (Boards[0].IsActive[2, 1] && Boards[0].IsActive[2, 5])
                        {
                            Boards[1].IsActive[1, 1] = false;
                            Boards[1].IsActive[1, 2] = false;
                            Boards[1].IsActive[1, 3] = false;
                            Boards[0].IsActive[2, 1] = false;
                            Boards[0].IsActive[2, 5] = false;
                            HasActive = false;
                        }
                        break;
                    case 10:
                    case 21:
                        if (Boards[0].IsActive[2, 6] && Boards[0].IsActive[6, 2])
                        {
                            Boards[1].IsActive[2, 4] = false;
                            Boards[1].IsActive[3, 3] = false;
                            Boards[1].IsActive[4, 2] = false;
                            Boards[0].IsActive[2, 6] = false;
                            Boards[0].IsActive[6, 2] = false;
                            HasActive = false;
                        }
                        break;
                    case 11:
                    case 12:
                        if (Boards[0].IsActive[3, 0] && Boards[0].IsActive[3, 6])
                        {
                            Boards[1].IsActive[2, 0] = false;
                            Boards[1].IsActive[2, 1] = false;
                            Boards[1].IsActive[2, 2] = false;
                            Boards[1].IsActive[2, 3] = false;
                            Boards[1].IsActive[2, 4] = false;
                            Boards[0].IsActive[3, 0] = false;
                            Boards[0].IsActive[3, 6] = false;
                            HasActive = false;
                        }
                        break;
                    case 14:
                    case 15:
                        if (Boards[0].IsActive[4, 1] && Boards[0].IsActive[4, 5])
                        {
                            Boards[1].IsActive[3, 1] = false;
                            Boards[1].IsActive[3, 2] = false;
                            Boards[1].IsActive[3, 3] = false;
                            Boards[0].IsActive[4, 1] = false;
                            Boards[0].IsActive[4, 5] = false;
                            HasActive = false;
                        }
                        break;
                    default:
                        return;
                }
                return;
            }
            string fileName;
            switch (i)
            {
                case 0:
                case 16:
                    fileName = "des_7.png";
                    break;
                case 1:
                case 22:
                    fileName = "des_5.png";
                    break;
                case 2:
                case 13:
                    fileName = "des_10.png";
                    break;
                case 3:
                case 20:
                    fileName = "des_11.png";
                    break;
                case 4:
                case 18:
                    fileName = "des_3.png";
                    break;
                case 5:
                case 19:
                    fileName = "des_2.png";
                    break;
                case 6:
                case 17:
                    fileName = "des_4.png";
                    break;
                case 7:
                case 23:
                    fileName = "des_12.png";
                    break;
                case 8:
                case 9:
                    fileName = "des_9.png";
                    break;
                case 10:
                case 21:
                    fileName = "des_0.png";
                    break;
                case 11:
                case 12:
                    fileName = "des_6.png";
                    break;
                case 14:
                case 15:
                    fileName = "des_8.png";
                    break;
                case 30:
                    fileName = "des_1.png";
                    break;
                default:
                    return;
            }

            PointF loc = new PointF(recF.Left, recF.Top);
            _boxBitmap = DxHelper.LoadFromFile(Path.Combine(_resPath, fileName));

            const float scaled = 2f;
            float scaledW = _boxBitmap.Size.Width / scaled,
                scaledH = _boxBitmap.Size.Height / scaled;
            float left = loc.X - 35,
                top = loc.Y - scaledH + 30,
                right = left + scaledW,
                bottom = top + scaledH;
            _boxRec = new Mathe.RawRectangleF(left, top, right, bottom);
            _tmpRectangles = Rectangles.ToArray().ToList();
            Rectangles.Clear();
            Rectangles.AddRange(new[]
            {
                new Mathe.RawRectangleF(0, 0, RenderForm.Width, top),
                new Mathe.RawRectangleF(0, top, left, bottom),
                new Mathe.RawRectangleF(right, top, RenderForm.Width, bottom),
                new Mathe.RawRectangleF(0, bottom, RenderForm.Width, RenderForm.Height),
            });
            _isBoxing = true;
        }

        private void LoadSettings()
        {
            for (var i = 0; i < Program.MainSettings.BoardInfomation.Count; i++)
            {
                var item = Program.MainSettings.BoardInfomation[i];
                Boards.Add(new Board(item.Width, item.Height));
                Boards[i].VisibleBoard = item.VisibleBoard;
                for (var j = 0; j < item.BoardPointInfomation.Count; j++)
                {
                    var item2 = item.BoardPointInfomation[j];
                    Boards[i].SetColor(item2.X, item2.Y, item2.Color);
                    Boards[i].SetVisible(item2.X, item2.Y, item2.Visible);
                    if (item2.ImagePath != null)
                        Boards[i].SetNormalImage(item2.X, item2.Y,
                            DxHelper.LoadFromFile(Path.Combine(_resPath, item2.ImagePath)));
                    if (item2.ImagePathA != null)
                        Boards[i].SetActiveImage(item2.X, item2.Y,
                            DxHelper.LoadFromFile(Path.Combine(_resPath, item2.ImagePathA)));
                }
            }
        }
    }
}