using IffleyRoutesRecord.Models.DTOs.Requests;

namespace IffleyRoutesRecord.Logic.Interfaces
{
    public interface IIssueRequestValidator
    {
        void ValidateIssue(CreateProblemIssueRequest issue);
    }
}