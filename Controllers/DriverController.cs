using Crud.Interface;
using Crud.Model;
using Crud.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        public readonly IDriver _driver;
        public DriverController(IDriver driver) {
            _driver = driver;
        
        }
        [HttpPost]
        public async Task<IActionResult>FileUpload(IFormFile file)
        {
            var result=_driver.ImportExcel(file);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Driver>>> GetAll(int page = 1)
        {
            var result = await _driver.GetALL(page);
            return Ok(result);
        }
        [HttpGet("download")]
        public async Task<IActionResult> DownloadExcel()
        {
            try
            {
                var content = await _driver.GenerateExcelAsync();
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = "Drivers.xlsx";

                return File(content, contentType, fileName);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
