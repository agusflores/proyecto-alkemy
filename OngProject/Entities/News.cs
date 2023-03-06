using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OngProject.Entities
{
    public class News : Entity
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

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
