using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bluebeam_Maze_Solver
{
    public enum ExitCode : int
    {
        GOOD = 0,
        BAD_INPUT = 1,
        UNSOLVEABLE = 2,
        INVALID_OUTPUT_PATH = 3,
    }
}
