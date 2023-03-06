using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs.TestimonialDTO
{
    public class TestimonialsDisplayDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
