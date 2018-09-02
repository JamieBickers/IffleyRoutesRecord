using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.DTOs.Received;
using IffleyRoutesRecord.Logic.DTOs.Sent;
using IffleyRoutesRecord.Logic.Entities;
using IffleyRoutesRecord.Logic.Interfaces;

namespace IffleyRoutesRecord.Logic.Models
{
    public class ProblemCreator : IProblemCreator
    {
        private readonly IffleyRoutesRecordContext iffleyRoutesRecordContext;
        private readonly IStyleSymbolManager styleSymbolManager;
        private readonly IRuleManager ruleManager;
        private readonly IHoldManager holdManager;
        private readonly IGradeManager gradeManager;
        private readonly IProblemReader problemReader;

        public ProblemCreator(IffleyRoutesRecordContext iffleyRoutesRecordContext, IStyleSymbolManager styleSymbolManager,
            IRuleManager ruleManager, IHoldManager holdManager, IGradeManager gradeManager, IProblemReader problemReader)
        {
            this.iffleyRoutesRecordContext = iffleyRoutesRecordContext;
            this.styleSymbolManager = styleSymbolManager;
            this.ruleManager = ruleManager;
            this.holdManager = holdManager;
            this.gradeManager = gradeManager;
            this.problemReader = problemReader;
        }

        public ProblemDto CreateProblem(CreateProblemDto problem)
        {
            var problemDbo = AddProblemToDatabase(problem);
            ruleManager.AddRulesToDatabase(problem.NewRules, problem.ExistingRuleIds, problemDbo.Id);
            holdManager.AddProblemHoldsToDatabase(problem.Holds, problemDbo.Id);
            styleSymbolManager.AddProblemStyleSymbolsToDatabase(problem.StyleSymbolIds, problemDbo.Id);

            iffleyRoutesRecordContext.SaveChanges();

            return problemReader.GetProblem(problemDbo.Id);
        }

        private Problem AddProblemToDatabase(CreateProblemDto problem)
        {
            var problemDbo = new Problem()
            {
                Name = problem.Name,
                Description = problem.Description,
                TechGradeId = problem.TechGradeId,
                BGradeId = problem.BGradeId,
                PoveyGradeId = problem.PoveyGradeId,
                FurlongGradeId = problem.FurlongGradeId,
                DateSet = problem.DateSet,
                FirstAscent = problem.FirstAscent
            };

            iffleyRoutesRecordContext.Problem.Add(problemDbo);
            return problemDbo;
        }
    }
}
