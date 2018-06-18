using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Yutang.Layer;

namespace Yutang.Forms
{
    public partial class ControlForm : Form
    {
        private readonly string _resPath = Path.Combine(Environment.CurrentDirectory, "templet");

        public ControlForm()
        {
            InitializeComponent();
        }

        private void ControlForm_Load(object sender, EventArgs e)
        {

        }

        private void btnSevenA_Click(object sender, EventArgs e)
        {
            var boards = (Chessboard)RenderForm.LayerList["chess"];
            boards.Boards[1].SetImage(2, 2, DxHelper.LoadFromFile(Path.Combine(_resPath, "7.png")));
        }

        private void btnSevenB_Click(object sender, EventArgs e)
        {
            var boards = (Chessboard)RenderForm.LayerList["chess"];
            boards.Boards[1].SetImage(2, 2, DxHelper.LoadFromFile(Path.Combine(_resPath, "7A.png")));
        }

        private void btnSevenC_Click(object sender, EventArgs e)
        {
            var boards = (Chessboard)RenderForm.LayerList["chess"];
            boards.Boards[1].SetImage(2, 2, DxHelper.LoadFromFile(Path.Combine(_resPath, "7B.png")));
        }

        private void btnSevenD_Click(object sender, EventArgs e)
        {
            var boards = (Chessboard)RenderForm.LayerList["chess"];
            boards.Boards[1].SetImage(2, 2, DxHelper.LoadFromFile(Path.Combine(_resPath, "7C.png")));
        }
    }
}
