using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OngProject.Core.Interfaces;
using OngProject.Entities;
using OngProject.Core.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace OngProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class SlidesController : ControllerBase
    {

        private readonly ISlidesBusiness _service;
        public SlidesController(ISlidesBusiness service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSlides()
        {

            var slides = await _service.GetAllSlides();

            if (slides != null)
            {
                return Ok(slides);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet("id")]
        public async Task<IActionResult> GetSlideById([FromQuery] int id)
        {
            var slide = await _service.GetSlideById(id);
            if (slide != null)
            {
                return Ok(slide);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSlide([FromBody] SlideDTO slidesDTO)
        {
            try
            {
                var result = await _service.CreateSlide(slidesDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveSlide(int id)
        {
            try
            {
                bool slide = await _service.RemoveSlide(id);
                return Ok();
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSlide([FromRoute] int id, [FromForm] SlideUpdateDTO slidesDTO)
        {
            var slide = await _service.UpdateSlide(id, slidesDTO);
            if (slide != null)
            {
                return Ok(slide);
            }
            else
            {
                return NotFound(400);
            }
        }
    }
}
