using ExcelFileImport.Model;
using ExcelFileImport.Model.DatabaseQueries;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace ExcelFileImport.Application.GetFileData
{
    public class GetFileData
    {
        private readonly IConfiguration _configuration;

        public GetFileData(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetData(FileDataModel filters)
        {
            try
            {
                string sqlQuery = SearchDataQueries.SelectData;

                sqlQuery = ApplyFilters(filters, sqlQuery);

                return ExecuteQuery(sqlQuery);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }

        private string ExecuteQuery(string sqlQuery)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("ConnString"));
            using var command = new SqlCommand(sqlQuery, connection);

            connection.Open();

            var dataTable = new DataTable();
            using (var reader = command.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            var result = JsonConvert.SerializeObject(dataTable);

            return result;
        }

        private static string ApplyFilters(FileDataModel filters, string sqlQuery)
        {
            if (ValidateFilters(filters.ClientCode))
            {
                sqlQuery += $" AND ClientCode = {filters.ClientCode}";
            }
            if (ValidateFilters(filters.Quantity))
            {
                sqlQuery += $" AND FD.Quantity = {filters.Quantity}";
            }
            if (ValidateFilters(filters.InitialDate) && ValidateFilters(filters.EndDate))
            {
                sqlQuery += $" AND FD.Date BETWEEN '{filters.InitialDate}' AND '{filters.EndDate}'";
            }
            if (ValidateFilters(filters.Revenue))
            {
                sqlQuery += $" AND FD.Revenue = {filters.Revenue}";
            }
            if (ValidateFilters(filters.ProductCategory))
            {
                sqlQuery += $" AND FD.ProductCategory LIKE '%{filters.ProductCategory}%'";
            }
            if (ValidateFilters(filters.ProductSku))
            {
                sqlQuery += $" AND FD.ProductSku LIKE '%{filters.ProductSku}%'";
            }
            if (ValidateFilters(filters.FileAlias))
            {
                sqlQuery += $" AND FD.FileDetailsId = {filters.FileAlias}";
            }

            return sqlQuery;
        }

        private static bool ValidateFilters(string? filter) => !string.IsNullOrEmpty(filter);
    }
}
