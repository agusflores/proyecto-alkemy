using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Helper;
using OngProject.Core.Interfaces;
using OngProject.Core.Models.DTOs;
using OngProject.Core.Models.DTOs.NewsDTO;
using OngProject.Core.Models.DTOs.PagedListDTO;
using OngProject.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class NewsController : ControllerBase
    {

        private readonly INewsBusiness _service;

        public NewsController(INewsBusiness service)
        {
            _service = service;
        }
        /// GET: news
        /// <summary>
        ///     Gets all news.
        /// </summary>
        /// <remarks>
        ///     Gets information paged about the news in the database.
        /// </remarks>
        /// <response code="200">OK. Returns news information.</response>  
        /// <response code="400">BadRequest. Invalid request received.</response> 
        /// <response code="401">Unauthorized. Invalid JWT Token or it wasn't provided.</response>
        /// <response code="404">Not found. Server couldn't find the news.</response> 
        /// <response code="500">Internal Server Error.</response> 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetNewsDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllNews([FromQuery(Name = "numberPage")] int numberPage = 1, [FromQuery(Name = "quantityPage")] int quantityPage = 10)
        {
            var host = HttpContext.Request.Host.Value;
            var path = HttpContext.Request.Path.Value;
            var newsDTO = await _service.GetAllNews();

            if (newsDTO != null)
            {
                PagedListHelper<GetNewsDto> page = PagedListHelper<GetNewsDto>.Create(newsDTO, numberPage, quantityPage);
                PagedListDTO<GetNewsDto> listNews = new(page, host, path);
                if (listNews.totalPage < numberPage)
                {
                    return Ok("Pagina Actual inexistente");
                }
                return Ok(listNews);
            }
            else
            {
                return NotFound();
            }

        }

        /// <summary>
        ///     Creates a new News.
        /// </summary>
        /// <remarks>
        ///     Adds a new News in the database.
        /// </remarks>
        /// <param name="InserNewDto">New News data transfer object.</param>
        /// <response code="200">OK. Returns a result object along with the new News information.</response>        
        /// <response code="400">BadRequest. News could not be created.</response>    
        /// <response code="401">Unauthorized. Invalid JWT Token or it wasn't provided.</response>    
        /// <response code="500">Internal Server Error.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateNews([FromForm] InserNewDto inserNewDto)
        {
            var news = await _service.CreateNews(inserNewDto);
            if (news != null)
            {
                return Ok(new {message= "News  saved successfully" });
            }
            else
            {
                return NotFound();
            }

        }

        /// <summary>
        ///     Deletes a News.
        /// </summary>
        /// <remarks>
        ///     Deletes a News in the database.
        /// </remarks>
        /// <param name="id">Id of the News that'll be removed from the database</param>
        /// <response code="200">OK. Returns a result object if the News was successfully removed.</response>        
        /// <response code="400">BadRequest. News could not be removed.</response>    
        /// <response code="401">Unauthorized. Invalid JWT Token or it wasn't provided.</response>
        /// <response code="404">Not found. Server couldn't find the News with the id provided.</response> 
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveNews([FromQuery(Name = "id")] int id)
        {

            bool news = await _service.RemoveNews(id);

            if (news)
            {
                return Ok("News deleted");
            }
            else
            {
                return NotFound();
            }

        }

        /// <summary>
        ///     Updates a News.
        /// </summary>
        /// <remarks>
        ///     Updates a News in the database.
        /// </remarks>
        /// <param name="InserNewDto">New value for the News</param>
        /// <param name="id">Id from the News for changes</param>
        /// <response code="200">Ok. Return the new News updated</response>
        /// <response code="400">BadRequest. News could not be updated.</response>    
        /// <response code="401">Unauthorized. Invalid JWT Token or it wasn't provided.</response>   
        /// <response code="404">Not found. Server couldn't find the News.</response> 
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetNewsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateNews([FromRoute] int id, [FromForm] InserNewDto newsDTO)
        {
            try
            {
                var news = await _service.UpdateNews(id, newsDTO);
                if (news != null)
                {
                    return Ok(news);
                }
                else
                {
                    return NotFound(400);
                }
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex);
            }
        }

        /// <summary>
        ///     Gets a News information.
        /// </summary>
        /// <remarks>
        ///     Gets information about the news with the id provided.
        /// </remarks>
        /// <param name="id">News id that will be searched.</param>        
        /// <response code="200">OK. Returns news information.</response> 
        /// <response code="400">BadRequest. Invalid request received.</response> 
        /// <response code="401">Unauthorized. Invalid JWT Token or it wasn't provided.</response>
        /// <response code="404">Not found. Server couldn't find the news.</response> 
        /// <response code="500">Internal Server Error.</response>
        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetNewsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNewById([FromQuery(Name = "id")] int id)
        {

            var news = await _service.GetNewsById(id);

            if (news != null)
            {
                return Ok(news);
            }
            else
            {
                return NotFound();
            }

        }

        /// <summary>
        ///     Gets the comments of a news.
        /// </summary>
        /// <remarks>
        ///     Gets the comments of the news with the id provided.
        /// </remarks>
        /// <param name="id">News id that will be searched.</param>        
        /// <response code="200">OK. Returns the comments of the news.</response> 
        /// <response code="400">BadRequest. Invalid request received.</response> 
        /// <response code="401">Unauthorized. Invalid JWT Token or it wasn't provided.</response>
        /// <response code="404">Not found. Server couldn't find the comments of the news.</response> 
        /// <response code="500">Internal Server Error.</response>
        [HttpGet("{id}/comments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CommentGetDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNewByIdComments( int id)
        {
            var news = await _service.GetNewsByIdComments(id);

            if (news != null)
            {  
                return Ok(news);
            }
            else
            {
                return NotFound();
            }

        }

    }
}
