using System;
using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs.NewsDTO
{
    public class GetNewsDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [MaxLength(65535)]
        public string Content { get; set; }

        [Required(ErrorMessage = "Image is required")]
        [MaxLength(255)]
        public string Image { get; set; }

    }
}
