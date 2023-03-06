using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Helper;
using OngProject.Core.Interfaces;
using OngProject.Core.Models.DTOs.PagedListDTO;
using System.Linq;
using OngProject.Entities;
using System;
using System.Threading.Tasks;
using OngProject.Core.Mapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace OngProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class MembersController : ControllerBase
    {
        private readonly IMembersBusiness _membersBusiness;
        public MembersController(IMembersBusiness service)
        {
            _membersBusiness = service;
        }

        /// <summary>
        ///     Devuelve todos los miembros.
        /// </summary>
        /// <remarks>
        ///     Obtiene la lista de miembros en formato de paginación.
        /// </remarks>
        /// <response code="200">OK. Devuelve la información de los miembros.</response>  
        /// <response code="401">Unauthorized. El JWT es inválido.</response>
        /// <response code="404">Not found. Hubo un error en la lista de miembros.</response> 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllMembers([FromQuery(Name = "numberPage")] int numberPage = 1, [FromQuery(Name = "quantityPage")] int quantityPage = 10)
        {
            var host = HttpContext.Request.Host.Value;
            var path = HttpContext.Request.Path.Value;
            var membersDTO = await _membersBusiness.GetAllMembers();
            if (membersDTO != null)
            {
                PagedListHelper<MembersDTO> paged = PagedListHelper<MembersDTO>.Create(membersDTO, numberPage, quantityPage);
                PagedListDTO<MembersDTO> memberList = new(paged, host, path);
                if (memberList.totalPage < numberPage)
                {
                    return Ok("Pagina Actual inexistente");
                }
                return Ok(memberList);
            }
            else
            {
                return NotFound();
            }
        }



        /// <summary>
        ///     Crear un nuevo Miembro.
        /// </summary>
        /// <remarks>
        ///     Inserta un nuevo miembro en la base de datos.
        /// </remarks>
        /// <param name="memberDTO">Nuevo Objeto de miembros.</param>
        /// <response code="200">OK. Devuelve el Miembro que se cargó en la base de datos.</response>        
        /// <response code="404">NotFound. No se puedo crear el Miembro.</response>    
        /// <response code="401">Unauthorized. El JWT es inválido.</response>    
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateMember(MembersDTO memberDTO)
        {
            var newMember = await _membersBusiness.CreateMember(memberDTO);
            if (newMember)
                return Ok("Member created " + newMember);
            else
                return NotFound("Something went wrong");

        }


        /// <summary>
        ///     Eliminación de miembros.
        /// </summary>
        /// <remarks>
        ///     Elimina los miembros de la base de datos.
        /// </remarks>
        /// <param name="id">Id del miembro que se eliminará de la base de datos.</param>
        /// <response code="200">OK. Devuelve que fue borrado de la base de datos.</response>        
        /// <response code="401">Unauthorized. El JWT es inválido.</response>
        /// <response code="404">Not found. No se encontró el miembro con ese Id.</response> 
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveMember([FromQuery(Name = "id")] int id)
        {
            var members = await _membersBusiness.GetMemberById(id);

            if (members != null)
            {
                var flag = await _membersBusiness.DeleteMember(members);
                return Ok(flag);
            }
            else
            {
                return NotFound();
            }
        }


        /// <summary>
        ///     Actualización de Miembros.
        /// </summary>
        /// <remarks>
        ///     Actualiza un miembro en la base de datos.
        /// </remarks>
        /// <param name="membersUpdateDTO">Nueva información del miembro a editar.</param>
        /// <param name="id">Id del miembro a cambiar en la base de datos.</param>
        /// <response code="200">Ok. devuelve el miembro que fue editado en la base de datos.</response>
        /// <response code="401">Unauthorized. El JWT es inválido.</response>   
        /// <response code="404">Not found. El servidor no encontró el Id del miembro. </response> 
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMembers([FromRoute] int id, [FromForm] MembersUpdateDTO membersUpdateDTO)
        {
            var member = await _membersBusiness.GetMemberById(id);
            if (member != null)
            {
                var memberUpdated = await _membersBusiness.UpdateMembers(id, membersUpdateDTO);
                MembersMapper mapper = new();
                var membersDTO = mapper.FromMembersToMembersDisplayDTO(memberUpdated);
                return Ok(membersDTO);
            }
            else
            {
                return NotFound("No se encontro un miembro con ese ID");
            }
        }


        /// <summary>
        ///     Devuelve la información del miembro.
        /// </summary>
        /// <remarks>
        ///     Devuelve la información del miembro en base a su Id.
        /// </remarks>
        /// <param name="id">Id del miembro a buscar</param>        
        /// <response code="200">OK. Devuelve la información del miembro.</response> 
        /// <response code="401">Unauthorized. El JWT es inválido.</response>
        /// <response code="404">Not found. El servidor no encontró el miembro con ese Id.</response> 
        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMemberById([FromQuery(Name = "id")] int id)
        {
            var member = await _membersBusiness.GetMemberById(id);

            if (member != null)
            {
                return Ok(member);
            }
            else
            {
                return NotFound();
            }

        }
    }
}