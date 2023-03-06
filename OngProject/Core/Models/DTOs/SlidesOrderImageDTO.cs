using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs
{
    public class SlidesOrderImageDTO
    {
        [MaxLength(255)]
        public string ImageUrl { get; set; }
        [Required]
        public int Order { get; set; }
    }
}
