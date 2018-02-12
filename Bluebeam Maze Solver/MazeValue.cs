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
        public static readonly Dictionary<MazeValue, int> MAPPING;
        public static readonly Dictionary<int, MazeValue> REVERSE_MAPPING;

        static ColorMap()
        {
            MAPPING = new Dictionary<MazeValue, int>();
            MAPPING[MazeValue.Wall] = Color.Black.ToArgb();
            MAPPING[MazeValue.Start] = Color.Blue.ToArgb();
            MAPPING[MazeValue.Path] = Color.Green.ToArgb();
            MAPPING[MazeValue.OpenSpace] = Color.White.ToArgb();
            MAPPING[MazeValue.End] = Color.Red.ToArgb();

            REVERSE_MAPPING = new Dictionary<int, MazeValue>();
            REVERSE_MAPPING[Color.Black.ToArgb()] = MazeValue.Wall;
            REVERSE_MAPPING[Color.Blue.ToArgb()] = MazeValue.Start;
            REVERSE_MAPPING[Color.Green.ToArgb()] = MazeValue.Path;
            REVERSE_MAPPING[Color.White.ToArgb()] = MazeValue.OpenSpace;
            REVERSE_MAPPING[Color.Red.ToArgb()] = MazeValue.End;
        }
    }
}
