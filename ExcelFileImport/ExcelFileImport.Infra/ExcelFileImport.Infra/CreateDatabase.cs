using ExcelFileImport.Model.DatabaseStructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;
namespace ExcelFileImport.Bootstrap.Configuration
{
    public static class CreateDatabase
    {
        public static void CreateDB(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("ConnString")!;

            using SqlConnection connection = new(connectionString);
            try { connection.Open(); }
            catch
            {
                try
                {
                    string databaseName = connection.Database;
                    CreateDB(connection, databaseName);

                    Thread.Sleep(5000); //Infelizmente não achei outra forma de esperar a criação do banco

                    connection.ConnectionString = connectionString;
                    try
                    {
                        connection.Open();
                        ExecuteSqlCommand(connection, CreateDatabaseScript.CreateFileDetailsTable);
                        ExecuteSqlCommand(connection, CreateDatabaseScript.CreateFileDataTable);
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error creating tables: {ex.Message}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating database: {ex.Message}");
                }
            }
        }

        private static void CreateDB(SqlConnection connection, string databaseName)
        {
            connection.ConnectionString = connection.ConnectionString.Replace(databaseName, "master");

            string createDatabaseScript = CreateDatabaseScript.CreateDB.Replace("@databaseName", databaseName);

            connection.Open();
            ExecuteSqlCommand(connection, createDatabaseScript);
            connection.Close();
        }
        private static void ExecuteSqlCommand(SqlConnection connection, string script)
        {
            using SqlCommand command = new(script, connection);
            command.ExecuteNonQuery();
        }
    }
}
