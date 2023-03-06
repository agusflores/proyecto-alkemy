using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs.TestimonialDTO
{
    public class TestimonialInsertDto
    {
        [Required(ErrorMessage = "Testimonial needs a Name")]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "Testimonial needs a Content")]
        [MaxLength(65535)]
        public string Content { get; set; }
    }
}
