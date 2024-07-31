using Crud.Interface;
using Crud.Model;
using Crud.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _productService;
        public ProductController(IProduct productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> AddProduct(ProductDto product)
        {
            if(product == null)
            {
                return BadRequest();
            }
            var result =await _productService.AddProduct(product);
            
            return Ok(result);
        }
    }
}
