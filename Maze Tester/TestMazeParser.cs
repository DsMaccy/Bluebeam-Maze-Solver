﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bluebeam_Maze_Solver;
using System.IO;
using System.Drawing;

namespace MazeTester
{
    /// <summary>
    /// The Maze Parser takes an image and sets a publicly readable instance variable.
    /// Test to ensure the resulting maze value is properly set.
    /// </summary>
    [TestClass]
    public class TestMazeParser
    {
        protected delegate MazeValue[,] parseFunctionOnString(string str, bool pathInvalid = true);
        protected delegate MazeValue[,] parseFunctionOnImage(Bitmap bm, bool pathInvalid = true);

        protected parseFunctionOnString parseOnString;
        protected parseFunctionOnImage parseOnImage;

        public TestMazeParser()
        {
            parseOnString = MazeParser.Parse;
            parseOnImage = MazeParser.Parse;
        }

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            if (Directory.Exists(FileSystemConstants.OUTPUT_FOLDER))
            {
                Cleanup();
            }

            Directory.CreateDirectory(FileSystemConstants.OUTPUT_FOLDER);
        }

        [ClassCleanup]
        public static void Cleanup()
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

        [TestMethod]
        public void TestPureBlueFile()
        {
            string filename = Path.Combine(FileSystemConstants.PARSE_TEST_FOLDER, "pure_blue.bmp");
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
        public void TestPureRedFile()
        {
            string filename = Path.Combine(FileSystemConstants.PARSE_TEST_FOLDER, "pure_red.png");
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
        public void TestPureBlackFile()
        {
            string filename = Path.Combine(FileSystemConstants.PARSE_TEST_FOLDER, "pure_black.png");
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
        public void TestPureWhiteFile()
        {
            string filename = Path.Combine(FileSystemConstants.PARSE_TEST_FOLDER, "pure_white.png");
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
        public void TestAlternatingColors()
        {
            string filename = Path.Combine(FileSystemConstants.PARSE_TEST_FOLDER, "alternating.png");
            MazeValue[,] maze = parseOnString(filename);
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    switch (i % 4)
                    {
                        case 0:
                            Assert.IsTrue(maze[i, j] == MazeValue.Wall);
                            break;
                        case 1:
                            Assert.IsTrue(maze[i, j] == MazeValue.End);
                            break;
                        case 2:
                            Assert.IsTrue(maze[i, j] == MazeValue.Start);
                            break;
                        case 3:
                            Assert.IsTrue(maze[i, j] == MazeValue.OpenSpace);
                            break;
                        default:
                            Assert.Fail();
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
                    MazeValue[,] maze = parseOnString(filename);
                    Assert.Fail();
                }
                catch (BadImageFormatException)
                {
                    // Test passed
                }
            }
        }

        [TestMethod]
        public void TestParseEmptyImage()
        {
            Random rngesus = new Random();
            string outputFile = Path.Combine(FileSystemConstants.OUTPUT_FOLDER, "tmp_empty.bmp");
            File.Create(outputFile).Close();
            try
            {
                MazeValue[,] maze = parseOnString(outputFile);
                Assert.Fail();
            }
            catch(BadImageFormatException)
            {
                // Pass
            }
        }
    }
}
