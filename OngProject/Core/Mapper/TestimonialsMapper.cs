using OngProject.Core.Models.DTOs.ActivitiesDTO;
using OngProject.Core.Models.DTOs.TestimonialDTO;
using OngProject.Entities;
using System.Collections.Generic;

namespace OngProject.Core.Mapper
{
    public class TestimonialsMapper
    {

        public TestimonialsDisplayDTO FromTestimonialsToTestimonialsDisplayDTO(Testimonials  testimonials)
        {
            var dto = new TestimonialsDisplayDTO
            {
                Name = testimonials.Name,
                Content = testimonials.Content,
                Image = testimonials.Image
            };
            return dto;
        }

        public Testimonials UpdateTestimonials(Testimonials currentTestimonials, TestimonialUpdateDto testimonialsToUpdate, string newImageTestimonials)
        {
            currentTestimonials.Name = testimonialsToUpdate.Name;
            currentTestimonials.Image = newImageTestimonials;
            currentTestimonials.Content = testimonialsToUpdate.Content;
            return currentTestimonials;
        }

        public TestimonialsDTO TestimonialsToTestimonialsDTO(Testimonials test)
        {
            var testDTO = new TestimonialsDTO()
            {
                Name = test.Name,
                Image = test.Image,
                Content = test.Content
            };
            return testDTO;
        }


    }
}
