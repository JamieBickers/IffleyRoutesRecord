using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.Interfaces;
using IffleyRoutesRecord.Logic.StaticHelpers;
using IffleyRoutesRecord.Models.DTOs.Requests;
using IffleyRoutesRecord.Models.DTOs.Responses;
using IffleyRoutesRecord.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Logic.Managers
{
    public class IssueManager : IIssueManager
    {
        private readonly IffleyRoutesRecordContext repository;
        private readonly IProblemReader problemReader;
        private readonly IIssueRequestValidator validator;

        public IssueManager(IffleyRoutesRecordContext repository, IProblemReader problemReader, IIssueRequestValidator validator)
        {
            this.repository = repository;
            this.problemReader = problemReader;
            this.validator = validator;
        }

        public IEnumerable<Issue> GetIssues()
        {
            return repository.Issue.ToList();
        }

        public IEnumerable<ProblemIssueResponse> GetProblemIssues()
        {
            var issues = repository.ProblemIssue.Include(problemIssue => problemIssue.Problem);

            foreach (var issue in issues)
            {
                yield return Mapper.Map(issue);
            }
        }

        public void CreateIssue(CreateIssueRequest issue, string userEmail)
        {
            if (issue is null)
            {
                throw new ArgumentNullException(nameof(issue));
            }

            var issueDbo = new Issue()
            {
                Description = issue.Description,
                LoggedBy = userEmail
            };
            repository.Issue.Add(issueDbo);
            repository.SaveChanges();
        }

        public void CreateProblemIssue(CreateProblemIssueRequest issue, string userEmail)
        {
            if (issue is null)
            {
                throw new ArgumentNullException(nameof(issue));
            }

            validator.ValidateIssue(issue);

            var issueDbo = new ProblemIssue()
            {
                Description = issue.Description,
                ProblemId = issue.ProblemId,
                LoggedBy = userEmail
            };
            repository.ProblemIssue.Add(issueDbo);
            repository.SaveChanges();
        }
    }
}
