using AngularCrudApI1.CustomActionFilter;
using AngularCrudApI1.Model;
using AngularCrudApI1.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AngularCrudApI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _product;
        private readonly ILogger<ProductController> logger;

        public ProductController(IProductRepository product, ILogger<ProductController> logger)
        {
            _product = product;
            this.logger = logger;
        }

        [HttpGet]
        [Route("GetProducts")]
        public async Task<IActionResult> Get()
        {   
            logger.LogInformation("GetProducts Action method Invoked");
            var prdData = await _product.GetProduct();
            return Ok(prdData);
        }


        // POST api/<StudentController>
        [HttpPost]
        [ValidateModelAttribute]
        [Route("CreateProduct")]
        public async Task<IActionResult> Post(Product prdRequest)
        {
            var result = await _product.CreateProduct(prdRequest);
            if (result.Prid == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something Went Wrong");
            }

            return Ok("Added Successfully");
        }

    }
}
