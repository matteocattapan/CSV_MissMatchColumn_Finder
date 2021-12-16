using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSV_MissMatchColumn_Finder
{
    internal class Program
    {
        public delegate void UpdateConsoleEventHandler(object source, EventArgs args);

        private static List<CsvData> _csvSourceData1 = new List<CsvData>();
        private static List<CsvData> _csvSourceData2 = new List<CsvData>();
        private static readonly Dictionary<string, bool> _reportRows = new Dictionary<string, bool>();

        public event UpdateConsoleEventHandler updateEvent;

        private static void Main(string[] args)
        {
            Console.WriteLine("Hello! We start matching soon..");

            // Read args 
            // [0] _> csv path 1
            // [1] _> column match 1
            // [2] _> csv path 2
            // [3] _> column match 2

            var csvPath1 = args[0];
            int.TryParse(args[1], out var columnMatch1);
            int.TryParse(args[2], out var columnReport);
            var csvPath2 = args[3];
            int.TryParse(args[4], out var columnMatch2);

            Console.WriteLine("### CONFIGURATION ###");
            Console.WriteLine($"Path_1: {csvPath1}");
            Console.WriteLine($"Column_1: {columnMatch1}");
            Console.WriteLine($"Column_Report: {columnReport}");
            Console.WriteLine($"Path_2: {csvPath2}");
            Console.WriteLine($"Column_2: {columnMatch2}");
            Console.WriteLine("######");

            LoadCsv(csvPath1, columnMatch1, columnReport, ref _csvSourceData1);
            LoadCsv(csvPath2, columnMatch2, columnReport, ref _csvSourceData2);

            Console.WriteLine($"Source 1: {_csvSourceData1.Count}");
            Console.WriteLine($"Source 2: {_csvSourceData2.Count}");

            WorkWithSource();

            WriteInReport();
        }

        private static void WriteInReport()
        {
            var savePath = "csv_infected_rows_report";
            var saveExt = ".csv";

            var allReports = Directory.GetFiles(AppContext.BaseDirectory, $"{savePath}*{saveExt}", SearchOption.TopDirectoryOnly);
            if (allReports.Length > 0)
                savePath = $"{savePath}_{allReports.Length}";

            foreach (var row in _reportRows)
                File.AppendAllText(savePath + saveExt, $"{row.Key};{row.Value}" + Environment.NewLine, Encoding.UTF8);
        }

        private static void WorkWithSource()
        {
            var reference_source = _csvSourceData2.AsParallel().Select(o => o.Value).ToList();
            foreach (var csvData in _csvSourceData1)
                _reportRows.Add($"{csvData.Value}|{csvData.ColumnReportResult}", true);

            var count = _reportRows.Count;
            var processorMaxDegreese = Convert.ToInt32(Math.Ceiling(Environment.ProcessorCount * 0.75 * 2.0));
            _csvSourceData1
                .AsParallel()
                .WithDegreeOfParallelism(processorMaxDegreese)
                .ForAll(csvData =>
                {
                    // logic
                    var find_item = reference_source.Find(o => o == csvData.Value);
                    var find = !string.IsNullOrEmpty(find_item);
                    var key = $"{csvData.Value}|{csvData.ColumnReportResult}";
                    _reportRows[key] = find;

                    count--;
                    Console.Write($"\r[{count}]");
                });
        }

        private static void LoadCsv(string path, int column, int report_column_output, ref List<CsvData> source)
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
                if (data.TryParse(line, currentRow, column, report_column_output, Path.GetFileName(path)))
                    source.Add(data);

                currentRow++;
            }

            Console.WriteLine("Import complete.");
        }
    }
}