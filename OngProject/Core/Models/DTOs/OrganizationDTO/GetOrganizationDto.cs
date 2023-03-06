using OngProject.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OngProject.Core.Models.DTOs.OrganizationDTO
{
    public class GetOrganizationDto
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public int? Phone { get; set; }
        public string FacebookUrl { get; set; }
        public string LinkedinUrl { get; set; }
        public string InstagramUrl { get; set; }
        public List<SlideDTO> _listSlides { get; set; }

        public GetOrganizationDto() { }

        public GetOrganizationDto(Organization org)
            :base()
        {
            _listSlides = new List<SlideDTO>();
            Name = org.Name;
            Image = org.Image;
            Address = org.Address;
            Phone = org.Phone;
            FacebookUrl = org.FacebookUrl;
            LinkedinUrl = org.LinkedinUrl;
            InstagramUrl = org.InstagramUrl;
        }

    }
}
