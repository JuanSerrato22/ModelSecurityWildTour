using System;
using System.ComponentModel.DataAnnotations;
using Entity.Model.Base;

namespace Entity.Model
{
    public class User : GenericModel
    {
        public string? Username { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        public DateTime RegistrationDate { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; } = string.Empty;

        // Relación con Person (opcional)
        public int? PersonId { get; set; }
        public virtual Person? Person { get; set; }

        // Propiedades adicionales para autenticación
        public DateTime? LastLoginDate { get; set; }
        public bool IsEmailConfirmed { get; set; } = false;
        public new bool IsActive { get; set; } = true;
    }
}