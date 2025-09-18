using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entity.Model.Base;

namespace Entity.Model
{
    public class Person : GenericModel
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public int Document { get; set; }

        public long PhoneNumber { get; set; }

        // Relación inversa con User
        public virtual ICollection<User>? Users { get; set; }
    }
}