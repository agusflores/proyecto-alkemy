using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs
{
    public class SlideUpdateDTO
    {
        public IFormFile Image { get; set; }
        public string Text { get; set; }
        public int OrganizationId { get; set; }
        public int Order { get; set; }
    }
}
