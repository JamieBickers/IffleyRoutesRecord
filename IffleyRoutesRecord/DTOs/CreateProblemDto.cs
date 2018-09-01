﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace IffleyRoutesRecord.DTOs
{
    public class CreateProblemDto : IValidatableObject
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(5000)]
        public string Description { get; set; }

        public DateTimeOffset? DateSet { get; set; }
        public string FirstAscent { get; set; }

        [Range(1, int.MaxValue)]
        public int? TechGradeId { get; set; }

        [Range(1, int.MaxValue)]
        public int? BGradeId { get; set; }

        [Range(1, int.MaxValue)]
        public int? PoveyGradeId { get; set; }

        [Range(1, int.MaxValue)]
        public int? FurlongGradeId { get; set; }

        public IList<CreateHoldOnProblemDto> Holds { get; set; }
        public IEnumerable<CreateProblemRuleDto> NewRules { get; set; }
        public IEnumerable<int> ExistingRuleIds { get; set; }
        public IEnumerable<int> StyleSymbolIds { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult("Name cannot be empty or whitespace.");
            }

            if (DateSet > DateTimeOffset.Now)
            {
                yield return new ValidationResult("Date set cannot be in teh future.");
            }

            if (TechGradeId == null && BGradeId == null && PoveyGradeId == null && FurlongGradeId == null)
            {
                yield return new ValidationResult("Must include at least one grade.");
            }

            if (Holds == null || !Holds.Any())
            {
                yield return new ValidationResult("Must include at least 1 hold.");
            }

            foreach (var error in Holds.SelectMany(hold => hold.Validate(validationContext)))
            {
                yield return error;
            }

            if (NewRules != null)
            {
                foreach (var error in NewRules.SelectMany(rule => rule.Validate(validationContext)))
                {
                    yield return error;
                }
            }

            if (ExistingRuleIds?.Any(ruleId => ruleId <= 0) ?? false)
            {
                yield return new ValidationResult("Existing rule IDs must be positive integers.");
            }

            if (StyleSymbolIds?.Any(styleSymbolId => styleSymbolId <= 0) ?? false)
            {
                yield return new ValidationResult("Style symbol IDs must be positive integers.");
            }

            //TODO:
            var standingStartHolds = Holds.Where(hold => hold.IsStandingStartHold);
            if (standingStartHolds.Any())
            {
                
            }
        }
    }
}
