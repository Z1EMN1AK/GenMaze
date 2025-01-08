namespace Genetic_Algorithm.Source
{
    public class Specimen
    {
        public int tabSizeC;

        public Position pos;

        public bool reachedEnd;

        public char[] genTabC { get; set; }
        public decimal fitness { get; set; }

        public Specimen(int genSize)
        {
            tabSizeC = genSize;
            genTabC = new char[tabSizeC];

            reachedEnd = false;

            pos.x = 0;
            pos.y = 0;

            Random rd = new Random();

            for (int i = 0; i < tabSizeC; i++)
            {
                string data = "LRUD";

                genTabC[i] = data[rd.Next(0, 4)];
            }
        }

        public string PrintG() // Display the genetic code
        {
            for (int i = 0; i < tabSizeC; i++)
            {
                Console.Write(genTabC[i]);
            }
            Console.WriteLine();

            string s = new string(genTabC);

            return s;
        }

        public void Mutate(int place)
        {
            string data = "LRUD";
            Random rd = new Random();

            string tmp = data[rd.Next(0, 4)].ToString();

            while (tmp == genTabC[place].ToString())
            {
                tmp = data[rd.Next(0, 4)].ToString();
            }
            genTabC[place] = tmp[0];
        }

        public Specimen HeuristicCrossover(Specimen parent1, Random rd)
        {
            // Create new child
            Specimen child = new Specimen(tabSizeC);

            for (int i = 0; i < genTabC.Length; i++)
            {
                // Take a gene from the fitter parent
                if ((decimal)rd.NextDouble() < fitness / (fitness + parent1.fitness + 0.0001m))
                {
                    child.genTabC[i] = genTabC[i];
                }
                else
                {
                    child.genTabC[i] = parent1.genTabC[i];
                }
            }

            return child;
        }


        public void Fitness(Position actualPosition, Position endPosition, Position mapBoundries, int traversedTiles, bool wallCollision, Options options)
        {
            // Calculate the Euclidean distance from the target
            decimal distance = (decimal)Math.Sqrt(Math.Pow(actualPosition.x - endPosition.x, 2) + Math.Pow(actualPosition.y - endPosition.y, 2));
            decimal maxDistance = (decimal)Math.Sqrt(Math.Pow(mapBoundries.x, 2) + Math.Pow(mapBoundries.y, 2)); // Maximal euclidean distance on the map

            // Fitness according to the distance
            decimal distanceScore = 1.0m - distance / maxDistance;

            // Penalty for visiting too many fields
            decimal penalty = Math.Min(0.2m, 0.02m * traversedTiles);

            // Penalty for wall collision
            decimal collisionPenalty = wallCollision ? 0.2m : 0.0m;

            // Bonus for reaching the end
            decimal endBonus = reachedEnd ? 0.5m : 0.0m;

            // Calculate the final fitness
            fitness = Math.Max(0, distanceScore - penalty - collisionPenalty + endBonus);

            // If the specimen reached the end set flag
            if (reachedEnd && options.logToFile)
            {
                LogSpecimenDetails(actualPosition, endPosition, options);
            }
        }

        // Method to log details to a file
        private void LogSpecimenDetails(Position actualPosition, Position endPosition, Options options)
        {
            using (StreamWriter sw = new StreamWriter(options.logFilePath, append: true))
            {
                DateTime now = DateTime.Now;

                sw.WriteLine();
                sw.WriteLine(now);
                sw.WriteLine("Specimen reached the end!");
                sw.WriteLine("Position: " + actualPosition.x + " " + actualPosition.y);
                sw.WriteLine("End Position: " + endPosition.x + " " + endPosition.y);
                sw.Write("Genetic code: ");

                for (int i = 0; i < tabSizeC; i++)
                {
                    sw.Write(genTabC[i]);
                }

                sw.WriteLine();
            }
        }
    }
}
