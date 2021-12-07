using System;

namespace CSV_MissMatchColumn_Finder
{
    public class CsvData
    {
        public string CSVFileName { get; set; }
        public int Row { get; set; }
        public string Value { get; set; }

        public bool TryParse(string row_data, int row_count, int column, string csv_name)
        {
            try
            {
                this.CSVFileName = csv_name;
                this.Row = row_count;
                this.Value = row_data.Split(',')[column];
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}