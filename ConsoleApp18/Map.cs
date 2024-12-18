using GeneticAlgorithm;


namespace GeneticAlgorithm
{
    public class Map
    {
        public char[][] map { get; set; }
        public Position mapBoundries;

        public Map(int sizeX, int sizeY, string path)
        {
            map = new char[sizeX][];
            for (int i = 0; i < sizeX; i++)
            {
                map[i] = new char[sizeY];
            }

            try
            {
                using (StreamReader mapFile = new StreamReader(path))
                {
                    string data = mapFile.ReadToEnd();
                    mapFile.Close();

                    data = data.Replace("\r\n", "").Replace("\n", "");

                    int pos = 0;
                    for (int i = 0; i < sizeX; i++)
                    {
                        for (int j = 0; j < sizeY - 1; j++)
                        {
                            map[i][j] = data[pos];
                            pos++;
                        }
                    }

                    mapBoundries.x = sizeX;
                    mapBoundries.y = sizeY;
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Error! File not found! Details: {ex.Message}");
            }
        }
    }
}
