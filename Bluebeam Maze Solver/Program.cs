using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bluebeam_Maze_Solver
{
    class Program
    {
        
        static int Main(string[] args)
        {
            // Check command line arguments
            if (args.Length != 3)
            {
                ErrorHandler.PrintUsage();
                return 0;
            }
            string source_file_name = args[1];
            string destination_file_name = args[2];

            MazeParser parser = new MazeParser(source_file_name);
            MazeSolver solver;

            solver.solve(parser.Maze);
            parser.GenerateFile();

            return 0;
        }
    }
}
