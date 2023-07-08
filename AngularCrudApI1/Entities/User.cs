using System.ComponentModel.DataAnnotations;

namespace AngularCrudApI1.Entities
{

    /*This Entity represents the table with 3 column  in database and and we are
     comparing the authrequest credential with this user table credential 
    if match then we create token*/

    public class User
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        [Key]
        public int UserId { get; set; }

        public List<UserRefreshToken> UserRefreshTokens { get; set; }
    }
}
