using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OngProject.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class User : Entity
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

        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public User() { }

        public User(string firstname, string lastname, string email, string password, string photo, Role role)
        {
            FirstName = firstname;
            LastName = lastname;
            Email = email;
            Password = password;
            Photo = photo;
            Role = role; 
        }
    }
}