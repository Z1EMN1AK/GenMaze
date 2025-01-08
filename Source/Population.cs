using System;
using MazeGenerator;

namespace Genetic_Algorithm.Source
{
    public class Population
    {
        public Specimen[] speciments { get; set; } // Array of specimens in the population
        public Map map { get; set; } // Map object representing the environment

        Position startPos; // Starting position on the map
        Position endPos; // Ending position on the map

        decimal lastFitness = 0; // Last recorded fitness value

        // Constructor to initialize the population
        public Population(int popSize, Options options)
        {
            
            if(options.generateMap)
            {
                Console.Clear();

                int x, y;

                Console.Write("Enter the width of the map: ");
                x = int.Parse(Console.ReadLine());

                Console.Write("Enter the height of the map: ");
                y = int.Parse(Console.ReadLine());

                LabGen labGen = new LabGen(x, y); // Initialize the labyrinth generator

                map = new Map(labGen.Generate()); // Generate the labyrinth

            } else map = new Map(options.path); // Initialize the map

            

            // Find the start and end positions on the map
            for (int x = 0; x < map.mapSizeX; x++)
            {
                for (int y = 0; y < map.mapSizeY; y++)
                {
                    if (map.map[x][y] == 'S')
                    {
                        startPos.x = x;
                        startPos.y = y;
                    }
                    else if (map.map[x][y] == 'E')
                    {
                        endPos.x = x;
                        endPos.y = y;
                    }
                }
            }

            speciments = new Specimen[popSize]; // Initialize the array of specimens

            int genSize = (map.mapSizeX + map.mapSizeY) * 4; // Calculate the size of the genetic code

            // Create specimens with the calculated genetic code size
            for (int i = 0; i < popSize; i++)
            {
                speciments[i] = new Specimen(genSize);
            }
        }

        // Method to sort the specimens based on their fitness
        public void Sort()
        {
            Array.Sort(speciments, (a, b) => b.fitness.CompareTo(a.fitness));
        }

        // Method to display a specific specimen
        public void Show(int specimentNumber)
        {
            Console.ForegroundColor = ConsoleColor.White;
            speciments[specimentNumber].PrintG(); // Print the genetic code of the specimen

            // Change the console color based on the fitness comparison
            if (speciments[specimentNumber].fitness > lastFitness)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (speciments[specimentNumber].fitness < lastFitness)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            lastFitness = speciments[specimentNumber].fitness; // Update the last fitness value

            Console.Write("Fitness: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(speciments[specimentNumber].fitness); // Display the fitness value

            char[][] tempTab = map.map.Select(row => row.ToArray()).ToArray(); // Create a copy of the map

            Position actualPos = startPos; // Set the starting position

            // Traverse the map based on the genetic code
            for (int i = 0; i < speciments[specimentNumber].genTabC.Length; i++)
            {
                switch (speciments[specimentNumber].genTabC[i])
                {
                    case 'L': // Move left
                        if (actualPos.x == 0)
                            break;
                        else if (actualPos.x > 0 && tempTab[actualPos.x - 1][actualPos.y] != '#')
                        {
                            actualPos.x--;
                            tempTab[actualPos.x][actualPos.y] = '*';
                        }
                        break;

                    case 'R': // Move right
                        if (actualPos.x > map.mapBoundries.x - 1)
                            break;
                        else if (actualPos.x < map.mapBoundries.x - 1 && tempTab[actualPos.x + 1][actualPos.y] != '#')
                        {
                            actualPos.x++;
                            tempTab[actualPos.x][actualPos.y] = '*';
                        }
                        break;

                    case 'U': // Move up
                        if (actualPos.y == 0)
                            break;
                        else if (actualPos.y > 0 && tempTab[actualPos.x][actualPos.y - 1] != '#')
                        {
                            actualPos.y--;
                            tempTab[actualPos.x][actualPos.y] = '*';
                        }
                        break;

                    case 'D': // Move down
                        if (actualPos.y > map.mapBoundries.y - 1)
                            break;
                        else if (actualPos.y < map.mapBoundries.y - 1 && tempTab[actualPos.x][actualPos.y + 1] != '#')
                        {
                            actualPos.y++;
                            tempTab[actualPos.x][actualPos.y] = '*';
                        }
                        break;
                }
            }

            // Display the map with the path taken by the specimen
            for (int i = 0; i < map.mapBoundries.x; i++)
            {
                for (int j = 0; j < map.mapBoundries.y; j++)
                {
                    if (tempTab[i][j] == '*')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if (tempTab[i][j] == 'S')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (tempTab[i][j] == 'E')
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else Console.ForegroundColor = ConsoleColor.Yellow;

                    Console.Write(tempTab[i][j]);
                }
                Console.WriteLine();
            }

            tempTab = map.map.Select(row => row.ToArray()).ToArray(); // Reset the map copy
        }

        // Method to evaluate the fitness of each specimen
        public void Evaluate(Options options)
        {
            char[][] tempTab = map.map.Select(row => row.ToArray()).ToArray(); // Create a copy of the map

            foreach (Specimen s in speciments)
            {
                Position actualPos = new Position { x = startPos.x, y = startPos.y }; // Set the starting position

                bool wallEncountered = false; // Flag to check if a wall is encountered
                int traversedTiles = 0; // Count of traversed tiles

                // Traverse the map based on the genetic code
                for (int i = 0; i < s.genTabC.Length; i++)
                {
                    switch (s.genTabC[i])
                    {
                        case 'L': // Move left
                            if (actualPos.x > 0 && tempTab[actualPos.x - 1][actualPos.y] != '#')
                            {
                                if (tempTab[actualPos.x - 1][actualPos.y] == '*')
                                    traversedTiles++;

                                actualPos.x--;
                                tempTab[actualPos.x][actualPos.y] = '*'; // Set the tile as visited
                            }
                            else if (tempTab[actualPos.x][actualPos.y] == '#' || tempTab[actualPos.x][actualPos.y] == 'S')
                                wallEncountered = true;

                            break;

                        case 'R': // Move right
                            if (actualPos.x < map.mapBoundries.x - 1 && tempTab[actualPos.x + 1][actualPos.y] != '#')
                            {
                                if (tempTab[actualPos.x + 1][actualPos.y] == '*')
                                    traversedTiles++;

                                actualPos.x++;
                                tempTab[actualPos.x][actualPos.y] = '*'; // Set the tile as visited
                            }
                            else if (tempTab[actualPos.x][actualPos.y] == '#' || tempTab[actualPos.x][actualPos.y] == 'S')
                                wallEncountered = true;

                            break;

                        case 'U': // Move up
                            if (actualPos.y > 0 && tempTab[actualPos.x][actualPos.y - 1] != '#')
                            {
                                if (tempTab[actualPos.x][actualPos.y - 1] == '*')
                                    traversedTiles++;

                                actualPos.y--;
                                tempTab[actualPos.x][actualPos.y] = '*'; // Set the tile as visited
                            }
                            else if (tempTab[actualPos.x][actualPos.y] == '#' || tempTab[actualPos.x][actualPos.y] == 'S')
                                wallEncountered = true;

                            break;

                        case 'D': // Move down
                            if (actualPos.y < map.mapBoundries.y - 1 && tempTab[actualPos.x][actualPos.y + 1] != '#')
                            {
                                if (tempTab[actualPos.x][actualPos.y + 1] == '*')
                                    traversedTiles++;

                                actualPos.y++;
                                tempTab[actualPos.x][actualPos.y] = '*'; // Set the tile as visited
                            }
                            else if (tempTab[actualPos.x][actualPos.y] == '#' || tempTab[actualPos.x][actualPos.y] == 'S')
                                wallEncountered = true;

                            break;
                    }

                    // Check if the end position is reached
                    if (actualPos.x == endPos.x && actualPos.y == endPos.y)
                        s.reachedEnd = true;
                    else if (wallEncountered)
                    {
                        s.reachedEnd = false;
                        break;
                    }
                }

                // Calculate the fitness of the specimen
                s.Fitness(actualPos, endPos, map.mapBoundries, traversedTiles, wallEncountered, options);
            }

            tempTab = map.map.Select(row => row.ToArray()).ToArray(); // Reset the map copy
        }
    }
}
