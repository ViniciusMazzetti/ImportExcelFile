using ExcelFileImport.Model;
using ExcelFileImport.Model.DatabaseQueries;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace ExcelFileImport.Application.GetFileDetails
{
    public class GetFileDetails
    {
        private readonly IConfiguration _configuration;

        public GetFileDetails(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetData()
        {
            try
            {
                string sqlQuery = "SELECT [Id], [FileAlias] FROM [FILEDETAILS]";

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
    }
}
