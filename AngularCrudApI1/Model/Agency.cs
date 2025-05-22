using System.ComponentModel.DataAnnotations;

namespace AngularCrudApI1.Model
{
    public class Agency
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string RegistrationNumber { get; set; }

        [Required]
        public DateTime EstablishedDate { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string AgencyType { get; set; }

        public string Description { get; set; }
    }
}
