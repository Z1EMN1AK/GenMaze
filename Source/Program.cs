using System;
using System.IO;


namespace Genetic_Algorithm.Source
{
    class Program
    {
        static void Main(string[] args)
        {
            // Start the program
            StartProgram();
        }
        
        public static void StartProgram()
        {
            // Create a new Options object to store user preferences
            Options options = new Options();

            // Ask the user how many generations to show results after
            Console.WriteLine("Show results after how many generations: ");
            options.stopAfterGeneration = Convert.ToInt32(Console.ReadLine());

            Console.Clear();

            // Ask the user if they want to show results if the end is reached
            Console.WriteLine("Show results if reached the end (Y/N): ");
            char decision = Console.ReadKey().KeyChar;

            // Set the option based on user input
            if (decision == 'Y' || decision == 'y')
            {
                options.stopIfReachedEnd = true;
            }
            else
            {
                options.stopIfReachedEnd = false;
            }

            Console.Clear();

            // Ask the user if they want to save results to a file
            Console.WriteLine("Save results to file (Y/N): ");
            decision = Console.ReadKey().KeyChar;

            // Set the option based on user input
            if (decision == 'Y' || decision == 'y')
            {
                options.logToFile = true;
            }
            else
            {
                options.logToFile = false;
            }

            Console.Clear();

            // If the user wants to log to a file, ask for the file path
            if (options.logToFile)
            {
                do
                {
                    Console.WriteLine("Enter log file path: ");
                    options.logFilePath = Console.ReadLine();
                }
                while (options.logFilePath == null || !File.Exists(options.logFilePath));
            }

            do
            {
                Console.WriteLine("Generate map (Y/N): ");
                decision = Console.ReadKey().KeyChar;
                if (decision == 'Y' || decision == 'y')
                {
                    options.generateMap = true;
                }
                else
                {
                    options.generateMap = false;
                }
            } while (decision != 'Y' && decision != 'y' && decision != 'N' && decision != 'n');

            Console.Clear();

            // Ask the user for the map file path
            if (!options.generateMap)
            {
                do
                {
                    Console.WriteLine("Enter map file path: ");
                    options.path = Console.ReadLine();
                }
                while (options.path == null || !File.Exists(options.path));
            }

            // Create a new AlgoObject with the specified parameters and options
            AlgoObject program = new AlgoObject(1000,options);
            // Run the algorithm with the specified options
            program.Run(options);

            // Wait for a key press before closing the console
            Console.ReadKey();
        }
    }
}
