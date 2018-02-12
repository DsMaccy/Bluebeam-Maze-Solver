using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bluebeam_Maze_Solver;
using System.IO;

namespace MazeTester
{
    /// <summary>
    /// The Maze Parser takes an image and sets a publicly readable instance variable.
    /// Test to ensure the resulting maze value is properly set.
    /// </summary>
    [TestClass]
    public class TestMazeParser
    {
        [TestMethod]
        public void TestPureGreenFile()
        {
            string filename = Path.Combine(FileSystemConstants.PARSE_TEST_FOLDER, "pure_green.bmp");
            MazeParser mp = new MazeParser(filename);
        }
        [TestMethod]
        public void TestPureBlueFile()
        {
            string filename = Path.Combine(FileSystemConstants.PARSE_TEST_FOLDER, "pure_green.bmp");
            MazeParser mp = new MazeParser(filename);
            for (int i = 0; i < mp.Maze.GetLength(0); i++)
            {
                for (int j = 0; j < mp.Maze.GetLength(1); j++)
                {
                    Assert.IsTrue(mp.Maze[i, j] == MazeValue.Start);
                }
            }
        }
        [TestMethod]
        public void TestPureRedFile()
        {
            string filename = Path.Combine(FileSystemConstants.PARSE_TEST_FOLDER, "pure_red.png");
            MazeParser mp = new MazeParser(filename);
            for (int i = 0; i < mp.Maze.GetLength(0); i++)
            {
                for (int j = 0; j < mp.Maze.GetLength(1); j++)
                {
                    Assert.IsTrue(mp.Maze[i, j] == MazeValue.End);
                }
            }
        }
        [TestMethod]
        public void TestPureBlackFile()
        {
            string filename = Path.Combine(FileSystemConstants.PARSE_TEST_FOLDER, "pure_black.png");
            MazeParser mp = new MazeParser(filename);
            for (int i = 0; i < mp.Maze.GetLength(0); i++)
            {
                for (int j = 0; j < mp.Maze.GetLength(1); j++)
                {
                    Assert.IsTrue(mp.Maze[i, j] == MazeValue.Wall);
                }
            }
        }
        [TestMethod]
        public void TestPureWhiteFile()
        {
            string filename = Path.Combine(FileSystemConstants.PARSE_TEST_FOLDER, "pure_white.png");
            MazeParser mp = new MazeParser(filename);
            for (int i = 0; i < mp.Maze.GetLength(0); i++)
            {
                for (int j = 0; j < mp.Maze.GetLength(1); j++)
                {
                    Assert.IsTrue(mp.Maze[i, j] == MazeValue.OpenSpace);
                }
            }
        }
        [TestMethod]
        public void TestAlternatingColors()
        {
            string filename = Path.Combine(FileSystemConstants.PARSE_TEST_FOLDER, "alternating.png");
            MazeParser mp = new MazeParser(filename);
            for (int i = 0; i < mp.Maze.GetLength(0); i++)
            {
                for (int j = 0; j < mp.Maze.GetLength(1); j++)
                {
                    switch (j % 4)
                    {
                        case 0:
                            Assert.IsTrue(mp.Maze[i, j] == MazeValue.Wall);
                            break;
                        case 1:
                            Assert.IsTrue(mp.Maze[i, j] == MazeValue.OpenSpace);
                            break;
                        case 2:
                            Assert.IsTrue(mp.Maze[i, j] == MazeValue.End);
                            break;
                        case 3:
                            Assert.IsTrue(mp.Maze[i, j] == MazeValue.Start);
                            break;
                    }                    
                }
            }
        }
        [TestMethod]
        public void TestBadImages()
        {
            foreach (string filename in Directory.EnumerateFiles(FileSystemConstants.BAD_IMAGES_FOLDER))
            {
                try
                {
                    MazeParser mp = new MazeParser(filename);
                    Assert.Fail();
                }
                catch (BadImageFormatException)
                {
                    // Test passed
                }
            }
        }
    }
}
