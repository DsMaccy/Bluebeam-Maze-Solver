using System;
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
    public class TestMazeParserGenerator
    {
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
            // Needed to wait for garbage collector in order to properly delete file handlers
            GC.Collect();
            GC.WaitForPendingFinalizers();

            foreach (string path in Directory.EnumerateFiles(FileSystemConstants.OUTPUT_FOLDER))
            {
                File.Delete(path);
            }
        }

        #region Test Methods

        [TestMethod]
        public void TestGenerateEmptyImage()
        {
            Random rngesus = new Random();
            MazeValue[,] maze = new MazeValue[0, 0]; 
            string output_file = Path.Combine(FileSystemConstants.OUTPUT_FOLDER, "tmp_empty.bmp");
            Assert.IsFalse(MazeParser.GenerateFile(maze, output_file));
        }

        [TestMethod]
        public void TestGenerateInvalidImagePath()
        {
            Random rngesus = new Random();
            string output_file = Path.Combine(FileSystemConstants.OUTPUT_FOLDER, "tmp_invalid_path.bmp");
            File.Create(output_file).Close();
            try
            {
                int width = 10;

                MazeValue[,] maze = new MazeValue[width, width];
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        maze[i, j] = MazeValue.Wall;
                    }
                }
                
                // Make sure the GenerateFile function returns false if file already exists
                Assert.IsFalse(MazeParser.GenerateFile(maze, output_file));

                // Make sure the file has not changed (is empty)
                Assert.IsTrue(File.ReadAllBytes(output_file).Length == 0);
                try
                {
                    Bitmap image = new Bitmap(output_file);
                    Assert.Fail();
                }
                catch (ArgumentException)
                {
                    // Pass
                }
            }
            finally
            {
                //File.Delete(output_file);
            }
        }

        [TestMethod]
        public void TestCorrectnessForPNGFormat()
        {
            CreateAndTestRandomlyGeneratedImage(".png");
        }

        [TestMethod]
        public void TestCorrectnessForBMPFormat()
        {
            CreateAndTestRandomlyGeneratedImage(".bmp");
        }

        [TestMethod]
        public void TestCorrectnessForJPEGFormat()
        {
            CreateAndTestRandomlyGeneratedImage(".jpeg", true);
            CreateAndTestRandomlyGeneratedImage(".jpg", true);
        }

        #endregion

        #region Helper Methods

        private void CreateAndTestRandomlyGeneratedImage(string fileExtension, bool useFuzzy = false)
        {
            int NUM_TESTS = 10;
            Random rngesus = new Random();

            Array mazeValues = Enum.GetValues(typeof(MazeValue));
            for (int test_num = 0; test_num < NUM_TESTS; test_num++)
            {
                int width = rngesus.Next(75, 100);
                int height = rngesus.Next(75, 100);
                string output_file = Path.Combine(FileSystemConstants.OUTPUT_FOLDER, "tmp_" + 
                                                  test_num + fileExtension);

                // Create maze
                MazeValue[,] maze = new MazeValue[width, height];
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        maze[i, j] = (MazeValue)rngesus.Next(0, mazeValues.Length);
                    }
                }

                Assert.IsTrue(MazeParser.GenerateFile(maze, output_file));

                // Use the fuzzy parser on jpeg images
                MazeValue[,] mazeFromFile;
                if (!useFuzzy)
                {
                    mazeFromFile = MazeParser.Parse(output_file, false);
                }
                else
                {
                    mazeFromFile = MazeParser.FuzzyParse(output_file, false);
                }

                Assert.IsTrue(CheckMazeEquality(maze, mazeFromFile));
            }
        }

        private bool CheckMazeEquality(MazeValue[,] maze1, MazeValue[,] maze2)
        {
            if (maze1.GetLength(0) != maze2.GetLength(0) || maze1.GetLength(1) != maze2.GetLength(1))
            {
                return false;
            }

            for (int i = 0; i < maze1.GetLength(0); i++)
            {
                for (int j = 0; j < maze1.GetLength(1); j++)
                {
                    if (maze1[i, j] != maze2[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion
    }
}
