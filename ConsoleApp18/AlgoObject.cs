using GeneticAlgorithm;
using System;


namespace GeneticAlgorithm
{
    public class AlgoObject
    {
        Population p { get; set; }
        ulong iteration = 0;

        public AlgoObject(int popSize, int mapSizeX, int mapSizeY, Options options)
        {
            p = new Population(popSize, mapSizeX, mapSizeY, options.path);
        }

        public void Run(Options options)
        {
            bool solutionFound = false;

            p.Evaluate(options);

            p.Sort();

            Random rd = new Random();
            double mutationRate;

            for (; ; )
            {
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

                if (p.speciments[0].fitness < 0.9m)
                    mutationRate = 0.01;
                else mutationRate = 0.05;

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

                int specimentsToCross = (int)(spcSize * 0.5); // 50% of the population will be crossed

                int[] tab = new int[specimentsToCross];

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

                p.Evaluate(options);

                p.Sort();

                if (iteration % (ulong)options.stopAfterGeneration == 0) // display every stopAfterGeneration
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
