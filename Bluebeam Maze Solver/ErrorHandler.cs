using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bluebeam_Maze_Solver
{
    class ErrorHandler
    {
        public static void HandleError(ExitCode exitcode)
        {
            switch (exitcode)
            {
                case ExitCode.BAD_INPUT:
                    Console.WriteLine("The source file provided was not properly formatted.\n" +
                                        "Please make sure the image has only the colors black, white, red and blue.\n" +
                                        "All of these colors except for black are required.\n" +
                                        "Also, please make sure that the file is in a supported format.");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public static void PrintUsage()
        {
            Console.WriteLine("maze.exe <source_image> <destination_image>");
            Console.WriteLine();
            Console.WriteLine("Supported file types: ");
            Console.WriteLine("\t.png");
            Console.WriteLine("\t.bmp");
            Console.WriteLine("\t.jpg");
            Console.WriteLine("\t.jpeg");
            Console.WriteLine();
            Console.WriteLine("This program will take as input a maze and produce a copied image that has a path in the maze filled out in green");
            Console.WriteLine("Input prerequisites: ");
            Console.WriteLine("\tThe maze should only have the colors: ");
            Console.WriteLine("\t\tblack (0, 0, 0), white (255, 255, 255), red (255, 0, 0) and blue (0, 0, 255)");
        }
    }
}
