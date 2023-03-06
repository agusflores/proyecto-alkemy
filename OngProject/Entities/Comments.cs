using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OngProject.Entities
{
    public class Comments : Entity
    {
        public int UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Body { get; set; }

        [ForeignKey("News")]
        public int NewsId { get; set; }
        public News News { get; set; }
    }
}
