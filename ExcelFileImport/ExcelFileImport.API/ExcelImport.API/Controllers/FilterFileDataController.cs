using ExcelFileImport.Application.GetFileData;
using ExcelFileImport.Application.GetFileDetails;
using ExcelFileImport.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;

namespace ExcelFileImport.API.Controllers
{
    [ApiController]
    [Route($"api/[controller]")]
    public class FilterFileDataController : ControllerBase
    {
        private readonly GetFileData excelSearcher;
        private readonly GetFileDetails getFiles;
        private readonly IConfiguration _configuration;

        public FilterFileDataController(IConfiguration configuration)
        {
            _configuration = configuration;
            excelSearcher = new GetFileData(_configuration);
            getFiles = new GetFileDetails(_configuration);
        }

        [HttpPost(Name = "Filter File Data")]
        public IActionResult FilterFileData([FromBody] FileDataModel filters)
        {
            try
            {
                var searchResults = excelSearcher.GetData(filters);
                return Ok(searchResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("GetFiles", Name = "Get Files")]
        public IActionResult GetFiles()
        {
            try
            {
                var searchResults = getFiles.GetData();
                return Ok(searchResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }
}