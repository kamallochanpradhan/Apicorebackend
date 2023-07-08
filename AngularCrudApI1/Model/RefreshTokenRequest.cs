using System.ComponentModel.DataAnnotations;

namespace AngularCrudApI1.Model
{
    public class RefreshTokenRequest
    {
        [Required]
        public string ExpiredToken { get; set; }

        [Required]
        public string RefreshToken { get; set; }

    }
}
