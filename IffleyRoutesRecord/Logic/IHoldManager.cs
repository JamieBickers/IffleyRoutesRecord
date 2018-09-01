using System.Collections.Generic;
using IffleyRoutesRecord.DTOs;

namespace IffleyRoutesRecord.Logic
{
    public interface IHoldManager
    {
        IList<HoldDto> GetHoldsOnProblem(int problemId);
        void AddProblemHoldsToDatabase(IList<CreateHoldOnProblemDto> holds, int problemId);
    }
}