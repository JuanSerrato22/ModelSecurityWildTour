using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Model;
using Entity.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;


namespace Business.Implements
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public PersonService(IPersonRepository personRepository, IMapper mapper)
        {
            _personRepository = personRepository;
            _mapper = mapper;
        }

        public async Task<List<PersonDTO>> GetAllAsync()
        {
            var person = await _personRepository.GetAllAsync();
            return _mapper.Map<List<PersonDTO>>(person);
        }

        public async Task<PersonDTO> GetByIdAsync(int id)
        {
            var person = await _personRepository.GetByIdAsync(id);
            return _mapper.Map<PersonDTO>(person);
        }

        public async Task<PersonDTO> CreateAsync(PersonDTO personDto)
        {
            var person = _mapper.Map<Person>(personDto);
            var personCreado = await _personRepository.CreateAsync(person);
            return _mapper.Map<PersonDTO>(personCreado);
        }

        public async Task<PersonDTO> UpdateAsync(int id, PersonDTO personDto)
        {
            var person = await _personRepository.GetByIdAsync(id);
            if (person == null) return null!; // Devuelve null forzando la nulabilidad, pero cumple la firma

            person.FirstName = personDto.FirstName ?? person.FirstName;
            person.LastName = personDto.LastName ?? person.LastName;
            person.Document = personDto.Document;
            person.PhoneNumber = personDto.PhoneNumber;

            var actualizado = await _personRepository.UpdateAsync(person);
            return _mapper.Map<PersonDTO>(actualizado);
        }

        public async Task<PersonDTO> UpdatePartialAsync(int id, JsonPatchDocument<PersonDTO> patchDoc)
        {
            var person = await _personRepository.GetByIdAsync(id);
            if (person == null) return null!;

            var personDto = _mapper.Map<PersonDTO>(person);
            patchDoc.ApplyTo(personDto);

            // Mapea de nuevo a la entidad y actualiza
            _mapper.Map(personDto, person);
            var updated = await _personRepository.UpdateAsync(person);
            return _mapper.Map<PersonDTO>(updated);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var person = await _personRepository.GetByIdAsync(id);
            if (person == null) return false;

            if (person.Active)  
            {
                person.Active = false;
                person.DeleteAt = DateTime.Now;
            }
            else
            {
                person.Active = true;
                person.DeleteAt = null;
            }

            await _personRepository.UpdateAsync(person);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _personRepository.DeleteAsync(id);
        }
    }
}