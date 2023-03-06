using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs
{
    public class ContactDTO
    {
        [Required]
        public string Name { get; set; }
        public int Phone { get; set; }
        [Required]
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
