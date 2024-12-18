using System;
using System.IO;

using GeneticAlgorithm;


namespace GeneticAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            StartProgram();
        }


        public static void StartProgram()
        {
            Options options = new Options();

            Console.WriteLine("Show results after how many generations: ");
            options.stopAfterGeneration = Convert.ToInt32(Console.ReadLine());

            Console.Clear();

            Console.WriteLine("Show results if reached the end (Y/N): ");

            char decision = Console.ReadKey().KeyChar;

            if (decision == 'Y' || decision == 'y')
            {
                options.stopIfReachedEnd = true;
            }
            else
            {
                options.stopIfReachedEnd = false;
            }

            Console.Clear();

            Console.WriteLine("Save results to file (Y/N): ");
            decision = Console.ReadKey().KeyChar;

            if (decision == 'Y' || decision == 'y')
            {
                options.logToFile = true;
            }
            else
            {
                options.logToFile = false;
            }

            do
            {
                Console.WriteLine("Enter map file path: ");
                options.path = Console.ReadLine();
            }
            while (options.path == null || !System.IO.File.Exists(options.path));

            AlgoObject program = new AlgoObject(1000, 20, 40, options);
            program.Run(options);

            Console.ReadKey();
        }
    }
}
