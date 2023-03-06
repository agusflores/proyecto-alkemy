using OngProject.Core.Models.DTOs.UserDTO;
using OngProject.Entities;

namespace OngProject.Core.Interfaces
{
    public interface IEntityMapper
    {
        public User FromRegisterDtoToUser(UserRegisterDTO register);
        public UserRegisterDTO FromUserToUserDto(User user);
    }
}
