using System.ComponentModel.DataAnnotations;

namespace OngProject.Entities
{
    public class MembersDTO
    {
        public string Name { get; set; }
        public string FacebookUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string LinkedinUrl { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
    }
}
