﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                return 0;
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
                MazeParser parser = new MazeParser(source_file_name);
                MazeSolver solver = new DummySolver();

                if (!solver.solve(parser.Maze))
                {
                    ErrorHandler.HandleError(ExitCode.UNSOLVEABLE);
                    return (int)ExitCode.UNSOLVEABLE;
                }
                if (parser.GenerateFile(destination_file_name))
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
        }
    }
}
