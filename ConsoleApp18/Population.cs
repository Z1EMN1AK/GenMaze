using GeneticAlgorithm;
using System;


namespace GeneticAlgorithm
{
    public class Population
    {
        public Specimen[] speciments { get; set; }
        public Map map { get; set; }

        Position startPos;
        Position endPos;

        decimal lastFitness = 0;

        public Population(int popSize, int mapSizeX, int mapSizeY, string path)
        {
            map = new Map(mapSizeX, mapSizeY, path);

            for (int x = 0; x < mapSizeX; x++)
            {
                for (int y = 0; y < mapSizeY; y++)
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

            speciments = new Specimen[popSize];

            int genSize = (mapSizeX + mapSizeY) * 4; // We are assuming twice the size of the map

            for (int i = 0; i < popSize; i++)
            {
                speciments[i] = new Specimen(genSize);
            }
        }

        public void Sort()
        {
            Array.Sort(speciments, (a, b) => b.fitness.CompareTo(a.fitness));
        }

        public void Show(int specimentNumber)
        {
            Console.ForegroundColor = ConsoleColor.White;
            //speciments[specimentNumber].PrintB();
            speciments[specimentNumber].PrintG();

            if (speciments[specimentNumber].fitness > lastFitness)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }

            else if (speciments[specimentNumber].fitness < lastFitness)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            lastFitness = speciments[specimentNumber].fitness;

            Console.Write("Fitness: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(speciments[specimentNumber].fitness);

            char[][] tempTab = map.map.Select(row => row.ToArray()).ToArray();

            Position actualPos = startPos;

            for (int i = 0; i < speciments[specimentNumber].genTabC.Length; i++)
            {
                switch (speciments[specimentNumber].genTabC[i])
                {
                    case 'L':

                        if (actualPos.x == 0)
                            break;

                        else if (actualPos.x > 0 && tempTab[actualPos.x - 1][actualPos.y] != '#')
                        {
                            actualPos.x--;
                            tempTab[actualPos.x][actualPos.y] = '*';
                        }
                        break;

                    case 'R':

                        if (actualPos.x > map.mapBoundries.x - 1)
                            break;

                        else if (actualPos.x < map.mapBoundries.x - 1 && tempTab[actualPos.x + 1][actualPos.y] != '#')
                        {
                            actualPos.x++;
                            tempTab[actualPos.x][actualPos.y] = '*';
                        }
                        break;

                    case 'U':

                        if (actualPos.y == 0)
                            break;

                        else if (actualPos.y > 0 && tempTab[actualPos.x][actualPos.y - 1] != '#')
                        {
                            actualPos.y--;
                            tempTab[actualPos.x][actualPos.y] = '*';
                        }
                        break;

                    case 'D':

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

            tempTab = map.map.Select(row => row.ToArray()).ToArray();
        }

        public void Evaluate(Options options)
        {
            char[][] tempTab = map.map.Select(row => row.ToArray()).ToArray();

            foreach (Specimen s in speciments)
            {
                Position actualPos = new Position { x = startPos.x, y = startPos.y };

                bool[][] visited = new bool[map.mapBoundries.x][];
                for (int i = 0; i < map.mapBoundries.x; i++)
                {
                    visited[i] = new bool[map.mapBoundries.y];
                }

                bool wallEncountered = false;
                int traversedTiles = 0;

                for (int i = 0; i < s.genTabC.Length; i++)
                {

                    // Make the move based on the instructions in the genes
                    switch (s.genTabC[i])
                    {
                        case 'L': // Left
                            if (actualPos.x > 0 && tempTab[actualPos.x - 1][actualPos.y] != '#')
                            {
                                if (tempTab[actualPos.x - 1][actualPos.y] == '*')
                                    traversedTiles++;

                                actualPos.x--;
                                tempTab[actualPos.x][actualPos.y] = '*';  // Set the tile as visited
                            }
                            else if (tempTab[actualPos.x][actualPos.y] == '#' || tempTab[actualPos.x][actualPos.y] == 'S')
                                wallEncountered = true;

                            break;

                        case 'R': // Right
                            if (actualPos.x < map.mapBoundries.x - 1 && tempTab[actualPos.x + 1][actualPos.y] != '#')
                            {
                                if (tempTab[actualPos.x + 1][actualPos.y] == '*')
                                    traversedTiles++;

                                actualPos.x++;
                                tempTab[actualPos.x][actualPos.y] = '*';  // Set the tile as visited
                            }
                            else if (tempTab[actualPos.x][actualPos.y] == '#' || tempTab[actualPos.x][actualPos.y] == 'S')
                                wallEncountered = true;

                            break;

                        case 'U': // Up
                            if (actualPos.y > 0 && tempTab[actualPos.x][actualPos.y - 1] != '#')
                            {
                                if (tempTab[actualPos.x][actualPos.y - 1] == '*')
                                    traversedTiles++;

                                actualPos.y--;
                                tempTab[actualPos.x][actualPos.y] = '*';  // Set the tile as visited
                            }
                            else if (tempTab[actualPos.x][actualPos.y] == '#' || tempTab[actualPos.x][actualPos.y] == 'S')
                                wallEncountered = true;

                            break;

                        case 'D': // Down
                            if (actualPos.y < map.mapBoundries.y - 1 && tempTab[actualPos.x][actualPos.y + 1] != '#')
                            {
                                if (tempTab[actualPos.x][actualPos.y + 1] == '*')
                                    traversedTiles++;

                                actualPos.y++;
                                tempTab[actualPos.x][actualPos.y] = '*';  // Set the tile as visited
                            }
                            else if (tempTab[actualPos.x][actualPos.y] == '#' || tempTab[actualPos.x][actualPos.y] == 'S')
                                wallEncountered = true;

                            break;
                    }

                    if (actualPos.x == endPos.x && actualPos.y == endPos.y)
                        s.reachedEnd = true;
                    else if (wallEncountered)
                    {
                        s.reachedEnd = false;
                        break;
                    }

                }

                s.Fitness(actualPos, endPos, map.mapBoundries, traversedTiles, wallEncountered, options);
            }

            tempTab = map.map.Select(row => row.ToArray()).ToArray();
        }
    }
}
