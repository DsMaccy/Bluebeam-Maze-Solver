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
            string source_file_name = args[0];
            string destination_file_name = args[1];

            if (!File.Exists(source_file_name))
            {
                ErrorHandler.HandleError(ExitCode.BAD_INPUT);
                return (int)ExitCode.BAD_INPUT;
            }
            if (File.Exists(destination_file_name))
            {
                ErrorHandler.HandleError(ExitCode.INVALID_OUTPUT_PATH);
                return (int)ExitCode.INVALID_OUTPUT_PATH;
            }

            try
            {
                MazeValue[,] maze = MazeParser.Parse(source_file_name);
                MazeSolver solver = new Solvers.BasicSolver();
                bool solution_found = solver.solve(ref maze);
                if (!solution_found)
                {
                    ErrorHandler.HandleError(ExitCode.UNSOLVEABLE);
                    return (int)ExitCode.UNSOLVEABLE;
                }

                if (MazeParser.GenerateFile(maze, destination_file_name))
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
