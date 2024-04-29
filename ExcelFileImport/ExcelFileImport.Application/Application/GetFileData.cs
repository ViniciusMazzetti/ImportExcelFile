using ExcelFileImport.Model;
using System.Data;
using System.Data.SqlClient;

namespace ExcelFileImport.Application.GetFileData
{
    public class GetFileData
    {
        private readonly string connectionString;

        public GetFileData(string connectionString)
        {
            this.connectionString = connectionString;
        }
        //Id ClientCode  ProductCategory ProductSku  Date Quantity    Revenue
        public DataTable GetData(FileDataModel filters)
        {
            // Construct the SQL query based on the provided filters
            string sqlQuery = "SELECT * FROM [FileData] WHERE 1 = 1";

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

            // Execute the SQL query and retrieve the results
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    connection.Open();

                    var dataTable = new DataTable();
                    using (var reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                    return dataTable;
                }
            }
        }
    }
}
