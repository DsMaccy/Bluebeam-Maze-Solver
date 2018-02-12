using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace Bluebeam_Maze_Solver
{
    public class MazeParser
    {
        #region Private Instance Variables
        private Bitmap img;
        private MazeValue[,] maze;
        
        #endregion

        public MazeValue[,] Maze
        {
            get
            {
                return maze;
            }
        }

        private void TranslateImageToMazeArray()
        {
            maze = new MazeValue[img.Width, img.Height];
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    if (!ColorMap.REVERSE_MAPPING.TryGetValue(img.GetPixel(i, j).ToArgb(), out maze[i, j]) ||
                        maze[i, j] == MazeValue.Path)
                    {
                        throw new BadImageFormatException("The image has an unrecognized color");
                    }
                }
            }
        }

        public MazeParser(Bitmap image)
        {
            this.img = image;
            TranslateImageToMazeArray();
        }

        public MazeParser(string filename)
        {
            try
            {
                img = new Bitmap(filename);
            }
            catch (Exception)
            {
                throw new BadImageFormatException("Image either non existent or not in the correct format: " + filename);
            }
            TranslateImageToMazeArray();
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
