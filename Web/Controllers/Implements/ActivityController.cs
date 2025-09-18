using Business.Interfaces;
using Entity.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Controllers.Interface;

namespace Web.Controllers.Implements
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityController : BaseController, IActivityController
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService ?? throw new ArgumentNullException(nameof(activityService));
        }

        /// <summary>
        /// Obtiene todas las actividades
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _activityService.GetAllAsync();
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Obtiene una actividad por su ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _activityService.GetByIdAsync(id);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Obtiene actividades activas
        /// </summary>
        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var result = await _activityService.GetActiveActivitiesAsync();
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Obtiene actividades por categoría
        /// </summary>
        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategory(string category)
        {
            var result = await _activityService.GetActivitiesByCategoryAsync(category);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Crea una nueva actividad
        /// </summary>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Create([FromBody] ActivityDTO activityDto)
        {
            if (activityDto == null)
            {
                return BadRequest(new {
                    success = false,
                    message = "Los datos de la actividad son requeridos",
                    errors = new[] { "El campo activityDto es requerido" }
                });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new {
                    success = false,
                    message = "Datos de entrada inválidos",
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            var result = await _activityService.CreateAsync(activityDto);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Actualiza completamente una actividad
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActivityDTO activityDto)
        {
            var result = await _activityService.UpdateAsync(id, activityDto);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Actualiza parcialmente una actividad usando JSON Patch
        /// </summary>
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePartial(int id, [FromBody] JsonPatchDocument<ActivityDTO> patchDoc)
        {
            var result = await _activityService.UpdatePartialAsync(id, patchDoc);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Realiza eliminación lógica de una actividad
        /// </summary>
        [HttpDelete("soft/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var result = await _activityService.SoftDeleteAsync(id);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Elimina permanentemente una actividad
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _activityService.DeleteAsync(id);
            return HandleServiceResult(result);
        }
    }
}