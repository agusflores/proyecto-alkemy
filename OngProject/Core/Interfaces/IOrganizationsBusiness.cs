using OngProject.Core.Models.DTOs;
using OngProject.Core.Models.DTOs.OrganizationDTO;
using OngProject.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces
{
    public interface IOrganizationsBusiness
    {
        public Task<List<GetOrganizationDto>> GetAllOrganization();

        public Task<Organization> GetByIdOrganization(int id);

        public Task<OrganizationDisplayDTO> InsertOrganization(CreateOrganizationDTO organization);

        public Task<UpdateOrganizationDTO> UpdateOrganization(int id, UpdateOrganizationDTO organization);

        public Task<bool> DeleteOrganization(int id);     
    }
}
