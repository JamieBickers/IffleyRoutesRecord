using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.Interfaces;
using IffleyRoutesRecord.Logic.StaticHelpers;
using IffleyRoutesRecord.Models.DTOs.Requests;
using IffleyRoutesRecord.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace IffleyRoutesRecord.Logic.Validators
{
    public class ProblemRequestValidator : IProblemRequestValidator
    {
        private readonly IffleyRoutesRecordContext repository;

        public ProblemRequestValidator(IffleyRoutesRecordContext repository)
        {
            this.repository = repository;
        }

        public void Validate(CreateProblemRequest problem)
        {
            if (problem is null)
            {
                throw new ArgumentNullException(nameof(problem));
            }

            ValidateCreateProblemRequest(problem);
            ValidateCreateHoldsOnProblemRequest(problem.Holds);
            ValidateAddProblemRulesRequest(problem.NewRules, problem.ExistingRuleIds);
            ValidateAddProblemStyleSymbolsRequest(problem.StyleSymbolIds);
        }

        private void ValidateCreateProblemRequest(CreateProblemRequest problem)
        {
            if (problem is null)
            {
                throw new ArgumentNullException(nameof(problem));
            }

            repository.Problem.VerifyEntityWithNameDoesNotExists(problem.Name);

            VerifyGradeExists(problem.TechGradeId, repository.TechGrade);
            VerifyGradeExists(problem.BGradeId, repository.BGrade);
            VerifyGradeExists(problem.PoveyGradeId, repository.PoveyGrade);
            VerifyGradeExists(problem.FurlongGradeId, repository.FurlongGrade);
        }

        private void VerifyGradeExists<TGrade>(int? gradeId, DbSet<TGrade> grades) where TGrade : BaseEntity
        {
            if (grades is null)
            {
                throw new ArgumentNullException(nameof(grades));
            }

            if (gradeId != null)
            {
                grades.VerifyEntityWithIdExists(gradeId.Value);
            }
        }

        private void ValidateCreateHoldsOnProblemRequest(IEnumerable<CreateHoldOnProblemRequest> holds)
        {
            if (holds is null)
            {
                throw new ArgumentNullException(nameof(holds));
            }

            foreach (var hold in holds)
            {
                repository.Hold.VerifyEntityWithIdExists(hold.HoldId);

                if (hold.ExistingHoldRuleIds != null)
                {
                    foreach (int ruleId in hold.ExistingHoldRuleIds)
                    {
                        repository.HoldRule.VerifyEntityWithIdExists(ruleId);
                    }
                }

                if (hold.NewHoldRules != null)
                {
                    foreach (var newRule in hold.NewHoldRules)
                    {
                        repository.HoldRule.VerifyEntityWithNameDoesNotExists(newRule.Name);
                    }
                }
            }
        }

        private void ValidateAddProblemRulesRequest(IEnumerable<CreateProblemRuleRequest> newRules, IEnumerable<int> existingRuleIds)
        {
            if (newRules != null)
            {
                foreach (var newRule in newRules)
                {
                    repository.GeneralRule.VerifyEntityWithNameDoesNotExists(newRule.Name);
                }
            }

            if (existingRuleIds != null)
            {
                foreach (int ruleId in existingRuleIds)
                {
                    repository.GeneralRule.VerifyEntityWithIdExists(ruleId);
                }
            }
        }

        private void ValidateAddProblemStyleSymbolsRequest(IEnumerable<int> styleSymbolIds)
        {
            // Style symbols are not required
            if (styleSymbolIds is null)
            {
                return;
            }

            foreach (int styleSymbolId in styleSymbolIds)
            {
                repository.StyleSymbol.VerifyEntityWithIdExists(styleSymbolId);
            }
        }
    }
}
