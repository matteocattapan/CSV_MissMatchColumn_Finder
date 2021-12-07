using System;

namespace CSV_MissMatchColumn_Finder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello! We start matching soon..");

            // Read args 
            // [0] _> csv path 1
            // [1] _> column match 1
            // [2] _> csv path 2
            // [3] _> column match 2

            var csv_path_1 = args[0];
            int.TryParse(args[1], out var column_match_1);
            var csv_path_2 = args[2];
            int.TryParse(args[3], out var column_match_2);

            Console.WriteLine("### CONFIGURATION ###");
            Console.WriteLine($"Path_1: {csv_path_1}");
            Console.WriteLine($"Column_1: {column_match_1}");
            Console.WriteLine($"Path_2: {csv_path_2}");
            Console.WriteLine($"Column_2: {column_match_2}");
            Console.WriteLine("######");
        }
    }
}
