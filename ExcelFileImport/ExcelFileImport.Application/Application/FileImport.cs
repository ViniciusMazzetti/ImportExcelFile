using OfficeOpenXml;
using System.Data;
using System.Data.SqlClient;

namespace ExcelFileImport.Application.FileImport
{
    public class FileImport
    {
        private readonly string connectionString;

        public FileImport(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task ImportExcelFile(byte[] excelFile)
        {
            await Task.Run(() =>
            {
                using (var memoryStream = new MemoryStream(excelFile))
                {
                    using (var package = new ExcelPackage(memoryStream))
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        var worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first worksheet

                        // Create DataTable to hold Excel data
                        var dataTable = new DataTable();
                        foreach (var firstRowCell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
                        {
                            dataTable.Columns.Add(firstRowCell.Text);
                        }

                        // Populate DataTable with Excel data
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

                        // Bulk insert into SQL Server
                        using (var connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            using (var bulkCopy = new SqlBulkCopy(connection))
                            {
                                bulkCopy.DestinationTableName = "FileData"; // Set your destination table name

                                // Get the number of columns in the DataTable (assuming it matches the number of columns in SQL Server table)
                                int columnCount = dataTable.Columns.Count;

                                // Define column mappings using ordinal positions
                                for (int i = 0; i < columnCount; i++)
                                {
                                    bulkCopy.ColumnMappings.Add(i, i+1);
                                }

                                bulkCopy.WriteToServer(dataTable);
                            }
                        }
                    }
                }
            });
        }
    }
}