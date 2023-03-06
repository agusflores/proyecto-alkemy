using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs.TestimonialDTO
{
    public class TestimonialsDTO
    {
        public string Name { get; set; }

        public string Image { get; set; }

        public string Content { get; set; }
    }
}
