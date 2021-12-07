using System;
using System.Collections.Generic;
using System.IO;

namespace CSV_MissMatchColumn_Finder
{
    class Program
    {

        private static List<CsvData> _csvSourceData1 = new List<CsvData>();
        private static List<CsvData> _csvSourceData2 = new List<CsvData>();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello! We start matching soon..");

            // Read args 
            // [0] _> csv path 1
            // [1] _> column match 1
            // [2] _> csv path 2
            // [3] _> column match 2

            var csvPath1 = args[0];
            int.TryParse(args[1], out var columnMatch1);
            var csvPath2 = args[2];
            int.TryParse(args[3], out var columnMatch2);

            Console.WriteLine("### CONFIGURATION ###");
            Console.WriteLine($"Path_1: {csvPath1}");
            Console.WriteLine($"Column_1: {columnMatch1}");
            Console.WriteLine($"Path_2: {csvPath2}");
            Console.WriteLine($"Column_2: {columnMatch2}");
            Console.WriteLine("######");

            LoadCsv(csvPath1, columnMatch1, ref _csvSourceData1);
            LoadCsv(csvPath2, columnMatch2, ref _csvSourceData2);

            Console.WriteLine($"Source 1: {_csvSourceData1.Count}");
            Console.WriteLine($"Source 2: {_csvSourceData2.Count}");
        }

        private static void LoadCsv(string path, int column, ref List<CsvData> source)
        {
            using var reader = new StreamReader(File.OpenRead(path));
            var currentRow = 1;

            Console.WriteLine($"Start import.. {Path.GetFileName(path)}");
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    currentRow++;
                    continue;
                }

                if (currentRow == 1)
                {
                    //Jump CSV Header
                    currentRow++;
                    continue;
                }

                var data = new CsvData();
                if (data.TryParse(line, currentRow, column, Path.GetFileName(path)))
                {
                    source.Add(data);
                }

                currentRow++;
            }

            Console.WriteLine($"Import complete.");
        }
    }
}
