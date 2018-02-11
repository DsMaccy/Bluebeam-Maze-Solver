using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace Bluebeam_Maze_Solver
{
    class MazeParser
    {
        #region Private Instance Variables
        private Bitmap img;
        private GraphicsUnit parse_units = GraphicsUnit.Pixel;
        private MazeValue[,] maze;
        
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
            img = Bitmap.FromFile(filename) as Bitmap;
            maze = new MazeValue[img.Width, img.Width];
        }

        /// <summary>
        /// This file takes the currently stored maze and generates a file from the maze
        /// </summary>
        /// <param name="filename">The output file that is the resulting image</param>
        public bool GenerateFile(string filename)
        {
            if (!File.Exists(filename))
            {
                img.Save(filename);
                return true;
            }
            return false;
        }
    }
}
