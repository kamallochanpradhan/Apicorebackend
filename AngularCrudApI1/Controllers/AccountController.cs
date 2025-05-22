using AngularCrudApI1.Model;
using AngularCrudApI1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AngularCrudApI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IjwtService _jwtservice;
        private readonly MyAngularDataContext _appDBContext;

        public AccountController(IjwtService jwtservice, MyAngularDataContext appDBContext)
        {
            this._jwtservice = jwtservice;
            _appDBContext = appDBContext;
        }

        /// <summary>
        /// Verify user and Issue access Jwt token and refresh token
        /// </summary>
        /// <param name="authRequest">It accepts auth Request User Credentials</param>
        /// <returns>Token</returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> AuthToken([FromBody] AuthRequest authRequest)
        {
            //if failng we are sending the reason why we  are failing with response with status code 
            if (!ModelState.IsValid)
                return BadRequest(new AuthResponse { IsSuccess = false, Reason = "UserName and Password must be provided." });
            //we can get the ip address HttpContext.Connection.RemoteIpAddress.ToString()
            var authResponse = await _jwtservice.GetTokenAsync(authRequest, HttpContext.Connection.RemoteIpAddress.ToString());
            if (authResponse == null)
                return Unauthorized();
            return Ok(authResponse);

        }
    /*This endpoint:Receives an object with:ExpiredToken and RefreshToken,Validates the request then Finds the token in DB (UserRefreshTokens)
     * based on:Token, RefreshToken  and IP address.Marks the old token as invalid.Generates new tokens and returns them.*/
        /// <summary>
        /// Get the refresh token request..for refresh token we need expiration and refresh token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new AuthResponse { IsSuccess = false, Reason = "Token Must be provided." });
            string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            var token = GetjwtToken(request.ExpiredToken);

            var userRefreshToken = _appDBContext.UserRefreshTokens.FirstOrDefault(x => x.IsInvalidated == false
              && x.Token == request.ExpiredToken && x.RefreshToken == request.RefreshToken
              && x.IpAddress == ipAddress);

            AuthResponse response = ValidateDetails(token, userRefreshToken);
            if(!response.IsSuccess)
                return BadRequest(response);
            /*When user logs out:Frontend clears tokens and Backend sets IsInvalidated = true for the token in DB.*/
            /*In your DB, the IsInvalidated column is a boolean:
             * false (or 0) → Token is still valid
             * true (or 1) → Token has been revoked (e.g., on logout or suspicious activity).
             * If it's true, then the token has been 
             * explicitly blocked by the server — even if it's not expired yet.*/
            userRefreshToken.IsInvalidated = true;
            _appDBContext.UserRefreshTokens.Update(userRefreshToken);
            await _appDBContext.SaveChangesAsync();

            var userName = token.Claims.FirstOrDefault(x => x.Type ==JwtRegisteredClaimNames.NameId).Value;
            var authResponse = await _jwtservice.GetRefreshTokenAsync(ipAddress,userRefreshToken.UserId,userName);
            return Ok(authResponse);
        }

        private AuthResponse ValidateDetails(JwtSecurityToken token, Entities.UserRefreshToken? userRefreshToken)
        {
            if (userRefreshToken == null)
                return new AuthResponse { IsSuccess = false, Reason = "Invalid Token Details" };
            if (token.ValidTo > DateTime.UtcNow)
                return new AuthResponse { IsSuccess = false, Reason = "Token not expired. " };
            //that means token expires
            if (!userRefreshToken.IsActive)
                return new AuthResponse { IsSuccess = false, Reason ="Refresh Token Expired"};
            return new AuthResponse { IsSuccess = true };
        }

        private JwtSecurityToken GetjwtToken(string expiredToken)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.ReadJwtToken(expiredToken);
        }
    }
}

/*✅ Flow when access token expires:
Access token (valid for 2 mins) expires.

Angular app sends a new request → gets a 401 Unauthorized.

Angular's AuthInterceptor kicks in and sends the refresh token to get a new access token.

If refresh token is still valid (within 20 minutes), backend sends a new access token + new refresh token.

Angular retries the original request with new access token → ✅ works.

*/