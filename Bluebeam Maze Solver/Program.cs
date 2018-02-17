using System;
using System.IO;

namespace Bluebeam_Maze_Solver
{
    class Program
    {
        
        static int Main(string[] args)
        {
            // Check command line arguments
            if (args.Length != 2)
            {
                ErrorHandler.PrintUsage();
                return (int)ExitCode.GOOD;
            }
            string sourceFileName = args[0];
            string destinationFileName = args[1];

            if (!File.Exists(sourceFileName))
            {
                ErrorHandler.HandleError(ExitCode.BAD_INPUT);
                return (int)ExitCode.BAD_INPUT;
            }
            if (File.Exists(destinationFileName))
            {
                ErrorHandler.HandleError(ExitCode.INVALID_OUTPUT_PATH);
                return (int)ExitCode.INVALID_OUTPUT_PATH;
            }

            try
            {
                MazeValue[,] maze = MazeParser.Parse(sourceFileName);
                MazeSolver solver = new Solvers.BasicSolver();
                bool solutionFound = solver.solve(ref maze);
                if (!solutionFound)
                {
                    ErrorHandler.HandleError(ExitCode.UNSOLVEABLE);
                    return (int)ExitCode.UNSOLVEABLE;
                }

                if (MazeParser.GenerateFile(maze, destinationFileName))
                {
                    Console.WriteLine("Solution maze has been successfully created.");
                    return (int)ExitCode.GOOD;
                }
                ErrorHandler.HandleError(ExitCode.INVALID_OUTPUT_PATH);
                return (int)ExitCode.INVALID_OUTPUT_PATH;
            }
            catch (BadImageFormatException)
            {
                ErrorHandler.HandleError(ExitCode.BAD_INPUT);
                return (int)ExitCode.BAD_INPUT;
            }
            catch (OutOfMemoryException)
            {
                ErrorHandler.HandleError(ExitCode.INPUT_TOO_LARGE);
                return (int)ExitCode.INPUT_TOO_LARGE;
            }
        }
    }
}
