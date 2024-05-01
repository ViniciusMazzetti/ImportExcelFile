namespace ExcelFileImport.Model
{
    public class ExcelFileDataModel
    {
        public required string FileData { get; set; }
        public FileDetails? FileDetails { get; set; }
        public string? FileAlias { get; set; }
    }
    public class FileDetails
    {
        public string? FileName { get; set; }
        public long FileSize { get; set; }
    }
}
