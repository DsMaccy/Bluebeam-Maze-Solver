using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bluebeam_Maze_Solver
{
    public class DummySolver : MazeSolver
    {
        public DummySolver()
        {

        }

        /// <summary>
        /// Do nothing
        /// </summary>
        /// <param name="maze">The parameter is ignored</param>
        public bool solve(MazeValue[,] maze)
        {
            return false;
        }
    }
}
