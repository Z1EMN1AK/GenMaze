using System;


namespace GeneticAlgorithm
{
    public struct Options
    {
        public int stopAfterGeneration { get; set; }
        public bool stopIfReachedEnd { get; set; }
        public string path { get; set; }
        public bool logToFile { get; set; }
    }
}
