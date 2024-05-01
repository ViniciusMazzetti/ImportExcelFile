using ExcelFileImport.Model;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace ExcelFileImport.Application.FileImport
{
    public class FileImport
    {
        private readonly IConfiguration _configuration;

        public FileImport(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task ImportExcelFile(byte[] excelFile, ExcelFileDataModel fileDetails)
        {
            try
            {
                await Task.Run(() =>
                {
                    using var memoryStream = new MemoryStream(excelFile);
                    using var package = new ExcelPackage(memoryStream);
                    using var connection = new SqlConnection(_configuration.GetConnectionString("ConnString"));
                    connection.Open();

                    DataTable dataTable = GetExcelData(package);

                    var fileDateilsId = InsertFileDetails(fileDetails.FileDetails.FileName, fileDetails.FileDetails.FileSize, fileDetails.FileAlias, connection);
                    InsertIntoDatabase(dataTable, connection, fileDateilsId);
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }
        public static int InsertFileDetails(string fileName, long fileSize, string fileAlias, SqlConnection connection)
        {
            int fileId = 0;

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    INSERT INTO [dbo].[FileDetails] ([FileName], [FileSize], [FileAlias], [CreatedDate])
                    VALUES (@FileName, @FileSize, @FileAlias, GETDATE());
                    SELECT SCOPE_IDENTITY();";

                command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = fileName;
                command.Parameters.Add("@FileSize", SqlDbType.BigInt).Value = fileSize;
                command.Parameters.Add("@FileAlias", SqlDbType.NVarChar).Value = fileAlias;

                fileId = Convert.ToInt32(command.ExecuteScalar());
            }

            return fileId;
        }

        private static void InsertIntoDatabase(DataTable dataTable, SqlConnection connection, int fileDetailsId)
        {
            using var bulkCopy = new SqlBulkCopy(connection);
            bulkCopy.DestinationTableName = "FileData";

            int columnCount = dataTable.Columns.Count;

            for (int i = 0; i < columnCount; i++)
            {
                bulkCopy.ColumnMappings.Add(i, i + 1);
            }

            bulkCopy.WriteToServer(dataTable);

            using var command = connection.CreateCommand();

            command.CommandText = @"
                    UPDATE [dbo].[FileData]
                    SET [FileDetailsId] = @FileDetailsId
                    WHERE [FileDetailsId] IS NULL";

            command.Parameters.Add("@FileDetailsId", SqlDbType.Int).Value = fileDetailsId;

            command.ExecuteNonQuery();
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