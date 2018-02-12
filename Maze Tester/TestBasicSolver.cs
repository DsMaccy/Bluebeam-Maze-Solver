using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bluebeam_Maze_Solver.Solvers;

namespace MazeTester
{
    [TestClass]
    public class TestBasicSolver : TestSolverBase
    {
        public TestBasicSolver() : base(new BasicSolver())
        { }
    }
}
