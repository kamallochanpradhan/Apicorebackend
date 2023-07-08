using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularCrudApI1.Entities
{
    [Table("UserRefreshToken")]
    public class UserRefreshToken
    {

        [Key]
        public int UserRefreshTokenId { get; set; }

        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Preserve the Expiration Date
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// NotMapped we used when we dont want to move this to database while migration
        /// its a kind of readonly property
        /// </summary>
        [NotMapped]
        public bool IsActive
        {
            get
            {
                //less than current date time means that  not expired
                return ExpirationDate >DateTime.UtcNow;
            }
        }
        /*Preserve the Ip address..easier to validate authenticity of the users*/
        public string IpAddress { get; set; }

        /*Token is generated but not expired assume that there is some situation where 
         we need to deactive the token then we can use this flag*/
        public bool IsInvalidated { get; set; }

        /*As this class need foreign key relationship with the User table */
        public int UserId { get; set; }


        [ForeignKey("UserId")]
        public virtual User User { 
            get; set;
        }
    }
}
