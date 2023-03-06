using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Interfaces;
using OngProject.Core.Models.DTOs.UserDTO;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUsersBusiness _service;
        private readonly IAuthBusiness _authBusiness;
        private readonly ISendGridBusiness _sendGridBusiness;

        public UserController(IUsersBusiness service, IAuthBusiness authBusiness, ISendGridBusiness sendGridBusiness)
        {
            _service = service;
            _authBusiness = authBusiness;
            _sendGridBusiness = sendGridBusiness;
        }

        /// <summary>
        ///     Get all users information.
        /// </summary>
        /// <remarks>
        ///     Get the information about all the users stored in the database.
        /// </remarks>
        /// <response code="200">OK. Returns users information.</response>  
        /// <response code="400">Bad request. Invalid request received.</response>    
        /// <response code="401">Unauthorized. Invalid JWT Token or it wasn't provided.</response>     
        /// <response code="403">Forbidden. You do not have the permissions to perform this action.</response>
        /// <response code="404">Not Found. Server couldn't find any user.</response> 
        /// <response code="500">Internal Server Error.</response>
        [HttpGet]
        [Route("/users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllUsers()
        {
            var _listUsers = await _service.GetAll();
            if (_listUsers != null)
            {
                return Ok(_listUsers);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Registra un nuevo usuario en el sistema y le envia un mensaje de bienvenida a su correo.
        /// </summary>
        /// <param name="userDto">DTO con información básica y necesaria para registrar a un usuario.</param>
        /// <returns>JWT string con los claims básicos del usuario.</returns>
        /// 
        /// <response code="403">Si el email a registrar ya existe.</response>
        /// <response code="200">Si el usuario se registró exitosamente.</response>
        [HttpPost("/auth/register")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateUser([FromBody] UserRegisterDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            }
            else
            {
                var token = await _service.Insert(userDTO);

                if (token != "")
                {
                    await _sendGridBusiness.WelcomeEmail(userDTO.Email);
                    return Ok(token);
                }
                else
                {
                    return Forbid();
                }
            }
        }

        /// <summary>
        /// Loguea un usuario existente en sistema, dandole acceso los recursos según privilegios..
        /// </summary>
        /// <param name="userLoginDto">DTO con información básica y necesaria para loguear a un usuario.</param>
        /// <returns>JWT string con los claims básicos del usuario.</returns>
        /// 
        /// <response code="200">Si el usuario se logueó exitosamente.</response>
        /// <response code="400">Si el email o password no fueron introducidos o si el usuario no existe.</response>
        [HttpPost("/auth/login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDTO userLoginDTO)
        {
            // 1. Valida si el campo email y password fueron enviados correctamente en la peticion
            if (!ModelState.IsValid)
            {
                return BadRequest(string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            }
            else
            {
                // 2. Verifica si existe un usuario con el mail ingresado
                var userToLogin = await _service.GetByEmail(userLoginDTO.Email);
                if (userToLogin == null)
                {
                    return BadRequest("No existe un usuario con el mail ingresado");
                }
                else
                {
                    //3. Si existe usuario con el mail ingresado, comparar passwords encriptadas 
                    var user = await _service.ValidateUser(userToLogin, userLoginDTO.Password);
                    //4. Si la password ingresada es correcta, retorna el token otorgado al usuario, por lo contrario retorna false. 
                    if (user == null)
                    {
                        return Ok(false);
                    }
                    return Ok(_authBusiness.GetToken(user));
                }
            }
        }

        /// <summary>
        ///     Deletes an user.
        /// </summary>
        /// <remarks>
        ///     Deletes the user in the database.
        /// </remarks>
        /// <param name="id">User ID that will be deleted.</param>
        /// <response code="200">OK. Returns a result object.</response>  
        /// <response code="400">Bad request. User couldn't be updated.</response>    
        /// <response code="401">Unauthorized. Invalid JWT Token or it wasn't provided.</response>     
        /// <response code="403">Forbidden. You do not have the permissions to perform this action.</response>
        /// <response code="404">Not Found. Server couldn't find any user with the ID provided.</response> 
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveUser([FromQuery(Name = "id")] int id)
        {
            bool user = await _service.Delete(id);
            if (user)
            {
                return Ok("User deleted " + user);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        ///     Updates an user information.
        /// </summary>
        /// <remarks>
        ///     Updates an user information with the values provided.
        /// </remarks>
        /// <param name="id">User ID that will be updated.</param>
        /// <param name="userDTO">Dto that allow to update the user</param>
        /// <response code="200">OK. Returns a result object.</response>  
        /// <response code="400">Bad request. User couldn't be updated.</response>    
        /// <response code="401">Unauthorized. Invalid JWT Token or it wasn't provided.</response>     
        /// <response code="403">Forbidden. You do not have the permissions to perform this action.</response>
        /// <response code="404">Not Found. Server couldn't find any user with the ID provided.</response> 
        /// <response code="500">Internal Server Error.</response>
        [HttpPut("id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser([FromQuery(Name = "id")] int id, [FromBody] UserUpdateDTO userDTO)
        {
            var user = await _service.Update(id, userDTO);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        ///     Gets an user information.
        /// </summary>
        /// <remarks>
        ///     Gets information about the user.
        /// </remarks>
        /// <param name="id">User id that will be searched.</param>        
        /// <response code="200">OK. Returns user information.</response> 
        /// <response code="400">BadRequest. Invalid request received.</response> 
        /// <response code="404">Not found. Server couldn't find the user.</response> 
        /// <response code="500">Internal Server Error.</response> 
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserById([FromQuery(Name = "id")] int id)
        {
            var user = await _service.GetById(id);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
