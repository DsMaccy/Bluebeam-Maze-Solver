# Bluebeam Maze Solver
## Table of Contents
### 1. Overview
### 2. System and Versions
### 3. Risk Assessment
### 4. Initial Brainstorming
### 5. Architectural Layout

## Overview
The Bluebeam Maze Solver is a project created as part of an interview for the company Bluebeam.  This is a simple command line project that takes a maze image and creates a copy with added coloring to show a valid solution path. 

## System and Version
This project was created using the default starter project for C# command line and unit test projects through Visual Studio Community 2015.  This project was tested for Windows 8.1 and Windows 10. 
The images supported by this program are bmp, png, and jpg.

## Risk Assessment
There are two major initial risks for me in this assignment:
1. Reading, and creating image files
2. Creating an image analysis strategy that is able to handle the maze efficiently

## Initial Brainstorming
The maze is a 2 dimensional world.  That means I can easily create a structured graph by housing nodes in a 2 dimensional array.  The matrix format will give me information about my neighbors.  
For the solving of the maze:
The simple solution is DFS and BFS.  This will guarantee me solutions and even optimal solutions.  The thing that worries me is whether they will be computationally heavy. 
An alternative solution is to run multiple DFS/BFS algorithms using an overarching sampling process to pick random starting points throughout the maze.  This may not guarantee the best algorithm, but if I can connect the disjoint solvers by keeping both a global and local tally of visited nodes, then I may be able to reduce the overall runtime by taking advantage of multithreading. I expect that the solution.  This promises to be complicated in the glue logic of the multiple search samples. 
Another brainstorm idea that I have and like is to use quad-tree compression to section out regions of availabe space.  I can then use BFS on the quad-tree compressed image to give myself an optimal solution.  Then within each of the compressed quadtrees, I can use an additional BFS to connect the neighboring regions.  These should fall under simplified cases based on how the groups neighbor eachother but requires further analysis.  


## Architectural Layout
The fact that I am uncertain about how intensive the maze analysis is going to be necessitates the use of the strategy pattern in order to iteratively design and test maze solvers. 

