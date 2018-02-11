using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bluebeam_Maze_Solver.Solvers
{
    /// <summary>
    /// Use BFS to solve the maze
    /// </summary>
    class BasicSolver : MazeSolver
    {
        private int BFS(ref MazeValue[,] maze, Point startingPoint, bool mutateMaze)
        {
            return 0;
        }

        private List<Point>FindStartingPoints(MazeValue[,] maze)
        {
            return null;
        }

        public bool solve(MazeValue[,] maze)
        {
            List<Point> starting_points = FindStartingPoints(maze);
            int min_distance = int.MaxValue;
            Point bestStartingPoint = new Point();
            foreach (Point point in starting_points)
            {
                int distance = BFS(ref maze, point, false);
                if (distance < min_distance)
                {
                    min_distance = distance;
                    bestStartingPoint = point;
                }
            }
        }
    }
}
