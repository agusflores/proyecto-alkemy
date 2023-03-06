using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs
{
    public class CommentCreateDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Body { get; set; }

       [Required]
        public int NewsId { get; set; }
    }
}
