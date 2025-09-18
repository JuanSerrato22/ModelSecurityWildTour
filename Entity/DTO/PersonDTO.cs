using System;
using System.ComponentModel.DataAnnotations;

namespace Entity.DTO
{
    public class PersonDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        [StringLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El documento es requerido")]
        public int Document { get; set; }

        [Range(1000000000, 9999999999999, ErrorMessage = "El número de teléfono debe tener entre 10 y 13 dígitos")]
        public long PhoneNumber { get; set; }
    }
}