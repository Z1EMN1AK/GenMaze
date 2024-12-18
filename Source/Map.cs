namespace Genetic_Algorithm.Source
{
    public class Map
    {
        // 2D array to store the map
        public char[][] map { get; set; }

        // Position struct to store map boundaries
        public Position mapBoundries;

        // Constructor to initialize the map with given size and path to the map file
        public Map(int sizeX, int sizeY, string path)
        {
            // Initialize the map array with given size
            map = new char[sizeX][];
            for (int i = 0; i < sizeX; i++)
            {
                map[i] = new char[sizeY];
            }

            try
            {
                // Read the map file
                using (StreamReader mapFile = new StreamReader(path))
                {
                    string data = mapFile.ReadToEnd();
                    mapFile.Close();

                    // Remove new line characters from the data
                    data = data.Replace("\r\n", "").Replace("\n", "");

                    int pos = 0;
                    // Fill the map array with data from the file
                    for (int i = 0; i < sizeX; i++)
                    {
                        for (int j = 0; j < sizeY - 1; j++)
                        {
                            map[i][j] = data[pos];
                            pos++;
                        }
                    }

                    // Set the map boundaries
                    mapBoundries.x = sizeX;
                    mapBoundries.y = sizeY;
                }
            }
            catch (FileNotFoundException ex)
            {
                // Handle the case when the file is not found
                Console.WriteLine($"Error! File not found! Details: {ex.Message}");
            }
        }
    }
}
