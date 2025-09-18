using System;
using System.ComponentModel.DataAnnotations;

namespace Entity.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "El nombre de usuario no puede exceder 100 caracteres")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        [StringLength(255, ErrorMessage = "El correo electrónico no puede exceder 255 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 255 caracteres")]
        public string Password { get; set; } = string.Empty;

        public DateTime RegistrationDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsActive { get; set; }

        // Relación con Person (opcional)
        public int? PersonId { get; set; }
        public PersonDTO? Person { get; set; }
    }
}