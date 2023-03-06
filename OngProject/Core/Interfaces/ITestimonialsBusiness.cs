using OngProject.Core.Models.DTOs.TestimonialDTO;
using OngProject.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces
{
    public interface ITestimonialsBusiness
    {
        Task<IQueryable<TestimonialsDTO>> GetAll();
        Task<Testimonials> GetById(int id);
        public Task<bool> DeleteTestimonials(Testimonials testimonials);
        Task<Testimonials> Insert(TestimonialInsertDto testimonialsDto);
        Task<Testimonials> UpdateTestimonials(int id, TestimonialUpdateDto testimonialsToUpdate);
    }
}
