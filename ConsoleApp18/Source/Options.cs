using System;


namespace Genetic_Algorithm.Source
{
    public struct Options
    {
        public int stopAfterGeneration { get; set; }
        public bool stopIfReachedEnd { get; set; }
        public string? path { get; set; }
        public bool logToFile { get; set; }
        public string? logFilePath { get; set; }

        public bool generateMap { get; set; }
    }
}
