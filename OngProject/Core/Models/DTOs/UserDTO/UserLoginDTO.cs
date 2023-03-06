using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs.UserDTO
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
