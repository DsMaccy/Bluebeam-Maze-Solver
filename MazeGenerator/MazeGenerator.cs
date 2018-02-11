using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

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

        public Image Image
        {
            get
            {
                return image;
            }
        }

        public MazeGenerator(ImageSize size)
        {
            switch(size)
            {
                case ImageSize.VerySmall:
                    image = Bitmap.FromFile("Blank Image Tiny.bmp") as Bitmap;
                    break;
                case ImageSize.Small:
                    image = Bitmap.FromFile("Blank Image Small.bmp") as Bitmap;
                    break;
                case ImageSize.Medium:
                    image = Bitmap.FromFile("Blank Image Medium.bmp") as Bitmap;
                    break;
                case ImageSize.Large:
                    image = Bitmap.FromFile("Blank Image Large.bmp") as Bitmap;
                    break;
                default:
                    throw new NotImplementedException();
            }

            FirstPassMakeGrid();

            SecondPassDesignateStartAndEnd();

            ThirdPassOpenPath();
        }


        #region Private Helper Methods

        private void FirstPassMakeGrid()
        {
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    if (i % 2 == 0 && j % 2 == 0)
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

        private void SecondPassDesignateStartAndEnd()
        {
            Random rngesus = new Random();
            const int REGION_WIDTH = 4;
            
            int start_x = rngesus.Next(REGION_WIDTH / 2 - 1, image.Width - REGION_WIDTH / 2);
            int start_y = rngesus.Next(REGION_WIDTH / 2 - 1, image.Height - REGION_WIDTH / 2);
            for (int i = -REGION_WIDTH / 2; i < REGION_WIDTH / 2; i++)
            {
                for (int j = -REGION_WIDTH / 2; j < REGION_WIDTH / 2; j++)
                {
                    image.SetPixel(start_x + i, start_y + j, Color.Blue);
                }
            }

            int end_x, end_y;
            do
            {
                end_x = rngesus.Next(REGION_WIDTH / 2 - 1, image.Width - REGION_WIDTH / 2);
                end_y = rngesus.Next(REGION_WIDTH / 2 - 1, image.Height - REGION_WIDTH / 2);
            } while (end_x == start_x && end_y == start_y);

            for (int i = -REGION_WIDTH / 2; i < REGION_WIDTH / 2; i++)
            {
                for (int j = -REGION_WIDTH / 2; j < REGION_WIDTH / 2; j++)
                {
                    image.SetPixel(end_x + i, end_y + j, Color.Red);
                }
            }
        }


        private void ThirdPassOpenPath()
        {
            // Skip by two in each direction to get to the center of each grid cell
            bool forward_pass = true;
            for (int i = 1; i < image.Width; i += 2)
            {
                if (forward_pass)
                {
                    for (int j = 1; j < image.Height; j += 2)
                    {
                        // TODO
                    }
                }
                else
                {
                    for (int j = image.Height - 2; j > 0; j -= 2)
                    {
                        // TODO
                    }
                }
                forward_pass = !forward_pass;
            }

            // TODO: Final modifications to make sure maze is solveable
        }

        #endregion
    }
}
