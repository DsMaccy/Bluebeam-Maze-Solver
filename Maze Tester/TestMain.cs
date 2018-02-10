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

        private readonly string OUTPUT_FILE = FileSystemConstants.OUTPUT_FOLDER + "\\" + "test_out.png";

        [ClassInitialize]
        public void Initialize()
        {
            Directory.CreateDirectory(FileSystemConstants.TEMP_FOLDER);

            if (!Directory.Exists(FileSystemConstants.OUTPUT_FOLDER))
            {
                Directory.CreateDirectory(FileSystemConstants.OUTPUT_FOLDER);
            }
        }

        [TestMethod, Timeout(5000)]
        public void TestBadInputs()
        {
            foreach (string filename in Directory.EnumerateFiles(FileSystemConstants.BAD_IMAGES_FOLDER))
            {
                Console.WriteLine(filename);
                string arguments = filename + " " + OUTPUT_FILE;
                System.Diagnostics.Process proc = System.Diagnostics.Process.Start("main.exe", arguments);
                proc.WaitForExit();
                Assert.IsTrue(proc.ExitCode == (int)ExitCode.BAD_INPUT);
            }
        }

        [ClassCleanup]
        public void Cleanup()
        {

            // Remove temp files and folder
            foreach (string filename in Directory.EnumerateFiles(FileSystemConstants.TEMP_FOLDER))
            {
                File.Delete(filename);
            }
            Directory.Delete(FileSystemConstants.TEMP_FOLDER);
        }
    }
}
