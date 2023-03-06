using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OngProject.Core.Interfaces;
using OngProject.Core.Mapper;
using OngProject.Core.Models.DTOs;
using OngProject.DataAccess;
using OngProject.Entities;
using OngProject.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OngProject.Core.Business
{
    public class SlidesBusiness : ISlidesBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAmazonS3Client _amazonS3Client;
        private readonly IOrganizationsBusiness _organizationBusiness;
        private readonly OngDbContext _context;
        public SlidesBusiness(IUnitOfWork unitOfWork, IAmazonS3Client amazonS3Client, OngDbContext context, IOrganizationsBusiness organizationBusiness)
        {
            _unitOfWork = unitOfWork;
            _amazonS3Client = amazonS3Client;
            _context = context;
            _organizationBusiness = organizationBusiness;
        }

        public async Task<SlideDTO> CreateSlide(SlideDTO slideDTO)
        {
            var fileName = Guid.NewGuid().ToString();
            try
            {
                MemoryStream fileStream = new MemoryStream(Convert.FromBase64String(slideDTO.ImageBase64.Split(",", 2)[1]));
                var url = await _amazonS3Client.UploadObject(fileStream);

                if (slideDTO.Order == null || slideDTO.Order == 0)
                {
                    var lastSlide = await _context.Slides.OrderByDescending(s => s.LastModified).FirstAsync();
                    slideDTO.Order = lastSlide.Order;
                }

                var organization = await _organizationBusiness.GetByIdOrganization(slideDTO.OrganizationId);

                if (organization == null)
                {
                    throw new Exception("Ingreso un 'ID' de una organizacion inexistente");
                }

                Slides slide = new Slides
                {
                    Text = slideDTO.Text,
                    OrganizationId = slideDTO.OrganizationId,
                    ImageUrl = url,
                    Order = slideDTO.Order.Value
                };

                await _unitOfWork.SlidesRepository.Insert(slide);
                await _unitOfWork.Complete();
                return new SlideDTO(slide);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
                //await _amazonS3Client.DeleteObject(fileName);
            }
        }

        public async Task<IEnumerable<SlidesOrderImageDTO>> GetAllSlides()
        {
            var slides = await _unitOfWork.SlidesRepository.GetAll();
            var slidesOrderImage = new List<SlidesOrderImageDTO>();
            foreach (var slide in slides)
            {
                var slideDTO = new SlidesOrderImageDTO()
                {
                    ImageUrl = slide.ImageUrl,
                    Order = slide.Order
                };

                slidesOrderImage.Add(slideDTO);
            }
            return slidesOrderImage;
        }

        public async Task<SlideDTO> GetSlideById(int id)
        {
            var slide = await _unitOfWork.SlidesRepository.GetById(id);
            if (slide == null)
            {
                return null;
            }
            return new SlideDTO(slide);
        }

        public async Task<bool> RemoveSlide(int id)
        {
            var existing = await _unitOfWork.SlidesRepository.GetById(id);

            if (existing == null)
                throw new Exception("Slide not found.");

            await _unitOfWork.SlidesRepository.Delete(existing);
            _unitOfWork.SaveChanges();

            return true;
        }

        public async Task<SlideDTO> UpdateSlide(int id, SlideUpdateDTO slideDTO)
        {
            var mapper = new SlideMapper();
            try
            {
                var existingSlide = await _unitOfWork.SlidesRepository.GetById(id);

                if (existingSlide != null)
                {
                    existingSlide.Text = slideDTO.Text;
                    existingSlide.OrganizationId = slideDTO.OrganizationId;
                    existingSlide.ImageUrl = await _amazonS3Client.UploadObject(slideDTO.Image);
                    existingSlide.Order = slideDTO.Order;

                    _unitOfWork.SlidesRepository.Update(existingSlide);
                    _unitOfWork.SaveChanges();

                    var dto = mapper.FromSlidesToSlidesDto(existingSlide);
                    return dto;
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
