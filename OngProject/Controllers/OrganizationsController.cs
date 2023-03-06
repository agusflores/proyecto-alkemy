using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Interfaces;
using OngProject.Core.Mapper;
using OngProject.Core.Models.DTOs;
using OngProject.Core.Models.DTOs.OrganizationDTO;
using OngProject.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationsBusiness _organizationsService;

        public OrganizationsController(IOrganizationsBusiness service)
        {
            _organizationsService = service;
        }

        [HttpGet("/api/organization/public")]

        public async Task<IActionResult> GetAllOrganization()
        {
            var organizations = await _organizationsService.GetAllOrganization();
            if (organizations.Count() > 0)
                return Ok(organizations);
            else return NotFound();
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetByIdOrganization([FromQuery(Name = "id")] int id)
        {
            var organization = await _organizationsService.GetByIdOrganization(id);

            if (organization != null)
            {
                return Ok(organization);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertOrganization([FromBody] CreateOrganizationDTO organizationDTO)
        {
            if (organizationDTO.Name != null)
            {
                var newOrganization = await _organizationsService.InsertOrganization(organizationDTO);
                if (newOrganization != null)
                    return Ok("Member created " + newOrganization);
                else
                    return NotFound("Something went wrong");
            }
            else return NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrganization(int id, [FromQuery] UpdateOrganizationDTO updateOrganizationDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                }
                else
                {
                    var result = await _organizationsService.UpdateOrganization(id, updateOrganizationDTO);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganization([FromQuery(Name = "id")] int id)
        {
            var organization = await _organizationsService.GetByIdOrganization(id);

            if (organization != null)
            {
                var flag = await _organizationsService.DeleteOrganization(organization.Id);
                return Ok(flag);
            }
            else
            {
                return NotFound();
            }
        }


    }
}
