using System;

namespace CSV_MissMatchColumn_Finder
{
    public class CsvData
    {
        public string CSVFileName { get; set; }
        public int Row { get; set; }
        public string Value { get; set; }
        public string ColumnReportResult { get; set; }

        public bool TryParse(string row_data, int row_count, int column, int report_column_output, string csv_name)
        {
            try
            {
                var split = row_data.Split(',');
                this.CSVFileName = csv_name;
                this.Row = row_count;
                Guid.TryParse(split[column], out var guid);
                if (guid == Guid.Empty)
                    return false;
                this.Value = guid.ToString();

                if (report_column_output <= split.Length)
                {
                    this.ColumnReportResult = split[report_column_output];
                }

                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}