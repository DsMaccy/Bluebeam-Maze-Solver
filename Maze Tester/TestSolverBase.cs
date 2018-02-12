using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bluebeam_Maze_Solver;
using MazeGenerator;
using System.IO;

namespace MazeTester
{
    /// <summary>
    /// This class assumes that the MazeParser works
    /// </summary>
    [TestClass]
    public abstract class TestSolverBase
    {
        private MazeSolver solver;
        private MazeGenerator.MazeGenerator generator;
        MazeParser parser;

        public TestSolverBase(MazeSolver solver)
        {
            this.solver = solver;
        }

        [TestMethod]
        public void TestRandomlyGeneratedMazes()
        {
            const int NUM_TESTS = 4;
            for (int i = 0; i < NUM_TESTS; i++)
            {
                ImageSize size;
                switch (i % 4)
                {
                    case 0:
                        size = ImageSize.VerySmall;
                        break;
                    case 1:
                        size = ImageSize.Small;
                        break;
                    case 2:
                        size = ImageSize.Medium;
                        break;
                    case 3:
                    default:
                        size = ImageSize.Large;
                        break;

                }
                generator = new MazeGenerator.MazeGenerator(size);
                parser = new MazeParser(generator.Image);
                solver.solve(parser.Maze);
            }
        }

        [TestMethod]
        public void TestUnsolveableMazes()
        {
            foreach (string filename in Directory.EnumerateFiles(FileSystemConstants.UNSOLVEABLE_MAZES_FOLDER))
            {
                parser = new MazeParser(filename);
                MazeValue[,] values = DeepCopy(parser.Maze);
                Assert.IsFalse(solver.solve(parser.Maze));
                Assert.IsTrue(CheckEqual(values, parser.Maze));
            }
        }

        protected void CheckMaze(Bluebeam_Maze_Solver.MazeValue[,] maze)
        {
            int width = maze.GetLength(0);
            int height = maze.GetLength(1);
            bool start_neighbor_found = false;
            bool end_neighbor_found = false;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (maze[i, j] == MazeValue.Path)
                    {
                        start_neighbor_found = (i + 1 < width && maze[i + 1, j] == MazeValue.Start) ||
                                               (j + 1 < height && maze[i, j + 1] == MazeValue.Start) ||
                                               (i - 1 > 0 && maze[i - 1, j] == MazeValue.Start) ||
                                               (j - 1 > 0 && maze[i, j - 1] == MazeValue.Start);

                        end_neighbor_found = (i + 1 < width && maze[i + 1, j] == MazeValue.End) ||
                                             (j + 1 < height && maze[i, j + 1] == MazeValue.End) ||
                                             (i - 1 > 0 && maze[i - 1, j] == MazeValue.End) ||
                                             (j - 1 > 0 && maze[i, j - 1] == MazeValue.End);

                        bool hasNeighborPath = (i + 1 < width && maze[i + 1, j] == MazeValue.End) ||
                                               (j + 1 < height && maze[i, j + 1] == MazeValue.End) ||
                                               (i - 1 > 0 && maze[i - 1, j] == MazeValue.End) ||
                                               (j - 1 > 0 && maze[i, j - 1] == MazeValue.End);

                        Assert.IsTrue(hasNeighborPath);
                    }
                }
            }

            Assert.IsTrue(start_neighbor_found);
            Assert.IsTrue(end_neighbor_found);
        }
        protected MazeValue[,] DeepCopy(MazeValue[,] values)
        {
            MazeValue[,] newMaze = new MazeValue[parser.Maze.GetLength(0), parser.Maze.GetLength(1)];
            int width = values.GetLength(0);
            int height = values.GetLength(1);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    newMaze[i, j] = values[i, j];
                }
            }
            return newMaze;
        }
        protected bool CheckEqual(MazeValue[,] maze1, MazeValue[,] maze2)
        {
            Assert.Equals(maze1.GetLength(0), maze2.GetLength(0));
            Assert.Equals(maze1.GetLength(1), maze2.GetLength(1));

            int width = maze1.GetLength(0);
            int height = maze1.GetLength(1);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (maze1[i, j] != maze2[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
