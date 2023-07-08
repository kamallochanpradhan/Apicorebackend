using AngularCrudApI1.Entities;
using AngularCrudApI1.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AngularCrudApI1.Services
{
    //https://www.youtube.com/watch?v=8ni7Xg_UxBs

    //UserName: Test   -- in  db
    //Password: Password    --  in db

    public class JwtService : IjwtService
    {
        private readonly MyAngularDataContext _appDBContext;

        private readonly IConfiguration _configuration;

        public JwtService(MyAngularDataContext appDBContext, IConfiguration configuration)
        {
            _appDBContext = appDBContext;
            _configuration = configuration;
        }

        public async Task<AuthResponse> GetRefreshTokenAsync(string ipAddress, int userId, string userName)
        {
            var refreshToken = GenerateRefreshToken();
            var accessToken=GenerateToken(userName);
            return await SaveTokenDetails(ipAddress,userId,accessToken,refreshToken);
        }

        /// <summary>
        /// Validate Username and craete token
        /// </summary>
        /// <param name="authRequest">user credential</param>
        /// <returns>token</returns>
        public async Task<AuthResponse> GetTokenAsync(AuthRequest authRequest, string iPaddress)
        {
            var user = _appDBContext.Users.FirstOrDefault(x => x.UserName.Equals(authRequest.UserName)
              && x.Password.Equals(authRequest.Password));

            if (user == null)
                return await Task.FromResult<AuthResponse>(null);
            string tokenString = GenerateToken(user.UserName);
            string refreshToken = GenerateRefreshToken();
            return await SaveTokenDetails(iPaddress, user.UserId, tokenString, refreshToken);

        }

        /// <summary>
        /// Creating Token Object and saving to Database and returning response
        /// </summary>
        /// <param name="iPaddress"></param>
        /// <param name="userId"></param>
        /// <param name="tokenString"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        private async Task<AuthResponse> SaveTokenDetails(string iPaddress, int  userId, string tokenString, string refreshToken)
        {
            var userRefreshToken = new UserRefreshToken
            {
                CreatedDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddMinutes(5),
                //Here we can get the ip address from controller
                IpAddress = iPaddress,
                IsInvalidated = false,
                RefreshToken = refreshToken,
                Token = tokenString,
                UserId = userId
            };
            await _appDBContext.UserRefreshTokens.AddAsync(userRefreshToken);
            await _appDBContext.SaveChangesAsync();

            return new AuthResponse { Token = tokenString, RefreshToken = refreshToken,IsSuccess=true };
        }


        /*refresh token can be any guid or any encrypted random no that converted that to a
         * base 64 string */
        /// <summary>
        /// Generate Refresh token using RNGCryptoServiceProvider
        /// </summary>
        /// <returns></returns>
        private string GenerateRefreshToken()
        {
            /*Generating 64 byte encrypted random no,
            RNGCryptoServiceProvider used for creating encrypted random no,*/
            var byteArray = new byte[64];

            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                // this method will fill empty bytearray to random no
                cryptoProvider.GetBytes(byteArray);
                return Convert.ToBase64String(byteArray);
            }

        }

        /// <summary>
        /// Nothing for refresh token..all is same just I did some refactorization
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        private string GenerateToken(string UserName)
        {
            // Here we got the key and converted to Bytes
            var jwtKey = _configuration.GetValue<string>("JwtSettings:Key");
            var keyBytes = Encoding.ASCII.GetBytes(jwtKey);

            /*JwtSecurityTokenHandler is a class provided by the System.IdentityModel.Tokens.Jwt namespace in 
             * .NET. It is used for handling JSON Web Tokens (JWT) in various operations such as token validation, 
             * token creation, and token parsing.*/

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
                    new Claim(ClaimTypes.NameIdentifier, UserName)
                }),

                //Here we give the Expiration time
                Expires = DateTime.UtcNow.AddSeconds(90),

                //Algorith for encrypting the tooken
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256)
            };

            /* using the token handlers CreateToken method  we generate the
             * token that gives us dot net Objects ..like Token Parsing*/

            var token = tokenHandler.CreateToken(descriptor);

            //here it will serilize the dotnet object token  and return the string
            string tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public async  Task<bool> IsTokenValid(string accessToken, string ipAddress)
        {
            var isValid=_appDBContext.UserRefreshTokens.FirstOrDefault(x=>x.Token==accessToken
            && x.IpAddress==ipAddress) != null;
            return await Task.FromResult(isValid);
        }
    }
}
