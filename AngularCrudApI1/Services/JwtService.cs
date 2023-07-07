using AngularCrudApI1.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AngularCrudApI1.Services
{
    //https://www.youtube.com/watch?v=8ni7Xg_UxBs

    //UserName: Test   -- in  db
    //Password: Password    --  in db

    public class JwtService : IjwtService
    {
        private readonly MyAngularDataContext _appDBContext;

        private readonly IConfiguration  _configuration;

        public JwtService(MyAngularDataContext appDBContext, IConfiguration configuration)
        {
            _appDBContext = appDBContext;
            _configuration = configuration;
        }

        /// <summary>
        /// Validate Username and craete token
        /// </summary>
        /// <param name="authRequest">user credential</param>
        /// <returns>token</returns>
        public async Task<string> GetTokenAsync(AuthRequest authRequest)
        {
            var user = _appDBContext.Users.FirstOrDefault(x => x.UserName.Equals(authRequest.UserName)
              && x.Password.Equals(authRequest.Password));

            if (user == null)
                return await Task.FromResult<string>(null);

            // Here we got the key and converted to Bytes
            var jwtKey = _configuration.GetValue<string>("JwtSettings:Key");
            var keyBytes=Encoding.ASCII.GetBytes(jwtKey);

            var tokenHandler = new JwtSecurityTokenHandler();

            /* we have to generate token based on certain parameter..so we have to define
             that parameter in some  object and that object is SecurityTokenDescriptor 
            */ 
            var descriptor = new SecurityTokenDescriptor()
            {
                /* And here define the parameter which needed to generate the token like
                 * Subject,Expires, SigningCredentials*/
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserName)
                }),
                Expires = DateTime.UtcNow.AddSeconds(60),

                //Algorith for encrypting the tooken
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(keyBytes),SecurityAlgorithms.HmacSha256)
            };

            /* using the token handlers CreateToken method  we generate the
             * token that gives us dot net Objects*/

            var token = tokenHandler.CreateToken(descriptor);

            //here it will serilize the dotnet object token  and return the string
            return await Task.FromResult(tokenHandler.WriteToken(token));

        }
    }
}
