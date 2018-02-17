using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MazeGenerator.Properties;

namespace MazeGenerator
{
    public enum ImageSize
    {
        VerySmall,
        Small,
        Medium,
        Large
    }

    public class MazeGenerator
    {

        private Bitmap image;
        private int expectedShortestPath;
        private int startX, startY;
        private int endX, endY;
        private const int REGION_WIDTH = 4;


        public Bitmap Image
        {
            get
            {
                return image;
            }
        }

        public int ExpectedShortestPath
        {
            get
            {
                return 0;
            }
        }

        public MazeGenerator(ImageSize size)
        {
            switch(size)
            {
                case ImageSize.VerySmall:
                    //image = Bitmap.FromResource(MazeGenerator);
                    image = new Bitmap(Resources.Blank_Image_Tiny);
                    break;
                case ImageSize.Small:
                    image = new Bitmap(Resources.Blank_Image_Small);
                    break;
                case ImageSize.Medium:
                    image = new Bitmap(Resources.Blank_Image_Medium);
                    break;
                case ImageSize.Large:
                    image = new Bitmap(Resources.Blank_Image_Large);
                    break;
                default:
                    throw new NotImplementedException();
            }

            MakeGrid();

            DesignateStartAndEnd();

            CreatePathBetweenStartAndEnd();

            OpenPath();
        }


        #region Private Helper Methods

        /// <summary>
        /// Create Grid of closed blocks
        /// </summary>
        private void MakeGrid()
        {
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    if (i % 2 == 1 && j % 2 == 1)
                    {
                        image.SetPixel(i, j, Color.White);
                    }
                    else
                    {
                        image.SetPixel(i, j, Color.Black);
                    }
                }
            }
        }

        /// <summary>
        /// Define starting and ending regions
        /// </summary>
        private void DesignateStartAndEnd()
        {
            Random rngesus = new Random();
            
            
            startX = rngesus.Next(REGION_WIDTH / 2, image.Width - REGION_WIDTH / 2 - 1);
            startY = rngesus.Next(REGION_WIDTH / 2, image.Height - REGION_WIDTH / 2 - 1);
            do
            {
                endX = rngesus.Next(REGION_WIDTH / 2, image.Width - REGION_WIDTH / 2 - 1);
                endY = rngesus.Next(REGION_WIDTH / 2, image.Height - REGION_WIDTH / 2 - 1);
            } while (endX == startX && endY == startY);



            for (int i = -REGION_WIDTH / 2; i < REGION_WIDTH / 2; i++)
            {
                for (int j = -REGION_WIDTH / 2; j < REGION_WIDTH / 2; j++)
                {
                    Console.WriteLine(i + " " + j);
                    Console.WriteLine(startX + " " + startY);
                    image.SetPixel(startX + i, startY + j, Color.Blue);
                }
            }

            for (int i = -REGION_WIDTH / 2; i < REGION_WIDTH / 2; i++)
            {
                for (int j = -REGION_WIDTH / 2; j < REGION_WIDTH / 2; j++)
                {
                    Console.WriteLine(endX + " " + endY);
                    Console.WriteLine(startX + " " + startY);
                    image.SetPixel(endX + i, endY + j, Color.Red);
                }
            }
        }

        /// <summary>
        /// This method guarantees that there the starting and ending regions are connected
        /// </summary>
        private void CreatePathBetweenStartAndEnd()
        {
            bool[,] visited;
            Point curr, prev;
            bool validPathCreated = false;
            List<Point> path;
            Random rngesus = new Random();
            do
            {
                curr = new Point(startX, startY);
                prev = curr;
                visited = new bool[image.Width, image.Height];
                path = new List<Point>();
                List<Point> validNeighbors;
                while (true)
                {
                    if (image.GetPixel(curr.X, curr.Y).ToArgb() == Color.Red.ToArgb())
                    {
                        validPathCreated = true;
                        break;
                    }

                    visited[curr.X, curr.Y] = true;
                    path.Add(curr);
                    validNeighbors = new List<Point>();
                    if (curr.X + 1 < image.Width && !visited[curr.X + 1, curr.Y])
                    { validNeighbors.Add(new Point(curr.X + 1, curr.Y)); }
                    if (curr.Y + 1 < image.Height && !visited[curr.X, curr.Y + 1])
                    { validNeighbors.Add(new Point(curr.X, curr.Y + 1)); }
                    if (curr.X > 0 && !visited[curr.X - 1, curr.Y])
                    { validNeighbors.Add(new Point(curr.X - 1, curr.Y)); }
                    if (curr.Y > 0 && !visited[curr.X, curr.Y - 1])
                    { validNeighbors.Add(new Point(curr.X, curr.Y - 1)); }

                    // Path got stuck in a spiral
                    if (validNeighbors.Count == 0)
                    {
                        break;
                    }

                    double sumCount = 0;
                    double[] individualCounts = new double[validNeighbors.Count];
                    const double CORRECT_DIRECTION_WEIGHT_MODIFIER = 10.0;
                    const double CONSISTENT_DIRECTION_WEIGHT_MODIFIER = 100.0;
                    for (int i = 0; i < validNeighbors.Count; i++)
                    {
                        Point p = validNeighbors[i];
                        individualCounts[i] = 1;
                        sumCount += 1;

                        // More heavily weight the values that are in the
                        if ((p.X - curr.X) * (endX - curr.X) > 0)
                        {
                            sumCount += CORRECT_DIRECTION_WEIGHT_MODIFIER;
                            individualCounts[i] += CORRECT_DIRECTION_WEIGHT_MODIFIER;
                        }
                        if ((p.Y - curr.Y) * (endY - curr.Y) > 0)
                        {
                            sumCount += CORRECT_DIRECTION_WEIGHT_MODIFIER;
                            individualCounts[i] += CORRECT_DIRECTION_WEIGHT_MODIFIER;
                        }

                        // More heavily weight directions that are consistent with the last move
                        if ((p.Y - curr.Y) == (curr.Y - prev.Y) && (p.X - curr.X) == (curr.X - prev.X))
                        {
                            sumCount += CONSISTENT_DIRECTION_WEIGHT_MODIFIER;
                            individualCounts[i] += CONSISTENT_DIRECTION_WEIGHT_MODIFIER;
                        }
                    }
                    double selectionValue = rngesus.NextDouble() * sumCount;
                    for (int i = 0; i < validNeighbors.Count; i++)
                    {
                        selectionValue -= individualCounts[i];
                        if (selectionValue < 0)
                        {
                            prev = curr;
                            curr = validNeighbors[i];
                            break;
                        }
                    }
                }
            } while (!validPathCreated);

            // At this point, a valid path has been found and is stored in the list path
            // Go through and actually open the path up
            foreach (Point p in path)
            {
                if (image.GetPixel(p.X, p.Y).ToArgb() == Color.Black.ToArgb())
                {
                    image.SetPixel(p.X, p.Y, Color.White);
                }
            }
        }

        /// <summary>
        /// Randomly open space up
        /// </summary>
        private void OpenPath()
        {
            // These probabilities should add approximately to 1.0 (precision not necessary);
            const double SINGLE_SIDE_PROBABILITY = 0.4;
            const double BOTH_SIDE_PROBABILITY = 0.19999;
            const double NEITHER_PROBABILITY = 1.0 - (BOTH_SIDE_PROBABILITY + 2 * SINGLE_SIDE_PROBABILITY);
            const double PROB_SUM = BOTH_SIDE_PROBABILITY + 2 * SINGLE_SIDE_PROBABILITY + NEITHER_PROBABILITY;

            // Skip by two in each direction to get to the center of each grid cell
            Random rngesus = new Random();
            bool forwardPass = true;
            for (int i = 1; i < image.Width; i += 2)
            {
                for (int j = 1; j < image.Height; j += 2)
                {
                    double roll = rngesus.NextDouble();
                    if (roll < (SINGLE_SIDE_PROBABILITY + BOTH_SIDE_PROBABILITY) && i + 2 < image.Width)
                    {
                        if (image.GetPixel(i + 1, j).ToArgb() == Color.Black.ToArgb())
                        {
                            image.SetPixel(i + 1, j, Color.White);
                        }
                    }
                    if (forwardPass)
                    {
                        if (roll >= SINGLE_SIDE_PROBABILITY && roll < (PROB_SUM - NEITHER_PROBABILITY) && j + 2 < image.Height)
                        {
                            if (image.GetPixel(i, j + 1).ToArgb() == Color.Black.ToArgb())
                            {
                                image.SetPixel(i, j + 1, Color.White);
                            }
                        }
                    }
                    else
                    {
                        if (roll >= SINGLE_SIDE_PROBABILITY && roll < NEITHER_PROBABILITY && j - 2 >= 0)
                        {
                            if (image.GetPixel(i, j - 1).ToArgb() == Color.Black.ToArgb())
                            {
                                image.SetPixel(i, j - 1, Color.White);
                            }
                        }
                    }
                }
                forwardPass = !forwardPass;
            }
        }

        #endregion
    }
}
