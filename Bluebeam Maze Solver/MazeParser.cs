using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Bluebeam_Maze_Solver
{
    public class MazeParser
    {
        #region Public Static Methods
        public static MazeValue[,] Parse(string filename)
        {
            return Parse(filename, true);
        }

        public static MazeValue[,] Parse(string filename, bool pathInvalid)
        {
            Bitmap img;
            try
            {
                img = new Bitmap(filename);
            }
            
            catch (Exception e)
            {
                throw new BadImageFormatException("Image either non existent or not in the correct format: " + filename);
            }

            return Parse(img, pathInvalid);
        }

        public static MazeValue[,] Parse(Bitmap img)
        {
            return Parse(img, true);
        }

        public static MazeValue[,] Parse(Bitmap img, bool pathInvalid)
        {
            MazeValue[,] maze = new MazeValue[img.Width, img.Height];
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);
                    if (!ColorMap.REVERSE_MAPPING.TryGetValue(pixel.ToArgb(), out maze[i, j]) ||
                        (pathInvalid && maze[i, j] == MazeValue.Path))
                    {
                        throw new BadImageFormatException("The image has an unrecognized color");
                    }
                }
            }

            return maze;
        }

        public static MazeValue[,] FuzzyParse(string filename)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        /// <summary>
        /// This file takes the currently stored maze and generates a file from the maze
        /// </summary>
        /// <param name="filename">The output file that is the resulting image</param>
        public static bool GenerateFile(MazeValue[,] maze, string filename)
        {
            if (File.Exists(filename) || maze.GetLength(0) == 0 || maze.GetLength(1) == 0)
            {
                return false;
            }
            // TODO: Implement
            
            Bitmap img = new Bitmap(maze.GetLength(0), maze.GetLength(1));
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    img.SetPixel(i, j, Color.FromArgb(ColorMap.MAPPING[maze[i, j]]));
                }
            }
            
            // Derive the output file format based on the output file name
            ImageFormat outputFormat = ImageFormat.Png;
            string fileExtension = Path.GetExtension(filename).ToLower();
            if (fileExtension == ".bmp")
            {
                outputFormat = ImageFormat.Bmp;
            }
            else if (fileExtension == ".jpg" || fileExtension == ".jpeg")
            {
                outputFormat = ImageFormat.Jpeg;
            }

            FileStream stream = new FileStream(filename, FileMode.CreateNew);
            img.Save(stream, outputFormat);
            stream.Close();
            return true;
        }

        #endregion
    }
}
