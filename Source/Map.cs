namespace Genetic_Algorithm.Source
{
    public class Map
    {
        // 2D array to store the map
        public char[][] map { get; set; } = Array.Empty<char[]>();
        public int mapSizeX { get; set; }
        public int mapSizeY { get; set; }

        // Position struct to store map boundaries
        public Position mapBoundries;

        // Constructor to initialize the map with path to the map file
        public Map(string path)
        {
            try
            {
                // Read the map file
                using (StreamReader mapFile = new StreamReader(path))
                {
                    string data = mapFile.ReadToEnd();
                    mapFile.Close();

                    // Remove new line characters from the data
                    data = data.Replace("\r\n", "\n");

                    // Split the data into lines
                    string[] lines = data.Split('\n');

                    // Calculate the size of the map
                    int sizeX = lines.Length;
                    int sizeY = lines[0].Length;

                    // Initialize the map array with calculated size
                    map = new char[sizeX][];
                    for (int i = 0; i < sizeX; i++)
                    {
                        map[i] = new char[sizeY];
                    }

                    // Fill the map array with data from the file
                    for (int i = 0; i < sizeX; i++)
                    {
                        for (int j = 0; j < sizeY; j++)
                        {
                            map[i][j] = lines[i][j];
                        }
                    }

                    // Set the map boundaries
                    mapBoundries.x = sizeX;
                    mapBoundries.y = sizeY;

                    mapSizeX = sizeX;
                    mapSizeY = sizeY;
                }
            }
            catch (FileNotFoundException ex)
            {
                // Handle the case when the file is not found
                Console.WriteLine($"Error! File not found! Details: {ex.Message}");
            }
        }

        public Map(char[][] tab)
        {
            this.map = tab;
            this.mapSizeX = tab.Length;
            this.mapSizeY = tab[0].Length;
            this.mapBoundries.x = tab.Length;
            this.mapBoundries.y = tab[0].Length;
        }
    }
}
