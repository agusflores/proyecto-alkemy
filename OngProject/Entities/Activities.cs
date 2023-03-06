using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Entities
{
    public class Activities : Entity
    {

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(65535)]
        public string Content { get; set; }

        [Required]
        [MaxLength(255)]
        public string Image { get; set; }

    }
}
