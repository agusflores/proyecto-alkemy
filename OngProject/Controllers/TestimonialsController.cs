using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Helper;
using OngProject.Core.Interfaces;
using OngProject.Core.Mapper;
using OngProject.Core.Models.DTOs.PagedListDTO;
using OngProject.Core.Models.DTOs.TestimonialDTO;
using OngProject.Entities;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class TestimonialsController : ControllerBase
    {
        private readonly ITestimonialsBusiness _service;
        public TestimonialsController(ITestimonialsBusiness service)
        {
            _service = service;
        }

        /// GET: testimonials
        /// <summary>
        ///     Gets all testimonials.
        /// </summary>
        /// <remarks>
        ///     Gets information paged about the testimonials in the database.
        /// </remarks>
        /// <response code="200">OK. Returns testimonials information.</response>  
        /// <response code="400">BadRequest. Invalid request received.</response> 
        /// <response code="401">Unauthorized. Invalid JWT Token or it wasn't provided.</response>
        /// <response code="404">Not found. Server couldn't find the testimonials.</response> 
        /// <response code="500">Internal Server Error.</response> 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllTestimonials([FromQuery(Name = "numberPage")] int numberPage = 1, [FromQuery(Name = "quantityPage")] int quantityPage = 10)
        {
            var host = HttpContext.Request.Host.Value;
            var path = HttpContext.Request.Path.Value;

            var testDto = await _service.GetAll();

            if (testDto != null)
            {
                PagedListHelper<TestimonialsDTO> paged = PagedListHelper<TestimonialsDTO>.Create(testDto, numberPage, quantityPage);
                PagedListDTO<TestimonialsDTO> testList = new(paged, host, path);
                if (testList.totalPage < numberPage)
                {
                    return Ok("Pagina Actual inexistente");
                }
                return Ok(testList);
            }
            else
            {
                return NotFound();
            }

        }


        /// <summary>
        ///     Creates a new testimonial.
        /// </summary>
        /// <remarks>
        ///     Adds a new testimonial in the database.
        /// </remarks>
        /// <param name="TestimonialInsertDto">New testimonial data transfer object.</param>
        /// <response code="200">OK. Returns a result object along with the new testimonial information.</response>        
        /// <response code="400">BadRequest. Testimonial could not be created.</response>    
        /// <response code="401">Unauthorized. Invalid JWT Token or it wasn't provided.</response>    
        /// <response code="500">Internal Server Error.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTestimonials([FromForm] TestimonialInsertDto testDTO)
        {
            try
            {
                var test = await _service.Insert(testDTO);
                return Ok(test);
            }
            catch (System.Exception err)
            {
                return NotFound(err.Message);
            }
        }
        /// <summary>
        ///     Deletes a testimonial.
        /// </summary>
        /// <remarks>
        ///     Deletes a testimonial in the database.
        /// </remarks>
        /// <param name="id">Id of the testimonial that'll be removed from the database</param>
        /// <response code="200">OK. Returns a result object if the testimonial was successfully removed.</response>        
        /// <response code="400">BadRequest. Testimonial could not be removed.</response>    
        /// <response code="401">Unauthorized. Invalid JWT Token or it wasn't provided.</response>
        /// <response code="404">Not found. Server couldn't find the testimonial with the id provided.</response> 
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("/testimonials/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveTestimonials([FromQuery(Name = "id")] int id)
        {

            var testimonials = await _service.GetById(id);

            if (testimonials != null)
            {
                var flag = await _service.DeleteTestimonials(testimonials);
                return Ok(flag);
            }
            else
            {
                return NotFound();
            }

        }

        /// <summary>
        ///     Updates a testimonial.
        /// </summary>
        /// <remarks>
        ///     Updates a testimonial in the database.
        /// </remarks>
        /// <param name="TestimonialUpdateDto">New value for the testimonial</param>
        /// <param name="id">Id from testimonial for changes</param>
        /// <response code="200">Ok. Return the new testimonial updated</response>
        /// <response code="400">BadRequest. Testimonial could not be updated.</response>    
        /// <response code="401">Unauthorized. Invalid JWT Token or it wasn't provided.</response>   
        /// <response code="404">Not found. Server couldn't find the testimonial.</response> 
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTestimonials([FromRoute] int id, [FromForm] TestimonialUpdateDto testDTO)
        {
            var testimonial = await _service.GetById(id);
            if (testimonial != null)
            {
                var testimonialUpdated = await _service.UpdateTestimonials(id, testDTO);
                TestimonialsMapper mapper = new();
                var testimonialDTO = mapper.FromTestimonialsToTestimonialsDisplayDTO(testimonialUpdated);
                return Ok(testimonialDTO);
            }
            else
            {
                return NotFound("No se encontro un testimonio con ese ID");
            }
        }

        /// <summary>
        ///     Gets a testimonial information.
        /// </summary>
        /// <remarks>
        ///     Gets information about the testimonial with the id provided.
        /// </remarks>
        /// <param name="id">Testimonial id that will be searched.</param>        
        /// <response code="200">OK. Returns testimonial information.</response> 
        /// <response code="400">BadRequest. Invalid request received.</response> 
        /// <response code="401">Unauthorized. Invalid JWT Token or it wasn't provided.</response>
        /// <response code="404">Not found. Server couldn't find the testimonial.</response> 
        /// <response code="500">Internal Server Error.</response>
        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTestimonialsById([FromQuery(Name = "id")] int id)
        {

            var test = await _service.GetById(id);

            if (test != null)
            {
                return Ok(test);
            }
            else
            {
                return NotFound();
            }

        }
    }
}
