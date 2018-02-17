using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bluebeam_Maze_Solver.Solvers
{
    public interface MazeNode
    {
        /// <summary>
        /// Determines whether a given point is represeneted by this node object
        /// </summary>
        /// <param name="p">The point to check for in this node</param>
        /// <returns>True if the point <paramref name="p"/> is contained within this node, otherwise return false</returns>
        bool Contains(Point p);

        /// <summary>
        /// Return a list of Points contained by this MazeNode that correspond to a minimal path between the enter and exit points
        /// </summary>
        /// <param name="enterNeighbor">The entering position for a path</param>
        /// <param name="exitNeighbor">The exiting position for a path</param>
        /// <returns>a list of points in the path that connects the enter and exit neighbor or null if the enter and exit parameters are not actually neighbors</returns>
        Point[] GetPath(Point enterNeighbor, Point exitNeighbor);

        /// <summary>
        /// Gives a weight representing how many pixels would be required to create a path from an entering node to an exiting node cutting through this node
        /// </summary>
        /// <param name="enterNeighbor"></param>
        /// <param name="exitNeighbor"></param>
        /// <returns>An integer of the number of pixels needed to connect the <paramref name="enterNeighbor"/> and the <paramref name="exitNeighbor"/>, return -1 if the exit and enter neighbors are not valid neighbors</returns>
        int GetPathWeight(Point enterNeighbor, Point exitNeighbor);

        /// <summary>
        /// Determines whether the provided point is neighbors this node
        /// </summary>
        /// <param name="p">The query point that is determined to be or not be a neighbor</param>
        /// <returns>True iff the point p represents a neighbor to this MazeNode object</returns>
        bool isNeighbor(Point p);
    }

    class PixelNode : MazeNode
    {
        private Point pixel;
        public PixelNode(int X, int Y)
        {
            pixel = new Point(X, Y);
        }

        public bool Contains(Point p)
        {
            return p == pixel;
        }

        public Point[] GetPath(Point enterNeighbor, Point exitNeighbor)
        {
            if (isNeighbor(enterNeighbor) && isNeighbor(exitNeighbor))
            {
                return new Point[] { pixel };
            }
            return null;
        }

        public int GetPathWeight(Point enterNeighbor, Point exitNeighbor)
        {
            if (isNeighbor(enterNeighbor) && isNeighbor(exitNeighbor))
            {
                return 1;
            }
            return -1;
        }

        public bool isNeighbor(Point p)
        {
            int manhattanDistance = Math.Abs(pixel.X - p.X) + Math.Abs(pixel.Y - p.Y);
            return manhattanDistance == 1;
        }
    }

    class RectangleNode : MazeNode
    {
        private Point corner1;
        private Point corner2;

        public bool Contains(Point p)
        {
            int minX = Math.Min(corner1.X, corner2.X);
            int minY = Math.Min(corner1.Y, corner2.Y);
            int maxX = Math.Max(corner1.X, corner2.X);
            int maxY = Math.Max(corner1.Y, corner2.Y);

            return p.X <= maxX && p.X >= minX &&
                   p.Y <= maxY && p.Y >= minY;
        }

        public Point[] GetPath(Point enterNeighbor, Point exitNeighbor)
        {
            if (isNeighbor(enterNeighbor) && isNeighbor(exitNeighbor))
            {
                throw new NotImplementedException();
            }
            return null;
        }

        public int GetPathWeight(Point enterNeighbor, Point exitNeighbor)
        {
            if (isNeighbor(enterNeighbor) && isNeighbor(exitNeighbor))
            {
                throw new NotImplementedException();
            }
            return -1;
        }

        public bool isNeighbor(Point p)
        {
            throw new NotImplementedException();
        }
    }

    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    class PathNode : MazeNode
    {
        private List<Point> points;

        public bool Contains(Point p)
        {
            return points.Contains(p);
        }

        public Point[] GetPath(Point enterNeighbor, Point exitNeighbor)
        {
            throw new NotImplementedException();
        }

        public int GetPathWeight(Point enterNeighbor, Point exitNeighbor)
        {
            int enterToHead = Math.Abs(points[0].X - enterNeighbor.X) + Math.Abs(points[0].Y - enterNeighbor.Y);
            int enterToTail = Math.Abs(points[points.Count - 1].X - enterNeighbor.X) + Math.Abs(points[points.Count - 1].Y - enterNeighbor.Y);

            int exitToHead = Math.Abs(points[0].X - exitNeighbor.X) + Math.Abs(points[0].Y - exitNeighbor.Y);
            int exitToTail = Math.Abs(points[points.Count - 1].X - exitNeighbor.X) + Math.Abs(points[points.Count - 1].Y - exitNeighbor.Y);

            if ((enterToHead == 1 && exitToHead == 1) || (enterToTail == 1 && exitToTail == 1))
            {
                return -1;
            }

            if ((enterToHead == 1 && exitToTail == 1) || (enterToTail == 1 && exitToHead == 1))
            {
                return points.Count;
            }

            return -1;
        }

        public bool isNeighbor(Point p)
        {
            int manhattanDistanceFromStart = Math.Abs(points[0].X - p.X) + Math.Abs(points[0].Y - p.Y);
            int manhattanDistanceFromEnd = Math.Abs(points[points.Count - 1].X - p.X) + Math.Abs(points[points.Count - 1].Y - p.Y);

            return manhattanDistanceFromStart == 1 || manhattanDistanceFromEnd == 1;
        }

        public bool addPoint(Point point)
        {
            int pointToHead = Math.Abs(points[0].X - point.X) + Math.Abs(points[0].Y - point.Y);
            int pointToTail = Math.Abs(points[points.Count - 1].X - point.X) + Math.Abs(points[points.Count - 1].Y - point.Y);


            if (pointToHead == 1 && pointToTail == 1)
            {
                return false;
            }
            else if (pointToHead == 1)
            {
                points.Insert(0, point);
                return true;
            }
            else if (pointToTail == 1)
            {
                points.Add(point);
                return true;
            }
            return false;
        }
    }
}
