using Crud.Interface;
using Crud.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IEnumerable<Driver>>> GetAll(int page=1)
        {
            var result=await _driver.GetALL(page);
            return Ok(result);

        }
    }
}
