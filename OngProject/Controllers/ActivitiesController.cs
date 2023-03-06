using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Interfaces;
using OngProject.Core.Mapper;
using OngProject.Core.Models.DTOs.ActivitiesDTO;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivitiesBusiness _service;
        public ActivitiesController(IActivitiesBusiness service)
        {
            _service = service;
        }

        /// GET: activities
        /// <summary>
        ///     Gets all activities.
        /// </summary>
        /// <remarks>
        ///     Gets information paged about the activities in the database.
        /// </remarks>
        /// <response code="200">OK. Returns activities information.</response>  
        /// <response code="400">BadRequest. Invalid request received.</response> 
        /// <response code="401">Unauthoried.</response> 
        /// <response code="404">Not found. Server couldn't find the activities.</response> 
        /// <response code="500">Internal Server Error.</response> 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllActivities()
        {

            var activities = await _service.GetAllActivities();

            if (activities != null)
            {
                return Ok(activities);
            }
            else
            {
                return NotFound();
            }

        }

        /// <summary>
        ///     Creates a new Activity.
        /// </summary>
        /// <remarks>
        ///     Adds a new activity in the database.
        /// </remarks>
        /// <param name="activitiesDTO">New Activity data transfer object.</param>
        /// <response code="200">OK. Returns a result object along with the new activity information.</response>        
        /// <response code="400">BadRequest. Activity could not be created.</response>    
        /// <response code="401">Unauthorized. Invalid JWT Token or it wasn't provided.</response>    
        /// <response code="500">Internal Server Error.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateActivities([FromQuery] ActivitiesCreateDTO activitiesDTO)
        {

            var activities = await _service.CreateActivities(activitiesDTO);

            if (activities != null)
            {
                return Ok(activities);
            }
            else
            {
                return NotFound();
            }

        }

        /// <summary>
        ///     Deletes an activity.
        /// </summary>
        /// <remarks>
        ///     Deletes an Activity in the database.
        /// </remarks>
        /// <param name="id">Id of the activity that'll be removed from the database</param>
        /// <response code="200">OK. Returns a result object if the activity was successfully removed.</response>        
        /// <response code="400">BadRequest. Activity could not be removed.</response>    
        /// <response code="401">Unauthorized. Invalid JWT Token or it wasn't provided.</response>
        /// <response code="404">Not found. Server couldn't find the activity with the id provided.</response> 
        /// <response code="500">Internal Server Error.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveActivities([FromRoute] int id)
        {

            bool activities = await _service.RemoveActivities(id);

            if (activities!=false)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }

        /// <summary>
        ///     Updates an activity.
        /// </summary>
        /// <remarks>
        ///     Updates an activity in the database.
        /// </remarks>
        /// <param name="activitiesDTO">New value for the activity</param>
        /// <param name="id">Id from activity for changes</param>
        /// <response code="200">Ok. Return the new activity updated</response>
        /// <response code="400">BadRequest. Activity could not be updated.</response>    
        /// <response code="401">Unauthorized. Invalid JWT Token or it wasn't provided.</response>   
        /// <response code="404">Not found. Server couldn't find the activity.</response> 
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateActivities([FromRoute] int id, [FromQuery] ActivitiesCreateDTO activitiesDTO)
        {
            var existActivityByID = await _service.GetActivitiesById(id);
            if (existActivityByID != null)
            {
                var activitiesUpdated = await _service.UpdateActivities(id, activitiesDTO);
                ActivitiesMapper mapper = new();
                var dto = mapper.FromActivitiesToActivitiesDisplayDto(activitiesUpdated);
                return Ok(dto);
            }
            else
            {
                return NotFound("No se encontro una actividad con ese ID");
            }
        }

        /// <summary>
        ///     Gets an activity information.
        /// </summary>
        /// <remarks>
        ///     Gets information about the activity with the id provided.
        /// </remarks>
        /// <param name="id">Activity id that will be searched.</param>        
        /// <response code="200">OK. Returns activity information.</response> 
        /// <response code="400">BadRequest. Invalid request received.</response> 
        /// <response code="404">Not found. Server couldn't find the activity.</response> 
        /// <response code="500">Internal Server Error.</response> 
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetActivitiesById([FromRoute] int id)
        {

            var activities = await _service.GetActivitiesById(id);

            if (activities != null)
            {
                ActivitiesMapper mapper = new();
                var activity = mapper.FromActivitiesToActivitiesDisplayDto(activities);
                return Ok(activity);
            }
            else
            {
                return NotFound();
            }

        }
    }
}
