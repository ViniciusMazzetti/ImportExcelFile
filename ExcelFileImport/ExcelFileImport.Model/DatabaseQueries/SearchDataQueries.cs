namespace ExcelFileImport.Model.DatabaseQueries
{
    public class SearchDataQueries
    {
        public const string SelectData = @"set dateformat ymd; 
                                            SELECT FD.*, F.FileAlias
                                            FROM [FileData] FD 
                                            INNER JOIN [FileDetails] F ON FD.FileDetailsId = F.Id      
                                            WHERE 1 = 1";
    }
}
