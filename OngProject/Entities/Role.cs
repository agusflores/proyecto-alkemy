using System;
using System.ComponentModel.DataAnnotations;

namespace OngProject.Entities
{
    public class Role : Entity
    {
        [Required(ErrorMessage ="Name is required.")]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required(ErrorMessage ="Description is required.")]
        [MaxLength(255)]
        public string Description { get; set; }
    }
}
