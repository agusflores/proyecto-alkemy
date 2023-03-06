using AutoMapper;
using OngProject.Core.Models.DTOs.UserDTO;
using OngProject.Entities;

namespace OngProject.Core.Mapper
{
    public class EntityMapper 
    {
        public User FromRegisterDtoToUser(UserRegisterDTO register)
        {
            var user = new User()
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                Email = register.Email,
                Password = register.Password
            };
            return user;
        }

        public UserRegisterDTO FromUserToUserDto(User user)
        {
            var userDto = new UserRegisterDTO()
            {
                //FirstName = $"{user.FirstName} {user.LastName}",
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password
            };
            return userDto;
        }
    }
}
