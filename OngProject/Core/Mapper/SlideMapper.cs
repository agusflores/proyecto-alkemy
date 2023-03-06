using OngProject.Core.Models.DTOs;
using OngProject.Entities;

namespace OngProject.Core.Mapper
{
    public class SlideMapper
    {
        public Slides FromSlideUpdateDtoToSlide(SlideUpdateDTO slideUpdateDto)
        {
            var slide = new Slides
            {
                Order = slideUpdateDto.Order,
                Text = slideUpdateDto.Text,
                OrganizationId = slideUpdateDto.OrganizationId
            };
            return slide;
        }

        public Slides FromSlidesCreateDtoToSlides(SlideCreateDTO slideCreateDto)
        {
            var slides = new Slides
            {
                Order = slideCreateDto.Order,
                Text = slideCreateDto.Text,
                OrganizationId = slideCreateDto.OrganizationId
            };
            return slides;
        }

        public SlideDTO FromSlidesToSlidesDto(Slides slide)
        {
            var dto = new SlideDTO
            {
                Order = slide.Order,
                Text = slide.Text,
                OrganizationId = slide.OrganizationId,
                ImageBase64 = slide.ImageUrl
            };
            return dto;
        }


    }
}
