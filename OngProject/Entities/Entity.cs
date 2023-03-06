using System;
using System.ComponentModel.DataAnnotations;

namespace OngProject.Entities
{
    public class Entity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;  
        
    }
}
