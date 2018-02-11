using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bluebeam_Maze_Solver
{
    public enum MazeValue
    {
        Wall,
        OpenSpace,
        Start,
        End,
        Path,
    }
    public class ColorMap
    {
        public static readonly Dictionary<MazeValue, Color> MAPPING;
        public static readonly Dictionary<Color, MazeValue> REVERSE_MAPPING;

        static ColorMap()
        {
            MAPPING = new Dictionary<MazeValue, Color>();
            MAPPING[MazeValue.Wall] = Color.Black;
            MAPPING[MazeValue.Start] = Color.Blue;
            MAPPING[MazeValue.Path] = Color.Green;
            MAPPING[MazeValue.OpenSpace] = Color.White;
            MAPPING[MazeValue.End] = Color.Red;

            REVERSE_MAPPING = new Dictionary<Color, MazeValue>();
            REVERSE_MAPPING[Color.Black] = MazeValue.Wall;
            REVERSE_MAPPING[Color.Blue] = MazeValue.Start;
            REVERSE_MAPPING[Color.Green] = MazeValue.Path;
            REVERSE_MAPPING[Color.White] = MazeValue.OpenSpace;
            REVERSE_MAPPING[Color.Red] = MazeValue.End;
        }
    }
}
