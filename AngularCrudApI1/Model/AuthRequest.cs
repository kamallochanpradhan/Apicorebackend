using System.ComponentModel.DataAnnotations;

namespace AngularCrudApI1.Model
{
    /// <summary>
    /// user credential property which will pass on logi
    /// </summary>
    public class AuthRequest
    {   
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
