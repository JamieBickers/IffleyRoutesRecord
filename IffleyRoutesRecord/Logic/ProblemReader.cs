using IffleyRoutesRecord.DTOs;
using IffleyRoutesRecord.Entities;
using IffleyRoutesRecord.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Logic
{
    public class ProblemReader : IProblemReader
    {
        private readonly IffleyRoutesRecordContext iffleyRoutesRecordContext;
        private readonly IStyleSymbolManager styleSymbolManager;
        private readonly IRuleManager ruleManager;
        private readonly IHoldManager holdManager;
        private readonly IGradeManager gradeManager;

        public ProblemReader(IffleyRoutesRecordContext iffleyRoutesRecordContext, IStyleSymbolManager styleSymbolManager,
            IRuleManager ruleManager, IHoldManager holdManager, IGradeManager gradeManager)
        {
            this.iffleyRoutesRecordContext = iffleyRoutesRecordContext;
            this.styleSymbolManager = styleSymbolManager;
            this.ruleManager = ruleManager;
            this.holdManager = holdManager;
            this.gradeManager = gradeManager;
        }

        public ProblemDto GetProblem(int problemId)
        {
            var problem = GetProblemFromDatabase(problemId);

            if (problem is null)
            {
                return null;
            }

            return CreateProblemDto(problem);
        }

        public IEnumerable<ProblemDto> GetProblems()
        {
            return iffleyRoutesRecordContext
                .Problem
                .AsEnumerable()
                .Select(problem => CreateProblemDto(problem));
        }

        private ProblemDto CreateProblemDto(Problem problem)
        {
            return new ProblemDto()
            {
                ProblemId = problem.Id,
                Name = problem.Name,
                Description = problem.Description,
                DateSet = problem.DateSet,
                FirstAscent = problem.FirstAscent,
                TechGrade = gradeManager.GetTechGradeOnProblem(problem.Id),
                BGrade = gradeManager.GetBGradeOnProblem(problem.Id),
                PoveyGrade = gradeManager.GetPoveyGradeOnProblem(problem.Id),
                FurlongGrade = gradeManager.GetFurlongGradeOnProblem(problem.Id),
                Holds = holdManager.GetHoldsOnProblem(problem.Id),
                Rules = ruleManager.GetProblemRules(problem.Id),
                StyleSymbols = styleSymbolManager.GetStyleSymbolsOnProblem(problem.Id)
            };
        }

        private Problem GetProblemFromDatabase(int problemId)
        {
            return iffleyRoutesRecordContext.Problem.SingleOrDefault(route => route.Id == problemId);
        }
    }
}