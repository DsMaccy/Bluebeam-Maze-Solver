using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bluebeam_Maze_Solver.Solvers;

namespace MazeTester
{
    [TestClass]
    public class TestCompressedSolver : TestSolverBase
    {
        public TestCompressedSolver() : base(new CompressedSolver())
        { }
    }
}
