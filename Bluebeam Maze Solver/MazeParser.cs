using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bluebeam_Maze_Solver
{
    class MazeParser
    {
        #region Private Instance Variables
        private Image img;
        private GraphicsUnit parse_units = GraphicsUnit.Pixel;
        private MazeValue[,] maze;
        private Dictionary<MazeValue, Color> color_mapping;
        #endregion

        public MazeValue[,] Maze
        {
            get
            {
                return maze;
            }
        }

        public MazeParser(string filename)
        {
            color_mapping = new Dictionary<MazeValue, Color>();
            color_mapping[MazeValue.Wall] = Color.Black;
            color_mapping[MazeValue.Start] = Color.Blue;
            color_mapping[MazeValue.Path] = Color.Green;
            color_mapping[MazeValue.OpenSpace] = Color.White;
            color_mapping[MazeValue.End] = Color.Red;


            Image img = Image.FromFile(filename);
            maze = new MazeValue[(int)(img.GetBounds(ref parse_units).Height), (int)(img.GetBounds(ref parse_units).Width)];
            
        }

        /// <summary>
        /// This file takes the currently stored maze and generates a file from the maze
        /// </summary>
        /// <param name="filename">The output file that is the resulting image</param>
        public void GenerateFile(string filename)
        {
        }
    }
}
