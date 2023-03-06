using Microsoft.AspNetCore.Http;

namespace OngProject.Core.Models.DTOs
{
    public class SlideCreateDTO
    {
        public IFormFile Image { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        public int OrganizationId { get; set; }
    }
}
