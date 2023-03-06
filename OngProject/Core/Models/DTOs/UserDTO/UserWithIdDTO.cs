using OngProject.Entities;

namespace OngProject.Core.Models.DTOs.UserDTO
{
    public class UserWithIdDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public int RoleId { get; set; }
    }
}
