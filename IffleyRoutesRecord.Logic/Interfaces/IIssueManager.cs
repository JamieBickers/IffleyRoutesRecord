using IffleyRoutesRecord.Models.DTOs.Requests;
using IffleyRoutesRecord.Models.DTOs.Responses;
using IffleyRoutesRecord.Models.Entities;
using System.Collections.Generic;

namespace IffleyRoutesRecord.Logic.Interfaces
{
    public interface IIssueManager
    {
        IEnumerable<Issue> GetIssues();
        IEnumerable<ProblemIssueResponse> GetProblemIssues();
        void CreateIssue(CreateIssueRequest issue);
        void CreateProblemIssue(CreateProblemIssueRequest issue);
    }
}