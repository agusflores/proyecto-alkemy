﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OngProject.Entities
{
    public class OrganizationDisplayDTO
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public int? Phone { get; set; }
        public string Email { get; set; }
        public string WelcomeText { get; set; }
        public string AboutUsText { get; set; }
        public string FacebookUrl { get; set; }
        public string LinkedinUrl { get; set; }
        public string InstagramUrl { get; set; }
    }
}
