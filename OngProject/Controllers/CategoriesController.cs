using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OngProject.Core.Helper;
using OngProject.Core.Interfaces;
using OngProject.Core.Models.DTOs.CategoriesDTO;
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

    public class CategoriesController: ControllerBase
    {
        private readonly ICategoriesBusiness _service;
        public CategoriesController(ICategoriesBusiness service)
        {
            _service = service;
        }
        /// <summary>
        /// Get a list of all categories
        /// </summary>
        /// <param name="numberPage">Page number</param>
        /// <param name="quantityPage">Number of filtered records</param>
        /// <returns></returns>
        /// <response code="200">OK. Returns Categories information.</response>  
        /// <response code="400">BadRequest. Invalid request received.</response> 
        /// <response code="401">Unauthoried.</response> 
        /// <response code="404">Not found. Server couldn't find the Categories.</response> 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK,Type=typeof(List<GetNameCategoriesDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllCategories([FromQuery(Name = "numberPage")] int numberPage = 1, [FromQuery(Name = "quantityPage")] int quantityPage = 10)
        {
            var host = HttpContext.Request.Host.Value;
            var path = HttpContext.Request.Path.Value;

            var categoryDTO = await _service.GetAllCategories();

            if (categoryDTO != null)
            {
                PagedListHelper<GetNameCategoriesDTO> paged = PagedListHelper<GetNameCategoriesDTO>.Create(categoryDTO, numberPage, quantityPage);
                PagedListDTO<GetNameCategoriesDTO> testList = new(paged, host, path);
                if(testList.totalPage < numberPage)
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
        /// Create a new category
        /// </summary>
        /// <param name="categoryDTO"> Name Category</param>
        /// <returns></returns>
        ///
        /// <response code="200">OK. Returns the new saved category.</response>  
        /// <response code="400">BadRequest. Invalid request received.</response> 
        /// <response code="401">Unauthoried.</response> 
       
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCategoriesDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      
        public async Task<IActionResult> CreateCategory([FromQuery] CreateCategoriesDTO categoryDTO)
        {

            var category = await _service.CreateCategory(categoryDTO);

            if (category != null)
            {
                return Ok(category);
            }
            else
            {
                return NotFound();
            }

        }
        /// <summary>
        /// Delete a Category by ID
        /// </summary>
        /// <param name="id">Category id to remove</param>
        /// <returns></returns>
        /// <response code="200">OK. return an OK as response.</response>  
        /// <response code="400">BadRequest. Invalid request received.</response> 
        /// <response code="401">Unauthoried.</response> 
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RemoveCategory(int id)
        {
            try
            {
                bool category = await _service.RemoveCategory(id);
                return Ok();
            }
            catch (System.Exception err)
            {
                return NotFound(err.Message);
            }
        }
        /// <summary>
        /// Update a Category
        /// </summary>
        /// <param name="id">Category ID to update</param>
        /// <param name="categoryDTO"></param>
        /// <returns></returns>
        /// <response code="200">OK. Return the updated category.</response>  
        /// <response code="400">BadRequest. Invalid request received.</response> 
        /// <response code="401">Unauthoried.</response> 
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromForm] CategoryUpdateDto categoryDTO)
        {
            try
            {
                var category = await _service.UpdateCategory(id, categoryDTO);
                return Ok(category);
            }
            catch (Exception er)
            {
                if (er.Message.Contains("Not Found"))
                    return NotFound(er.Message);

                return BadRequest(er.Message);
            }

        }
        /// <summary>
        /// Get a category by an ID
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns></returns>
        /// <response code="200">OK. Return a category by ID.</response>  
        /// <response code="400">BadRequest. Invalid request received.</response> 
        /// <response code="401">Unauthoried.</response> 
        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCategoriesDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCategoryById([FromQuery(Name = "id")] int id)
        {

            var category = await _service.GetCategoryById(id);

            if (category != null)
            {
                return Ok(category);
            }
            else
            {
                return NotFound();
            }

        }
    }
}
