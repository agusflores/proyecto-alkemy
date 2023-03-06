using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs.CategoriesDTO
{
    public class CategoryUpdateDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(255, ErrorMessage = "Name must have less than or equal to 255 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(255, ErrorMessage = "Description must have less than or equal to 255 characters.")]
        public string Description { get; set; }

        //[MaxLength(255)]
        public IFormFile Image { get; set; }
    }
}
