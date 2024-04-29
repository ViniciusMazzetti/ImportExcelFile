using ExcelFileImport.Application;
using ExcelFileImport.Application.FileImport;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;

namespace ExcelFileImport.API.Controllers
{
    [ApiController]
    [Route($"api/[controller]")]
    public class ImportFileController : Controller
    {
        [HttpPost(Name = "Import File")]
        public async Task<IActionResult> ImportFile([FromBody] FileDataModesl model)
        {
            try
            {
                byte[] fileData = Convert.FromBase64String(model.FileData);

                string connectionString = "Server=(local)\\sqlserver; Database=ExcelFileImport; User Id=sa; Password=rodel123; Persist Security Info=True; MultipleActiveResultSets=true; Encrypt=false";                

                var importer = new FileImport(connectionString);
                await importer.ImportExcelFile(fileData);

                return StatusCode((int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    $"Message error: some server error has happened. Call the system administrator. {ex.Message}");
            }
        }
        public class FileDataModesl
        {
            public required string FileData { get; set; }
        }

        [HttpGet("test", Name = "TestFile")]
        public async Task<IActionResult> TestFile()
        {
            try
            {
                return StatusCode((int)HttpStatusCode.Created, "ok");
            }
            catch (Exception ex)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    $"Message error: some server error has happened. Call the system administrator. {ex.Message}");
            }
        }
    }
}
