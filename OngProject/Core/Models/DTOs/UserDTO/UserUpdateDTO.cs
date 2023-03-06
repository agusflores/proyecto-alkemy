using Microsoft.AspNetCore.Http;
using OngProject.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OngProject.Core.Models.DTOs.UserDTO
{
    public class UserUpdateDTO
    {
        [Required, MaxLength(255)]
        [Display(Name = "First_Name")]
        public string FirstName { get; set; }

        [Required, MaxLength(255)]
        [Display(Name = "Last_Name")]
        public string LastName { get; set; }

        [Required, MaxLength(320)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required, MaxLength(255)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [MaxLength(255)]
        [Display(Name = "Photo")]
        public string Photo { get; set; }
    }
}
