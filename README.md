# Bluebeam Maze Solver
## Table of Contents
1. Overview
2. System and Versions
3. Risk Assessment
4. Initial Brainstorming
5. Projects
5. Architecture
6. Architectural Issues

## Overview
The Bluebeam Maze Solver is a project created as part of an interview for the company Bluebeam.  This is a simple command line project that takes a maze image and creates a copy with added coloring to show a valid solution path. 

## System and Version
This project was created using the default starter project for C# command line and unit test projects through Visual Studio Community 2015.  This project was tested for Windows 8.1. 
The images supported by this program are bmp, png, and jpg.

## Risk Assessment
There are two major initial risks for me in this assignment:
1. Reading, and creating image files
2. Creating an image analysis strategy that is able to handle the maze efficiently

## Initial Brainstorming
##### Data Formating
The maze is a 2 dimensional world.  That means I can easily create a structured graph by housing nodes in a 2 dimensional array.  The matrix format will give me information about my neighbors.  I expect that regardless of the image format, I will be able to retrieve the image data as a grid of pixels.

##### For the solving of the maze:
1. The simple solution is DFS and BFS.  This will guarantee me solutions and even optimal solutions.  The thing that worries me is whether they will be computationally heavy. 
2. An alternative solution is to run multiple DFS/BFS algorithms using an overarching sampling process to pick random starting points throughout the maze.  This may not guarantee the best algorithm, but if I can connect the disjoint solvers by keeping both a global and local tally of visited nodes, then I may be able to reduce the overall runtime by taking advantage of multithreading. I expect that the solution.  This promises to be complicated in the glue logic of the multiple search samples. 
3. Another idea is to compress the maze into regions and run BFS on the regions

## Projects
For this program, I had 4 visual studio projects.
##### Blubeam Maze Solver
This is the project for the provided assignment that actually implements the command line program.  This program takes an input image of a maze and produces a solution maze.  This project also handles a variety of alternative cases such as bad input, unsolveable mazes, and input that is too large.  Under the file ErrorCodes.cs, there are a list of possible outputs for the main program.  There is also a file called ErrorHandler.cs that has the command line outputs for the program for each of the different return codes.

##### Maze Tester
In this project, I put all of the unit tests and system tests.  I used test driven development and would run these tests often as I continued to modify the code.  As I thought of more corner cases and encountered additional problems, I added to these tests.  Although I did not have any automated continuous integration tests, this is also what I used this project for.

##### MazeGenerator
The MazeGenerator project was a class library that exposed a MazeGenerator API.  This api allowed for randomized maze generation based on 4 different sizes: 31x31, 301x301, 1001x1001, and 5001x5001.  The maze generator created the maze on the order of pixels. It started by creating an alterating grid of black and white such that there was a black border around the entire image. Currently, this library guarantees that the maze is solveable but will also be very easily solveable by any human.

##### Maze Generator UI
I added an additonal windows application project for the maze generator for convenience.  The UI is a simple dropdown menu with a generate button which spawns a save file dialog.  This proved useful for checking the result of the maze generator and also for picking the initial set of test files to add to the project.

## Architecture
##### Main Console Application
For the main application, I used a pipeline architecture to separate out the parsing and saving of the images from the solving of the maze.  The parser handled the translation of the image files.  I separated this out because I was considering possible changes to the semantics of which color meant what and also because I was uncertain of whether the different image file types required different code to analyze.  Fortunately, the latter was not an issue because the Bitmap object under System.Drawing allowed a consistent interface.  The former actually proved a bit helpful, however, with the fuzzy parser which allows for colors to be slightly off.  The fuzzy parser uses a 3-dimensional distance formula to check how similar the color found is to any of the colors being searched for.  If the identified color is similar enough to one of the colors being searched for, it is taken to be that color.  The parser converts the image into a matrix of MazeValue enum variables which specify what an individual pixel is supposed to represent (wall, start, open-space, end, or path). 

After the image is converted into a matrix of MazeValue objects, the new matrix is passed into a solver.  For the solver, I used the strategy pattern because I wasn't sure if I would need to create alternate solvers to improve the runtime or memory usage of the program.  The solvers inherit from the MazeSolver interface.  The MazeSolver requires only the single method *solve* to be implemented which returns a boolean value of true if the maze was solveable.  If the maze was indeed solveable, the MazeSolver should mutate the 2-dimensional array of MazeValues to change any appropriate value to the enum value Path.  A possible extension for the MazeSolver interface would be to add a method like *SolutionOptimal* which would return true if the solver guaranteed an optimal solution.  This might be useful for something like a quick solver which implemented DFS and simply provided the first valid solution.  Currently, the BasicSolver guarantees an optimal solution.

After the MazeSolver has mutated the matrix of MazeValues, the Parser is then called to save the resulting 2D array into the output file.  The program is currently set to fail if the output file was previously existed or if the path to the file is incomplete (e.g. missing directory.)  In hindsight, I realize as I write this that I could have used a prompt or a command line argument to ask the user if they wanted to overwrite the file.

##### Tester
I added a test file for each additional class I was testing with the exception of the MazeParser.  For the MazeParser, I created a test file to test the file generator.  I also created a class for the Parse method and created a child class for the Fuzzy Parser.  The idea behind making the Fuzzy Parse tester a child class of the strict Parse class (which required precise RGB values) was simply that in order for the Fuzzy Parser to be correct, it also needed to be able to correctly parse anything the strict Parse could handle.  

I similarly used inheritance for the solver tests.  All of the solvers should be able to solve any of the mazes.  For the test files for the solver, there is a parent abstract solver class which creates all of the solving test cases.  Each new solver would merely need to create a file for the new solver type and specify which solver class should be used. This would automatically propogate an additional set of tests for the new solver based on those defined in the abstract class.


## Architectural Issues
1. The separation of the parser and the solver require either specific data representations or requires a postprocessing analysis of the given format (Matrix of MazeValue types)


