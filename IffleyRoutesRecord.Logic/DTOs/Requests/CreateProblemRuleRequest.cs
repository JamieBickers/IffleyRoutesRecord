using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IffleyRoutesRecord.Logic.DTOs.Requests
{
    public class CreateProblemRuleRequest : IValidatableObject
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult("Name cannot be empty or whitespace.");
            }
        }
    }
}
