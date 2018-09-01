using IffleyRoutesRecord.DTOs;
using IffleyRoutesRecord.Entities;
using IffleyRoutesRecord.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IffleyRoutesRecord.Logic
{
    public class GradeManager : IGradeManager
    {
        private readonly IffleyRoutesRecordContext iffleyRoutesRecordContext;

        public GradeManager(IffleyRoutesRecordContext iffleyRoutesRecordContext)
        {
            this.iffleyRoutesRecordContext = iffleyRoutesRecordContext;
        }

        public TechGradeDto GetTechGradeOnProblem(int problemId)
        {
            var problem = iffleyRoutesRecordContext.Problem
                .Include(prob => prob.TechGrade)
                .SingleOrDefault(prob => prob.Id == problemId);

            if (problem.TechGradeId is null)
            {
                return null;
            }

            return new TechGradeDto()
            {
                GradeId = problem.TechGradeId.Value,
                Name = problem.TechGrade.Name,
                Rank = problem.TechGrade.Rank
            };
        }

        public BGradeDto GetBGradeOnProblem(int problemId)
        {
            var problem = iffleyRoutesRecordContext.Problem
                .Include(prob => prob.BGrade)
                .SingleOrDefault(prob => prob.Id == problemId);

            if (problem.BGradeId is null)
            {
                return null;
            }

            return new BGradeDto()
            {
                GradeId = problem.BGradeId.Value,
                Name = problem.BGrade.Name,
                Rank = problem.BGrade.Rank
            };
        }

        public PoveyGradeDto GetPoveyGradeOnProblem(int problemId)
        {
            var problem = iffleyRoutesRecordContext.Problem
                .Include(prob => prob.PoveyGrade)
                .SingleOrDefault(prob => prob.Id == problemId);

            if (problem.PoveyGradeId is null)
            {
                return null;
            }

            return new PoveyGradeDto()
            {
                GradeId = problem.PoveyGradeId.Value,
                Name = problem.PoveyGrade.Name,
                Rank = problem.PoveyGrade.Rank
            };
        }

        public FurlongGradeDto GetFurlongGradeOnProblem(int problemId)
        {
            var problem = iffleyRoutesRecordContext.Problem
                .Include(prob => prob.FurlongGrade)
                .SingleOrDefault(prob => prob.Id == problemId);

            if (problem.FurlongGradeId is null)
            {
                return null;
            }

            return new FurlongGradeDto()
            {
                GradeId = problem.FurlongGradeId.Value,
                Name = problem.FurlongGrade.Name,
                Rank = problem.FurlongGrade.Rank
            };
        }
    }
}
