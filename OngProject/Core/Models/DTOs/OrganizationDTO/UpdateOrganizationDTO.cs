using Microsoft.AspNetCore.Http;
using OngProject.Entities;
using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs.OrganizationDTO
{
    public class UpdateOrganizationDTO
    {
        [StringLength(50)]
        public string Name { get; set; }

        public string Image { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [Phone]
        public int? Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(100)]
        public string WelcomeText { get; set; }

        [StringLength(100)]
        public string AboutUsText { get; set; }

        [Url]
        public string FacebookUrl { get; set; }

        [Url]
        public string LinkedinUrl { get; set; }

        [Url]
        public string InstagramUrl { get; set; }

        public UpdateOrganizationDTO() { }

        public UpdateOrganizationDTO(Organization org)
        {
            Name = org.Name;
            Image = org.Image;
            Address = org.Address;
            Phone = org.Phone;
            Email = org.Email; 
            WelcomeText = org.WelcomeText;
            AboutUsText = org.AboutUsText;
            FacebookUrl = org.FacebookUrl;
            LinkedinUrl = org.LinkedinUrl;
            InstagramUrl = org.InstagramUrl;
        }
    }
}
