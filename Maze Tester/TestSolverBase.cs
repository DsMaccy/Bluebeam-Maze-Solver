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
        private const int NUM_RANDOM_TESTS = 1;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            if (!Directory.Exists(FileSystemConstants.OUTPUT_FOLDER))
            {
                Directory.CreateDirectory(FileSystemConstants.OUTPUT_FOLDER);
            }
        }

        [ClassCleanup]
        public void Cleanup()
        {
            // Needed to wait for garbage collector in order to properly delete file handlers
            GC.Collect();
            GC.WaitForPendingFinalizers();

            foreach (string path in Directory.EnumerateFiles(FileSystemConstants.OUTPUT_FOLDER))
            {
                File.Delete(path);
            }
            Directory.Delete(FileSystemConstants.OUTPUT_FOLDER);
        }

        public TestSolverBase(MazeSolver solver)
        {
            this.solver = solver;
        }


        [TestMethod]
        public void TestRandomlyGeneratedTinyMaze()
        {
            for (int i = 0; i < NUM_RANDOM_TESTS; i++)
            {
                ImageSize size = ImageSize.VerySmall;
                generator = new MazeGenerator.MazeGenerator(size);
                MazeValue[,] maze = MazeParser.Parse(generator.Image);
                solver.solve(ref maze);
            }
        }

        [TestMethod]
        public void TestRandomlyGeneratedSmallMaze()
        {
            for (int i = 0; i < NUM_RANDOM_TESTS; i++)
            {
                ImageSize size = ImageSize.Small;
                generator = new MazeGenerator.MazeGenerator(size);
                MazeValue[,] maze = MazeParser.Parse(generator.Image);
                solver.solve(ref maze);
            }
        }
        
        [TestMethod]
        public void TestRandomlyGeneratedMediumMaze()
        {
            for (int i = 0; i < NUM_RANDOM_TESTS; i++)
            {
                ImageSize size = ImageSize.Medium;
                generator = new MazeGenerator.MazeGenerator(size);
                MazeValue[,] maze = MazeParser.Parse(generator.Image);
                solver.solve(ref maze);
            }
        }

        [TestMethod]
        public void TestRandomlyGeneratedLargeMaze()
        {
            for (int i = 0; i < NUM_RANDOM_TESTS; i++)
            {
                ImageSize size = ImageSize.Large;
                generator = new MazeGenerator.MazeGenerator(size);
                MazeValue[,] maze = MazeParser.Parse(generator.Image);
                solver.solve(ref maze);
            }
        }

        [TestMethod]
        public void TestUnsolveableMazes()
        {
            foreach (string filename in Directory.EnumerateFiles(FileSystemConstants.UNSOLVEABLE_MAZES_FOLDER))
            {
                MazeValue[,] maze = MazeParser.Parse(filename);
                MazeValue[,] values = DeepCopy(maze);
                Assert.IsFalse(solver.solve(ref maze));
                Assert.IsTrue(CheckEqual(values, maze));
            }
        }

        [TestMethod]
        public void TestSmallMazes()
        {
            foreach (string filename in Directory.EnumerateFiles(FileSystemConstants.SMALL_MAZES_FOLDER))
            {
                MazeValue[,] maze = MazeParser.Parse(filename);
                MazeValue[,] values = DeepCopy(maze);
                Assert.IsTrue(solver.solve(ref maze));
                CheckMaze(maze);
            }
        }

        [TestMethod]
        public void TestLargeMazes()
        {
            foreach (string filename in Directory.EnumerateFiles(FileSystemConstants.LARGE_MAZES_FOLDER))
            {
                MazeValue[,] maze = MazeParser.Parse(filename);
                MazeValue[,] values = DeepCopy(maze);
                Assert.IsTrue(solver.solve(ref maze));
                CheckMaze(maze);
            }
        }

        [TestMethod]
        public void TestKnownMazes()
        {
            List<string> unprocessedFiles = new List<string>();
            foreach (string filepath in Directory.EnumerateFiles(FileSystemConstants.KNOWN_SOLUTION_MAZES_FOLDER))
            {
                string filename = Path.GetFileName(filepath);
                string nameWithoutExtension = filename.Substring(0, filename.IndexOf('.'));
                int expectedResult;
                if (int.TryParse(nameWithoutExtension, out expectedResult))
                {
                    MazeValue[,] maze = MazeParser.Parse(filepath);
                    solver.solve(ref maze);

                    int result = CheckMaze(maze);
                    Assert.AreEqual(expectedResult, result);
                }
                else
                {
                    unprocessedFiles.Add(filename);
                }

            }

            if (unprocessedFiles.Count > 0)
            {
                Console.WriteLine("The following files were not processed");
                foreach (string filename in unprocessedFiles)
                {
                    Console.WriteLine("\t" + filename);
                }
                Assert.Fail();
            }
            
        }

        protected int CheckMaze(Bluebeam_Maze_Solver.MazeValue[,] maze)
        {
            int pathCount = 0;
            int width = maze.GetLength(0);
            int height = maze.GetLength(1);
            bool startNeighborFound = false;
            bool endNeighborFound = false;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (maze[i, j] == MazeValue.Path)
                    {
                        pathCount++;
                        bool hasNeighborStart = (i + 1 < width && maze[i + 1, j] == MazeValue.Start) ||
                                                (j + 1 < height && maze[i, j + 1] == MazeValue.Start) ||
                                                (i - 1 >= 0 && maze[i - 1, j] == MazeValue.Start) ||
                                                (j - 1 >= 0 && maze[i, j - 1] == MazeValue.Start);

                        bool hasNeighborEnd = (i + 1 < width && maze[i + 1, j] == MazeValue.End) ||
                                              (j + 1 < height && maze[i, j + 1] == MazeValue.End) ||
                                              (i - 1 >= 0 && maze[i - 1, j] == MazeValue.End) ||
                                              (j - 1 >= 0 && maze[i, j - 1] == MazeValue.End);

                        bool hasNeighborPath = (i + 1 < width && maze[i + 1, j] == MazeValue.Path) ||
                                               (j + 1 < height && maze[i, j + 1] == MazeValue.Path) ||
                                               (i - 1 >= 0 && maze[i - 1, j] == MazeValue.Path) ||
                                               (j - 1 >= 0 && maze[i, j - 1] == MazeValue.Path);

                        Assert.IsTrue(hasNeighborPath || (hasNeighborStart && hasNeighborEnd));
                        startNeighborFound = startNeighborFound || hasNeighborStart;
                        endNeighborFound = startNeighborFound || hasNeighborEnd;
                    }
                    else if (maze[i, j] == MazeValue.Start)
                    {
                        bool hasNeighborEnd = (i + 1 < width && maze[i + 1, j] == MazeValue.End) ||
                                              (j + 1 < height && maze[i, j + 1] == MazeValue.End) ||
                                              (i - 1 >= 0 && maze[i - 1, j] == MazeValue.End) ||
                                              (j - 1 >= 0 && maze[i, j - 1] == MazeValue.End);
                        
                        startNeighborFound = startNeighborFound || hasNeighborEnd;
                        endNeighborFound = endNeighborFound || hasNeighborEnd;
                    }
                    else if (maze[i, j] == MazeValue.End)
                    {
                        bool hasNeighborStart = (i + 1 < width && maze[i + 1, j] == MazeValue.Start) ||
                                                (j + 1 < height && maze[i, j + 1] == MazeValue.Start) ||
                                                (i - 1 >= 0 && maze[i - 1, j] == MazeValue.Start) ||
                                                (j - 1 >= 0 && maze[i, j - 1] == MazeValue.Start);
                        startNeighborFound = startNeighborFound || hasNeighborStart;
                        endNeighborFound = endNeighborFound || hasNeighborStart;
                    }
                }
            }

            Assert.IsTrue(startNeighborFound);
            Assert.IsTrue(endNeighborFound);
            return pathCount;
        }
        protected MazeValue[,] DeepCopy(MazeValue[,] values)
        {
            int width = values.GetLength(0);
            int height = values.GetLength(1);

            MazeValue[,] newMaze = new MazeValue[width, height];

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
            Assert.IsTrue(maze1.GetLength(0) == maze2.GetLength(0));
            Assert.IsTrue(maze1.GetLength(1) == maze2.GetLength(1));

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
