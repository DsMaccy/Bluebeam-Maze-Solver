using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bluebeam_Maze_Solver
{
    public interface MazeSolver
    {
        /// <summary>
        /// Finds a path from the starting set (blue colored pixels) to the ending set (red colored pixels)
        /// Path not guaranteed to be optimal (depends on running strategy)
        /// </summary>
        /// <param name="maze">The maze that is being traversed.  The maze will be mutated to containing path pixels (green)</param>
        /// <returns>true if a path was found from the starting set to the ending set</returns>
        bool solve(ref MazeValue[,] maze);
    }
}
