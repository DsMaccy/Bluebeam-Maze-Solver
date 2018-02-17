using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bluebeam_Maze_Solver.Solvers
{
    delegate int[,] MazeDistanceGetter();

    class PointComparer : IComparer<Point>
    {
        private MazeDistanceGetter distanceMatrix = null;

        public PointComparer(MazeDistanceGetter distanceGetter)
        {
            distanceMatrix = distanceGetter;
        }        

        public int Compare(Point first, Point second)
        {
            if (distanceMatrix == null)
            {
                throw new ApplicationException("Need to set the distance callback first");
            }

            return distanceMatrix()[first.X, first.Y] - distanceMatrix()[second.X, second.Y];
        }
    }
    

    /// <summary>
    /// Convert the maze into a set of compressed nodes to minimize BFS and the checking of alternate paths
    /// </summary>
    public class CompressedSolver : MazeSolver
    {

        private int[,] distance;
        private Point[,] previous;

        public int[,] Distance()
        {
            return distance;
        }

        public CompressedSolver()
        {
            distance = null;
            previous = null;
        }

        

        public bool solve(ref MazeValue[,] maze)
        {
            List<MazeNode> startNode = buildMaze();
            resetDistanceAndPrevious();


            throw new NotImplementedException();
        }


        #region Private Helper Methods

        private void AddNeighbors(ref MazeValue[,] maze, ref SortedSet<Point> points, Point currentPoint)
        {
            int currentDistance = distance[currentPoint.X, currentPoint.Y];

            int nextX = currentPoint.X + 1;
            int nextY = currentPoint.Y;
            if (nextX < maze.GetLength(0) && currentDistance + 1 < distance[nextX, nextY] && maze[nextX, nextY] != MazeValue.Wall)
            {
                distance[nextX, nextY] = currentDistance + 1;
                previous[nextX, nextY] = currentPoint;
                points.Add(new Point(nextX, nextY));
            }

            nextX = currentPoint.X;
            nextY = currentPoint.Y + 1;
            if (nextY < maze.GetLength(1) && currentDistance + 1 < distance[nextX, nextY] && maze[nextX, nextY] != MazeValue.Wall)
            {
                distance[nextX, nextY] = currentDistance + 1;
                previous[nextX, nextY] = currentPoint;
                points.Add(new Point(nextX, nextY));
            }

            nextX = currentPoint.X - 1;
            nextY = currentPoint.Y;
            if (nextX >= 0 && currentDistance + 1 < distance[nextX, nextY] && maze[nextX, nextY] != MazeValue.Wall)
            {
                distance[nextX, nextY] = currentDistance + 1;
                previous[nextX, nextY] = currentPoint;
                points.Add(new Point(nextX, nextY));
            }

            nextX = currentPoint.X;
            nextY = currentPoint.Y - 1;
            if (nextY >= 0 && currentDistance + 1 < distance[nextX, nextY] && maze[nextX, nextY] != MazeValue.Wall)
            {
                distance[nextX, nextY] = currentDistance + 1;
                previous[nextX, nextY] = currentPoint;
                points.Add(new Point(nextX, nextY));
            }
        }

        private void MutateMaze(ref MazeValue[,] maze, Point endPoint)
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

        

        private void Dijkstras(ref MazeValue[,] maze, Point startingPoint) //, out List<Point> nearbyStartingPoints)
        {
            SortedSet<Point> points = new SortedSet<Point>(new PointComparer(Distance));
            AddNeighbors(ref maze, ref points, startingPoint);
            while (points.Count > 0)
            {
                Point currentPoint = points.Min;
                points.Remove(currentPoint);
                MazeValue currentMazeValue = maze[currentPoint.X, currentPoint.Y];
                
                switch (currentMazeValue)
                {
                    case MazeValue.End:
                        return;
                    case MazeValue.OpenSpace:
                        AddNeighbors(ref maze, ref points, currentPoint);
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

        #endregion
    }
}
