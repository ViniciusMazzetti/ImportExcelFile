using ExcelFileImport.Application.GetFileData;
using ExcelFileImport.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;

namespace ExcelFileImport.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilterFileDataController : ControllerBase
    {
        private readonly GetFileData excelSearcher;

        public FilterFileDataController()
        {
            string connectionString = "Server=(local)\\sqlserver; Database=ExcelFileImport; User Id=sa; Password=rodel123; Persist Security Info=True; MultipleActiveResultSets=true; Encrypt=false";
            excelSearcher = new GetFileData(connectionString);
        }

        [HttpPost("Search")]
        public IActionResult SearchExcelData([FromBody] FileDataModel filters)
        {
            try
            {
                DataTable searchResults = excelSearcher.GetData(filters);
                return Ok(searchResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }
}