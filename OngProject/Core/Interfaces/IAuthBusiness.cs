using OngProject.Entities;

namespace OngProject.Core.Interfaces
{
    public interface IAuthBusiness
    {
        /// <summary>
        ///     Devuelve el token generado para un usuario
        /// </summary>
        /// <param name="user">
        ///     Usuario al que se le otorgara el token
        /// </param>
        string GetToken(User user);
    }
}
