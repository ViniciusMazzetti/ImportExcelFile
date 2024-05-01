namespace ExcelFileImport.Model
{
    public class FileDataModel
    {
        public string? ClientCode { get; set; }
        public string? Quantity { get; set; }
        public string? InitialDate { get; set; }
        public string? EndDate { get; set; }
        public string? Revenue { get; set; }
        public string? ProductCategory { get; set; }
        public string? ProductSku { get; set; }
        public string? FileAlias { get; set; }
    }
}