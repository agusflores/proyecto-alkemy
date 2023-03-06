using OngProject.Core.Interfaces;
using OngProject.Core.Mapper;
using OngProject.Core.Models.DTOs.TestimonialDTO;
using OngProject.Entities;
using OngProject.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Business
{
    public class TestimonialsBusiness : ITestimonialsBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAmazonS3Client _amazonS3Client;
        public TestimonialsBusiness(IUnitOfWork unitOfWork, IAmazonS3Client amazonS3Client)
        {
            _unitOfWork = unitOfWork;
            _amazonS3Client = amazonS3Client;
        }


        public async Task<bool> DeleteTestimonials(Testimonials testimonials)
        {
            
            try
            {
                await _unitOfWork.TestimonialsRepository.Delete(testimonials);
                _unitOfWork.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {
                throw new Exception("Cannot delete Testimonial");
            }

        }

        public async Task<IQueryable<TestimonialsDTO>> GetAll()
        {
            var mapper = new TestimonialsMapper();
            List<Testimonials> testList;
            List<TestimonialsDTO> testDTOList = new List<TestimonialsDTO>();

            testList = (List<Testimonials>) await _unitOfWork.TestimonialsRepository.GetAll();

            foreach (Testimonials t in testList)
            {
                testDTOList.Add(mapper.TestimonialsToTestimonialsDTO(t));
            }
            IQueryable<TestimonialsDTO> test = testDTOList.AsQueryable();
            return test;
        }

        public async Task<Testimonials> GetById(int id)
        {
            var testimonials = await _unitOfWork.TestimonialsRepository.GetById(id);

            return testimonials;
        }

        public async Task<Testimonials> Insert(TestimonialInsertDto testimonialsDto)
        {
            var imageUrl = string.Empty;

            try
            {
                imageUrl = await _amazonS3Client.UploadObject(testimonialsDto.Image);
            }
            catch (System.Exception)
            {
                throw new Exception("Cannot save the image on Amazon S3");
            }

            var testimonial = new Testimonials()
            {
                Name = testimonialsDto.Name,
                Image = imageUrl,
                Content = testimonialsDto.Content,
            };

            await _unitOfWork.TestimonialsRepository.Insert(testimonial);
            _unitOfWork.SaveChanges();

            return testimonial;
        }

        public async Task<Testimonials> UpdateTestimonials(int id, TestimonialUpdateDto testimonialsToUpdate)
        {
            
            var currentTestimonial = await GetById(id);
            currentTestimonial.Image = testimonialsToUpdate == null ? currentTestimonial.Image : await _amazonS3Client.UploadObject(testimonialsToUpdate.Image);
            currentTestimonial.Content = testimonialsToUpdate.Content;
            currentTestimonial.Name = testimonialsToUpdate.Name;
            currentTestimonial.LastModified = DateTime.UtcNow;
            _unitOfWork.TestimonialsRepository.Update(currentTestimonial);
            _unitOfWork.SaveChanges();

            return currentTestimonial;
        }
    }
}
