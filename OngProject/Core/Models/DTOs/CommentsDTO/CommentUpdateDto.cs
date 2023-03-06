using OngProject.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs.CommentsDTO
{
    public class CommentUpdateDto
    {
        [Required(ErrorMessage = "Body is required.")]
        [MaxLength(255, ErrorMessage = "Body must have less than or equal to 255 characters.")]
        public string Body { get; set; }
    }
}
