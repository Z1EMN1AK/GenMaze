using System;



namespace Genetic_Algorithm.Source
{
    public class AlgoObject
    {
        // Population object to hold the population of specimens
        Population p { get; set; }
        // Counter for the number of iterations
        ulong iteration = 0;

        // Constructor to initialize the population with given parameters
        public AlgoObject(int popSize, Options options)
        {
            p = new Population(popSize, options);
        }

        // Method to run the genetic algorithm
        public void Run(Options options)
        {
            bool solutionFound = false;

            // Evaluate the initial population
            p.Evaluate(options);

            // Sort the population based on fitness
            p.Sort();

            Random rd = new Random();
            double mutationRate;

            // Infinite loop to run the genetic algorithm until a solution is found
            for (; ; )
            {
                // Check if the solution has been found
                if (options.stopIfReachedEnd == true)
                {
                    for (int i = 0; i < p.speciments.Length; i++)
                    {
                        if (p.speciments[i].reachedEnd)
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("Generation: ");
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine(iteration);
                            p.Show(i);

                            solutionFound = true;
                            break;
                        }
                    }
                }

                if (solutionFound)
                    break;

                // Adjust mutation rate based on the fitness of the best specimen
                if (p.speciments[0].fitness < 0.9m)
                    mutationRate = 0.01;
                else mutationRate = 0.05;

                // Apply mutation to the population
                for (int i = 0; i < p.speciments.Length; i++)
                {
                    for (int j = 0; j < p.speciments[i].genTabC.Length; j++)
                    {
                        if (rd.NextDouble() < mutationRate)
                        {
                            p.speciments[i].Mutate(j);
                        }
                    }
                }

                int spcSize = p.speciments.Length;

                // Determine the number of specimens to cross
                int specimentsToCross = (int)(spcSize * 0.5); // 50% of the population will be crossed

                int[] tab = new int[specimentsToCross];

                // Perform crossover to generate new specimens
                for (int i = 0; i < specimentsToCross; i++)
                {
                    int parent1 = rd.Next(0, specimentsToCross);
                    int parent2 = rd.Next(0, specimentsToCross);

                    while (parent1 == parent2)
                    {
                        parent1 = rd.Next(0, specimentsToCross);
                        parent2 = rd.Next(0, specimentsToCross);
                    }

                    p.speciments[parent1] = p.speciments[parent1].HeuristicCrossover(p.speciments[parent2], rd);

                    tab[i] = parent1;
                }

                // Evaluate the new population
                p.Evaluate(options);

                // Sort the population based on fitness
                p.Sort();

                // Display the best specimen every stopAfterGeneration iterations
                if (iteration % (ulong)options.stopAfterGeneration == 0)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Generation: ");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(iteration);
                    iteration++;
                    p.Show(0);
                }
                iteration++;
            }
        }
    }
}
