using System.Collections.Generic;
using IffleyRoutesRecord.Logic.DTOs.Requests;
using IffleyRoutesRecord.Logic.DTOs.Responses;

namespace IffleyRoutesRecord.Logic.Interfaces
{
    public interface IHoldManager
    {
        HoldResponse GetHold(int holdId);
        IEnumerable<HoldResponse> GetHolds();
        IList<HoldOnProblemResponse> GetHoldsOnProblem(int problemId);
        void AddProblemHoldsToDatabase(IList<CreateHoldOnProblemRequest> holds, int problemId);
    }
}