using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs.UserDTO
{
    public class UserRegisterDTO
    {
        [Required, MaxLength(255)]
        public string FirstName { get; set; }

        [Required, MaxLength(255)]
        public string LastName { get; set; }


        [Required, MaxLength(320)]
        public string Email { get; set; }

        [Required, MaxLength(20)]
        public string Password { get; set; }
    }
}
