using OngProject.Core.Models.DTOs.UserDTO;
using OngProject.Entities;

namespace OngProject.Core.Mapper
{
    public class UsersMapper
    {
        public static ViewUserDTO FromUsersToUsersDisplayDto(User user)
        {
            ViewUserDTO dto = new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Photo = user.Photo,
                RoleId = user.RoleId
            };
            return dto;
        }
    }
}
