using System;
using System.Drawing;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bluebeam_Maze_Solver;

namespace MazeTester
{
    [TestClass]
    public class TestMain
    {
        #region Private Instance Variables and Helpers

        private readonly string OUTPUT_FILE = FileSystemConstants.OUTPUT_FOLDER + "\\" + "test_out.png";
        private static string COPY_FOLDER_PATH = "Good images copy";

        private bool ImageChanged(Bitmap oldImage, Bitmap newImage)
        {
            Assert.IsTrue(oldImage.Height == newImage.Height && oldImage.Width == newImage.Width);
            for (int i = 0; i < oldImage.Width; i++)
            {
                for (int j = 0; j < oldImage.Height; j++)
                {
                    if (oldImage.GetPixel(i, j) == newImage.GetPixel(i, j))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void testValidSolution(string infile, string outfile)
        {
            Bitmap inputImage = Bitmap.FromFile(infile) as Bitmap;
            Bitmap outputImage = Bitmap.FromFile(outfile) as Bitmap;
            Assert.IsTrue(inputImage.Height == outputImage.Height && inputImage.Width == outputImage.Width);
            bool startNeighborFound = false;
            bool endNeighborFound = false;
            for (int i = 0; i < inputImage.Width; i++)
            {
                for (int j = 0; j < inputImage.Height; j++)
                {
                    if (outputImage.GetPixel(i, j).ToArgb() == ColorMap.MAPPING[MazeValue.Path])
                    {
                        Assert.IsTrue(inputImage.GetPixel(i, j).ToArgb() == ColorMap.MAPPING[MazeValue.OpenSpace]);

                        bool hasNeighborStart = (i + 1 < outputImage.Width && outputImage.GetPixel(i + 1, j).ToArgb() == ColorMap.MAPPING[MazeValue.Start]) ||
                                               (j + 1 < outputImage.Height && outputImage.GetPixel(i, j + 1).ToArgb() == ColorMap.MAPPING[MazeValue.Start]) ||
                                               (i - 1 >= 0 && outputImage.GetPixel(i - 1, j).ToArgb() == ColorMap.MAPPING[MazeValue.Start]) ||
                                               (j - 1 >= 0 && outputImage.GetPixel(i, j - 1).ToArgb() == ColorMap.MAPPING[MazeValue.Start]);
                        bool hasNeighborEnd = (i + 1 < outputImage.Width && outputImage.GetPixel(i + 1, j).ToArgb() == ColorMap.MAPPING[MazeValue.End]) ||
                                             (j + 1 < outputImage.Height && outputImage.GetPixel(i, j + 1).ToArgb() == ColorMap.MAPPING[MazeValue.End]) ||
                                             (i - 1 >= 0 && outputImage.GetPixel(i - 1, j).ToArgb() == ColorMap.MAPPING[MazeValue.End]) ||
                                             (j - 1 >= 0 && outputImage.GetPixel(i, j - 1).ToArgb() == ColorMap.MAPPING[MazeValue.End]);

                        bool hasNeighborPath = (i + 1 < outputImage.Width && outputImage.GetPixel(i + 1, j).ToArgb() == ColorMap.MAPPING[MazeValue.Path]) ||
                                               (j + 1 < outputImage.Height && outputImage.GetPixel(i, j + 1).ToArgb() == ColorMap.MAPPING[MazeValue.Path]) ||
                                               (i - 1 >= 0 && outputImage.GetPixel(i - 1, j).ToArgb() == ColorMap.MAPPING[MazeValue.Path]) ||
                                               (j - 1 >= 0 && outputImage.GetPixel(i, j - 1).ToArgb() == ColorMap.MAPPING[MazeValue.Path]);
                        
                        Assert.IsTrue(hasNeighborPath || (hasNeighborStart && hasNeighborEnd));
                        startNeighborFound = startNeighborFound || hasNeighborStart;
                        endNeighborFound = startNeighborFound || hasNeighborEnd;
                    }
                    else if (outputImage.GetPixel(i, j).ToArgb() == ColorMap.MAPPING[MazeValue.Start])
                    {
                        bool hasNeighborEnd = (i + 1 < outputImage.Width && outputImage.GetPixel(i + 1, j).ToArgb() == ColorMap.MAPPING[MazeValue.End]) ||
                                              (j + 1 < outputImage.Height && outputImage.GetPixel(i, j + 1).ToArgb() == ColorMap.MAPPING[MazeValue.End]) ||
                                              (i - 1 >= 0 && outputImage.GetPixel(i - 1, j).ToArgb() == ColorMap.MAPPING[MazeValue.End]) ||
                                              (j - 1 >= 0 && outputImage.GetPixel(i, j - 1).ToArgb() == ColorMap.MAPPING[MazeValue.End]);

                        startNeighborFound = startNeighborFound || hasNeighborEnd;
                        endNeighborFound = endNeighborFound || hasNeighborEnd;
                    }
                    else if (outputImage.GetPixel(i, j).ToArgb() == ColorMap.MAPPING[MazeValue.End])
                    {
                        bool hasNeighborStart = (i + 1 < outputImage.Width && outputImage.GetPixel(i + 1, j).ToArgb() == ColorMap.MAPPING[MazeValue.Start]) ||
                                                (j + 1 < outputImage.Height && outputImage.GetPixel(i, j + 1).ToArgb() == ColorMap.MAPPING[MazeValue.Start]) ||
                                                (i - 1 >= 0 && outputImage.GetPixel(i - 1, j).ToArgb() == ColorMap.MAPPING[MazeValue.Start]) ||
                                                (j - 1 >= 0 && outputImage.GetPixel(i, j - 1).ToArgb() == ColorMap.MAPPING[MazeValue.Start]);
                        startNeighborFound = startNeighborFound || hasNeighborStart;
                        endNeighborFound = endNeighborFound || hasNeighborStart;
                    }
                }
            }

            Assert.IsTrue(startNeighborFound);
            Assert.IsTrue(endNeighborFound);
        }
        
        #endregion



        #region Class Initializer and Destructor

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            if (Directory.Exists(FileSystemConstants.OUTPUT_FOLDER) || Directory.Exists(COPY_FOLDER_PATH))
            {
                Cleanup();
            }

            Directory.CreateDirectory(FileSystemConstants.OUTPUT_FOLDER);
            Directory.CreateDirectory(COPY_FOLDER_PATH);
            foreach (string filename in Directory.EnumerateFiles(FileSystemConstants.SMALL_MAZES_FOLDER))
            {
                File.Copy(filename, Path.Combine(COPY_FOLDER_PATH, Path.GetFileName(filename)));
            }
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Remove image copies
            if (Directory.Exists(COPY_FOLDER_PATH))
            {
                foreach (string filename in Directory.EnumerateFiles(COPY_FOLDER_PATH))
                {
                    File.Delete(filename);
                }
                Directory.Delete(COPY_FOLDER_PATH);
            }

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

        #endregion


        #region Test Methods

        [TestMethod, Timeout(5000)]
        public void TestNonExistingInput()
        {
            string filenamePrefix = "This_file_doesn't_exist";
            for (int i = 0; i < 10; i++)
            {
                string arguments = filenamePrefix + i + " " + OUTPUT_FILE;
                System.Diagnostics.Process proc = System.Diagnostics.Process.Start(FileSystemConstants.EXECUTABLE_NAME, arguments);
                proc.WaitForExit();
                Assert.IsTrue(proc.ExitCode == (int)ExitCode.BAD_INPUT);
            }
        }

        [TestMethod, Timeout(5000)]
        public void TestBadImages()
        {
            foreach (string filename in Directory.EnumerateFiles(FileSystemConstants.BAD_IMAGES_FOLDER))
            {
                string arguments = "\"" + filename + "\" \"" + OUTPUT_FILE + "\"";
                System.Diagnostics.Process proc = System.Diagnostics.Process.Start(FileSystemConstants.EXECUTABLE_NAME, arguments);
                proc.WaitForExit();
                Assert.IsTrue(proc.ExitCode == (int)ExitCode.BAD_INPUT);
            }
        }

        [TestMethod, Timeout(5000)]
        public void TestBadOutputPaths()
        {
            foreach (string filename in Directory.EnumerateFiles(COPY_FOLDER_PATH))
            {
                string arguments = "\"" + filename + "\" \"" + filename + "\"";
                Bitmap oldImage = Bitmap.FromFile(filename) as Bitmap;
                System.Diagnostics.Process proc = System.Diagnostics.Process.Start(FileSystemConstants.EXECUTABLE_NAME, arguments);
                proc.WaitForExit();
                Bitmap newImage = Bitmap.FromFile(filename) as Bitmap;
                Assert.IsTrue(proc.ExitCode == (int)ExitCode.INVALID_OUTPUT_PATH);
                Assert.IsFalse(ImageChanged(oldImage, newImage));
            }
        }


        /// <summary>
        /// This can only be tested with files that can't be held by the solver in memory
        /// This test may not apply to all solvers, and the limit reached is different for each solver and architecture
        /// </summary>
        //[TestMethod]
        public void TestOverlyLargeImage()
        {
            foreach (string filename in Directory.EnumerateFiles(FileSystemConstants.OVERLY_LARGE_MAZES_FOLDER))
            {
                string arguments = "\"" + filename + "\" \"" + OUTPUT_FILE + "\"";
                System.Diagnostics.Process proc = System.Diagnostics.Process.Start(FileSystemConstants.EXECUTABLE_NAME, arguments);
                proc.WaitForExit();
                Assert.IsTrue(proc.ExitCode == (int)ExitCode.INPUT_TOO_LARGE);
            }
        }

        [TestMethod]//, Timeout(5000)]
        public void TestUnsolveableMazes()
        {
            foreach (string filename in Directory.EnumerateFiles(FileSystemConstants.UNSOLVEABLE_MAZES_FOLDER))
            {
                string arguments = "\"" + filename + "\" \"" + OUTPUT_FILE + "\"";
                System.Diagnostics.Process proc = System.Diagnostics.Process.Start(FileSystemConstants.EXECUTABLE_NAME, arguments);
                proc.WaitForExit();
                Assert.IsTrue(proc.ExitCode == (int)ExitCode.UNSOLVEABLE);
            }
        }

        [TestMethod]//, Timeout(5000)]
        public void TestSmallMazes()
        {
            foreach (string filename in Directory.EnumerateFiles(FileSystemConstants.SMALL_MAZES_FOLDER))
            {
                string arguments = "\"" + filename + "\" \"" + OUTPUT_FILE + "\"";
                System.Diagnostics.Process proc = System.Diagnostics.Process.Start(FileSystemConstants.EXECUTABLE_NAME, arguments);
                proc.WaitForExit();
                Assert.IsTrue(proc.ExitCode == (int)ExitCode.GOOD);
                testValidSolution(filename, OUTPUT_FILE);

                // Cleanup output file
                GC.Collect();
                GC.WaitForPendingFinalizers();
                File.Delete(OUTPUT_FILE);
            }
        }

        [TestMethod]
        public void TestLargeMazes()
        {
            foreach (string filename in Directory.EnumerateFiles(FileSystemConstants.LARGE_MAZES_FOLDER))
            {
                string arguments = "\"" + filename + "\" \"" + OUTPUT_FILE + "\"";
                System.Diagnostics.Process proc = System.Diagnostics.Process.Start(FileSystemConstants.EXECUTABLE_NAME, arguments);
                proc.WaitForExit();
                Assert.IsTrue(proc.ExitCode == (int)ExitCode.GOOD);
                testValidSolution(filename, OUTPUT_FILE);

                // Cleanup output file
                GC.Collect();
                GC.WaitForPendingFinalizers();
                File.Delete(OUTPUT_FILE);
            }
        }

        #endregion
    }
}
