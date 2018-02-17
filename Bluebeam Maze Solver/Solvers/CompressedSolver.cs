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

        public Dictionary<MazeNode, int> Distance()
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
            List<MazeNode> startNodes;
            List<MazeNode> endNodes;
            BuildMaze(ref maze, out startNodes, out endNodes);
            ResetSolver();

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
            throw new NotImplementedException();
        }

        private void ResetSolver()
        {
            throw new NotImplementedException();
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
