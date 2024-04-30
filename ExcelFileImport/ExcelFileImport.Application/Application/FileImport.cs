using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System.Data;
using System.Data.SqlClient;

namespace ExcelFileImport.Application.FileImport
{
    public class FileImport
    {
        private readonly IConfiguration _configuration;

        public FileImport(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task ImportExcelFile(byte[] excelFile)
        {
            try
            {
                await Task.Run(() =>
                {
                    using var memoryStream = new MemoryStream(excelFile);
                    using var package = new ExcelPackage(memoryStream);

                    DataTable dataTable = GetExcelData(package);

                    InsertIntoDatabase(dataTable);
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }

        private void InsertIntoDatabase(DataTable dataTable)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("ConnString"));
            connection.Open();

            using var bulkCopy = new SqlBulkCopy(connection);
            bulkCopy.DestinationTableName = "FileData";

            int columnCount = dataTable.Columns.Count;

            for (int i = 0; i < columnCount; i++)
            {
                bulkCopy.ColumnMappings.Add(i, i + 1);
            }

            bulkCopy.WriteToServer(dataTable);
        }

        private static DataTable GetExcelData(ExcelPackage package)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var worksheet = package.Workbook.Worksheets[0];

            var dataTable = new DataTable();
            foreach (var firstRowCell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
            {
                dataTable.Columns.Add(firstRowCell.Text);
            }

            for (var rowNumber = 2; rowNumber <= worksheet.Dimension.End.Row; rowNumber++)
            {
                var row = worksheet.Cells[rowNumber, 1, rowNumber, worksheet.Dimension.End.Column];
                var newRow = dataTable.NewRow();
                foreach (var cell in row)
                {
                    newRow[cell.Start.Column - 1] = cell.Text;
                }
                dataTable.Rows.Add(newRow);
            }

            return dataTable;
        }
    }
}