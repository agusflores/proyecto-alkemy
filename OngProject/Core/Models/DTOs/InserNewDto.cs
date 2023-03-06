using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs
{
    public class InserNewDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [MaxLength(65535)]
        public string Content { get; set; }

        [Required(ErrorMessage = "Image is required")]
        
        public IFormFile Image { get; set; }
        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }
  
    }
}
