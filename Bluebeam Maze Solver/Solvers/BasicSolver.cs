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
    public class BasicSolver : MazeSolver
    {

        private void add_neighbors(ref MazeValue[,] maze, ref Queue<Tuple<Point, int>> points, ref bool[,] visited, ref Point[,] previous, Point currentPoint, int currentDistance)
        {
            if (currentPoint.X + 1 < maze.GetLength(0) && !visited[currentPoint.X + 1, currentPoint.Y])
            {
                if (previous[currentPoint.X + 1, currentPoint.Y].X == -1)
                {
                    previous[currentPoint.X + 1, currentPoint.Y] = currentPoint;
                }
                points.Enqueue(new Tuple<Point, int>(new Point(currentPoint.X + 1, currentPoint.Y), currentDistance + 1));
            }
            if (currentPoint.Y + 1 < maze.GetLength(1) && !visited[currentPoint.X, currentPoint.Y + 1])
            {
                if (previous[currentPoint.X, currentPoint.Y + 1].X == -1)
                {
                    previous[currentPoint.X, currentPoint.Y + 1] = currentPoint;
                }
                points.Enqueue(new Tuple<Point, int>(new Point(currentPoint.X, currentPoint.Y + 1), currentDistance + 1));
            }
            if (currentPoint.X - 1 >= 0 && !visited[currentPoint.X - 1, currentPoint.Y])
            {
                if (previous[currentPoint.X - 1, currentPoint.Y].X == -1)
                {
                    previous[currentPoint.X - 1, currentPoint.Y] = currentPoint;
                }
                points.Enqueue(new Tuple<Point, int>(new Point(currentPoint.X - 1, currentPoint.Y), currentDistance + 1));
            }
            if (currentPoint.Y - 1 >= 0 && !visited[currentPoint.X, currentPoint.Y - 1])
            {
                if (previous[currentPoint.X, currentPoint.Y - 1].X == -1)
                {
                    previous[currentPoint.X, currentPoint.Y - 1] = currentPoint;
                }
                points.Enqueue(new Tuple<Point, int>(new Point(currentPoint.X, currentPoint.Y - 1), currentDistance + 1));
            }
        }

        private void MutateMaze(ref MazeValue[,] maze, ref Point[,] previous, Point endPoint)
        {
            Point curr = endPoint;
            while (maze[curr.X, curr.Y] != MazeValue.Start)
            {
                if (maze[curr.X, curr.Y] == MazeValue.OpenSpace)
                {
                    maze[curr.X, curr.Y] = MazeValue.Path;
                }
                curr = previous[curr.X, curr.Y];
            }
        }

        private int BFS(ref MazeValue[,] maze, Point startingPoint, bool mutateMaze, out List<Point> nearbyStartingPoints)
        {
            nearbyStartingPoints = new List<Point>();
            bool[,] visited = new bool[maze.GetLength(0), maze.GetLength(1)];
            Point[,] previous = new Point[maze.GetLength(0), maze.GetLength(1)];

            // Initialize additional values
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    visited[i, j] = false;
                    previous[i, j] = new Point(-1, -1);
                }
            }

            Queue<Tuple<Point, int>> points = new Queue<Tuple<Point, int>>();
            points.Enqueue(new Tuple<Point, int>(startingPoint, 0));
            while (points.Count > 0)
            {
                Tuple<Point, int> curr = points.Dequeue();
                Point currentPoint = curr.Item1;
                int currentDistance = curr.Item2;
                MazeValue currentMazeValue = maze[currentPoint.X, currentPoint.Y];
                visited[currentPoint.X, currentPoint.Y] = true;

                switch(currentMazeValue)
                {
                    case MazeValue.End:
                        if (mutateMaze)
                        {
                            MutateMaze(ref maze, ref previous, currentPoint);
                        }
                        return currentDistance;
                    case MazeValue.Start:
                        nearbyStartingPoints.Add(currentPoint);
                        add_neighbors(ref maze, ref points, ref visited, ref previous, currentPoint, currentDistance);
                        break;
                    case MazeValue.OpenSpace:
                        add_neighbors(ref maze, ref points, ref visited, ref previous, currentPoint, currentDistance);
                        break;
                    case MazeValue.Wall:
                        // Do Nothing
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
            return -1;
        }

        private List<Point> FindStartingPoints(ref MazeValue[,] maze)
        {
            List<Point> starting_points = new List<Point>();
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    if (maze[i, j] == MazeValue.Start)
                    {
                        starting_points.Add(new Point(i, j));
                    }
                }
            }
            return starting_points;
        }

        public bool solve(ref MazeValue[,] maze)
        {
            List<Point> starting_points = FindStartingPoints(ref maze);
            int min_distance = int.MaxValue;
            Point bestStartingPoint = new Point();
            foreach (Point point in starting_points)
            {
                List<Point> connected_starters;
                int distance = BFS(ref maze, point, false, out connected_starters);
                if (distance != -1)
                {
                    if (distance < min_distance)
                    {
                        min_distance = distance;
                        bestStartingPoint = point;
                    }
                }
            }

            // There exists a shortest path
            if (min_distance != int.MaxValue)
            {
                List<Point> connectedStarters;// new List<Point>()
                BFS(ref maze, bestStartingPoint, true, out connectedStarters);
            }
            return min_distance != int.MaxValue;
        }
    }
}
