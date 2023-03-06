using OngProject.Entities;

namespace OngProject.Core.Models.DTOs
{
    public class SlideDTO
    {
        public string ImageBase64 { get; set; }
        public string Text { get; set; }
        public int? Order { get; set; }
        public int OrganizationId { get; set; }

        public SlideDTO() { }

        public SlideDTO(Slides s)
        {
            ImageBase64 = s.ImageUrl;
            Text = s.Text;
            Order = s.Order;
            OrganizationId = s.OrganizationId;
        }
    }
}
