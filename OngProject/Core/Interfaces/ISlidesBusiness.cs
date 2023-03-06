using Microsoft.AspNetCore.Http;
using OngProject.Core.Models.DTOs;
using OngProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces
{
    public interface ISlidesBusiness
    {
        public Task<IEnumerable<SlidesOrderImageDTO>> GetAllSlides();
        public Task<SlideDTO> GetSlideById(int id);
        //public Task<SlideDTO> CreateSlide(SlideCreateDTO slideCreateDto);
        public Task<SlideDTO> CreateSlide(SlideDTO slideDTO);
        public Task<bool> RemoveSlide(int id);
        public Task<SlideDTO> UpdateSlide(int id, SlideUpdateDTO slideDTO);

    }
}
