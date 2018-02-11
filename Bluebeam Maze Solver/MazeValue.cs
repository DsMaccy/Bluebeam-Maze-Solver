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

        static ColorMap()
        {
            MAPPING = new Dictionary<MazeValue, Color>();
            MAPPING[MazeValue.Wall] = Color.Black;
            MAPPING[MazeValue.Start] = Color.Blue;
            MAPPING[MazeValue.Path] = Color.Green;
            MAPPING[MazeValue.OpenSpace] = Color.White;
            MAPPING[MazeValue.End] = Color.Red;
        }
    }
}
