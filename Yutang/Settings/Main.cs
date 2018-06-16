using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mathe = SharpDX.Mathematics.Interop;
using Newtonsoft.Json;

namespace Yutang.Settings
{
    internal class Main
    {
        [JsonProperty("window_width")]
        public int WindowWidth { get; set; }
        [JsonProperty("window_height")]
        public int WindowHeight { get; set; }
        [JsonProperty("background")]
        public string Background { get; set; }

        [JsonProperty("limit_fps")]
        public bool LimitFps { get; set; }
        [JsonProperty("use_particle")]
        public bool UseParticle { get; set; }

        [JsonProperty("board_infomation")]
        public List<Board> BoardInfomation { get; set; } = new List<Board>();
        public class Board
        {
            [JsonProperty("width")]
            public int Width { get; set; }
            [JsonProperty("height")]
            public int Height { get; set; }
            [JsonProperty("visible_board")]
            public int[,] VisibleBoard { get; set; }

            [JsonProperty("board_point_infomation")]
            public List<BoardPointInfo> BoardPointInfomation { get; set; } = new List<BoardPointInfo>();
            public struct BoardPointInfo
            {
                [JsonProperty("x")]
                public int X { get; set; }
                [JsonProperty("y")]
                public int Y { get; set; }
                [JsonProperty("color")]
                public Mathe.RawColor4 Color { get; set; }
                [JsonProperty("visible")]
                public bool Visible { get; set; }
                [JsonProperty("image_path")]
                public string ImagePath { get; set; }
            }
        }
    }
}
