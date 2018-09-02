using System.Collections.Generic;
using IffleyRoutesRecord.Logic.DTOs.Received;
using IffleyRoutesRecord.Logic.DTOs.Sent;

namespace IffleyRoutesRecord.Logic.Interfaces
{
    public interface IHoldManager
    {
        HoldDto GetHold(int holdId);
        IEnumerable<HoldDto> GetHolds();
        IList<HoldOnProblemDto> GetHoldsOnProblem(int problemId);
        void AddProblemHoldsToDatabase(IList<CreateHoldOnProblemDto> holds, int problemId);
    }
}