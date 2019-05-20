using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.Interfaces;
using IffleyRoutesRecord.Logic.StaticHelpers;
using IffleyRoutesRecord.Models.DTOs.Requests;
using System;

namespace IffleyRoutesRecord.Logic.Validators
{
    public class IssueRequestValidator
    {
        private readonly IffleyRoutesRecordContext repository;

        public IssueRequestValidator(IffleyRoutesRecordContext repository)
        {
            this.repository = repository;
        }

        public void ValidateIssue(CreateProblemIssueRequest issue)
        {
            if (issue is null)
            {
                throw new ArgumentNullException(nameof(issue));
            }

            repository.Problem.VerifyEntityWithIdExists(issue.ProblemId);
        }
    }
}
