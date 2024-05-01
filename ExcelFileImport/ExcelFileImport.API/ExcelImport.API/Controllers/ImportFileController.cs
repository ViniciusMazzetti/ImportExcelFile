using ExcelFileImport.Application.FileImport;
using ExcelFileImport.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExcelFileImport.API.Controllers
{
    [ApiController]
    [Route($"api/[controller]")]
    public class ImportFileController : Controller
    {
        private readonly IConfiguration _configuration;
        public ImportFileController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost(Name = "Import File")]
        public async Task<IActionResult> ImportFile([FromBody] ExcelFileDataModel model)
        {
            try
            {
                byte[] fileData = Convert.FromBase64String(model.FileData);

                var importer = new FileImport(_configuration);

                await importer.ImportExcelFile(fileData, model);

                return StatusCode((int)HttpStatusCode.Created);
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
