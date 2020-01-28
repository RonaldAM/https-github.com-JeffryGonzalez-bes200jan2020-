using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Models
{
    public class PostReservationRequest : IValidatableObject
    {
        [Required]
        public string For { get; set; }
        [Required]
        public string[] Books { get; set; } // []

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Books.Length < 1)
            {
                yield return new ValidationResult("You have to reserve some books!", new string[] { nameof(Books) });
            }
        }
    }
}
