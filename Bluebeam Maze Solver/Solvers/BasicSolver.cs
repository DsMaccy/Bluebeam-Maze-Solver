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

        private void add_neighbors(ref MazeValue[,] maze, ref Queue<Point> points, ref int[,] distance, ref Point[,] previous, Point currentPoint)
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

            //Queue<Tuple<Point, int>> points = new Queue<Tuple<Point, int>>(maze.GetLength(0) * maze.GetLength(1));
            Queue<Point> points = new Queue<Point>(maze.GetLength(0) * maze.GetLength(1));
            add_neighbors(ref maze, ref points, ref distance, ref previous, startingPoint);
            while (points.Count > 0)
            {
                if (points.Count > maze.GetLength(0) * maze.GetLength(1))
                {
                    Console.WriteLine("Number of points in points: " + points.Count);
                }
                
                //Tuple<Point, int> curr = points.Dequeue();
                //Point currentPoint = curr.Item1;
                Point currentPoint = points.Dequeue();
                //distance[currentPoint.X, currentPoint.Y] = curr.Item2;
                MazeValue currentMazeValue = maze[currentPoint.X, currentPoint.Y];
                
                switch (currentMazeValue)
                {
                    case MazeValue.End:
                        return;
                    case MazeValue.Start:
                        //nearbyStartingPoints.Add(currentPoint);
                        //add_neighbors(ref maze, ref points, ref visited, ref previous, currentPoint, currentDistance);
                        break;
                    case MazeValue.OpenSpace:
                        add_neighbors(ref maze, ref points, ref distance, ref previous, currentPoint);
                        break;
                    case MazeValue.Wall:
                        // Do Nothing
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
        }

        private void FindStartAndEndPoints(ref MazeValue[,] maze, out List<Point> starting_points, out List<Point> ending_points)
        {
            starting_points = new List<Point>();
            ending_points = new List<Point>();
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    if (maze[i, j] == MazeValue.Start)
                    {
                        bool all_neighbors_are_starting_points = (i == 0 || maze[i - 1, j] == MazeValue.Start) &&
                                                                 (j == 0 || maze[i, j - 1] == MazeValue.Start) &&
                                                                 (i + 1 >= maze.GetLength(0) || maze[i + 1, j] == MazeValue.Start) &&
                                                                 (j + 1 >= maze.GetLength(1) || maze[i, j + 1] == MazeValue.Start);
                        if (!all_neighbors_are_starting_points)
                        {
                            starting_points.Add(new Point(i, j));
                        }
                    }
                    else if (maze[i, j] == MazeValue.End)
                    {
                        bool all_neighbors_are_ending_points = (i == 0 || maze[i - 1, j] == MazeValue.End) &&
                                                               (j == 0 || maze[i, j - 1] == MazeValue.End) &&
                                                               (i + 1 >= maze.GetLength(0) || maze[i + 1, j] == MazeValue.End) &&
                                                               (j + 1 >= maze.GetLength(1) || maze[i, j + 1] == MazeValue.End);
                        if (!all_neighbors_are_ending_points)
                        {
                            ending_points.Add(new Point(i, j));
                        }
                    }
                }
            }
        }

        public bool solve(ref MazeValue[,] maze)
        {
            //nearbyStartingPoints = new List<Point>();
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
            List<Point> starting_points, ending_points;
            FindStartAndEndPoints(ref maze, out starting_points, out ending_points);
            foreach (Point point in starting_points)
            {
                distance[point.X, point.Y] = 0;
            }

            // Run BFS for all the starting points
            foreach (Point point in starting_points)
            {
                BFS(ref maze, point, ref distance, ref previous); //, out connected_starters);
            }

            // Find the closest end point and backtrace until a starting point is found
            int min_distance = int.MaxValue;
            Point closestEndPoint = new Point();
            foreach (Point point in ending_points)
            {
                if (distance[point.X, point.Y] < min_distance)
                {
                    min_distance = distance[point.X, point.Y];
                    closestEndPoint = point;
                }
            }
            if (min_distance == int.MaxValue)
            {
                return false;
            }

            MutateMaze(ref maze, ref previous, closestEndPoint);
            return true;

        }
    }
}
