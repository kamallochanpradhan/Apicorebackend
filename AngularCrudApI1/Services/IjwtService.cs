using AngularCrudApI1.Model;

namespace AngularCrudApI1.Services
{
    /// <summary>
    /// Represents a Interface that declare  a specific functionality.
    /// </summary>
    public interface IjwtService
    {

        /// <summary>
        /// Get token 
        /// </summary>
        /// <param name="param1">authRequest is the user credential</param>
        Task<AuthResponse> GetTokenAsync(AuthRequest authRequest, string ipAddress);

        Task<AuthResponse> GetRefreshTokenAsync(string ipAddress,int userId,string userName);

        Task<bool> IsTokenValid(string accessToken, string ipAddress);

    }
}
