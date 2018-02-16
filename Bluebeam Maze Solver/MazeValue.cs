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
            // Note: The color green as 00ff00 is actually called lime...
            MAPPING = new Dictionary<MazeValue, int>();
            MAPPING[MazeValue.Wall] = Color.Black.ToArgb();
            MAPPING[MazeValue.Start] = Color.Blue.ToArgb();
            MAPPING[MazeValue.Path] = Color.Lime.ToArgb();
            MAPPING[MazeValue.OpenSpace] = Color.White.ToArgb();
            MAPPING[MazeValue.End] = Color.Red.ToArgb();

            REVERSE_MAPPING = new Dictionary<int, MazeValue>();
            REVERSE_MAPPING[Color.Black.ToArgb()] = MazeValue.Wall;
            REVERSE_MAPPING[Color.Blue.ToArgb()] = MazeValue.Start;
            REVERSE_MAPPING[Color.Lime.ToArgb()] = MazeValue.Path;
            REVERSE_MAPPING[Color.White.ToArgb()] = MazeValue.OpenSpace;
            REVERSE_MAPPING[Color.Red.ToArgb()] = MazeValue.End;
        }

        public static bool GetNearestValue(byte R, byte G, byte B, out MazeValue value, double maxDistance = -1)
        {
            Color[] colors = new Color[5] { Color.Black, Color.Blue, Color.White, Color.Red, Color.Lime };
            double minDistance = double.MaxValue;
            Color closestColor = new Color();
            foreach (Color color in colors)
            {
                double squareDistance = (color.R - R) * (color.R - R) + (color.G - G) * (color.G - G) + (color.B - B) * (color.B - B);

                if (squareDistance < minDistance)
                {
                    closestColor = color;
                    minDistance = squareDistance;
                }
            }

            if (maxDistance > 0 && Math.Sqrt(minDistance) < maxDistance)
            {
                value = REVERSE_MAPPING[closestColor.ToArgb()];
                return true;
            }

            value = MazeValue.Wall;  // Use the wall as a garbage value
            return false;
        }
    }
}
