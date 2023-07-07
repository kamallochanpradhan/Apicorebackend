using AngularCrudApI1.Model;
using AngularCrudApI1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularCrudApI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IjwtService _jwtservice;

        public AccountController(IjwtService jwtservice)
        {
            this._jwtservice = jwtservice;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AuthToken([FromBody] AuthRequest authRequest)
        {
            var token = await _jwtservice.GetTokenAsync(authRequest);
            if (token == null)
                return Unauthorized();
            return Ok(new AuthResponse { Token = token });

        }
    }
}
