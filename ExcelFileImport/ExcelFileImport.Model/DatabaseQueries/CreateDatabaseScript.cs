namespace ExcelFileImport.Model.DatabaseStructure
{
    public class CreateDatabaseScript
    {
        public const string CreateDB = @"CREATE DATABASE [@databaseName];";

        public const string CreateFileDetailsTable = @"
            CREATE TABLE [dbo].[FileDetails](
                [Id] [int] IDENTITY(1,1) NOT NULL,
                [FileName] [nvarchar](max) NULL,
                [CreatedDate] [datetime] NULL
            )";

        public const string CreateFileDataTable = @"
            CREATE TABLE [dbo].[FileData](
                [Id] [int] IDENTITY(1,1) NOT NULL,
                [ClientCode] [int] NULL,
                [ProductCategory] [nvarchar](max) NULL,
                [ProductSku] [nvarchar](max) NULL,  
                [Date] [datetime] NULL,
                [Quantity] [int] NULL,
                [Revenue] [decimal](18, 2) NULL
            )";
    }
}
