using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bluebeam_Maze_Solver;
using System.IO;

namespace MazeTester
{
    [TestClass]
    public class TestFuzzyParser : TestMazeParser
    {
        [ClassInitialize]
        public static new void Initialize(TestContext context)
        {
            if (Directory.Exists(FileSystemConstants.OUTPUT_FOLDER))
            {
                Cleanup();
            }

            Directory.CreateDirectory(FileSystemConstants.OUTPUT_FOLDER);
        }

        [ClassCleanup]
        public static new void Cleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (Directory.Exists(FileSystemConstants.OUTPUT_FOLDER))
            {
                // Remove any output images
                foreach (string filename in Directory.EnumerateFiles(FileSystemConstants.OUTPUT_FOLDER))
                {
                    File.Delete(filename);
                }
                Directory.Delete(FileSystemConstants.OUTPUT_FOLDER);
            }
        }


        public TestFuzzyParser()
        {
            parseOnString = MazeParser.FuzzyParse;
            parseOnImage = MazeParser.FuzzyParse;
        }

        [TestMethod]
        public void TestFuzzyWhite()
        {
            string filename = Path.Combine(FileSystemConstants.FUZZY_PARSE_TEST_FOLDER, "White.png");
            MazeValue[,] maze = parseOnString(filename);
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    Assert.IsTrue(maze[i, j] == MazeValue.OpenSpace);
                }
            }
        }

        [TestMethod]
        public void TestFuzzyRed()
        {
            string filename = Path.Combine(FileSystemConstants.FUZZY_PARSE_TEST_FOLDER, "Red.png");
            MazeValue[,] maze = parseOnString(filename);
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    Assert.IsTrue(maze[i, j] == MazeValue.End);
                }
            }
        }

        [TestMethod]
        public void TestFuzzyBlack()
        {
            string filename = Path.Combine(FileSystemConstants.FUZZY_PARSE_TEST_FOLDER, "Black.png");
            MazeValue[,] maze = parseOnString(filename);
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    Assert.IsTrue(maze[i, j] == MazeValue.Wall);
                }
            }
        }

        [TestMethod]
        public void TestFuzzyBlue()
        {
            string filename = Path.Combine(FileSystemConstants.FUZZY_PARSE_TEST_FOLDER, "Blue.png");
            MazeValue[,] maze = parseOnString(filename);
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    Assert.IsTrue(maze[i, j] == MazeValue.Start);
                }
            }
        }

        [TestMethod]
        public void TestFuzzyGreen()
        {
            string filename = Path.Combine(FileSystemConstants.FUZZY_PARSE_TEST_FOLDER, "Green.png");
            MazeValue[,] maze = parseOnString(filename, false);
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    Assert.IsTrue(maze[i, j] == MazeValue.Path);
                }
            }
        }
    }
}
