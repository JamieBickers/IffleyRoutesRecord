using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.Interfaces;
using IffleyRoutesRecord.Logic.StaticHelpers;
using IffleyRoutesRecord.Models.DTOs.Requests;
using IffleyRoutesRecord.Models.DTOs.Responses;
using IffleyRoutesRecord.Models.Entities;
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
            var issues = repository.ProblemIssue;

            foreach (var issue in issues)
            {
                var issueResponse = Mapper.Map(issue);
                var problem = problemReader.GetProblem(issue.ProblemId);
                issueResponse.Problem = problem;

                yield return issueResponse;
            }
        }

        public void CreateIssue(CreateIssueRequest issue)
        {
            if (issue is null)
            {
                throw new ArgumentNullException(nameof(issue));
            }

            var issueDbo = Mapper.Map(issue);
            repository.Issue.Add(issueDbo);
            repository.SaveChanges();
        }

        public void CreateProblemIssue(CreateProblemIssueRequest issue)
        {
            if (issue is null)
            {
                throw new ArgumentNullException(nameof(issue));
            }

            validator.ValidateIssue(issue);

            var issueDbo = Mapper.Map(issue);
            repository.ProblemIssue.Add(issueDbo);
            repository.SaveChanges();
        }
    }
}
