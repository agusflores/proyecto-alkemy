using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Entities
{
    public class Slides : Entity
    {

        [Required]
        [MaxLength(255)]
        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(255)]
        public string Text { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        public int OrganizationId { get; set; }
    }
}
