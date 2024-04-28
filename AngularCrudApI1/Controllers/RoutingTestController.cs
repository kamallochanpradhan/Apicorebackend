using AngularCrudApI1.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularCrudApI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutingTestController : ControllerBase
    {
        [HttpPost]
        [Route("bookstore/{bookid}/{isloggedin?}")]
        public async Task<IActionResult> Post([FromRoute] int? bookid, [FromRoute] bool? isloggedin)
        {
            /*1st prefere Route parameter if not fetch the value from querystring*/
            return await Task.FromResult<IActionResult>(Ok());
        }

        [HttpPost]
        [Route("bookstore1/{bookid}/{isloggedin?}")]
        public async Task<IActionResult> Post1([FromQuery] int? bookid, [FromQuery] bool? isloggedin)
        {
            return await Task.FromResult<IActionResult>(Ok());
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Post2([FromHeader(Name = "Host")] string hostnmae)
        {
            var objHostname = hostnmae;
            //read the data from  request body/request header (as JSON or XML or custom)
            return await Task.FromResult<IActionResult>(Ok());
        }

        [HttpPost]
        [Route("register2")]
        public async Task<IActionResult> Post2([FromBody] Product prdct)
        {
            //read the data from  request body (as JSON or XML or custom)
            return await Task.FromResult<IActionResult>(Ok());
        }
    }
}
