using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Interfaces;
using OngProject.Core.Models.DTOs.UserDTO;
using OngProject.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IUsersBusiness _UserBusiness;

        public AuthController(IAuthBusiness authBusiness, IUsersBusiness UserBusiness)
        {
            _UserBusiness = UserBusiness;
        }


        /// <summary>
        /// Retorna el usuario autenticado poseedor del JSON Web Token (JWT).
        /// </summary>
        /// <param name="strToken">The user's JWT</param>
        /// <returns>Dto con datos básicos del Usuario autenticado.</returns>
        /// 
        /// <response code="404">Si el usuarion no fue encontrado.</response>
        /// <response code="200">Si el elusuario fue encontrado y está autenticado.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("me")]
        public async Task<IActionResult> getUserByAuth(string strToken)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(strToken);
            var userId = int.Parse(jwt.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            var user = await _UserBusiness.GetById(userId);
            if (user != null)
            {
                ViewUserDTO viewUser = new(user)
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Photo = user.Photo,
                    RoleId = user.RoleId
                };
                return Ok(viewUser);
            }
            return NotFound();

        }

    }
}