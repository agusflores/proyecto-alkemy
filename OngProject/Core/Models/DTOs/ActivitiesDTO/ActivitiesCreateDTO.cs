using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs.ActivitiesDTO
{
    public class ActivitiesCreateDTO
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        [Required]
        [MaxLength(255)]
        public string Content { get; set; }
    }
}
