﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace IffleyRoutesRecord.Logic.DTOs.Requests
{
    public class CreateHoldOnProblemRequest : IValidatableObject
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int HoldId { get; set; }

        [Required]
        public bool IsStandingStartHold { get; set; }

        public IEnumerable<int> ExistingHoldRuleIds { get; set; }
        public IEnumerable<CreateHoldRuleRequest> NewHoldRules { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ExistingHoldRuleIds?.Any(ruleId => ruleId <= 0) ?? false)
            {
                yield return new ValidationResult("Existing rule ID cannot be less than 1.");
            }

            if (NewHoldRules != null)
            {
                foreach (var error in NewHoldRules.SelectMany(rule => rule.Validate(validationContext)))
                {
                    yield return error;
                }
            }
        }
    }
}
