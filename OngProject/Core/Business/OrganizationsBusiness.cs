using OngProject.Core.Interfaces;
using OngProject.Core.Mapper;
using OngProject.Core.Models.DTOs;
using OngProject.Core.Models.DTOs.OrganizationDTO;
using OngProject.Entities;
using OngProject.Repositories;
using OngProject.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace OngProject.Core.Business
{
    public class OrganizationsBusiness : IOrganizationsBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAmazonS3Client _amazonClient;

        public OrganizationsBusiness(IUnitOfWork unitOfWork, IAmazonS3Client amazonS3Client)
        {
            _unitOfWork = unitOfWork;
            _amazonClient = amazonS3Client;

        }

        public async Task<bool> DeleteOrganization(int id)
        {
            Organization organization = await _unitOfWork.OrganizationRepository.GetById(id);
            if (organization == null) return false;
            bool result = await _unitOfWork.OrganizationRepository.Delete(organization);
            _unitOfWork.SaveChanges();
            return result;
        }

        public async Task<List<GetOrganizationDto>> GetAllOrganization()
        {
            var listOrganizations = await _unitOfWork.OrganizationRepository.GetAll();
            var listSlides = await _unitOfWork.SlidesRepository.GetAll();

            var orderedListSlides = listSlides.OrderBy(s => s.Order);

            List<GetOrganizationDto> listDto = new List<GetOrganizationDto>();

            foreach (var organization in listOrganizations)
            {
                var organizacionDto = new GetOrganizationDto(organization);
                foreach (var slide in orderedListSlides)
                {
                    if (slide.OrganizationId == organization.Id)
                    {
                        organizacionDto._listSlides.Add(new SlideDTO(slide));
                    }
                }
                listDto.Add(organizacionDto);
            }
            return listDto;
        }

        public async Task<Organization> GetByIdOrganization(int id)
        {
            return await _unitOfWork.OrganizationRepository.GetById(id);
        }

        public async Task<OrganizationDisplayDTO> InsertOrganization(CreateOrganizationDTO organizationDTO)
        {
            if (organizationDTO != null)
            {
                var mapper = new OrganizationMapper();
                var organization = mapper.FromCreateOrganizationDTOToOrganization(organizationDTO);

                if (organization != null)
                {
                    organization.Image = await _amazonClient.UploadObject(organizationDTO.Image);
                }

                await _unitOfWork.OrganizationRepository.Insert(organization);
                _unitOfWork.SaveChanges();

                var dto = mapper.FromOrganizationToOrganizationDisplayDTO(organization);

                return dto;
            }
            return null;
        }

        public async Task<UpdateOrganizationDTO> UpdateOrganization(int id, UpdateOrganizationDTO organizationDTO)
        {
            try
            {
                var organizationToUpdate = await _unitOfWork.OrganizationRepository.GetById(id);
                if (organizationToUpdate == null)
                {
                    throw new Exception($"No existe una organizacion con el id {id}");
                }

                if (organizationDTO is null)
                {
                    throw new ArgumentNullException(nameof(organizationDTO));
                }

                organizationToUpdate.Name = organizationDTO.Name;
                organizationToUpdate.Image = organizationDTO.Image.ToString();
                organizationToUpdate.Address = organizationDTO.Address;
                organizationToUpdate.Phone = organizationDTO.Phone;
                organizationToUpdate.Email = organizationDTO.Email;
                organizationToUpdate.WelcomeText = organizationDTO.WelcomeText;
                organizationToUpdate.AboutUsText = organizationDTO.AboutUsText;
                organizationToUpdate.FacebookUrl = organizationDTO.FacebookUrl;
                organizationToUpdate.LinkedinUrl = organizationDTO.LinkedinUrl;
                organizationToUpdate.InstagramUrl = organizationDTO.InstagramUrl;
                organizationToUpdate.LastModified = DateTime.UtcNow;

                UpdateOrganizationDTO organizationUpdated = new UpdateOrganizationDTO(organizationToUpdate);

                _unitOfWork.OrganizationRepository.Update(organizationToUpdate);
                await _unitOfWork.Complete();
                return organizationUpdated;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
