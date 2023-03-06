using System.ComponentModel.DataAnnotations;

namespace OngProject.Entities
{
    public class Contacts : Entity
    {
        [Required, MaxLength(255)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required, MaxLength(20)]
        [Display(Name = "Phone")]
        public int Phone { get; set; }
        
        [Required, MaxLength(320)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required, MaxLength(500)]
        [Display(Name = "Message")]
        public string Message { get; set; }
       
        /*public Contact() { }

        public Contact(string name, int phone, string email, string message)
        {
            Name = name;
            Phone = phone;
            Email = email;
            Message = message;
        }*/
    }
}
