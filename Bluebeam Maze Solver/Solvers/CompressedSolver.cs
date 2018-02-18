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

        private Dictionary<MazeNode, int> distance;
        private Dictionary<MazeNode, Point> previous;
        private Dictionary<MazeNode, List<MazeNode>> adjacency_list;

        public Dictionary<MazeNode, int> Distance()
        {
            return distance;
        }

        public CompressedSolver()
        {
            ResetSolver();
        }

        public bool solve(ref MazeValue[,] maze)
        {
            ResetSolver();

            List<MazeNode> startNodes;
            List<MazeNode> endNodes;
            BuildMaze(ref maze, out startNodes, out endNodes);

            foreach (MazeNode node in startNodes)
            {
                Dijkstra(node);
            }

            MazeNode closestNode = null;
            int minDistance = int.MaxValue;
            foreach (MazeNode node in endNodes)
            {
                if (distance[node] < minDistance)
                {
                    minDistance = distance[node];
                    closestNode = node;
                }
            }

            if (closestNode == null)
            {
                return false;
            }
            MutateMaze(closestNode);
            return true;
        }


        #region Private Helper Methods

        private void BuildMaze(ref MazeValue[,] maze, out List<MazeNode> startNodes, out List<MazeNode> endNodes)
        {
            startNodes = new List<MazeNode>();
            endNodes = new List<MazeNode>();
            MazeNode[,] references = new MazeNode[maze.GetLength(0), maze.GetLength(1)];

            // Create Rectangle Nodes
            bool[,] added = new bool[maze.GetLength(0), maze.GetLength(1)]; 
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    if (added[i, j])
                    {
                        continue;
                    }
                    MazeValue positionValue = maze[i, j];
                    Point otherCorner = new Point(i, j);
                    bool layerAdded = true;
                    while (layerAdded)
                    {
                        for (int outerX = i; outerX <= otherCorner.X + 1 && outerX < maze.GetLength(0); outerX++)
                        {
                            if (maze[outerX, otherCorner.Y + 1] != positionValue)
                            {
                                layerAdded = false;
                                break;
                            }
                        }
                        if (layerAdded)
                        {
                            for (int outerY = i; outerY <= otherCorner.Y + 1 && outerY < maze.GetLength(1); outerY++)
                            {
                                if (maze[otherCorner.X + 1, outerY] != positionValue)
                                {
                                    layerAdded = false;
                                    break;
                                }
                            }
                        }
                        if (layerAdded)
                        {
                            otherCorner.X++; otherCorner.Y++;
                        }
                    }
                    if (otherCorner.X != i)
                    {
                        RectangleNode newNode = new RectangleNode();
                        // Add Rectangle Node using OtherCorner
                    }
                    else
                    {
                        int similarNeighborSum = 0;
                        Point neighbor1 = new Point(-1, -1);
                        Point neighbor2 = new Point(-1, -1);

                        int newX, newY;
                        newX = i - 1;
                        newY = j;
                        if (i > 0 && maze[newX, newY] == positionValue && !added[newX, newY])
                        {
                            similarNeighborSum++;
                            if (neighbor1.X != -1)
                            {
                                neighbor1 = new Point(newX, newY);
                            }
                            else
                            {
                                neighbor2 = new Point(newX, newY);
                            }
                        }
                        newX = i;
                        newY = j - 1;
                        if (j > 0 && maze[newX, newY] == positionValue && !added[newX, newY])
                        {
                            similarNeighborSum++;
                            if (neighbor1.X != -1)
                            {
                                neighbor1 = new Point(newX, newY);
                            }
                            else
                            {
                                neighbor2 = new Point(newX, newY);
                            }
                        }
                        newX = i + 1;
                        newY = j;
                        if (i + 1 < maze.GetLength(0) && maze[newX, newY] == positionValue && !added[newX, newY])
                        {
                            similarNeighborSum++;
                            if (neighbor1.X != -1)
                            {
                                neighbor1 = new Point(newX, newY);
                            }
                            else
                            {
                                neighbor2 = new Point(newX, newY);
                            }
                        }
                        newX = i;
                        newY = j + 1;
                        if (j + 1 < maze.GetLength(1) && maze[newX, newY] == positionValue && !added[newX, newY])
                        {
                            similarNeighborSum++;
                            if (neighbor1.X != -1)
                            {
                                neighbor1 = new Point(newX, newY);
                            }
                            else
                            {
                                neighbor2 = new Point(newX, newY);
                            }
                        }
                        if (similarNeighborSum == 2)
                        {
                            // TODO: Add Path Node
                        }
                        else
                        {
                            // TODO: Add Pixel Node
                        }
                    }
                }
            }

            

            throw new NotImplementedException();
        }

        private void ResetSolver()
        {
            distance = new Dictionary<MazeNode, int>();
            previous = new Dictionary<MazeNode, Point>();
            adjacency_list = new Dictionary<MazeNode, List<MazeNode>>();
        }

        private void Dijkstra(MazeNode node)
        {
            throw new NotImplementedException();
        }

        private void MutateMaze(MazeNode endPoint)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
