using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs.CategoriesDTO
{
    public class CreateCategoriesDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }
}
