using Business.Interfaces;
using Entity.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Controllers.Interface;

namespace Web.Controllers.Implements
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase, IPersonController
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var persons = await _personService.GetAllAsync();
            return Ok(persons);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var person = await _personService.GetByIdAsync(id);
            if (person == null) return NotFound();
            return Ok(person);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PersonDTO personDto)
        {
            var created = await _personService.CreateAsync(personDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PersonDTO personDto)
        {
            var updated = await _personService.UpdateAsync(id, personDto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePartial(int id, [FromBody] PersonDTO personDto)
        {
            if (personDto == null) return BadRequest();

            // Obtener la persona existente
            var person = await _personService.GetByIdAsync(id);
            if (person == null) return NotFound();

            // Actualizar solo los campos que vienen distintos de null
            var updatedPerson = new PersonDTO
            {
                Id = person.Id,
                FirstName = personDto.FirstName ?? person.FirstName,
                LastName = personDto.LastName ?? person.LastName,
                Document = personDto.Document != 0 ? personDto.Document : person.Document,
                PhoneNumber = personDto.PhoneNumber != 0 ? personDto.PhoneNumber : person.PhoneNumber
            };

            var updated = await _personService.UpdateAsync(id, updatedPerson);
            return Ok(updated);
        }


        [HttpDelete("soft/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var deleted = await _personService.SoftDeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _personService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}