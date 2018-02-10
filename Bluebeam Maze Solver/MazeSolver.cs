using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bluebeam_Maze_Solver
{
    interface MazeSolver
    {
        void solve(MazeValue[,] maze);
    }
}
