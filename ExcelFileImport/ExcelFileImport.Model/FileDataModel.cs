namespace ExcelFileImport.Model
{
    public class FileDataModel
    {
        public string? ClientCode { get; set; }
        public int? Quantity { get; set; }
        public string? Date { get; set; }
        public decimal? Revenue { get; set; }
        public string? ProductCategory { get; set; }
        public string? ProductSku { get; set; }
    }
}