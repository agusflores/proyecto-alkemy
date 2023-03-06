using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OngProject.Entities
{
    public class Organization : Entity
    {
        [Required]
        [MaxLength(255)]
        [Column(TypeName = "varchar")]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        [Column(TypeName = "varchar")]
        public string Image { get; set; }
        
        [MaxLength(255)]
        [Column(TypeName = "varchar")]
        public string Address { get; set; }

        [MaxLength(20)]
        public int? Phone { get; set; }

        [Required]
        [MaxLength(320)]
        [Column(TypeName = "varchar")]
        public string Email { get; set; }

        [Required]
        [MaxLength(500)]
        [Column(TypeName = "text")]
        public string WelcomeText { get; set; }

        [MaxLength(2000)]
        [Column(TypeName = "text")]
        public string AboutUsText { get; set; }

        [MaxLength(255)]
        public string FacebookUrl { get; set; }

        [MaxLength(255)]
        public string LinkedinUrl { get; set; }

        [MaxLength(255)]
        public string InstagramUrl { get; set; }

        public Organization() { }

        public Organization(string name, string image, string address, int? phone, string email, string welcomeText, string aboutUsText, string facebookUrl, string linkedinUrl, string instagramUrl)
        {
            Name = name;
            Image = image;
            Address = address;
            Phone = phone;
            Email = email;
            WelcomeText = welcomeText;
            AboutUsText = aboutUsText;
            FacebookUrl = facebookUrl;
            LinkedinUrl = linkedinUrl;
            InstagramUrl = instagramUrl;
        }
    }
}
