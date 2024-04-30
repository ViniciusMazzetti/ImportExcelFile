using ExcelFileImport.Model;
using ExcelFileImport.Model.DatabaseQueries;
using Microsoft.Extensions.Configuration;
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
        public DataTable GetData(FileDataModel filters)
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

        private DataTable ExecuteQuery(string sqlQuery)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("ConnString"));
            using var command = new SqlCommand(sqlQuery, connection);

            connection.Open();

            var dataTable = new DataTable();
            using (var reader = command.ExecuteReader())
            {
                dataTable.Load(reader);
            }
            return dataTable;
        }

        private static string ApplyFilters(FileDataModel filters, string sqlQuery)
        {
            if (!string.IsNullOrEmpty(filters.ClientCode))
            {
                sqlQuery += $" AND ClientCode = {filters.ClientCode}";
            }
            if (filters.Quantity != null)
            {
                sqlQuery += $" AND Quantity = {filters.Quantity}";
            }
            if (filters.Date != null)
            {
                sqlQuery += $" AND Date = {filters.Date}";
            }
            if (filters.Revenue != null)
            {
                sqlQuery += $" AND Revenue = {filters.Revenue}";
            }
            if (!string.IsNullOrEmpty(filters.ProductCategory))
            {
                sqlQuery += $" AND ProductCategory = {filters.ProductCategory}";
            }
            if (!string.IsNullOrEmpty(filters.ProductSku))
            {
                sqlQuery += $" AND ProductSku = {filters.ProductSku}";
            }

            return sqlQuery;
        }
    }
}
