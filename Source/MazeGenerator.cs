using System;
using System.Collections.Generic;
using Genetic_Algorithm.Source;

namespace MazeGenerator
{
    class LabGen
    {
        // 2D array representing the labyrinth grid
        private char[][] tab { get; set; }

        // Stack to manage the positions during the labyrinth generation
        private Stack<Position> stack = new Stack<Position>();

        // Dimensions of the labyrinth
        private int sizeX { get; set; }
        private int sizeY { get; set; }

        // Single instance of Random
        private Random rand = new Random();

        // Constructor to initialize the labyrinth with given dimensions
        public LabGen(int x, int y)
        {
            // Ensure that sizeX and sizeY are odd
            sizeX = (x % 2 == 0) ? x + 1 : x;
            sizeY = (y % 2 == 0) ? y + 1 : y;

            // Initialize the labyrinth grid with walls ('#')
            tab = new char[sizeX][];
            for (int i = 0; i < sizeX; i++)
            {
                tab[i] = new char[sizeY];
                for (int j = 0; j < sizeY; j++)
                {
                    tab[i][j] = '#';
                }
            }

            // Set the edges as walls (only once, already set above)
            // No need to set again, as all cells are already '#'
        }

        // Method to generate the labyrinth
        public char[][] Generate()
        {
            // Choosing a random starting position on the edge (excluding corners)
            Position startPos = GetRandomEdgePosition();

            // Set 'S' on the edge
            tab[startPos.x][startPos.y] = 'S';

            // Determine the adjacent cell inside the maze
            Position adjacent = GetAdjacentInside(startPos);

            // Set the adjacent cell as path
            tab[adjacent.x][adjacent.y] = ' ';
            stack.Push(adjacent);

            // Generating the labyrinth using DFS
            while (stack.Count > 0)
            {
                Position current = stack.Peek();
                Position next = GetNextPosition(current);

                if (next.x != -1 && next.y != -1)
                {
                    // Mark the next cell and the wall between current and next as a path (' ')
                    tab[next.x][next.y] = ' ';
                    tab[(current.x + next.x) / 2][(current.y + next.y) / 2] = ' ';
                    stack.Push(next);
                }
                else
                {
                    // Backtracking if there are no unvisited neighbors
                    stack.Pop();
                }
            }

            // Add 'E' to the opposite edge
            Position endPos = GetOppositeEdgePosition(startPos);

            // Set 'E' on the edge
            tab[endPos.x][endPos.y] = 'E';

            // Ensure that 'E' is connected to the maze by opening adjacent cell
            Position adjacentEnd = GetAdjacentInside(endPos);
            tab[adjacentEnd.x][adjacentEnd.y] = ' ';

            return tab;
        }

        // Method to choose a random position on the edge (excluding corners)
        private Position GetRandomEdgePosition()
        {
            Position pos = new Position();
            int edge = rand.Next(4);
            switch (edge)
            {
                case 0: // Top edge
                    pos.x = 0;
                    pos.y = rand.Next(1, sizeY - 1);
                    break;
                case 1: // Bottom edge
                    pos.x = sizeX - 1;
                    pos.y = rand.Next(1, sizeY - 1);
                    break;
                case 2: // Left edge
                    pos.x = rand.Next(1, sizeX - 1);
                    pos.y = 0;
                    break;
                case 3: // Right edge
                    pos.x = rand.Next(1, sizeX - 1);
                    pos.y = sizeY - 1;
                    break;
                default:
                    pos.x = -1;
                    pos.y = -1;
                    break;
            }
            return pos;
        }

        // Method to get the opposite edge
        private Position GetOppositeEdgePosition(Position startPos)
        {
            Position endPos = new Position();
            // Determine opposite edge
            if (startPos.x == 0)
            {
                // Start on top edge, end on bottom
                endPos.x = sizeX - 1;
                endPos.y = rand.Next(1, sizeY - 1);
            }
            else if (startPos.x == sizeX - 1)
            {
                // Start on bottom edge, end on top
                endPos.x = 0;
                endPos.y = rand.Next(1, sizeY - 1);
            }
            else if (startPos.y == 0)
            {
                // Start on left edge, end on right
                endPos.x = rand.Next(1, sizeX - 1);
                endPos.y = sizeY - 1;
            }
            else if (startPos.y == sizeY - 1)
            {
                // Start on right edge, end on left
                endPos.x = rand.Next(1, sizeX - 1);
                endPos.y = 0;
            }
            else
            {
                // Should not occur
                endPos.x = -1;
                endPos.y = -1;
            }
            return endPos;
        }

        // Method to get the internal neighbor from the edge position
        private Position GetAdjacentInside(Position pos)
        {
            Position adjacent = new Position();
            if (pos.x == 0) // Top edge
            {
                adjacent.x = 1; // Move one down
                adjacent.y = pos.y;
            }
            else if (pos.x == sizeX - 1) // Bottom edge
            {
                adjacent.x = sizeX - 2; // Move one up
                adjacent.y = pos.y;
            }
            else if (pos.y == 0) // Left edge
            {
                adjacent.x = pos.x;
                adjacent.y = 1; // Move one right
            }
            else if (pos.y == sizeY - 1) // Right edge
            {
                adjacent.x = pos.x;
                adjacent.y = sizeY - 2; // Move one left
            }
            else
            {
                adjacent.x = -1;
                adjacent.y = -1;
            }
            return adjacent;
        }


        // Method to get the next position from the current position
        private Position GetNextPosition(Position pos)
        {
            List<Position> neighbors = new List<Position>();

            // List of directions to choose from
            List<(int dx, int dy)> directions = new List<(int, int)>
            {
                (-2, 0), // Up
                (2, 0),  // Down
                (0, -2), // Left
                (0, 2)   // Right
            };

            // Randomize the directions
            for (int i = directions.Count - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                var temp = directions[i];
                directions[i] = directions[j];
                directions[j] = temp;
            }

            // Reviewing directions in random order
            foreach (var (dx, dy) in directions)
            {
                int newX = pos.x + dx;
                int newY = pos.y + dy;

                if (newX > 0 && newX < sizeX - 1 && newY > 0 && newY < sizeY - 1 && tab[newX][newY] == '#')
                {
                    neighbors.Add(new Position { x = newX, y = newY });
                }
            }

            // Choose a random neighbor
            if (neighbors.Count > 0)
            {
                return neighbors[rand.Next(neighbors.Count)];
            }

            // Return (-1, -1) if no unvisited neighbors
            return new Position { x = -1, y = -1 };
        }
    }
}
