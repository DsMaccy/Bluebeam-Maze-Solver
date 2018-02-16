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
                                      "\tPlease make sure the file exists and is in the correct format\n" +
                                      "\tPlease make sure the image has only black, white, red and blue colors.\n" +
                                      "\t(All of these colors except for black are required).");
                    break;
                case ExitCode.UNSOLVEABLE:
                    Console.WriteLine("The provided maze has no valid solution.\n");
                    break;
                case ExitCode.INVALID_OUTPUT_PATH:
                    Console.WriteLine("The output path provided is invalid.\n" +
                                      "\tPlease check that the parent directory exists.\n" +
                                      "\tPlease also be sure that the output file does not already exist");
                    break;
                case ExitCode.GOOD:
                    break;
                case ExitCode.INPUT_TOO_LARGE:
                    Console.WriteLine("The input image provided was way too big.  Please provide smaller inputs.");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public static void PrintUsage()
        {
            Console.WriteLine("Usage: maze.exe <source_image> <destination_image>");
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
            Console.WriteLine("\t\tblack (0, 0, 0)");
            Console.WriteLine("\t\twhite (255, 255, 255)");
            Console.WriteLine("\t\tred (255, 0, 0)");
            Console.WriteLine("\t\tblue (0, 0, 255)");
        }
    }
}
