using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Yutang.Layer;
using DX = SharpDX;
using D2D = SharpDX.Direct2D1;
using WIC = SharpDX.WIC;
using DW = SharpDX.DirectWrite;
using DXGI = SharpDX.DXGI;
using Mathe = SharpDX.Mathematics.Interop;

namespace Yutang.Form
{
    public partial class RenderForm : System.Windows.Forms.Form
    {
        // Basic
        public static D2D.Factory Factory { get; set; } = new D2D.Factory(); // Factory for creating 2D elements
        private static DW.Factory FactoryWrite { get; set; } = new DW.Factory(); // For creating DirectWrite Elements
        public static D2D.RenderTarget RenderTarget { get; set; } // Target of rendering

        // Layer
        private Dictionary<string, ILayer> _layerList;

        // Initial color
        private Mathe.RawColor4 _colorBack;

        // Text formats
        private DW.TextFormat _textFormat;

        // Brushes
        D2D.Brush _whiteBrush;

        // Shared use
        public static IntPtr Hwnd { get; private set; }
        public new static int Width { get; private set; }
        public new static int Height { get; private set; }

        private int _drawCount;
        private readonly Stopwatch _sw = new Stopwatch();
        private long _delay;

        public RenderForm()
        {
            InitializeComponent();

            ClientSize = new Size(500, 500);

            Load += LoadTarget;

            Width = ClientSize.Width;
            Height = ClientSize.Height;
            Hwnd = Handle;
        }

        private void LoadTarget(object sender, EventArgs e)
        {
            // Initial settings
            var pixelFormat = new D2D.PixelFormat(DXGI.Format.B8G8R8A8_UNorm, D2D.AlphaMode.Premultiplied);
            var winProp = new D2D.HwndRenderTargetProperties
            {
                Hwnd = Handle,
                PixelSize = new DX.Size2(ClientSize.Width, ClientSize.Height),
                PresentOptions = D2D.PresentOptions.RetainContents
            };
            var renderProp = new D2D.RenderTargetProperties(D2D.RenderTargetType.Default, pixelFormat, 96, 96,
                D2D.RenderTargetUsage.None, D2D.FeatureLevel.Level_DEFAULT);
            RenderTarget = new D2D.WindowRenderTarget(Factory, renderProp, winProp)
            {
                AntialiasMode = D2D.AntialiasMode.PerPrimitive,
                TextAntialiasMode = D2D.TextAntialiasMode.Grayscale,
                Transform = new Mathe.RawMatrix3x2 { M11 = 1f, M12 = 0f, M21 = 0f, M22 = 1f, M31 = 0, M32 = 0 }
            };

            // Create colors
            _colorBack = new Mathe.RawColor4(0, 0, 0, 1);

            _layerList = new Dictionary<string, ILayer>
            {
                {"back", new Background()},
                {"chess", new Chessboard()},
            };

            // Create brushes
            _whiteBrush = new D2D.SolidColorBrush(RenderTarget, new Mathe.RawColor4(1, 1, 1, 1));

            // Create text formats
            _textFormat = new DW.TextFormat(FactoryWrite, "Microsoft YaHei", 12);

            // Avoid artifacts
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
        }

        private void RenderForm_Load(object sender, EventArgs e)
        {

        }

        private void RenderForm_Paint(object sender, PaintEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized) return;
            if (RenderTarget == null || RenderTarget.IsDisposed) return;
            _drawCount++;
            if (_drawCount == 10000)
            {
                GC.Collect();
                _drawCount = 0;
            }

            _sw.Restart();
            // Begin rendering
            RenderTarget.BeginDraw();
            RenderTarget.Clear(_colorBack);

            // Draw layers
            foreach (var item in _layerList)
            {
                item.Value.Measure();
                item.Value.Draw();
            }

            RenderTarget.DrawText(_delay.ToString(), _textFormat, new Mathe.RawRectangleF(0, 0, 400, 200), _whiteBrush);
            // End drawing
            RenderTarget.EndDraw();

            _delay = _sw.ElapsedTicks;
            Invalidate();
        }

        private void RenderForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            RenderTarget.Dispose();
            foreach (var item in _layerList)
            {
                item.Value.Dispose();
            }

            FactoryWrite.Dispose();
            Factory.Dispose();
        }
    }
}