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

        private void AddNeighbors(ref MazeValue[,] maze, ref Queue<Point> points, ref int[,] distance, ref Point[,] previous, Point currentPoint)
        {
            int currentDistance = distance[currentPoint.X, currentPoint.Y];

            int nextX = currentPoint.X + 1;
            int nextY = currentPoint.Y;
            if (nextX < maze.GetLength(0) && currentDistance + 1 < distance[nextX, nextY] && maze[nextX, nextY] != MazeValue.Wall)
            {
                distance[nextX, nextY] = currentDistance + 1;
                previous[nextX, nextY] = currentPoint;
                points.Enqueue(new Point(nextX, nextY));
            }

            nextX = currentPoint.X;
            nextY = currentPoint.Y + 1;
            if (nextY < maze.GetLength(1) && currentDistance + 1 < distance[nextX, nextY] && maze[nextX, nextY] != MazeValue.Wall)
            {
                distance[nextX, nextY] = currentDistance + 1;
                previous[nextX, nextY] = currentPoint;
                points.Enqueue(new Point(nextX, nextY));
            }

            nextX = currentPoint.X - 1;
            nextY = currentPoint.Y;
            if (nextX >= 0 && currentDistance + 1 < distance[nextX, nextY] && maze[nextX, nextY] != MazeValue.Wall)
            {
                distance[nextX, nextY] = currentDistance + 1;
                previous[nextX, nextY] = currentPoint;
                points.Enqueue(new Point(nextX, nextY));
            }

            nextX = currentPoint.X;
            nextY = currentPoint.Y - 1;
            if (nextY >= 0 && currentDistance + 1 < distance[nextX, nextY] && maze[nextX, nextY] != MazeValue.Wall)
            {
                distance[nextX, nextY] = currentDistance + 1;
                previous[nextX, nextY] = currentPoint;
                points.Enqueue(new Point(nextX, nextY));
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

        private void BFS(ref MazeValue[,] maze, Point startingPoint, ref int[,] distance, ref Point[,] previous) //, out List<Point> nearbyStartingPoints)
        {
            Queue<Point> points = new Queue<Point>(maze.GetLength(0) * maze.GetLength(1));
            AddNeighbors(ref maze, ref points, ref distance, ref previous, startingPoint);
            while (points.Count > 0)
            {
                Point currentPoint = points.Dequeue();
                MazeValue currentMazeValue = maze[currentPoint.X, currentPoint.Y];
                
                switch (currentMazeValue)
                {
                    case MazeValue.End:
                        return;
                    case MazeValue.OpenSpace:
                        AddNeighbors(ref maze, ref points, ref distance, ref previous, currentPoint);
                        break;
                    case MazeValue.Start:
                    case MazeValue.Wall:
                        // Do Nothing
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
        }

        private void FindStartAndEndPoints(ref MazeValue[,] maze, out List<Point> startingPoints, out List<Point> endingPoints)
        {
            startingPoints = new List<Point>();
            endingPoints = new List<Point>();
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    if (maze[i, j] == MazeValue.Start)
                    {
                        bool allNeighborsAreStaringPoints = (i == 0 || maze[i - 1, j] == MazeValue.Start) &&
                                                                 (j == 0 || maze[i, j - 1] == MazeValue.Start) &&
                                                                 (i + 1 >= maze.GetLength(0) || maze[i + 1, j] == MazeValue.Start) &&
                                                                 (j + 1 >= maze.GetLength(1) || maze[i, j + 1] == MazeValue.Start);
                        if (!allNeighborsAreStaringPoints)
                        {
                            startingPoints.Add(new Point(i, j));
                        }
                    }
                    else if (maze[i, j] == MazeValue.End)
                    {
                        bool allNeighborsAreEndingPoints = (i == 0 || maze[i - 1, j] == MazeValue.End) &&
                                                               (j == 0 || maze[i, j - 1] == MazeValue.End) &&
                                                               (i + 1 >= maze.GetLength(0) || maze[i + 1, j] == MazeValue.End) &&
                                                               (j + 1 >= maze.GetLength(1) || maze[i, j + 1] == MazeValue.End);
                        if (!allNeighborsAreEndingPoints)
                        {
                            endingPoints.Add(new Point(i, j));
                        }
                    }
                }
            }
        }

        public bool solve(ref MazeValue[,] maze)
        {
            int[,] distance = new int[maze.GetLength(0), maze.GetLength(1)];
            Point[,] previous = new Point[maze.GetLength(0), maze.GetLength(1)];

            // Initialize additional values
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    distance[i, j] = int.MaxValue;
                    previous[i, j] = new Point(-1, -1);
                }
            }

            // Find and initialize start and end points
            List<Point> startingPoints, endingPoints;
            FindStartAndEndPoints(ref maze, out startingPoints, out endingPoints);
            foreach (Point point in startingPoints)
            {
                distance[point.X, point.Y] = 0;
            }

            // Run BFS for all the starting points
            foreach (Point point in startingPoints)
            {
                BFS(ref maze, point, ref distance, ref previous);
            }

            // Find the closest end point and backtrace until a starting point is found
            int minDistance = int.MaxValue;
            Point closestEndPoint = new Point();
            foreach (Point point in endingPoints)
            {
                if (distance[point.X, point.Y] < minDistance)
                {
                    minDistance = distance[point.X, point.Y];
                    closestEndPoint = point;
                }
            }
            if (minDistance == int.MaxValue)
            {
                return false;
            }

            MutateMaze(ref maze, ref previous, closestEndPoint);
            return true;

        }
    }
}
